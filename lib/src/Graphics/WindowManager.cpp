#include "libsatan/Graphics/WindowManager.hpp"

namespace libsatan::Graphics {
    void WindowManager::init(sf::Vector2u windowSize, sf::String& windowTitle)
    {
        _window.create(sf::VideoMode{windowSize}, windowTitle);
    }

    sf::Vector2u WindowManager::getWindowSize() const
    {
        return _window.getSize();
    }

    void WindowManager::processEventQueue(SceneManager& sceneMan)
    {
        bool successful = false;
        while (std::optional event = _window.pollEvent()) {
            sceneMan.handleEventWithScene(event, successful);
            if (!successful) {
                break;
            }
        }
    }

    void WindowManager::draw(sf::Drawable& drawable)
    {
        _window.draw(drawable);
    }
}
