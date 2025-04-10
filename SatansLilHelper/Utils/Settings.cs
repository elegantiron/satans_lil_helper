using System;

namespace SatansLilHelper.Utils;

internal sealed class Settings
{
    private static readonly Lazy<Settings> _instance = new(() => new Settings());
    public bool ShowStatus { get; set; }
    public bool ShowHealthBars { get; set; }

    private Settings() { }

    public static Settings Default => _instance.Value;
}
