#ifndef LIBSATAN_ENGINE_SCENESTACK_HPP
#define LIBSATAN_ENGINE_SCENESTACK_HPP
#include "libsatan/Engine/Scene.hpp"
#include "libsatan/System/GameTime.hpp"

#include <SFML/Window/Event.hpp>
#include <stack>

namespace libsatan::Engine {
    using namespace System;
    /**
     * @brief Used to manage scenes for a Game
     *
     */
    class SceneStack {
        std::stack<ScenePtr> _scenes;

        void popScene();

    public:
        void updateCurrentScene(const GameTime& gameTime, bool& successful);
        void handleEventWithScene(std::optional<sf::Event> event,
                                  bool&                    successful);
        void transitionScene(const ScenePtr& nextScene);
    };
}

#endif
