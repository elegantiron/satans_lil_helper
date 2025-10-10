#pragma once
#include <SFML/System/Vector2.hpp>
#include <boost/locale.hpp>

namespace SatansLilHelper::Constants {
    namespace Strings {
        using namespace boost::locale;
        extern const basic_message<char> Title;
        extern const basic_message<char> PressStart;
    }

    extern const sf::Vector2u WindowSize;
}
