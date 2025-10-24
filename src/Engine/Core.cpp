#include "Engine/Core.hpp"

namespace SatansLilHelper {
    Core* Core::_instance = nullptr;

    Core& Core::getInstance() {
        if (_instance == nullptr) {
            _instance = new Core;
        }
        return *_instance;
    }

    void Core::init(sf::VideoMode mode, const sf::String& windowTitle) {
        _winMan.init(mode, windowTitle);
    }
}
