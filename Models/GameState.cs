namespace CarGameSim.Models;

public class GameState
{
    public bool Started { get; set; }
    public bool Paused { get; set; }
    public bool MovingForward { get; set; }
    public bool MovingBackward { get; set; }
    public bool TurningLeft { get; set; }
    public bool TurningRight { get; set; }
}
