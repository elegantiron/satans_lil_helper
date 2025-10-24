#include "Graphics/WindowManager.hpp"

namespace SatansLilHelper {
    WindowManager::WindowManager()  = default;
    WindowManager::~WindowManager() = default;

    void WindowManager::init(sf::VideoMode     mode,
                             const sf::String& title,
                             unsigned int      frameRate) {
        _frameRate = frameRate;
        _title     = title;
        _window.create(mode, _title);
        _window.setFramerateLimit(_frameRate);
    }

    void WindowManager::init(sf::Vector2u size, const sf::String& title) {
        _title = title;
        _window.create(sf::VideoMode(size), _title);
    }

    sf::String WindowManager::getTitle() const {
        return _title;
    }

    void WindowManager::setTitle(const sf::String& title) {
        _title = title;
        _window.setTitle(_title);
    }

    sf::Vector2u WindowManager::getSize() const {
        return _window.getSize();
    }

    void WindowManager::setFrameRate(unsigned int limit) {
        _frameRate = limit;
        _window.setFramerateLimit(_frameRate);
    }

    unsigned int WindowManager::getFrameRate() const {
        return _frameRate;
    }
} // namespace SatansLilHelper
