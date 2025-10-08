#include "Engine/Core.hpp"

namespace SatansLilHelper::Engine {

    void Core::init(sf::VideoMode mode, const char* title, bool& successful) {
        _window.create(mode, title, sf::Style::Titlebar | sf::Style::Close);

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
        if (event->is<sf::Event::Closed>()) {
            keepRunning = false;
            return;
        }

        _sceneMan.handleEvent(event, successful, keepRunning);
    }

    void Core::run() {
        bool keepRunning = true;
        bool successful  = true;
        while (successful && keepRunning) {
            iterate(_clock.newFrame(), successful, keepRunning);
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
            _window.clear(_backgroundColor);
            _window.draw(_sceneMan);
            _window.display();
        }
    }

    void Core::quit(bool /*successful*/) {}

    const sf::RenderWindow& Core::getWindow() const {
        return _window;
    }

    void Core::setNextScene(ScenePtr pScene) {
        _sceneMan.setNextScene(std::move(pScene));
    }

    ScenePtr Core::getCurrentScene() const {
        return _sceneMan.getCurrentScene();
    }

    void Core::setOption(GameSetting optionID, bool value) {
        _settings.set(getInt(optionID), value);
    }

    const bitset<getInt(GameSetting::COUNT)>& Core::getSettings() const {
        return _settings;
    }

    bool Core::testOption(GameSetting optionID) const {
        return _settings.test(getInt(optionID));
    }
} // namespace SatansLilHelper::Engine
