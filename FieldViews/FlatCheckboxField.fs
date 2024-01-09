namespace Fable.Form.Antidote

open Feliz
open Feliz.Bulma
open Fable.Form.Antidote.Field
open Fable.Form.Antidote.Field.FlatCheckboxField
open Fable.Form.Antidote.Form.View

open Fable.Form.Antidote.Components

module FlatCheckboxField =

    let flatCheckboxField
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
            } : FlatCheckboxFieldConfig<'Msg>
        ) =

        let toOption (key : string, text : string) =

            let isChecked = selected.Contains key

            if not disabled then
                Bulma.control.p [
                    prop.key key
                    prop.className "flat-checkbox"

                    if not disabled
                    then
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

                    control.hasIconsRight

                    prop.children [
                        Html.span [
                            prop.style [
                                style.height.unset
                            ]
                            prop.classes [
                                "input" // Not a real input element but this gives a coherent look
                                if isChecked then "is-checked"
                            ]
                            prop.text text
                        ]

                        Bulma.icon [
                            icon.isRight
                            color.isSuccess

                            prop.children [
                                if isChecked then
                                    Html.i [
                                        prop.className "fa fa-check"
                                    ]
                            ]
                        ]
                    ]
                ]
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

        React.fragment [
            yield! attributes.Options
            |> List.map toOption
        ]
        |> Helpers.withLabelAndError attributes.Label showError error
