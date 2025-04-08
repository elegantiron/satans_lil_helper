using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatansLilHelper.Constants;

public enum ItemType
{
    // Base item types
    None = 0,
    Weapon1H = 1,
    Shield = 1 << 1,
    Armor = 1 << 2,
    Helm = 1 << 3,
    Gauntlets = 1 << 4,
    Boots = 1 << 5,
    RingL = 1 << 6,
    RingR = 1 << 7,
    Amulet = 1 << 8,
    Torch = 1 << 9,

    // Common combinations
    Base = RingL | RingR | Amulet | Torch,
    Warrior = Base | Weapon1H | Shield | Armor | Helm | Gauntlets | Boots,
}
