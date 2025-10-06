#include "Engine/Core.hpp"
#include "Scenes/Title.hpp"
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
    std::shared_ptr<Scenes::Title> pTitle;
    core.setNextScene(pTitle);
    core.run();
    return 0;
}
