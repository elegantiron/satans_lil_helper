#include "Constants.hpp"
#define _(STRING) translate(STRING)
#define N_(STRING) STRING
#define C_(CONTEXT, STRING) translate(CONTEXT, STRING)

namespace SatansLilHelper::Constants::Strings {

    extern const basic_message<char> Title      = _("Satan's Lil Helper");
    extern const basic_message<char> PressStart = _("Press START");

    namespace Menu {
        extern const basic_message<char> NewGame
            = _("new game");
        extern const basic_message<char> Bestiary
            = _("bestiary");
        extern const basic_message<char> Settings
            = _("settings");
    }

}
