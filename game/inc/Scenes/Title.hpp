#pragma once

#include "Engine/Scene.hpp"

#include <SFML/Graphics/Font.hpp>
#include <SFML/Graphics/Text.hpp>
#include <utility>

namespace SatansLilHelper::Scenes {
    class Title : public Engine::Scene {
        sf::Text   _title{Assets::Fonts::FairyDustB};
        sf::Text   _pressStart{Assets::Fonts::CrayonLibre};
        sf::Sprite _satanMain{Assets::Textures::Satan::Main};
        sf::Sprite _satanEyesOpen{Assets::Textures::Satan::EyesOpen};
        sf::Sprite _satanMouthClosed{Assets::Textures::Satan::MouthClosed};
        sf::Sprite _satanEyesClosed{Assets::Textures::Satan::EyesClosed};
        Duration   _blinkTime{0};
        bool       _eyesOpen{true};
        bool       isActive{true};

        void configureTexts();
        void configureSprites();
        void setSpritePositions();
        void setTextPositions();
        void init() override;
        void onBury() override;
        void onReveal() override;
        void draw(sf::RenderTarget& target,
                  sf::RenderStates  states) const override;
        void iterate(const GameTime& gameTime,
                     bool&           successful,
                     bool&           keepRunning) override;
        void handleEvent(std::optional<sf::Event> event,
                         bool&                    successful,
                         bool&                    keepRunning) override;

    public:
        Title(Engine::ScenePtr parent = nullptr) : Scene(std::move(parent)) {}
    };
} // namespace SatansLilHelper::Scenes
