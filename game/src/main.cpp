#include "Engine/Core.hpp"
using namespace SatansLilHelper;

int main() {
    Engine::Core& core       = Engine::Core::getInstance();
    bool          successful = false;
    core.init(sf::VideoMode(Constants::WindowSize),
              Constants::Title,
              successful);
    if (!successful) {
        return 1;
    }
    core.run();
    return 0;
}
