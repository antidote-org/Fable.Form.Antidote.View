namespace Fable.Form.Antidote

open Feliz
open Feliz.Bulma
open Fable.Form.Antidote.Field
open Fable.Form.Antidote.Field.FlatRadioField
open Fable.Form.Antidote.Form.View

open Fable.Form.Antidote.Components

module FlatRadioField =
    let flatRadioField
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
            } : FlatRadioFieldConfig<'Msg>
        ) =

        let toOption (key : string, text : string) =
            let isChecked = key = selected

            if isChecked || not disabled then
                Bulma.control.p [
                    prop.key key
                    prop.className "flat-checkbox"

                    prop.onClick (fun _ -> onChange key |> dispatch)

                    control.hasIconsRight
                    prop.children [
                        Html.span [
                            prop.style [
                                style.height.unset
                                if disabled then
                                    style.cursor.notAllowed
                                    style.custom("disabled", "disabled")

                            ]
                            // prop.style ()
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
                                        prop.className "fas fa-dot-circle"
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
