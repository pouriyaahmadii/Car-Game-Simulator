# Car Game Simulator

A simple, interactive 2D Car Game Simulator built using **C#** and **Avalonia UI**.

## Features

* **Driving Controls**: Navigate the car using arrow keys (Up, Down, Left, Right).
* **Speed Adjustment**: Control the speed of the vehicle with a slider.
* **Car Customization**: Change the car's color (Red, Green, Black, Gold, Blue) and type.
* **Interactive Elements**:
  * Honk the horn (using the UI button or the `H` key).
  * Toggle car lights (using the UI button or the `L` key).
* **Game State Management**: Start, Pause, and Reset the simulation.
* **Physics/Events**: Be careful! The car can flip over, which will trigger a game-over message and reset the simulation.

## Tech Stack

* **Language**: C#
* **Framework**: [Avalonia UI](https://avaloniaui.net/) (v11.1.3)
* **Runtime**: .NET 10.0

## Getting Started

### Prerequisites

* [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Run the Simulator

1. Clone this repository or download the source code.
2. Open a terminal in the project directory.
3. Run the following command:

```bash
dotnet run
```

## Controls

| Action | Key |
| :--- | :--- |
| Move Forward | `Up Arrow` |
| Move Backward| `Down Arrow` |
| Turn Left | `Left Arrow` |
| Turn Right | `Right Arrow` |
| Honk Horn | `H` |
| Toggle Lights| `L` |

## Note
The UI dialogs and some messages in this simulator are in Persian (Farsi).
