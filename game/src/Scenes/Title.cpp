#include "Scenes/Title.hpp"

#include <SFML/Graphics/RenderStates.hpp>
extern const Asset FairyDust;

namespace SatansLilHelper::Scenes {
    namespace {
        constexpr float     TITLE_SIZE       = 85;
        constexpr sf::Color TITLE_FILL_COLOR = {225, 40, 40, 255};
        constexpr int       TITLE_POS_Y      = 15;
        constexpr float     PRESS_START_SIZE = 35;
        constexpr float     FOUR_FIFTHS      = 4.F / 5.F;
    }

    SceneResult Title::update(const GameTime& /*gameTime*/) {
        return SceneResult::CONTINUE;
    }

    SceneResult Title::event(std::optional<sf::Event> event) {
        if (event->is<sf::Event::Closed>()) {
            return SceneResult::SUCCESS;
        }
        if (const auto* keyEvent = event->getIf<sf::Event::KeyPressed>()) {
            if (keyEvent->scancode == sf::Keyboard::Scancode::Escape) {
                return SceneResult::SUCCESS;
            }
        }
        return SceneResult::CONTINUE;
    }

    void Title::draw(sf::RenderTarget& target, sf::RenderStates states) const {
        target.draw(_title, states);
        target.draw(_pressStart, states);
    }

    Title::Title()
        : _font(FairyDust.data, FairyDust.size), _title(_font),
          _pressStart(_font) {}

    void Title::init() {
        auto& core = libslh::Engine::Core::getInstance();
        _title.setString(Constants::Title);
        _title.setCharacterSize(TITLE_SIZE);
        _title.setFillColor(TITLE_FILL_COLOR);
        auto size = _title.getLocalBounds();
        _title.setOrigin(sf::Vector2f(size.size.x / 2, 0));
        auto windowSize = core.getWindowSize();
        _title.setPosition(sf::Vector2f((float)windowSize.x / 2, TITLE_POS_Y));

        _pressStart.setString(Constants::PressStart);
        _pressStart.setCharacterSize(PRESS_START_SIZE);
        size = _pressStart.getLocalBounds();
        _pressStart.setOrigin(sf::Vector2f{size.size.x / 2, 0});
        _pressStart.setPosition(
            sf::Vector2f({static_cast<float>(windowSize.x) / 2,
                          static_cast<float>(windowSize.y) * FOUR_FIFTHS}));
    }
} // namespace SatansLilHelper::Scenes
