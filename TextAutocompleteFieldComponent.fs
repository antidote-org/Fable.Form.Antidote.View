namespace Fable.Form.Antidote

open Fable.Form.Antidote.Field
open Fable.Form.Antidote.Form.View
open Glutinum.Fuse
open Antidote.UFuzzy
open Feliz
open Feliz.Bulma
open Fable.Core.JsInterop
open Feliz
open Fable.Core.JS

module Hooks =
    [<Hook>]
    let useDebounce (value: 'T, timeout:int, callback : 'T -> unit) =
        let initialCall, setInitialCall = React.useState(true)

        React.useEffect(
            fun _ ->
                if initialCall then
                    { new System.IDisposable with
                        member __.Dispose() =
                            setInitialCall false
                    }
                else
                    let handler =
                        setTimeout (fun () ->
                            callback value
                        ) timeout

                    { new System.IDisposable with
                        member __.Dispose() =
                            clearTimeout(handler)
                    }
            , [| box value ; box timeout |]
        )


    type UseUFuzzyReturn =  {
            Result: UFuzzy.HaystackIdxs
            Search: string -> unit
            Term: string
            Reset: unit -> unit
        }

    type UFuzzyProps = {
            Data : Array<string>
            UFuzzyOptions: UFuzzy.IUFuzzyOptions
        }

    [<Hook>]
    let useUFuzzy (props: UFuzzyProps) : UseUFuzzyReturn =
        let term, setTerm = React.useState("")
        console.log("USE UFUZZY")
        let ufuzzy = uFuzzy.Create(props.UFuzzyOptions)
        let result = ufuzzy.filter(props.Data, term)

        let reset = fun () -> setTerm("")

        {
            Result = result
            Search = setTerm
            Term = term
            Reset = reset
        }


    // type UseFuseReturn<'T> =  {
    //         Result: ResizeArray<Fuse.FuseResult<'T>>
    //         Search: string -> unit
    //         Term: string
    //         Reset: unit -> unit
    //     }

    // type FuseProps<'T> = {
    //         Data : ResizeArray<'T>
    //     }

    // [<Hook>]
    // let useFuse (props: FuseProps<'T>) : UseFuseReturn<'T> =
    //     let (term, setTerm) = React.useState("")

    //     let fuse = fuse.Create(props.Data)
    //     let result = fuse.search(term)

    //     let reset = fun () -> setTerm("")

    //     {
    //         Result = result
    //         Search = setTerm
    //         Term = term
    //         Reset = reset
    //     }


module Components =

    // Workaround to have React-refresh working
    // I need to open an issue on react-refresh to see if they can improve the detection
    emitJsStatement () "import React from \"react\""

    type TextAutocompleteFieldComponentProps<'Msg> =
        {|
            config : TextAutocompleteFieldConfig<'Msg>
        |}

    let private dropdownMenu dropdownContent =
        Bulma.dropdownMenu [
            // prop.className classes.dropdownMenu

            prop.children [
                dropdownContent
            ]
        ]

    [<ReactComponent>]
    let TextAutocompleteFieldComponent (props : TextAutocompleteFieldComponentProps<'Msg>) =
        let (
                {
                    Dispatch = dispatch
                    OnChange = onChange
                    OnBlur = onBlur
                    Disabled = disabled
                    Value = value
                    Error = error
                    ShowError = showError
                    Attributes = attributes
                } : TextAutocompleteFieldConfig<'Msg>
            ) = props.config

        let isFocused, setFocused = React.useState false
        let term, setTerm = React.useState(value)
        let hasBeenSaved, setHasBeenSaved = React.useState false
        let mutable fuseHook = Some (Hooks.useUFuzzy({
            Data = attributes.Possibilities |> List.toArray
            UFuzzyOptions = UFuzzy.IUFuzzyOptions()
        }))

        Hooks.useDebounce(term, 500, fuseHook.Value.Search)
        // React.useWindowListener.onClick(fun ev ->
        //     let target = ev.target :?> Browser.Types.HTMLElement

        //     match target.closest(".form-antidote-autocomplete-field") with
        //     | Some _ ->
        //         ()
        //     | None ->
        //         setFocused false
        // )
        if hasBeenSaved
        then
            fuseHook <- None
            Html.div [
                prop.className "form-antidote-autocomplete-field"
                prop.children [
                    Bulma.input.text [
                        prop.disabled true
                        prop.value term
                    ]
                ]
            ]
        else
            let dropdownElement =
                if isFocused && (not (isNull fuseHook.Value.Result)) then
                    printfn $"uFuzzy: Result: {fuseHook.Value.Result}"
                    // If the search is not empty show them
                    if fuseHook.Value.Result.Count > 1 then
                        fuseHook.Value.Result
                        |> (fun a -> if a |> Seq.length > 50 then a |> Seq.take(10) else a)
                        |> Seq.map (fun result ->
                                let ttt =
                                    attributes.Possibilities
                                    |> List.toArray

                                Bulma.dropdownItem.a [
                                    prop.text ttt.[int result]
                                    prop.onClick (fun _ ->
                                        setTerm ttt.[int result]
                                        setHasBeenSaved true
                                        dispatch (onChange ttt.[int result])
                                        setFocused false
                                    )
                                ]
                        )
                        |> Bulma.dropdownContent
                        |> dropdownMenu
                    // Otherwise, display the non filtered possibilities
                    else
                        attributes.Possibilities
                        |> (fun a -> if a |> Seq.length > 50 then a |> Seq.take(50) else a)
                        |> Seq.map (fun possibility ->
                                Bulma.dropdownItem.a [
                                    prop.text possibility
                                    prop.onClick (fun _ ->
                                        setTerm possibility
                                        setHasBeenSaved true
                                        dispatch (onChange possibility)
                                        setFocused false
                                    )
                                ]
                        )
                        |> Bulma.dropdownContent
                        |> dropdownMenu
                else
                    null

            Html.div [
                prop.className "form-antidote-autocomplete-field"
                prop.children [

                    Bulma.input.text [
                        prop.onChange setTerm
                        if not disabled then
                            prop.onFocus (fun _ ->
                                setFocused true
                            )

                            // Fable.Form internals
                            match onBlur with
                            | Some onBlur ->
                                prop.onBlur (fun _ ->
                                    dispatch onBlur
                                )

                            | None ->
                                ()

                        prop.disabled disabled
                        prop.value term
                        prop.placeholder attributes.Placeholder
                        if showError && error.IsSome then
                            color.isDanger
                    ]
                    dropdownElement
                ]
            ]
