using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SatansLilHelper.Utils;

internal sealed class Settings
{
    private static Settings _instance = null;
    private static readonly Lock padlock = new();
    public bool ShowStatus { get; set; }
    public bool ShowHealthBars { get; set; }
    public bool TestSetting { get; set; }
    public bool AnotherTest { get; set; }

    Settings() { }

    public static Settings Default
    {
        get
        {
            lock (padlock)
            {
                _instance ??= new Settings();
                return _instance;
            }
        }
    }
}
