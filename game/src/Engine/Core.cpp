#include "Engine/Core.hpp"

namespace SatansLilHelper::Engine {
    void Core::init(sf::VideoMode mode, const char* title, bool& successful) {
        _window.create(mode, title);
        successful = true;
    }

    Core& Core::getInstance() {
        if (_instance == nullptr) {
            _instance = new Core();
        }
        return *_instance;
    }

    Core* Core::_instance{nullptr};

    void Core::iterate(const GameTime& gameTime,
                       bool&           successful,
                       bool&           keepRunning) {
        keepRunning = false;
        successful  = false;
        _sceneMan.iterate(gameTime, successful, keepRunning);
    }

    void Core::handleEvent(std::optional<sf::Event> event,
                           bool&                    successful,
                           bool&                    keepRunning) {
        _sceneMan.handleEvent(event, successful, keepRunning);
    }

    void Core::run() {
        bool keepRunning = false;
        bool successful  = false;
        while (successful && keepRunning) {
            GameTime gameTime;
            iterate(gameTime, successful, keepRunning);
            if (!successful || !keepRunning) {
                quit(successful);
                return;
            }
            while (std::optional event = _window.pollEvent()) {
                handleEvent(event, successful, keepRunning);
                if (!successful || !keepRunning) {
                    quit(successful);
                    return;
                }
            }
        }
    }

    void Core::quit(bool /*successful*/) {}

    const sf::RenderWindow& Core::getWindow() const {
        return _window;
    }
} // namespace SatansLilHelper::Engine
