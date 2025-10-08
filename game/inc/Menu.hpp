#pragma once
#include <list>

namespace SatansLilHelper {
    class Menu {
        sf::Font            _font;
        int                 _characterHeight;
        sf::Color           _fillColor;
        sf::Color           _outlineColor;
        float               _outlineThickness;
        std::list<sf::Text> _items;

        int _lineHeight{0};

        void calculatePositions();

    public:
        Menu(sf::Font  font,
             int       characterHeight,
             sf::Color fillColor,
             sf::Color outlineColor     = sf::Color::Black,
             float     outlineThickness = 0);
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
