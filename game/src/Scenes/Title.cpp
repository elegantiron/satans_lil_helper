#include "Scenes/Title.hpp"

#include "Engine/Core.hpp"

namespace Scenes {
void Title::update(const GameTime& gametime,
                   bool&           successful,
                   bool&           keepRunning) {
    successful = keepRunning = true;
}

void Title::handleEvent(SDL_Event* event, bool& successful, bool& keepRunning) {

}

void Title::init() {
    Core& core = Core::getInstance();
    core.setOptionValue(GameSettings::ID::EXIT_ON_ESCAPE, true);
}

void Title::onReveal() {
    Core& core = Core::getInstance();
    core.setOptionValue(GameSettings::ID::EXIT_ON_ESCAPE, true);
}
}
