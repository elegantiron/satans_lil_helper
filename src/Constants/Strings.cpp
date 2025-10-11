#include "Constants.hpp"
#define _(STRING) translate(STRING)
#define N_(STRING) STRING
#define C_(CONTEXT, STRING) translate(CONTEXT, STRING)

namespace SatansLilHelper::Constants::Strings {

    extern const basic_message<char> Title      = _("Satan's Lil Helper");
    extern const basic_message<char> PressStart = _("Press START");

    namespace Menu {
        extern const basic_message<char> NewGame
            = C_("section header", "new game");
        extern const basic_message<char> Bestiary
            = C_("section header", "bestiary");
        extern const basic_message<char> Settings
            = C_("section header", "settings");
    }

}
