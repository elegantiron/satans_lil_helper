using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SatansLilHelper.Utils;

public static class Constants
{
    public static readonly (int x, int y)[] NeighborDirections =
    [
        (-1, -1),
        (-1, 0),
        (-1, 1),
        (0, -1),
        (0, 1),
        (1, -1),
        (1, 0),
        (1, 1),
    ];

    public static readonly Dictionary<Keys, Point> MovementKeys = new()
    {
        { Keys.Up, new Point(0, -1) },
        { Keys.Down, new Point(0, 1) },
        { Keys.Right, new Point(1, 0) },
        { Keys.Left, new Point(-1, 0) },
        { Keys.Insert, new Point(-1, -1) },
        { Keys.NumPad1, new Point(-1, 1) },
        { Keys.NumPad2, new Point(0, 1) },
        { Keys.NumPad3, new Point(1, 1) },
        { Keys.NumPad4, new Point(-1, 0) },
        { Keys.NumPad6, new Point(1, 0) },
        { Keys.NumPad7, new Point(-1, -1) },
        { Keys.NumPad8, new Point(0, -1) },
        { Keys.NumPad9, new Point(1, -1) },
        { Keys.Delete, new Point(-1, 1) },
        { Keys.PageUp, new Point(1, -1) },
        { Keys.PageDown, new Point(1, 1) },
    };

    public static class Colors
    {
        public static readonly Color Impossible = new(0x80, 0x80, 0x80);
        public static readonly Color PlayerAttack = new(0xFF, 0x90, 0xFF, 0xFF);
        public static readonly Color EnemyAttack = new(0xFF, 0x70, 0x70, 0xFF);
        public static readonly Color AmericanRose = new(0xFF, 0x03, 0x3E, 0xFF);
        public static readonly Color RubineRed = new(0xD1, 0x00, 0x56, 0xFF);
        public static readonly Color TranslucentBlack = new(0x00, 0x00, 0x00, 0xB0);
        public static readonly Color DeadActor = new(0x80, 0x80, 0x80, 0x80);
        public static readonly Color LiveActor = Color.White;
        public static readonly Color PlayerHeal = Color.PeachPuff;
    }
}

public enum TextureID
{
    Missing,
    Player,
    ForestFloor,
    ForestWall,
    SatanMain,
    SatanEyesOpen,
    SatanEyesClosed,
    SatanMouthOpen,
    SatanMouthClosed,
    WhitePixel,
    Orc,
    Wolf,
}

public enum SongID
{
    HideAndSeek,
    Scavenge1,
    Scavenge2,
}

public enum EffectID
{
    Test,
}

public enum FontID
{
    Status,
    Title,
    Menu,
    Messages,
}

public enum ResourceID
{
    Health,
    Mana,
}

public enum AbilityID
{
    Strength,
    MagicPower,
    PhysicalDefense,
    MagicDefense,
    Evasion,
    Crit,
    Speed,
    Vision,
    LightRadius,
}

public enum SkillID { }

public enum SlotID { }

public enum ItemType
{
    // Base item types
    None = 0 << 0,
    Weapon1H = 1 << 0,
    Shield = 1 << 1,
    Armor = 1 << 2,
    Helm = 1 << 3,
    Gauntlets = 1 << 4,
    Boots = 1 << 5,
    RingL = 1 << 6,
    RingR = 1 << 7,
    Amulet = 1 << 8,

    // Common combinations
    Base = RingL | RingR | Amulet,
    Warrior = Base | Weapon1H | Shield | Armor | Helm | Gauntlets | Boots,
}

public enum Events
{
    AddLogMessage,
    PruneLogMessage,
    QuitGame,
}
