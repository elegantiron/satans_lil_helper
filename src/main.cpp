#include <SFML/Window/VideoMode.hpp>
#include <libslh/Engine/Core.hpp>
#include <libslh/Engine/Scene.hpp>
#include <memory>
#include "Scenes/Title.hpp"
using namespace SatansLilHelper;
using namespace boost::locale;
using namespace libslh;

int main() {
    Engine::Core& core       = Engine::Core::getInstance();
    bool          successful = false;
    core.init(sf::VideoMode(Constants::WindowSize),
              core.localize(Constants::Strings::Title),
              successful);
    if (!successful) {
        return 1;
    }
    std::shared_ptr<Scenes::Title> pTitle{std::make_shared<Scenes::Title>()};
    core.setNextScene(pTitle);
    core.run();
    return 0;
}
