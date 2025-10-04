#include "Scenes/Title.hpp"

#include <SFML/Graphics/RenderStates.hpp>


namespace SatansLilHelper {
    SceneResult Title::update(const GameTime& gameTime)
    {
        return SceneResult::CONTINUE;
    }

    SceneResult Title::event(std::optional<sf::Event> event)
    {
        if (event->is<sf::Event::Closed>()) {
            return SceneResult::SUCCESS;
        }
        return SceneResult::CONTINUE;
    }

    void Title::draw(sf::RenderTarget& target, sf::RenderStates states) const {}
}
