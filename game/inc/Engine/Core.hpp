#pragma once
#include "Clock.hpp"
#include "Engine/SceneManager.hpp"

#include <SFML/Graphics.hpp>
#include <SFMl/Window/Event.hpp>
#include <optional>

namespace SatansLilHelper::Engine {

    class Core {
        static Core* _instance;
        Core() = default;
        sf::RenderWindow _window;
        sf::Color        _backgroundColor{sf::Color::Black};
        SceneManager     _sceneMan;
        Clock            _clock;

        void handleEvent();
        void iterate(const GameTime& gameTime,
                     bool&           successful,
                     bool&           keepRunning);
        void handleEvent(std::optional<sf::Event> event,
                         bool&                    successful,
                         bool&                    keepRunning);

    public:
        static Core&            getInstance();
        const sf::RenderWindow& getWindow() const;
        void     init(sf::VideoMode mode, const char* title, bool& successful);
        void     setNextScene(ScenePtr pScene);
        void     run();
        void     quit(bool successful);
        ScenePtr getCurrentScene();
    };
}
