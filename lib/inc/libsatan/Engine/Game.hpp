#ifndef LIBSATAN_ENGINE_CORE_HPP
#define LIBSATAN_ENGINE_CORE_HPP
#include "libsatan/Engine/Scene.hpp"
#include "libsatan/Engine/SceneStack.hpp"
#include "libsatan/System/Clock.hpp"

#include <SFML/Graphics/Color.hpp>
#include <SFML/Graphics/RenderWindow.hpp>
#include <SFML/System/Vector2.hpp>
#include <bitset>
#include <memory>

namespace libsatan::Engine {
    using namespace System;
    enum class GameSetting : uint8_t
    {
        IMMEDIATE_SCENE_TRANSITION,
        SCENE_MULTISTACK,
        COUNT
    };

    /**
     * @brief Used to coordinate a game.
     *
     */
    class Game {
        Game() = default;
        static std::unique_ptr<Game>                      _instance;
        ScenePtr                                          _nextScene;
        sf::RenderWindow                                  _window;
        std::bitset<static_cast<int>(GameSetting::COUNT)> _settings{0};
        Clock                                             _clock;
        SceneManager                                        _sceneMan;

    public:
        /**
         * @brief Get the singleton instance
         *
         * @return Core&
         */
        static Game& getInstance();

        void run(sf::Vector2u    windowSize,
                 sf::String&     windowTitle,
                 const ScenePtr& pScene = nullptr);

        /**
         * @brief Set the next Scene for the game to run.
         *
         * @param pScene
         */
        void setNextScene(ScenePtr& pScene);
        /**
         * @brief Set the background color
         *
         * @param color
         */
        void         setBackgroundColor(sf::Color color);
        sf::Vector2u getWindowSize() const;

        void updateSetting(GameSetting setting, bool enabled);
        bool getSetting(GameSetting setting);

    private:
        void dumpCore();
        void gameLoop();
    };
} // namespace libsatan::Engine

#endif
