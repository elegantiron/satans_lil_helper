#include "Engine/Core.hpp"
#include "Scenes/Title.hpp"

#include <boost/locale.hpp>

using namespace SatansLilHelper;

int main() {
    using namespace boost::locale;
    generator gen;
    gen.add_messages_path("./i18n");
    gen.add_messages_domain("slh");
    std::locale::global(gen(""));
    Engine::Core& core       = Engine::Core::getInstance();
    bool          successful = false;
    core.init(sf::VideoMode(Constants::WindowSize),
              Constants::Strings::Title.str(),
              successful);
    if (!successful) {
        return 1;
    }
    std::shared_ptr<Scenes::Title> pTitle{std::make_shared<Scenes::Title>()};
    core.setNextScene(pTitle);
    core.run();
    return 0;
}
