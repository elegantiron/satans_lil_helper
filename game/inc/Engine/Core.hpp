#pragma once
#include "Engine/SceneManager.hpp"
#include "GameTime.hpp"

#include <SFML/Graphics.hpp>
#include <SFMl/Window/Event.hpp>
#include <optional>

namespace SatansLilHelper::Engine {

    class Core {
        static Core* _instance;
        Core() = default;
        sf::RenderWindow _window;
        SceneManager     _sceneMan;

        void handleEvent();
        void iterate();

    public:
        static Core&            getInstance();
        const sf::RenderWindow& getWindow() const;
        void init(sf::VideoMode mode, const char* title, bool& successful);
        void iterate(const GameTime& gameTime,
                     bool&           successful,
                     bool&           keepRunning);
        void handleEvent(std::optional<sf::Event> event,
                         bool&                    successful,
                         bool&                    keepRunning);
        void run();
        void quit(bool successful);
    };
}
