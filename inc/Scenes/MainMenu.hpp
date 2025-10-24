#pragma once

#include "Engine/Scene.hpp"

namespace SatansLilHelper::Scenes {

    class MainMenu : public Scene {
        // Menu _menu;

        void iterate(const GameTime& gameTime,
                     bool&           successful,
                     bool&           keepRunning) override;
        void handleEvent(const sf::Event& event,
                         bool&            successful,
                         bool&            keepRunning) override;
        void draw(sf::RenderTarget& target,
                  sf::RenderStates  states) const override;

    public:
        MainMenu(ScenePtr parent = nullptr);
        void addItem(const char* text);
    };
}
