#pragma once

#include "Engine/Scene.hpp"

#include <SFML/Graphics/Font.hpp>
#include <SFML/Graphics/Text.hpp>

extern const Asset FairyDust;
extern const Asset CrayonLibre;
extern const Asset SatanMain;
extern const Asset SatanEyesOpen;
extern const Asset SatanMouthClosed;

namespace SatansLilHelper::Scenes {
    class Title : public Engine::Scene {
        sf::Font    _titleFont{FairyDust.data, FairyDust.size};
        sf::Text    _title{_titleFont};
        sf::Font    _startFont{CrayonLibre.data, CrayonLibre.size};
        sf::Text    _pressStart{_startFont};
        sf::Texture _satanMainText{SatanMain.data, SatanMain.size};
        sf::Sprite  _satanMain{_satanMainText};
        sf::Texture _satanEyesOpenText{SatanEyesOpen.data, SatanEyesOpen.size};
        sf::Sprite  _satanEyesOpen{_satanEyesOpenText};
        sf::Texture _satanMouthClosedText{SatanMouthClosed.data,
                                          SatanMouthClosed.size};
        sf::Sprite  _satanMouthClosed{_satanMouthClosedText};
        bool        isActive{true};

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
        Title() = default;
    };
}
