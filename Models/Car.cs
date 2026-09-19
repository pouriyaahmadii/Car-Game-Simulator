using Avalonia.Media;

namespace CarGameSim.Models;

public class Car
{
    public double X { get; set; } = 140;
    public double Y { get; set; } = 260;
    public double Angle { get; set; } = 0;

    public double Speed { get; set; } = 5;
    public bool LightsOn { get; set; } = false;

    public string CarType { get; set; } = "Sedan";
    public Color CarColor { get; set; } = Colors.DodgerBlue;

    public double Width
    {
        get
        {
            return CarType switch
            {
                "SUV" => 90,
                "Truck" => 110,
                _ => 80
            };
        }
    }

    public double Height
    {
        get
        {
            return CarType switch
            {
                "SUV" => 50,
                "Truck" => 55,
                _ => 40
            };
        }
    }

    public void Reset()
    {
        X = 140;
        Y = 260;
        Angle = 0;
        Speed = 5;
        LightsOn = false;
        CarType = "Sedan";
        CarColor = Colors.DodgerBlue;
    }
}
