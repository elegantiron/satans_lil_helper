using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MLEM.Input;

namespace SatansLilHelper.Utils;

internal sealed class Settings
{
    private static readonly Lazy<Settings> _instance = new(() => new Settings());
    public bool ShowStatus { get; set; }
    public bool ShowHealthBars { get; set; }
    public bool AutoFinishTurn { get; set; }
    public Keybind Movement { get; set; }
    public Keybind MoveUp { get; set; }
    public Keybind MoveUpRight { get; set; }
    public Keybind MoveRight { get; set; }
    public Keybind MoveDownRight { get; set; }
    public Keybind MoveDown { get; set; }
    public Keybind MoveDownLeft { get; set; }
    public Keybind MoveLeft { get; set; }
    public Keybind MoveUpLeft { get; set; }
    public Keybind Confirm { get; set; }
    public Keybind Cancel { get; set; }
    public Keybind Map { get; set; }
    public Keybind MiniMap { get; set; }
    public Keybind Inventory { get; set; }
    public Keybind Skills { get; set; }

    private Settings()
    {
        // Movement bindings
        MoveUp = new Keybind().Add(Keys.Up).Add(Keys.NumPad8);
        MoveUpRight = new Keybind().Add(Keys.PageUp).Add(Keys.NumPad9);
        MoveRight = new Keybind().Add(Keys.Right).Add(Keys.NumPad6);
        MoveDownRight = new Keybind().Add(Keys.PageDown).Add(Keys.NumPad3);
        MoveDown = new Keybind().Add(Keys.Down).Add(Keys.NumPad2);
        MoveDownLeft = new Keybind().Add(Keys.Delete).Add(Keys.NumPad1);
        MoveLeft = new Keybind().Add(Keys.Left).Add(Keys.NumPad4);
        MoveUpLeft = new Keybind().Add(Keys.Insert).Add(Keys.NumPad7);

        Movement = new Keybind();
        List<Keybind> keybinds =
        [
            MoveUp,
            MoveUpRight,
            MoveRight,
            MoveDownRight,
            MoveDown,
            MoveDownLeft,
            MoveLeft,
            MoveUpLeft,
        ];
        foreach (Keybind bind in keybinds)
        foreach (Keybind.Combination combo in bind.Combinations)
            Movement.Add(combo);

        Confirm = new Keybind().Add(Keys.Enter).Add(Buttons.A);
        Cancel = new Keybind().Add(Keys.Escape).Add(Buttons.B);
        Map = new Keybind().Add(Keys.M);
        MiniMap = new Keybind().Add(Keys.Tab);
        Inventory = new Keybind().Add(Keys.I).Add(Buttons.X);
        Skills = new Keybind().Add(Keys.S).Add(Buttons.Y);
    }

    public static Settings Default => _instance.Value;
}
