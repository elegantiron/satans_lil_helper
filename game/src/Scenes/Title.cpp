#include "Scenes/Title.hpp"

extern const Asset FairyDust;

namespace SatansLilHelper::Scenes {
    void Title::init() {
        _title.setString(Constants::Title);
    }

    Title::Title()
        : _font(FairyDust.data, FairyDust.size), _title(_font),
          _pressStart(_font) {}
}
