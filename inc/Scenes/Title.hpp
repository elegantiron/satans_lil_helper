#pragma once

#include "Engine/Scene.hpp"

#include <SFML/Graphics/Font.hpp>
#include <SFML/Graphics/Sprite.hpp>
#include <SFML/Graphics/Text.hpp>

namespace SatansLilHelper::Scenes {

    class Title : public Scene {
        sf::Text   _title{Assets::Fonts::FairyDustB};
        sf::Text   _pressStart{Assets::Fonts::CrayonLibre};
        sf::Sprite _satanMain{Assets::Textures::Satan::Main};
        sf::Sprite _satanEyesOpen{Assets::Textures::Satan::EyesOpen};
        sf::Sprite _satanMouthClosed{Assets::Textures::Satan::MouthClosed};
        sf::Sprite _satanEyesClosed{Assets::Textures::Satan::EyesClosed};
        bool       _eyesOpen{true};
        bool       _isActive{true};

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
        void handleEvent(const sf::Event& event,
                         bool&            successful,
                         bool&            keepRunning) override;


    public:
        Title(ScenePtr parent = nullptr);

    private:
        // Timer _blinkTimer;
    };
} // namespace SatansLilHelper::Scenes
