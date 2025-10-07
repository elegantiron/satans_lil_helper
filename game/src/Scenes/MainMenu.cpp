#include "Scenes/MainMenu.hpp"

namespace SatansLilHelper::Scenes {
    void MainMenu::draw(sf::RenderTarget& target,
                        sf::RenderStates  states) const {}

    void MainMenu::iterate(const GameTime& gameTime,
                           bool&           successful,
                           bool&           keepRunning) {
        successful = keepRunning = true;
    }

    void MainMenu::handleEvent(std::optional<sf::Event> event,
                               bool&                    successful,
                               bool&                    keepRunning) {}
}
