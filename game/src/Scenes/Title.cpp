#include "Scenes/Title.hpp"

#include "Scenes/MainMenu.hpp"

namespace { // Local constants
    constexpr unsigned int TITLE_CHAR_SIZE      = 85;
    constexpr int          TITLE_Y_POS          = 15;
    constexpr sf::Color    TITLE_FILL_COLOR     = {0xFD, 0x28, 0x28, 0xFF};
    constexpr sf::Color    TITLE_OUTLINE_COLOR  = {0xFF, 0x00, 0x00, 0xFF};
    constexpr unsigned int PSTART_CHAR_SIZE     = 35;
    constexpr float        PSTART_Y_POS_FACTOR  = 4.F / 5.F;
    constexpr sf::Color    PSTART_FILL_COLOR    = {0x60, 0x60, 0x60, 0xFF};
    constexpr sf::Color    PSTART_OUTLINE_COLOR = {0x60, 0x00, 0x00, 0xFF};
} // namespace

namespace SatansLilHelper::Scenes {
    void Title::init() {
        configureTexts();
        configureSprites();

        setTextPositions();
        setSpritePositions();
    }

    void Title::configureTexts() {
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
        _pressStart.setOutlineThickness(1);
    }

    void Title::configureSprites() {
        auto bounds = _satanMain.getLocalBounds();
        _satanMain.setOrigin({bounds.size.x / 2, bounds.size.y / 2});
        bounds = _satanEyesOpen.getLocalBounds();
        _satanEyesOpen.setOrigin({bounds.size.x / 2, bounds.size.y / 2});
        bounds = _satanMouthClosed.getLocalBounds();
        _satanMouthClosed.setOrigin({bounds.size.x / 2, bounds.size.y / 2});
    }

    void Title::setSpritePositions() {
        auto&       core   = Engine::Core::getInstance();
        const auto& window = core.getWindow();
        auto        bounds = window.getSize();
        _satanMain.setPosition({(float)bounds.x / 2, (float)bounds.y / 2});
        _satanEyesOpen.setPosition({(float)bounds.x / 2, (float)bounds.y / 2});
        _satanMouthClosed.setPosition(
            {(float)bounds.x / 2, (float)bounds.y / 2});
    }

    void Title::setTextPositions() {
        auto&       core       = Engine::Core::getInstance();
        const auto& window     = core.getWindow();
        auto        windowSize = window.getSize();
        _title.setPosition(sf::Vector2f((float)windowSize.x / 2, TITLE_Y_POS));
        float pressStartYPos = (float)windowSize.y * PSTART_Y_POS_FACTOR;
        _pressStart.setPosition(
            sf::Vector2f((float)windowSize.x / 2, pressStartYPos));
    }

    void Title::draw(sf::RenderTarget& target, sf::RenderStates states) const {
        target.draw(_title, states);
        if (isActive) {
            target.draw(_pressStart, states);
            target.draw(_satanMain, states);
            target.draw(_satanEyesOpen, states);
            target.draw(_satanMouthClosed, states);
        }
    }

    void Title::iterate(const GameTime& /*gameTime*/,
                        bool& successful,
                        bool& keepRunning) {
        successful = keepRunning = true;
    }

    void Title::handleEvent(std::optional<sf::Event> event,
                            bool&                    successful,
                            bool&                    keepRunning) {
        auto& core = Engine::Core::getInstance();
        successful = keepRunning = true;
        if (const auto* keyEvent = event->getIf<sf::Event::KeyPressed>()) {
            if (keyEvent->scancode != sf::Keyboard::Scancode::Escape) {
                Engine::ScenePtr pMain = std::make_shared<Scenes::MainMenu>(
                    core.getCurrentScene());
                core.setNextScene(pMain);
            }
        }
    }

    void Title::onBury() {
        isActive = false;
    }

    void Title::onReveal() {
        isActive = true;
    }

} // namespace SatansLilHelper::Scenes
