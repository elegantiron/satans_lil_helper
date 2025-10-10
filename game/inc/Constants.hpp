#pragma once
#include <SFML/System/Vector2.hpp>
#include <boost/locale.hpp>

namespace SatansLilHelper::Constants {
    namespace Strings {
        using namespace boost::locale;
        extern const basic_message<char> Title;
        extern const basic_message<char> PressStart;

        namespace Menu {
            extern const basic_message<char> NewGame;
            extern const basic_message<char> Bestiary;
            extern const basic_message<char> Settings;
        }
    }

    extern const sf::Vector2u WindowSize;
}
