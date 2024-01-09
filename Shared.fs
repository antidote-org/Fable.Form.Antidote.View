namespace Fable.Form.Antidote

open Feliz
open Feliz.Bulma
open Fable.Form
open Fable.Form.Antidote.Form.View

module Helpers =

    let fieldLabel (label : string) =
        Html.p [
            // prop.className "is-size-4"
            prop.text label
        ]

    let errorMessage (message : string) =
        Bulma.help [
            color.isDanger
            prop.text message
        ]

    let errorMessageAsHtml (showError : bool) (error : Error.Error option) =
        match error with
        | Some (Error.External externalError) ->
            errorMessage externalError

        | _ ->
            if showError then
                error
                |> Option.map errorToString
                |> Option.map errorMessage
                |> Option.defaultValue (Bulma.help [ ])

            else
                Bulma.help [ ]

    let wrapInFieldContainer (children : ReactElement list) =
        Bulma.field.div [
            prop.children children
        ]


    let withLabelAndError
        (label : string)
        (showError : bool)
        (error : Error.Error option)
        (fieldAsHtml : ReactElement)
        : ReactElement =
        [
            fieldLabel label
            Bulma.control.div [
                fieldAsHtml
            ]
            errorMessageAsHtml showError error
        ]
        |> wrapInFieldContainer
