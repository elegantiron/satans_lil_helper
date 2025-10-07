#include "Menu.hpp"

#include <algorithm>
#include <utility>

namespace SatansLilHelper {
    Menu::Menu(sf::Font  font,
               int       characterHeight,
               sf::Color fillColor,
               sf::Color outlineColor,
               float     outlineThickness)
        : _font(std::move(font)), _characterHeight(characterHeight),
          _fillColor(fillColor), _outlineColor(outlineColor),
          _outlineThickness(outlineThickness) {}

    void Menu::addItem(const char* text) {
        sf::Text newItem(_font);
        newItem.setString(text);
        newItem.setCharacterSize(_characterHeight);
        newItem.setFillColor(_fillColor);
        newItem.setOutlineColor(_outlineColor);
        newItem.setOutlineThickness(_outlineThickness);
        auto bounds = newItem.getLocalBounds();
        newItem.setOrigin({bounds.size.x / 2, bounds.size.y / 2});
        _lineHeight = std::max<int>((int)bounds.size.y, _lineHeight);

        _items.emplace_back(std::move(newItem));
        calculatePositions();
    }

    void Menu::calculatePositions() {
        auto        count      = _items.size();
        auto&       core       = Engine::Core::getInstance();
        const auto& window     = core.getWindow();
        auto        windowSize = window.getSize();
        auto        xPos       = (float)windowSize.x / 2;
        float       offset     = (float)count / 2;
        float       startYPos
            = ((float)windowSize.y / 2) - (1.5F * offset * (float)_lineHeight);
        sf::Vector2f position = {xPos, startYPos};
    }
} // namespace SatansLilHelper
