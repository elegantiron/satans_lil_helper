#include "Scenes/MainMenu.hpp"

namespace SatansLilHelper::Scenes {
    void MainMenu::draw(sf::RenderTarget& target,
                        sf::RenderStates  states) const {
        target.draw(*_parent, states);
    }

    void MainMenu::iterate(const GameTime& gameTime,
                           bool&           successful,
                           bool&           keepRunning) {
        successful = keepRunning = true;
    }

    void MainMenu::handleEvent(std::optional<sf::Event> event,
                               bool&                    successful,
                               bool&                    keepRunning) {
        if (const auto& keyEvent = event->getIf<sf::Event::KeyPressed>()) {
            if (keyEvent->scancode == sf::Keyboard::Scancode::Escape) {
                keepRunning = false;
                return;
            }
        }
    }
}
