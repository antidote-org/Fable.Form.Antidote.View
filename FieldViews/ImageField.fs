namespace Fable.Form.Antidote

open Feliz
open Feliz.Bulma
open Fable.Form.Antidote.Form.View

module ImageField =

    let twoChoiceField
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
            } : TwoChoiceFieldConfig<'Msg>
        ) =

        let toOption (key : string, text : string) =
            let isChecked = key = selected

            if not disabled then
                Bulma.button.a [

                    prop.key key
                    prop.style [
                        style.custom("minWidth", "50%")
                        if isChecked then
                            style.backgroundColor "#48a5c3"
                            style.color "#fff"
                    ]

                    prop.onClick (fun _ -> onChange key |> dispatch)

                    prop.children [
                        Html.span [
                            prop.text text
                        ]
                    ]
                ]
            else if disabled && isChecked then
                Html.span text
            else
                Html.none

        React.fragment [
            Bulma.buttons[
                buttons.hasAddons
                buttons.isCentered

                [ attributes.Options1; attributes.Options2 ]
                |> List.map toOption
                |> prop.children
            ]
        ]
        |> Helpers.withLabelAndError attributes.Label showError error
