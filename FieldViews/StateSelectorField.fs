namespace Fable.Form.Antidote

open Feliz
open Feliz.Bulma
open Fable.Form.Antidote.Form.View



module StateSelectorField =

    let usStatesMap =
        [
            ("AL", "Alabama")
            ("AK", "Alaska")
            ("AZ", "Arizona")
            ("AR", "Arkansas")
            ("CA", "California")
            ("CO", "Colorado")
            ("CT", "Connecticut")
            ("DE", "Delaware")
            ("FL", "Florida")
            ("GA", "Georgia")
            ("HI", "Hawaii")
            ("ID", "Idaho")
            ("IL", "Illinois")
            ("IN", "Indiana")
            ("IA", "Iowa")
            ("KS", "Kansas")
            ("KY", "Kentucky")
            ("LA", "Louisiana")
            ("ME", "Maine")
            ("MD", "Maryland")
            ("MA", "Massachusetts")
            ("MI", "Michigan")
            ("MN", "Minnesota")
            ("MS", "Mississippi")
            ("MO", "Missouri")
            ("MT", "Montana")
            ("NE", "Nebraska")
            ("NV", "Nevada")
            ("NH", "New Hampshire")
            ("NJ", "New Jersey")
            ("NM", "New Mexico")
            ("NY", "New York")
            ("NC", "North Carolina")
            ("ND", "North Dakota")
            ("OH", "Ohio")
            ("OK", "Oklahoma")
            ("OR", "Oregon")
            ("PA", "Pennsylvania")
            ("RI", "Rhode Island")
            ("SC", "South Carolina")
            ("SD", "South Dakota")
            ("TN", "Tennessee")
            ("TX", "Texas")
            ("UT", "Utah")
            ("VT", "Vermont")
            ("VA", "Virginia")
            ("WA", "Washington")
            ("WV", "West Virginia")
            ("WI", "Wisconsin")
            ("WY", "Wyoming")
        ]
        |> List.sortBy (fun (key, value) -> value )
        |> Map.ofList

    [<ReactComponent>]
    let stateSelectorField
        (
            {
                Dispatch = dispatch
                OnChange = onChange
                OnBlur = onBlur
                Disabled = disabled
                Value = selectedKey
                Error = error
                ShowError = showError
                Attributes = attributes
            } : StateSelectorConfig<'Msg>
        ) =

        let isActive, setIsActive = React.useState false
        // let stateSelected, setStateSelected = React.useState selected
        // let selectedStateLabel, setSelectedStateLabel = React.useState None

        let labelForSelectedOpt = usStatesMap |> Map.tryFind selectedKey

        let toOption (option:(string * string) ) =
            let optionKey = fst option
            let optionLabel = snd option
            let isChecked = optionKey = selectedKey

            Html.a [
                prop.onClick (fun _ ->
                    setIsActive false
                    onChange optionKey |> dispatch
                )
                prop.style [
                    style.cursor.pointer
                ]
                prop.className "dropdown-item"
                prop.children [
                    Html.span [
                        prop.className "icon-text"
                        prop.children [
                            Html.span [
                                prop.text ($"{optionLabel}")
                            ]
                            if isChecked
                            then
                                Html.span [
                                    prop.className "icon"
                                    prop.children [
                                        Html.i [
                                            prop.className "fas fa-check"
                                        ]
                                    ]
                                ]
                        ]
                    ]
                ]
            ]

        let dropDown =
            Html.div [
                prop.classes [
                    "dropdown"
                    if isActive then "is-active" else ""
                ]
                prop.children [
                    Html.div [
                        prop.className "dropdown-trigger"
                        prop.children [
                            Html.a [
                                prop.onClick (fun _ -> setIsActive (not isActive))
                                prop.ariaControls "dropdown-menu"
                                prop.className "button"
                                prop.children [
                                    Html.span [
                                        prop.classes [ "icon"; "is-small" ]
                                        prop.children [
                                            match labelForSelectedOpt with
                                            | Some selectedLabel ->
                                                Bulma.icon [
                                                    icon.isSmall
                                                    prop.children [
                                                        Html.img [
                                                            prop.style [
                                                                style.maxWidth 24
                                                                style.maxHeight 24
                                                            ]
                                                            prop.src $"/images/usa-states/svg/{selectedLabel} - {selectedKey}.svg"
                                                        ]
                                                    ]

                                                ]
                                            | None ->
                                                Html.i [
                                                    prop.classes [ "fas"; "fa-map" ]
                                                ]
                                        ]
                                    ]
                                    Html.span
                                        (
                                            match labelForSelectedOpt with
                                            | Some label -> label
                                            | None -> "Select Your State"
                                        )
                                    Html.span [
                                        prop.classes [ "icon"; "is-small" ]
                                        prop.children [
                                            Html.i [
                                                prop.classes [ "fas"; "fa-angle-down" ]
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                    // Html.div [
                    //     prop.style [
                    //         style.maxWidth 60
                    //     ]
                    //     prop.children [
                    //         if stateSelected <> "" then
                    //             Html.img [
                    //                 prop.src $"images/usa-states/{stateSelected}.svg"
                    //                 // prop.alt "Placeholder image"
                    //             ]
                    //     ]
                    // ]
                    Html.div [
                        prop.className "dropdown-menu"
                        prop.id "dropdown-menu"
                        prop.role "menu"
                        prop.children [
                            Html.div [
                                prop.className "dropdown-content"
                                prop.style [
                                    style.maxHeight 200
                                    style.overflow.auto
                                ]
                                prop.children [
                                    usStatesMap
                                    |> Map.toList
                                    |> List.map(fun kv -> toOption kv)
                                    |> React.fragment
                                ]
                            ]
                        ]
                    ]
                ]
            ]

        if disabled
        then
            let label = usStatesMap[selectedKey]
            Html.span [
                prop.className "icon-text"
                prop.children [
                    // Html.span [
                    //     prop.className "icon"
                    //     prop.children [
                    //         Html.img [
                    //             prop.src $"images/usa-states/svg/{stateLabelAndKey}.svg"
                    //         ]
                    //     ]
                    // ]
                    Html.span [
                        prop.text ($"{label} - {selectedKey}")
                    ]
                ]
            ]
        else
            dropDown

        |> Helpers.withLabelAndError attributes.Label showError error





// namespace Fable.Form.Antidote

// open Feliz
// open Feliz.Bulma
// open Fable.Form.Antidote.Form.View
// open Fable.Form.Antidote.StateSelectorComponent


// module StateSelectorField =

//     let stateSelectorField
//         (
//             {
//                 Dispatch = dispatch
//                 OnChange = onChange
//                 OnBlur = onBlur
//                 Disabled = disabled
//                 Value = selected
//                 Error = error
//                 ShowError = showError
//                 Attributes = attributes
//             } : StateSelectorConfig<'Msg>
//         ) =

//         Html.span "TEST"
//         |> Helpers.withLabelAndError attributes.Label showError error
