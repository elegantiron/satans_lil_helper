#include <SFML/Graphics/Font.hpp>
#include <SFML/Graphics/Text.hpp>
#include <libslh/Engine/Scene.hpp>

namespace SatansLilHelper::Scenes {
    using SceneResult = libslh::Engine::SceneResult;
    using GameTime    = libslh::GameTime;

    class Title : public libslh::Engine::Scene {
        sf::Font _font;
        sf::Text _title;
        sf::Text _pressStart;
        

        void        init() override;
        SceneResult update(const GameTime& gameTime) override;
        SceneResult event(std::optional<sf::Event> event) override;
        void        draw(sf::RenderTarget& target,
                         sf::RenderStates  states) const override;

    public:
        Title();
    };
}
