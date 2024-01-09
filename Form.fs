namespace Fable.Form.Antidote

open Fable.Form.Antidote.Field
open Fable.Form.Antidote.Field.RadioField
open Fable.Form.Antidote.Field.TextField
open Fable.Form.Antidote.Field.TwoChoiceField
open Fable.Form.Antidote.Field.FlatCheckboxField
open Fable.Form.Antidote.Field.StateSelectorField
open Fable.Form.Antidote.Form.View
open Fable.Form.Antidote.Components

[<RequireQualifiedAccess>]
module Form =

    module View =

        open Feliz
        open Feliz.Bulma
        open Fable.Form
        open Fable.Form.Antidote
        open Fable.Form.Antidote.Form.View

        let radioField = RadioField.radioField
        let checkboxListField = CheckboxListField.checkboxListField
        let twoChoiceField = TwoChoiceField.twoChoiceField
        let flatCheckboxField = FlatCheckboxField.flatCheckboxField
        let flatRadioField = FlatRadioField.flatRadioField
        let stateSelectorField = StateSelectorField.stateSelectorField
        let takePhotoOrGetFromLibrary = TakePhotoOrGetFromLibrary.takePhotoOrGetFromLibrary

        let inputField
            (typ : InputType)
            (
                {
                    Dispatch = dispatch
                    OnChange = onChange
                    OnBlur = onBlur
                    Disabled = disabled
                    Value = value
                    Error = error
                    ShowError = showError
                    Attributes = attributes
                } : TextFieldConfig<'Msg, IReactProperty>
            ) =

            let inputFunc =
                match typ with
                | Text ->
                    Bulma.input.text

                | Password ->
                    Bulma.input.password

                | Email ->
                    Bulma.input.email

                | Color ->
                    Bulma.input.color

                | Date ->
                    Bulma.input.date

                | DateTimeLocal ->
                    Bulma.input.datetimeLocal

                | Number ->
                    Bulma.input.number

                | Search ->
                    Bulma.input.search

                | Tel ->
                    Bulma.input.tel

                | Time ->
                    Bulma.input.time


            inputFunc [
                prop.onChange (onChange >> dispatch)

                match onBlur with
                | Some onBlur ->
                    prop.onBlur (fun _ ->
                        dispatch onBlur
                    )

                | None ->
                    ()

                prop.disabled disabled
                prop.value value
                prop.placeholder attributes.Placeholder
                if showError && error.IsSome then
                    color.isDanger

                yield! attributes.HtmlAttributes
            ]
            |> Helpers.withLabelAndError attributes.Label showError error

        let textareaField
            (
                {
                    Dispatch = dispatch
                    OnChange = onChange
                    OnBlur = onBlur
                    Disabled = disabled
                    Value = value
                    Error = error
                    ShowError = showError
                    Attributes = attributes
                } : TextFieldConfig<'Msg, IReactProperty>
            ) =

            Bulma.textarea [
                prop.onChange (onChange >> dispatch)

                match onBlur with
                | Some onBlur ->
                    prop.onBlur (fun _ ->
                        dispatch onBlur
                    )

                | None ->
                    ()

                prop.disabled disabled
                prop.value value
                prop.placeholder attributes.Placeholder
                if showError && error.IsSome then
                    color.isDanger

                yield! attributes.HtmlAttributes
            ]
            |> Helpers.withLabelAndError attributes.Label showError error

        let checkboxField
            (
                {
                    Dispatch = dispatch
                    OnChange = onChange
                    OnBlur = onBlur
                    Disabled = disabled
                    Value = value
                    Attributes = attributes
                } : CheckboxFieldConfig<'Msg>
            ) =

            Bulma.control.div [
                //TODO: This field breaks inside huro. We're overriding it *with a huro checkbox* until real solution is found.

                //HURO CHECKBOX
                Html.label [
                    prop.classes [ "checkbox"; "is-outlined"; "is-success" ]
                    prop.children [
                        Html.input [
                            prop.onChange (onChange >> dispatch)
                            prop.type' "checkbox"
                            prop.disabled disabled
                            prop.isChecked value
                            match onBlur with
                            | Some onBlur ->
                                prop.onBlur (fun _ ->
                                    dispatch onBlur
                                )

                            | None ->
                                ()
                        ]
                        Html.span []
                        Html.text attributes.Text
                    ]
                ]

                //BULMA CHECKBOX (broken inside huro's framework)
                // Bulma.input.labels.checkbox [
                //     prop.children [
                //         Bulma.input.checkbox [
                //             prop.onChange (onChange >> dispatch)
                //             match onBlur with
                //             | Some onBlur ->
                //                 prop.onBlur (fun _ ->
                //                     dispatch onBlur
                //                 )

                //             | None ->
                //                 ()
                //             prop.disabled disabled
                //             prop.isChecked value
                //         ]

                //         Html.text attributes.Text
                //     ]
                // ]
            ]
            |> (fun x -> [ x ])
            |> Helpers.wrapInFieldContainer

        let switchField
            (
                {
                    Dispatch = dispatch
                    OnChange = onChange
                    OnBlur = onBlur
                    Disabled = disabled
                    Value = value
                    Error = error
                    ShowError = showError
                    Attributes = attributes
                } : SwitchFieldConfig<'Msg>
            ) =

            let switchElement =
                Bulma.control.div [
                    Html.div [
                        prop.className "field"
                        prop.children [
                            Html.input [
                                prop.id attributes.Id
                                prop.type' "checkbox"
                                prop.onChange (onChange >> dispatch)
                                match onBlur with
                                | Some onBlur ->
                                    prop.onBlur (fun _ ->
                                        dispatch onBlur
                                    )

                                | None ->
                                    ()
                                prop.className "switch"
                                prop.isChecked value
                            ]
                            Html.label [
                                prop.htmlFor attributes.Id
                                prop.text attributes.Text
                            ]
                        ]
                    ]
                ]

            [
                switchElement
                Helpers.errorMessageAsHtml showError error
            ]
            |> Helpers.wrapInFieldContainer



        let selectField
            (
                {
                    Dispatch = dispatch
                    OnChange = onChange
                    OnBlur = onBlur
                    Disabled = disabled
                    Value = value
                    Error = error
                    ShowError = showError
                    Attributes = attributes
                } : SelectFieldConfig<'Msg, IReactProperty>
            ) =

            let toOption (key : string, label : string) =
                Html.option [
                    prop.value key
                    prop.text label
                ]

            let placeholderOption =
                match attributes.Placeholder with
                | Some placeholder ->
                    Html.option [
                        prop.disabled true
                        prop.value ""

                        prop.text ("- " + placeholder + " -")
                    ]
                | None ->
                    null

            Bulma.select [
                prop.disabled disabled
                prop.onChange (onChange >> dispatch)

                match onBlur with
                | Some onBlur ->
                    prop.onBlur (fun _ ->
                        dispatch onBlur
                    )

                | None ->
                    ()

                prop.value value

                yield! attributes.HtmlAttributes

                prop.children [
                    placeholderOption

                    yield! attributes.Options
                    |> List.map toOption
                ]
            ]
            |> Helpers.withLabelAndError attributes.Label showError error

        // let flatCheckboxField
        //     (
        //         {
        //             Dispatch = dispatch
        //             OnChange = onChange
        //             OnBlur = onBlur
        //             Disabled = disabled
        //             Value = selected
        //             Error = error
        //             ShowError = showError
        //             Attributes = attributes
        //         } : FlatCheckboxFieldConfig<'Msg>
        //     ) =

        //     let toOption (key : string, text : string) =
        //         let isChecked =
        //             Set.contains key selected

        //         Bulma.control.p [
        //             prop.key key
        //             prop.className "flat-checkbox mb-3"

        //             prop.onClick (fun _ ->
        //                 // Compute the new state
        //                 let newSelected =
        //                     if isChecked then
        //                         Set.remove key selected
        //                     else
        //                         Set.add key selected

        //                 // Save the new state
        //                 dispatch (onChange newSelected)
        //             )

        //             control.hasIconsRight

        //             prop.children [
        //                 Html.span [
        //                     prop.classes [
        //                         "input" // Not a real input element but this gives a coherent look
        //                         if isChecked then "is-checked"
        //                     ]
        //                     prop.text text
        //                 ]

        //                 Bulma.icon [
        //                     icon.isRight
        //                     color.isSuccess

        //                     prop.children [
        //                         if isChecked then
        //                             Html.i [
        //                                 prop.className "fa fa-check has-text-success"
        //                             ]
        //                     ]
        //                 ]
        //             ]
        //         ]

        //     React.fragment [
        //         yield! attributes.Options
        //         |> List.map toOption
        //     ]
        //     |> Helpers.withLabelAndError attributes.Label showError error

        let reactComponentField
            (
                {
                    Dispatch = dispatch
                    OnChange = onChange
                    OnBlur = _
                    Disabled = disabled
                    Value = value
                    Error = error
                    ShowError = showError
                    Attributes = attributes
                } : ReactComponentFieldConfig<'Msg>
            ) =

            attributes.Render
                {
                    Value = value
                    OnChange = onChange >> dispatch
                    Disabled = disabled
                }
            |> Helpers.withLabelAndError attributes.Label showError error

        let textAutocompleteField
            (
                {
                    Dispatch = dispatch
                    OnChange = onChange
                    OnBlur = onBlur
                    Disabled = disabled
                    Value = value
                    Error = error
                    ShowError = showError
                    Attributes = attributes
                } : TextAutocompleteFieldConfig<'Msg> as config
            ) =

            TextAutocompleteFieldComponent
                {|
                    config = config
                |}
            |> Helpers.withLabelAndError attributes.Label showError error

        let tagListField
            (
                {
                    Dispatch = dispatch
                    OnChange = onChange
                    OnBlur = _
                    Disabled = _
                    Value = selected
                    Error = error
                    ShowError = showError
                    Attributes = attributes
                } : TagListFieldConfig<'Msg>
            ) =

            let toOption (key : string, text : string) =
                let isChecked =
                    Set.contains key selected

                Bulma.tag [
                    tag.isRounded
                    prop.style [
                        style.cursor.pointer
                    ]
                    prop.key key

                    if isChecked then
                        color.isPrimary

                    prop.onClick (fun _ ->
                        // Compute the new state
                        let newSelected =
                            if isChecked then
                                Set.remove key selected
                            else
                                Set.add key selected

                        // Save the new state
                        dispatch (onChange newSelected)
                    )

                    prop.text text
                ]

            Bulma.tags [
                yield! attributes.Options
                |> List.map toOption
            ]
            |> Helpers.withLabelAndError attributes.Label showError error

        let group (fields : ReactElement list) =
            Bulma.field.div [
                Bulma.columns [
                    columns.isMobile

                    fields
                    |> List.map Bulma.column
                    |> prop.children
                ]
            ]

        let section (title : string) (fields : ReactElement list) =
            Html.fieldSet [
                prop.className "fieldset"

                prop.children [
                    Html.legend [
                        prop.text title
                    ]

                    yield! fields
                ]
            ]

        let ignoreChildError
            (parentError : Error.Error option)
            (field : Form.FilledField<'Values, IReactProperty>)
            : Form.FilledField<'Values, IReactProperty> =

            match parentError with
            | Some _ ->
                field

            | None ->
                { field with Error = None }

        let formList
            (
                {
                    Dispatch = dispatch
                    Forms = forms
                    Label = label
                    Add = add
                    Disabled = disabled
                } : FormListConfig<'Msg>
            ) =

            let addButton =
                match disabled, add with
                | false, Some add ->
                    Html.a [
                        prop.classes ["button"; "is-small"; "is-primary" ]
                        prop.onClick (fun _ ->
                            add.Action() |> dispatch
                        )
                        prop.children [
                            Html.span [
                                prop.classes [ "icon" ]
                                prop.children [
                                    Html.i [
                                        prop.classes [ "fas"; "fa-plus" ]
                                    ]
                                ]
                            ]
                            Html.span add.Label
                        ]
                    ]
                    // Bulma.button.a [
                    //     prop.className "h-button is-primary"
                    //     prop.onClick (fun _ ->
                    //         add.Action() |> dispatch
                    //     )

                    //     prop.children [
                    //         Bulma.icon [
                    //             icon.isSmall

                    //             prop.children [
                    //                 Html.i [
                    //                     prop.className "fas fa-plus"
                    //                 ]
                    //             ]
                    //         ]

                    //         Html.p [ prop.text add.Label ]
                    //     ]
                    // ]

                | _ ->
                    Html.none

            Bulma.field.div [
                Bulma.control.div [
                    Helpers.fieldLabel label

                    yield! forms

                    addButton
                ]
            ]

        let formListItem
            (
                {
                    Dispatch = dispatch
                    Fields = fields
                    Delete = delete
                    Disabled = disabled
                } : FormListItemConfig<'Msg>
            ) =

            let removeButton =
                match disabled, delete with
                | false, Some delete ->
                    Html.a [
                        prop.classes ["button"; "is-small"; "is-danger" ]
                        prop.onClick (fun _ ->
                            delete.Action() |> dispatch
                        )
                        prop.children [
                            Html.span [
                                prop.classes [ "icon" ]
                                prop.children [
                                    Html.i [
                                        prop.classes [ "fas"; "fa-times" ]
                                    ]
                                ]
                            ]
                            Html.span delete.Label
                        ]
                    ]
                    // Bulma.button.a [
                    //     prop.className "h-button is-secondary"
                    //     prop.onClick (fun _ ->
                    //         delete.Action() |> dispatch
                    //     )

                    //     prop.children [
                    //         Bulma.icon [
                    //             icon.isSmall

                    //             prop.children [
                    //                 Html.i [
                    //                     prop.className "fas fa-times"
                    //                 ]
                    //             ]
                    //         ]

                    //         if delete.Label <> "" then
                    //             Html.span delete.Label
                    //     ]
                    // ]

                | _ ->
                    Html.none

            Html.div [
                prop.className "form-list"

                prop.children [
                    yield! fields

                    Bulma.field.div [
                        // field.isGrouped
                        // field.isGroupedRight

                        prop.children [
                            Bulma.control.div [
                                removeButton
                            ]
                        ]
                    ]
                ]
            ]

        let form
            (
                {
                    Dispatch = dispatch
                    OnSubmit = onSubmit
                    State = state
                    Action = action
                    Fields = fields
                } : FormConfig<'Msg>
            ) =

            Html.form [
                prop.onSubmit (fun ev ->
                    ev.stopPropagation()
                    ev.preventDefault()

                    onSubmit
                    |> Option.map dispatch
                    |> Option.defaultWith ignore
                )

                prop.children [
                    yield! fields

                    match state with
                    | Error error ->
                        Helpers.errorMessage error

                    | Success success ->
                        Bulma.field.div [
                            Bulma.control.div [
                                text.hasTextCentered
                                color.hasTextSuccess
                                text.hasTextWeightBold

                                prop.text success
                            ]
                        ]

                    | Loading
                    | Idle ->
                        Html.none

                    match action with
                    | Action.SubmitOnly submitLabel ->
                        Bulma.field.div [
                            field.isGrouped
                            field.isGroupedRight

                            prop.children [
                                Bulma.control.div [
                                    Bulma.button.button [
                                        prop.type'.submit
                                        color.isPrimary
                                        prop.text submitLabel
                                        // If the form is loading animate the submit button with the loading animation
                                        if state = Loading then
                                            button.isLoading
                                    ]
                                ]

                            ]
                        ]

                    | Action.Custom func ->
                        func state dispatch
                ]
            ]

        let htmlViewConfig<'Msg> : CustomConfig<'Msg, IReactProperty> =
            {
                Form = form
                TextField = inputField Text
                PasswordField = inputField Password
                EmailField = inputField Email
                TextAreaField = textareaField
                ColorField = inputField Color
                DateField = inputField  Date
                DateTimeLocalField = inputField DateTimeLocal
                NumberField = inputField Number
                SearchField = inputField Search
                TelField = inputField Tel
                TimeField = inputField Time
                CheckboxField = checkboxField
                CheckboxListField = checkboxListField
                SwitchField = switchField
                RadioField = radioField
                TwoChoiceField = twoChoiceField
                SelectField = selectField
                FlatCheckboxField = flatCheckboxField
                FlatRadioField = flatRadioField
                StateSelectorField = stateSelectorField
                ReactComponentField = reactComponentField
                TextAutocompleteField = textAutocompleteField
                TagListField = tagListField
                TakePhotoOrGetFromLibraryField = takePhotoOrGetFromLibrary
                Group = group
                Section = section
                FormList = formList
                FormListItem = formListItem
            }

        let asHtml (config : ViewConfig<'Values, 'Msg>) =
            custom htmlViewConfig config
