using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Media;
using CarGameSim.Models;
using System;

namespace CarGameSim;

public partial class MainWindow : Window
{
    private readonly Car _car = new();
    private readonly GameState _state = new();
    private readonly DispatcherTimer _timer;

    public MainWindow()
    {
        InitializeComponent();

        GameView.Car = _car;
        GameView.State = _state;
        GameView.OnCarFlipped += HandleCarFlipped;

        StartButton.Click += StartButton_Click;
        PauseButton.Click += PauseButton_Click;
        HornButton.Click += HornButton_Click;
        LightsButton.Click += LightsButton_Click;
        ResetButton.Click += ResetButton_Click;

        SpeedSlider.PropertyChanged += (_, e) =>
        {
            if (e.Property.Name == "Value")
            {
                _car.Speed = SpeedSlider.Value;
                GameView.InvalidateVisual();
            }
        };

        ColorCombo.SelectionChanged += ColorCombo_SelectionChanged;
        TypeCombo.SelectionChanged += TypeCombo_SelectionChanged;

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(20);
        _timer.Tick += (_, _) => GameView.UpdateGame();

        KeyDown += MainWindow_KeyDown;
        KeyUp += MainWindow_KeyUp;
        Opened += (_, _) => GameView.Focus();
    }

    private async void HandleCarFlipped()
    {
        _timer.Stop();
        _state.Started = false;
        await ShowMessage("خودرو واژگون شد و بازی از ابتدا شروع می‌شود.");
        ResetGame();
    }

    private void StartButton_Click(object? sender, RoutedEventArgs e)
    {
        _state.Started = true;
        _state.Paused = false;
        _timer.Start();
        GameView.Focus();
    }

    private void PauseButton_Click(object? sender, RoutedEventArgs e)
    {
        if (!_state.Started) return;

        _state.Paused = !_state.Paused;

        if (_state.Paused)
            _timer.Stop();
        else
            _timer.Start();

        GameView.Focus();
    }

    private void HornButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            Console.Beep();
        }
        catch
        {
        }

        GameView.Focus();
    }

    private void LightsButton_Click(object? sender, RoutedEventArgs e)
    {
        _car.LightsOn = !_car.LightsOn;
        GameView.InvalidateVisual();
        GameView.Focus();
    }

    private void ResetButton_Click(object? sender, RoutedEventArgs e)
    {
        ResetGame();
        GameView.Focus();
    }

    private void ResetGame()
    {
        _timer.Stop();

        _car.Reset();

        _state.Started = false;
        _state.Paused = false;
        _state.MovingForward = false;
        _state.MovingBackward = false;
        _state.TurningLeft = false;
        _state.TurningRight = false;

        SpeedSlider.Value = 5;
        ColorCombo.SelectedIndex = 0;
        TypeCombo.SelectedIndex = 0;

        GameView.InvalidateVisual();
    }

    private void ColorCombo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ColorCombo.SelectedItem is ComboBoxItem item)
        {
            string? value = item.Content?.ToString();

            _car.CarColor = value switch
            {
                "قرمز" => Colors.Red,
                "سبز" => Colors.Green,
                "مشکی" => Colors.Black,
                "زرد" => Colors.Gold,
                _ => Colors.DodgerBlue
            };

            GameView.InvalidateVisual();
        }

        GameView.Focus();
    }

    private void TypeCombo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (TypeCombo.SelectedItem is ComboBoxItem item)
        {
            _car.CarType = item.Content?.ToString() ?? "Sedan";
            GameView.InvalidateVisual();
        }

        GameView.Focus();
    }

    private void MainWindow_KeyDown(object? sender, KeyEventArgs e)
    {
        if (!_state.Started || _state.Paused)
            return;

        if (e.Key == Key.Up) _state.MovingForward = true;
        if (e.Key == Key.Down) _state.MovingBackward = true;
        if (e.Key == Key.Left) _state.TurningLeft = true;
        if (e.Key == Key.Right) _state.TurningRight = true;

        if (e.Key == Key.H)
        {
            try { Console.Beep(); } catch { }
        }

        if (e.Key == Key.L)
        {
            _car.LightsOn = !_car.LightsOn;
            GameView.InvalidateVisual();
        }
    }

    private void MainWindow_KeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Up) _state.MovingForward = false;
        if (e.Key == Key.Down) _state.MovingBackward = false;
        if (e.Key == Key.Left) _state.TurningLeft = false;
        if (e.Key == Key.Right) _state.TurningRight = false;
    }

    private async System.Threading.Tasks.Task ShowMessage(string message)
    {
        var dialog = new Window
        {
            Width = 380,
            Height = 160,
            Title = "پیام",
            Content = new StackPanel
            {
                Margin = new Avalonia.Thickness(20),
                Spacing = 15,
                Children =
                {
                    new TextBlock
                    {
                        Text = message,
                        TextWrapping = TextWrapping.Wrap
                    },
                    new Button
                    {
                        Content = "باشه",
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Width = 100,
                        Name = "OkButton"
                    }
                }
            }
        };

        var ok = ((dialog.Content as StackPanel)!.Children[1] as Button)!;
        ok.Click += (_, _) => dialog.Close();

        await dialog.ShowDialog(this);
    }
}
