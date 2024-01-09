namespace Fable.Form.Antidote

open Feliz
open Feliz.Bulma
open Fable.Form.Antidote.Form.View

module StateSelectorComponent =

    [<ReactComponent>]
    let StateSelectorComponent() =

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
            |> Map.ofList


        let disabled, setIsDisabled = React.useState false
        let isActive, setIsActive = React.useState false

        let toOption (key : string, text : string) =
            Bulma.dropdownItem.a [
                prop.key key
                prop.text text
            ]

                // Bulma.button.a [

                //     prop.key key
                //     prop.style [
                //         style.custom("minWidth", "50%")
                //         if isChecked then
                //             style.backgroundColor "#48a5c3"
                //             style.color "#fff"
                //     ]

                //     prop.onClick (fun _ -> onChange key |> dispatch)

                //     prop.children [
                //         Html.span [
                //             prop.text text
                //         ]
                //     ]
                // ]
            // else if disabled && isChecked then
            //     Html.span text
            // else
            //     Html.none

        React.fragment [
            Bulma.fieldBody [
                prop.children [
                    Html.div [
                        Bulma.dropdown [
                            if isActive then dropdown.isActive
                            prop.children [
                                Bulma.dropdownTrigger [
                                    Bulma.button.a [
                                        button.isSmall
                                        prop.onClick (fun _ -> setIsActive (not isActive))
                                        // prop.onBlur (fun _ -> setIsTypeDropdownActive false)
                                        prop.children [
                                            Html.span "Select One"
                                            // Icon [
                                            //     icon.icon mdi.chevronDown
                                            //     // icon.width 30
                                            // ]
                                        ]
                                    ]
                                ]
                                Bulma.dropdownMenu [
                                    Bulma.dropdownContent [
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
            ]
        ]
