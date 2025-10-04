#pragma once
#include "libsatan/Engine/SceneManager.hpp"

#include <SFML/Graphics/RenderWindow.hpp>

namespace libsatan::Graphics {
    using namespace Engine;

    class WindowManager {
        sf::RenderWindow _window;
        sf::Color        _backgroundColor{sf::Color::Black};

    public:
        void         draw(sf::Drawable& drawable);
        void         init(sf::Vector2u windowSize, sf::String& windowTitle);
        void         handleEvents(SceneManager& sceneMan, bool& hasScenes);
        sf::Vector2u getWindowSize() const;
    };
}
