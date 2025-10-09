# Inspector Crawford and the Hallway Murder
A murder mystery investigation game created for **Ludum Dare 58** game jam.

![Screenshot](ScreenShots/Screenshot%202025-10-06%20145736.png)

## About

Investigate a murder mystery by jumping between portrait frames. As Inspector Crawford, examine Lord Blackwood's death from multiple perspectives—each frame shows a different angle of the crime scene and a different witness to interrogate.

![Investigation Scene](ScreenShots/Screenshot%202025-10-06%20150921.png)

Click between living portraits to interview suspects: the nervous housekeeper, the family doctor, grieving relatives. Each conversation reveals clues about what happened that night. Switch perspectives to examine the scene from different angles and piece together the truth.

![Clues](ScreenShots/Screenshot%202025-10-06%20150928.png)

Once you've gathered enough evidence, make your final accusation: Who killed Lord Blackwood? What was the murder weapon? And why did they do it?

![Final Deduction](ScreenShots/Screenshot%202025-10-06%20150934.png)

A short detective game about shifting perspectives, literally.

![Dialogs](ScreenShots/Screenshot%202025-10-06%20150437.png)

## Features

- **Point-and-click navigation** between portrait frames
- **Character dialog system** with multiple witnesses
- **Clue collection and tracking** system
- **Final deduction system** (Who/What/Why)
- **Multiple perspective views** of the crime scene

## Technical Details

Built with **Godot 4.5** using C# and .NET 8.0.

### Running the Game

1. Install [Godot 4.5](https://godotengine.org/download) with .NET support
2. Clone this repository
3. Open the project in Godot
4. Press F5 to run

Alternatively, build from the command line:
```bash
dotnet build LD58.sln
```

### Building for Web

Not supported in Godot .NET 4.5, but [@xr0gu3](https://github.com/xr0gu3) has made a docker image that can be used to test web export using the proposed changes from [[.NET] Add web export support
#106125](https://github.com/godotengine/godot/pull/106125) ([his comment](https://github.com/godotengine/godot/pull/106125#issuecomment-3342182607))

Run `docker run --rm -v ".:/src" kstgrd/godot-mono-web:4.5.dev` while in the project root.

## Game Jam

Created for **Ludum Dare 58** game jam.

Theme: Collector
