namespace Fable.Form.Antidote

open Feliz
open Feliz.Bulma
open Fable.Form.Antidote.Field
open Fable.Form.Antidote.Field.RadioField
open Fable.Form.Antidote.Field.TextField
open Fable.Form.Antidote.Form.View

open Fable.Form.Antidote.Components

module RadioTableField =
    let radioTableField
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

        let toRow (key : string, label : string) =
            Html.tableRow [
                prop.onClick (fun _ -> onChange key |> dispatch)
                prop.children [
                    Html.td [
                        if (key = value : bool) then color.isInfo
                        prop.children [
                            Html.label [
                                prop.className "radio"
                                prop.children [
                                    Html.input [
                                        prop.name attributes.Label
                                        prop.type' "radio"
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
                        ]
                    ]
                ]
            ]

        Bulma.table [
            table.isBordered
            table.isFullWidth
            prop.children [
                // Html.thead [
                //     Html.tableRow [
                //         Html.th "Test"
                //     ]
                // ]
                Html.tbody [
                    prop.children [
                        yield! attributes.Options
                        |> List.map toRow
                    ]
                ]
            ]
        ] |> Helpers.withLabelAndError attributes.Label showError error
        // Bulma.control.div [
        //     attributes.Options
        //     |> List.map radio
        //     |> prop.children
        // ]
        // |> Helpers.withLabelAndError attributes.Label showError error
