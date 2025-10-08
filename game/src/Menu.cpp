#include "Menu.hpp"

#include <algorithm>
#include <utility>

namespace {
    constexpr float HEIGHT_MOD = 1.5F;
}

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
        // TODO check items' actual sizes
        auto        count      = _items.size();
        auto&       core       = Engine::Core::getInstance();
        const auto& window     = core.getWindow();
        auto        windowSize = window.getSize();
        auto        xPos       = (float)windowSize.x / 2;
        float       offset     = (float)count / 2;
        float       startYPos  = ((float)windowSize.y / 2)
                        - (HEIGHT_MOD * offset * (float)_lineHeight);
        sf::Vector2f position = {xPos, startYPos};
        for (auto& text : _items) {
            text.setPosition(position);
            position.y += HEIGHT_MOD * (float)_lineHeight;
        }
    }

    void Menu::setCharacterHeight(unsigned int height) {
        _characterHeight = height;
        for (auto& text : _items) {
            text.setCharacterSize(_characterHeight);
        }
        calculatePositions();
    }

    unsigned int Menu::getCharacterHeight() const {
        return _characterHeight;
    }

    void Menu::setDefaultFillColor(sf::Color color) {
        _defaultFillColor = color;
        for (int i = 0; i < _items.size(); ++i) {
            if (i == _selectedIdx) {
                continue;
            }
            _items[i].setFillColor(_defaultFillColor);
        }
    }

    sf::Color Menu::getDefaultFillColor() const {
        return _defaultFillColor;
    }

    void Menu::setDefaultOutlineThickness(float thickness) {
        _defaultOutlineThickness = thickness;
        for (int i = 0; i < _items.size(); ++i) {
            if (i == _selectedIdx) {
                continue;
            }
            _items[i].setOutlineThickness(_defaultOutlineThickness);
        }
        calculatePositions();
    }

    float Menu::getDefaultOutlineThickness() const {
        return _defaultOutlineThickness;
    }

    void Menu::setDefaultOutlineColor(sf::Color color) {
        for (int i = 0; i < _items.size(); ++i) {
            if (i == _selectedIdx) {
                continue;
            }
            _items[i].setOutlineColor(color);
        }
    }

    sf::Color Menu::getDefaultOutlineColor() const {
        return _defaultOutlineColor;
    }

    void Menu::setSelectedFillColor(sf::Color color) {
        _selectedFillColor = color;
        _items[_selectedIdx].setFillColor(_selectedFillColor);
    }

    sf::Color Menu::getSelectedFillColor() const {
        return _selectedFillColor;
    }

    void Menu::setSelectedOutlineThickness(float thickness) {
        _selectedOutlineThickness = thickness;
        _items[_selectedIdx].setOutlineThickness(_selectedOutlineThickness);
        calculatePositions();
    }

    float Menu::getSelectedOutlineThickness() const {
        return _selectedOutlineThickness;
    }

    void Menu::setSelectedOutlineColor(sf::Color color) {
        _selectedOutlineColor = color;
        _items[_selectedIdx].setOutlineColor(color);
    }

    sf::Color Menu::getSelectedOutlineColor() const {
        return _selectedOutlineColor;
    }
} // namespace SatansLilHelper
