namespace Fable.Form.Antidote

open Feliz
open Feliz.Bulma
open Fable.Form.Antidote.Field
open Fable.Form.Antidote.Field.RadioField
open Fable.Form.Antidote.Field.TextField
open Fable.Form.Antidote.Form.View

open Fable.Form.Antidote.Components

module RadioField =
    let radioField
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
            } : RadioFieldConfig<'Msg>
        ) =

        let radio (key : string, label : string) =
            Html.label [
                prop.className "radio"
                prop.children [
                    Html.input [
                        prop.type' "radio"
                        prop.name attributes.Label
                        prop.isChecked (key = value : bool)
                        prop.disabled disabled
                        prop.onChange (fun (_ : bool) -> onChange key |> dispatch)
                        match onBlur with
                        | Some onBlur ->
                            prop.onBlur (fun _ ->
                                dispatch onBlur
                            )

                        | None ->
                            ()
                    ]
                    Html.span []
                    Html.text label
                ]
            ]
            // Bulma.input.labels.radio [
            //     Bulma.input.radio [
            //         prop.name attributes.Label
            //         prop.isChecked (key = value : bool)
            //         prop.disabled disabled
            //         prop.onChange (fun (_ : bool) -> onChange key |> dispatch)
            //         match onBlur with
            //         | Some onBlur ->
            //             prop.onBlur (fun _ ->
            //                 dispatch onBlur
            //             )

            //         | None ->
            //             ()
            //     ]

            //     Html.text label
            // ]


        Bulma.control.div [
            attributes.Options
            |> List.map radio
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
