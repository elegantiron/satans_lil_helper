#pragma once
#include "Clock.hpp"
#include "Engine/SceneManager.hpp"

#include <SFML/Graphics.hpp>
#include <SFMl/Window/Event.hpp>
#include <bitset>
#include <optional>

namespace SatansLilHelper::Engine {
    using std::bitset;
    enum class GameSetting : uint8_t {
        EXIT_ON_ESCAPE,
        COUNT
    };

    constexpr int getInt(GameSetting setting) {
        return static_cast<int>(setting);
    }

    class Core {
        bitset<getInt(GameSetting::COUNT)> _settings;
        static Core*                       _instance;
        sf::RenderWindow                   _window;
        SceneManager                       _sceneMan;
        Clock                              _clock;
        sf::Color                          _backgroundColor = sf::Color::Black;

        Core() = default;

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
        void init(sf::VideoMode mode, const char* title, bool& successful);
        void setNextScene(ScenePtr pScene);
        void run();
        void quit(bool successful);
        void setOption(GameSetting optionID, bool value);
        bool testOption(GameSetting optionID) const;
        const bitset<getInt(GameSetting::COUNT)>& getSettings() const;
        [[nodiscard]]
        ScenePtr getCurrentScene() const;
    };
} // namespace SatansLilHelper::Engine
