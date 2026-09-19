using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CarGameSim.Models;
using System;

namespace CarGameSim.Controls;

public class GameCanvas : Control
{
    public Car Car { get; set; } = new();
    public GameState State { get; set; } = new();

    public event Action? OnCarFlipped;

    public GameCanvas()
    {
        Focusable = true;
    }

    public void UpdateGame()
    {
        if (!State.Started || State.Paused)
            return;

        if (State.TurningLeft) Car.Angle -= 4;
        if (State.TurningRight) Car.Angle += 4;

        double radians = Car.Angle * Math.PI / 180.0;

        if (State.MovingForward)
        {
            Car.X += Math.Cos(radians) * Car.Speed;
            Car.Y += Math.Sin(radians) * Car.Speed;
        }

        if (State.MovingBackward)
        {
            Car.X -= Math.Cos(radians) * Car.Speed;
            Car.Y -= Math.Sin(radians) * Car.Speed;
        }

        KeepInsideBounds();
        CheckFlipCondition();

        InvalidateVisual();
    }

    private void KeepInsideBounds()
    {
        if (Bounds.Width <= 0 || Bounds.Height <= 0)
            return;

        if (Car.X < 60) Car.X = 60;
        if (Car.Y < 60) Car.Y = 60;
        if (Car.X > Bounds.Width - 60) Car.X = Bounds.Width - 60;
        if (Car.Y > Bounds.Height - 60) Car.Y = Bounds.Height - 60;
    }

    private void CheckFlipCondition()
    {
        double normalized = ((Car.Angle % 360) + 360) % 360;
        if (normalized >= 150 && normalized <= 210)
        {
            OnCarFlipped?.Invoke();
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        DrawRoad(context);
        DrawCar(context);
        DrawStatus(context);
    }

    private void DrawRoad(DrawingContext context)
    {
        var roadRect = new Rect(50, 50, Bounds.Width - 100, Bounds.Height - 100);
        context.FillRectangle(new SolidColorBrush(Colors.DarkSlateGray), roadRect);
        context.DrawRectangle(new Pen(Brushes.Black, 3), roadRect);

        var dashPen = new Pen(Brushes.White, 3);
        double centerX = Bounds.Width / 2;

        for (double y = 60; y < Bounds.Height - 60; y += 30)
        {
            context.DrawLine(dashPen, new Point(centerX, y), new Point(centerX, y + 15));
        }
    }

    private void DrawCar(DrawingContext context)
    {
        using (context.PushTransform(
            Matrix.CreateTranslation(-Car.Width / 2, -Car.Height / 2) *
            Matrix.CreateRotation(Car.Angle * Math.PI / 180.0) *
            Matrix.CreateTranslation(Car.X, Car.Y)))
        {
            var bodyRect = new Rect(0, 0, Car.Width, Car.Height);
            context.FillRectangle(new SolidColorBrush(Car.CarColor), bodyRect);
            context.DrawRectangle(new Pen(Brushes.Black, 2), bodyRect);

            var windowRect = new Rect(Car.Width * 0.25, Car.Height * 0.25, Car.Width * 0.5, Car.Height * 0.5);
            context.FillRectangle(new SolidColorBrush(Colors.LightBlue), windowRect);

            DrawWheels(context);
            DrawFrontMarker(context);

            if (Car.LightsOn)
                DrawLights(context);
        }
    }

    private void DrawWheels(DrawingContext context)
    {
        var wheelBrush = new SolidColorBrush(Colors.Black);

        context.FillRectangle(wheelBrush, new Rect(-5, 5, 10, 12));
        context.FillRectangle(wheelBrush, new Rect(-5, Car.Height - 17, 10, 12));
        context.FillRectangle(wheelBrush, new Rect(Car.Width - 5, 5, 10, 12));
        context.FillRectangle(wheelBrush, new Rect(Car.Width - 5, Car.Height - 17, 10, 12));
    }

    private void DrawFrontMarker(DrawingContext context)
    {
        var markerBrush = new SolidColorBrush(Colors.White);

        context.DrawGeometry(markerBrush, null, new EllipseGeometry(new Rect(Car.Width - 10, 6, 8, 8)));
        context.DrawGeometry(markerBrush, null, new EllipseGeometry(new Rect(Car.Width - 10, 18, 8, 8)));
    }

    private void DrawLights(DrawingContext context)
    {
        var lightBrush = new SolidColorBrush(Color.FromArgb(120, 255, 255, 0));

        var geo1 = new StreamGeometry();
        using (var gc = geo1.Open())
        {
            gc.BeginFigure(new Point(Car.Width, 8), true);
            gc.LineTo(new Point(Car.Width + 45, 0));
            gc.LineTo(new Point(Car.Width + 45, 18));
            gc.EndFigure(true);
        }

        var geo2 = new StreamGeometry();
        using (var gc = geo2.Open())
        {
            gc.BeginFigure(new Point(Car.Width, Car.Height - 8), true);
            gc.LineTo(new Point(Car.Width + 45, Car.Height - 18));
            gc.LineTo(new Point(Car.Width + 45, Car.Height));
            gc.EndFigure(true);
        }

        context.DrawGeometry(lightBrush, null, geo1);
        context.DrawGeometry(lightBrush, null, geo2);
    }

    private void DrawStatus(DrawingContext context)
    {
        string status = !State.Started ? "شروع نشده" : (State.Paused ? "متوقف" : "در حال اجرا");
        string lightStatus = Car.LightsOn ? "روشن" : "خاموش";

        var text = new FormattedText(
            $"وضعیت: {status}\nسرعت: {Car.Speed}\nچراغ: {lightStatus}\nنوع خودرو: {Car.CarType}",
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new Typeface("Arial"),
            16,
            Brushes.Black);

        context.DrawText(text, new Point(10, 10));
    }
}
