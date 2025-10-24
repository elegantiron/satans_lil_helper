#include "Scenes/MainMenu.hpp"

#include <SFML/Graphics/RenderTarget.hpp>

namespace {
    constexpr int CHAR_HEIGHT = 55;
}

namespace SatansLilHelper::Scenes {
    void MainMenu::draw(sf::RenderTarget& target,
                        sf::RenderStates  states) const {
        target.draw(*_parent, states);
    }

    void MainMenu::iterate(const GameTime& gameTime,
                           bool&           successful,
                           bool&           keepRunning) {
        (void)gameTime;
        successful = keepRunning = true;
    }

    void MainMenu::handleEvent(const sf::Event& event,
                               bool&            successful,
                               bool&            keepRunning) {
        successful = keepRunning = true;
        if (const auto& keyEvent = event.getIf<sf::Event::KeyPressed>()) {
            if (keyEvent->scancode == sf::Keyboard::Scancode::Escape) {
                keepRunning = false;
                return;
            }
        }
    }

    MainMenu::MainMenu(ScenePtr parent)
        : Scene(std::move(parent)) //,
                                   //   _menu(Assets::Fonts::CrayonLibre,
                                   //         CHAR_HEIGHT,
                                   //         sf::Color::White,
                                   //         sf::Color::Red)
    {}
}
