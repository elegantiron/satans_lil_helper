#pragma once
#include <vector>

namespace SatansLilHelper {
    class Menu {
        sf::Font     _font;
        unsigned int _characterHeight;
        sf::Color    _defaultFillColor;
        float        _defaultOutlineThickness;
        sf::Color    _defaultOutlineColor;
        sf::Color    _selectedFillColor;
        float        _selectedOutlineThickness;
        sf::Color    _selectedOutlineColor;
        int          _selectedIdx{0};

        std::vector<sf::Text> _items;

        int _lineHeight{0};

        void calculatePositions();

    public:
        Menu(sf::Font  font,
             int       characterHeight,
             sf::Color defaultFillColor,
             sf::Color selectedFillColor);
        Menu(sf::Font  font,
             int       characterHeight,
             sf::Color defaultFillColor,
             float     defaultOutlineThickness,
             sf::Color defaultoutlineColor,
             sf::Color selectedFillColor,
             float     selectedOutlineThickness,
             sf::Color selectedOutlineColor);

        void addItem(const char* text);

        void         setCharacterHeight(unsigned int height);
        unsigned int getCharacterHeight() const;
        void         setDefaultFillColor(sf::Color color);
        sf::Color    getDefaultFillColor() const;
        void         setDefaultOutlineThickness(float thickness);
        float        getDefaultOutlineThickness() const;
        void         setDefaultOutlineColor(sf::Color color);
        sf::Color    getDefaultOutlineColor() const;
        void         setSelectedFillColor(sf::Color color);
        sf::Color    getSelectedFillColor() const;
        void         setSelectedOutlineThickness(float thickness);
        float        getSelectedOutlineThickness() const;
        void         setSelectedOutlineColor(sf::Color color);
        sf::Color    getSelectedOutlineColor() const;
    };
}
