using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SatansLilHelper.Constants;

public static class Colors
{
    // Our colors
    public static readonly Color Impossible = new(0x80, 0x80, 0x80);
    public static readonly Color PlayerAttack = new(0xFF, 0x90, 0xFF, 0xFF);
    public static readonly Color EnemyAttack = new(0xFF, 0x70, 0x70, 0xFF);
    public static readonly Color AmericanRose = new(0xFF, 0x03, 0x3E, 0xFF);
    public static readonly Color RubineRed = new(0xD1, 0x00, 0x56, 0xFF);
    public static readonly Color TranslucentBlack = new(0x00, 0x00, 0x00, 0xB0);
    public static readonly Color DeadActor = new(0x80, 0x80, 0x80, 0x80);
    public static readonly Color White = new(0xFF, 0xFF, 0xFF, 0xFF);

    // Color aliases
    public static readonly Color HiddenTile = White;
    public static readonly Color LiveActor = White;
    public static readonly Color PlayerHeal = Color.PeachPuff;
    public static readonly Color Red = Color.Red;
    public static readonly Color Green = Color.Green;
}
