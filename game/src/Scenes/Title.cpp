#include "Scenes/Title.hpp"

extern const Asset FairyDust;

namespace { // Local constants
    constexpr unsigned int TITLE_CHAR_SIZE      = 85;
    constexpr int          TITLE_Y_POS          = 15;
    constexpr sf::Color    TITLE_FILL_COLOR     = {0xFD, 0x28, 0x28, 0xFF};
    constexpr sf::Color    TITLE_OUTLINE_COLOR  = {0xFF, 0x00, 0x00, 0xFF};
    constexpr unsigned int PSTART_CHAR_SIZE     = 35;
    constexpr float        PSTART_Y_POS_FACTOR  = 4.F / 5.F;
    constexpr sf::Color    PSTART_FILL_COLOR    = {0xF0, 0xF0, 0xF0, 0xFF};
    constexpr sf::Color    PSTART_OUTLINE_COLOR = {0xFF, 0xFF, 0xFF, 0xFF};
} // namespace

namespace SatansLilHelper::Scenes {
    void Title::init() {
        _title.setString(Constants::Title);
        _title.setCharacterSize(TITLE_CHAR_SIZE);
        auto bounds = _title.getLocalBounds();
        _title.setOrigin(sf::Vector2f(bounds.size.x / 2, 0));
        _title.setFillColor(TITLE_FILL_COLOR);
        _title.setOutlineColor(TITLE_OUTLINE_COLOR);

        _pressStart.setString(Constants::PressStart);
        _pressStart.setCharacterSize(PSTART_CHAR_SIZE);
        bounds = _pressStart.getLocalBounds();
        _pressStart.setOrigin(sf::Vector2f(bounds.size.x / 2, 0));
        _pressStart.setFillColor(PSTART_FILL_COLOR);
        _pressStart.setOutlineColor(PSTART_OUTLINE_COLOR);

        setTextPositions();
    }

    Title::Title()
        : _font(FairyDust.data, FairyDust.size), _title(_font),
          _pressStart(_font) {}

    void Title::setTextPositions() {
        auto&       core       = Engine::Core::getInstance();
        const auto& window     = core.getWindow();
        auto        windowSize = window.getSize();
        _title.setPosition(sf::Vector2f((float)windowSize.x / 2, TITLE_Y_POS));
        float pressStartYPos = (float)windowSize.y * PSTART_Y_POS_FACTOR;
        _pressStart.setPosition(
            sf::Vector2f((float)windowSize.x / 2, pressStartYPos));
    }
}
