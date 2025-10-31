#include "Scenes/MainMenu.hpp"

#include "Constants.hpp"

#include <SFML/Graphics/RenderTarget.hpp>
#include <libslh/Engine/Core.hpp>

namespace SatansLilHelper::Scenes {
    void MainMenu::draw(sf::RenderTarget& target,
                        sf::RenderStates  states) const {
        target.draw(*_parent, states);
        target.draw(_menu, states);
    }

    void MainMenu::iterate(const GameTime& gameTime,
                           bool&           successful,
                           bool&           keepRunning) {
        (void)gameTime;
        successful = keepRunning = true;
    }

    void MainMenu::handleEvent(std::optional<sf::Event> event,
                               bool&                    successful,
                               bool&                    keepRunning) {
        successful = keepRunning = true;
        if (const auto& keyEvent = event->getIf<sf::Event::KeyPressed>()) {
            if (keyEvent->scancode == sf::Keyboard::Scancode::Escape) {
                keepRunning = false;
                return;
            }
        }
    }

    MainMenu::MainMenu(Engine::ScenePtr parent)
        : Scene(std::move(parent)),
          _menu(Assets::Fonts::Menu::Default, Assets::Fonts::Menu::Selected) {
        auto& core = libslh::Engine::Core::getInstance();
        _menu.addItem(core.localize(Constants::Strings::Menu::NewGame));
        _menu.addItem(core.localize(Constants::Strings::Menu::Bestiary));
        _menu.addItem(core.localize(Constants::Strings::Menu::Settings));
    }
}
