#include "Constants.hpp"

namespace SatansLilHelper::Constants::Strings {

    extern const basic_message<char> Title = translate("Satan's Lil Helper");
    extern const basic_message<char> PressStart = translate("Press START");

    namespace Menu {
        extern const basic_message<char> NewGame  = translate("new game");
        extern const basic_message<char> Bestiary = translate("bestiary");
        extern const basic_message<char> Settings = translate("settings");
    }

    extern const basic_message<char> Inventory = translate("inventory");
    extern const basic_message<char> InventoryHeader
        = translate("section", "inventory");
}
