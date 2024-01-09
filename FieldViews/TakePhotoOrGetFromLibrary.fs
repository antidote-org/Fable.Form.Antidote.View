namespace Fable.Form.Antidote

open Feliz
open Feliz.Bulma
open Fable.Form.Antidote.Field
open Fable.Form.Antidote.Field.FlatRadioField
open Fable.Form.Antidote.Form.View
// open Glutinum.Capacitor.Camera
// open type Glutinum.Capacitor.Camera.Exports
open System
open Browser
open Fable.Core

open Fable.Form.Antidote.Components

module TakePhotoOrGetFromLibrary =

    let private renderFileButton (label : string) (iconClasses: string) onClick =
        Html.div [
            prop.className "file is-centered"
            prop.children [
                Html.label [
                    prop.className "file-label"
                    prop.children [
                        Html.input [
                            prop.className "file-input"
                            prop.type' "file"
                            prop.accept "image/*"
                            prop.onClick onClick
                        ]
                        Html.span [
                            prop.className "file-cta"
                            prop.children [
                                Html.span [
                                    prop.className "file-icon"
                                    prop.children [
                                        Html.i [
                                            prop.className iconClasses
                                        ]
                                    ]
                                ]
                                Html.span [
                                    prop.className "file-label"
                                    prop.text label
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    let private compressOnLoad dispatch onChange (imageTemp:Types.HTMLImageElement)  =
        fun ev ->
            let maxWidth = 720.0
            let maxHeight = 720.0
            let scale =
                let widthScale = maxWidth / float imageTemp.width
                let heightScale = maxHeight / float imageTemp.height
                min 1.0 (min widthScale heightScale)

            let canvas = document.createElement("canvas") :?> Types.HTMLCanvasElement
            canvas.width <- int (scale * float imageTemp.width)
            canvas.height <- int (scale * float imageTemp.height)
            // Draw the image onto the canvas
            let ctx = canvas.getContext("2d") :?> Types.CanvasRenderingContext2D
            ctx.drawImage(U3.Case1 imageTemp, 0, 0, canvas.width, canvas.height);
            let compressedImage = canvas.toDataURL("image/jpeg")

            onChange (compressedImage) |> dispatch

    // let private renderTakePhoto onChange dispatch =

    //     let onClick (ev : Browser.Types.MouseEvent) =
    //         ev.preventDefault()

    //         promise {
    //             let! image =
    //                 Camera.getPhoto(
    //                     ImageOptions(
    //                         quality = 90,
    //                         allowEditing = true,
    //                         resultType = CameraResultType.Base64
    //                     )
    //                 )

    //             // By security, we check if the base64String is provided
    //             if image.base64String.IsSome then
    //                 let imageTemp = document.createElement("img") :?> Browser.Types.HTMLImageElement

    //                 // Ensure the canvas size is proportional and doesn't exceed the maximum dimensions.
    //                 imageTemp.src <- "data:image/jpeg;base64," + image.base64String.Value
    //                 imageTemp.onload <- compressOnLoad dispatch onChange imageTemp

    //         }
    //         |> Promise.start

    //     renderFileButton "Take a photo" "fas fa-camera" onClick

    // let private renderPickAnImage onChange dispatch =

    //     let onClick (ev : Browser.Types.MouseEvent) =
    //         ev.preventDefault()

    //         promise {
    //             let! images =
    //                 Camera.pickImages(
    //                     GalleryImageOptions(
    //                         quality = 90,
    //                         limit = 1
    //                     )
    //                 )

    //             if images.photos.Count = 1 then

    //                 let imageDataUrl = images.photos[0].webPath

    //                 let! res = Fetch.fetch imageDataUrl []
    //                 let! blob = res.blob()

    //                 let fileReader = FileReader.Create()

    //                 fileReader.onload <- fun ev ->
    //                     let base64data = fileReader.result |> unbox<string>
    //                     let base64String = base64data.Substring(base64data.IndexOf(',') + 1)

    //                     let imageTemp = document.createElement("img") :?> Browser.Types.HTMLImageElement
    //                     imageTemp.src <- "data:image/jpeg;base64," + base64String

    //                     imageTemp.onload <- compressOnLoad dispatch onChange imageTemp

    //                 fileReader.readAsDataURL(blob)
    //         }
    //         |> Promise.start

    //     renderFileButton "Choose an image" "fas fa-image" onClick

    let private renderPreview (base64String : string) onChange dispatch =
        let dataImageSrc =  base64String

        Html.div [
            prop.style [
                style.maxWidth (length.px 400)
                style.margin.auto
                style.position.relative
            ]
            prop.children [
                Html.img [
                    prop.src dataImageSrc
                ]
                Bulma.delete [
                    delete.isMedium
                    prop.style [
                        style.position.absolute
                        style.top (length.px -12)
                        style.right (length.px -12)
                        style.backgroundColor (color.rgb(223, 84, 107))
                    ]
                    prop.onClick (fun ev ->
                        ev.preventDefault()
                        onChange "" |> dispatch
                    )
                    color.isDanger
                ]
            ]
        ]

    let takePhotoOrGetFromLibrary
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
            } : TakePhotoOrGetFromLibraryFieldConfig<'Msg>
        ) =

        React.fragment [
            Bulma.columns [
                prop.children [
                    Bulma.column [
                        prop.children [
                            // renderTakePhoto onChange dispatch
                            Html.span "Implement Camera!!"
                        ]
                    ]
                    Bulma.column [
                        prop.children [
                        //    renderPickAnImage onChange dispatch
                            Html.span "Implement Camera!!"

                        ]
                    ]
                ]
            ]

            // If a file is selected, we display a preview
            if not(String.IsNullOrEmpty selected) then
                renderPreview selected onChange dispatch
        ]
        |> Helpers.withLabelAndError attributes.Label showError error
