# Fable.Form.Antidote.View
Github Repo [link](https://github.com/antidote-org/Fable.Form.Antidote)

## Overview


Extension "views" for new (opinionated) Fable.Forms fields outside of the base HTML form fields. These fields were created for Antidote.FormStudio, but can be used to extend Fable.Forms in a stand-alone fashion. Also note, that this repo adds the "form field views" and require the "extended fable forms functionality" in the separate nuget/repo.


## Installation

```
## using nuget
dotnet add package Fable.Form.Antidote
dotnet add package Fable.Form.Antidote.View
```

## or with paket

```
paket add Fable.Form.Antidote --project /path/to/project.fsproj
paket add Fable.Form.Antidote.View --project /path/to/project.fsproj
```


## To publish

*For maintainers only*

```ps1
cd Fable.Form.Antidote
dotnet pack -c Release
dotnet nuget push .\bin\Release\Fable.Form.Antidote.View.X.X.X.snupkg -s nuget.org -k <nuget_key>
dotnet nuget push .\bin\Release\Fable.Form.Antidote.View.X.X.X.nupkg -s nuget.org -k <nuget_key>
```
