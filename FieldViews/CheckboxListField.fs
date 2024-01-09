namespace Fable.Form.Antidote

open Feliz
open Feliz.Bulma
open Fable.Form.Antidote.Field
open Fable.Form.Antidote.Field.CheckboxListField
open Fable.Form.Antidote.Form.View

open Fable.Form.Antidote.Components

module CheckboxListField =

    let checkboxListField
        (
            {
                Dispatch = dispatch
                OnChange = onChange
                OnBlur = onBlur
                Disabled = disabled
                Value = selected
                Error = error
                ShowError = showError
                Attributes = attributes
            } : CheckboxListFieldConfig<'Msg>
        ) =

        let toOption (key : string, text : string) =

            let isChecked = selected.Contains key

            if not disabled then
                //TODO: This field breaks inside huro. We're overriding it *with a huro checkbox* until real solution is found.

                //HURO CHECKBOX
                Html.div [
                    prop.className "control"
                    prop.children [
                        Html.label [
                            prop.classes [ "checkbox"; "is-outlined"; "is-success" ]
                            prop.children [
                                Html.input [
                                    prop.isChecked isChecked
                                    prop.disabled disabled
                                    prop.type' "checkbox"
                                ]
                                Html.span []
                                Html.text text
                            ]
                            prop.onChange (fun (_ :bool) ->
                                // Compute the new state
                                let newSelected =
                                    if isChecked then
                                        Set.remove key selected
                                    else
                                        Set.add key selected

                                // Save the new state
                                dispatch (onChange newSelected)
                            )
                            match onBlur with
                            | Some onBlur ->
                                prop.onBlur (fun _ ->
                                    dispatch onBlur
                                )

                            | None ->
                                ()
                        ]
                    ]
                ]

                //BULMA CHECKBOX (broken inside huro's framework)

                // Bulma.input.labels.radio [
                //     Bulma.input.checkbox [
                //         prop.name attributes.Label
                //         prop.isChecked isChecked
                //         prop.disabled disabled
                //         // prop.onChange (fun (_ : bool) -> onChange key |> dispatch)
                //         prop.onChange (fun (_ :bool) ->
                //             // Compute the new state
                //             let newSelected =
                //                 if isChecked then
                //                     Set.remove key selected
                //                 else
                //                     Set.add key selected

                //             // Save the new state
                //             dispatch (onChange newSelected)
                //         )
                //         match onBlur with
                //         | Some onBlur ->
                //             prop.onBlur (fun _ ->
                //                 dispatch onBlur
                //             )

                //         | None ->
                //             ()
                //     ]

                //     Html.text text
                // ]
            else if disabled && isChecked then
                Bulma.control.p [
                    prop.key key
                    prop.className "flat-checkbox"

                    control.hasIconsRight

                    prop.children [
                        Html.span [
                            prop.style [
                                style.height.unset
                            ]
                            prop.classes [
                                "input" // Not a real input element but this gives a coherent look
                                "is-checked"
                            ]
                            prop.text text
                        ]

                        Bulma.icon [
                            icon.isRight
                            color.isSuccess

                            prop.children [
                                Html.i [
                                    prop.className "fa fa-check"
                                ]
                            ]
                        ]
                    ]
                ]
            else
                Html.none

        // React.fragment [
        //     yield! attributes.Options
        //     |> List.map toOption
        // ]
        // |> Helpers.withLabelAndError attributes.Label showError error
        Bulma.control.div [
            attributes.Options
            |> List.map toOption
            |> List.map (fun x ->
                match attributes.Layout with
                | Layout.Horizontal ->
                    x
                | Layout.Vertical ->
                    Html.div [
                        prop.className "field"
                        prop.children [
                            x
                        ]
                    ]
            )
            |> prop.children
        ]
        |> Helpers.withLabelAndError attributes.Label showError error
