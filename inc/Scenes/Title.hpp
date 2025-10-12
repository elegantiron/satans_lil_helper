#pragma once

#include <SFML/Graphics/Font.hpp>
#include <SFML/Graphics/Sprite.hpp>
#include <SFML/Graphics/Text.hpp>
#include <libslh/Engine/Scene.hpp>
#include <libslh/Engine/Timer.hpp>

using namespace libslh;

namespace SatansLilHelper::Scenes {

    class Title : public Engine::Scene {
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
        void handleEvent(std::optional<sf::Event> event,
                         bool&                    successful,
                         bool&                    keepRunning) override;

        void blinkCallback(bool& runAgain, Duration& nextInterval);

    public:
        Title(Engine::ScenePtr parent = nullptr);

    private:
        Engine::Timer<Title> _blinkTimer;
    };
} // namespace SatansLilHelper::Scenes
