#pragma once

#include <SFML/Graphics/RenderWindow.hpp>

namespace {
    constexpr unsigned int DEFAULT_FPS = 60;
}

namespace SatansLilHelper {
    class WindowManager {
        sf::RenderWindow _window;
        sf::String       _title     = "";
        unsigned int     _frameRate = DEFAULT_FPS;

    public:
        WindowManager();
        ~WindowManager();

        void         init(sf::VideoMode     mode,
                          const sf::String& title,
                          unsigned int      frameRate = DEFAULT_FPS);
        void         init(sf::Vector2u size, const sf::String& title);
        sf::String   getTitle() const;
        void         setTitle(const sf::String& title);
        sf::Vector2u getSize() const;
        void         setSize(sf::Vector2u size);
        void         setFrameRate(unsigned int limit);
        unsigned int getFrameRate() const;
    };
}
