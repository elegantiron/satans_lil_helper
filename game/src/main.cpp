#include "Engine/Core.hpp"
#include "Scenes/Title.hpp"
using namespace SatansLilHelper;
using namespace boost::locale;

int main() {
    generator gen;
    gen.add_messages_path("./l10n");
    gen.add_messages_domain("slh");
    gen.set_default_messages_domain("slh");
    std::locale::global(gen(""));
    Engine::Core& core       = Engine::Core::getInstance();
    bool          successful = false;
    core.init(sf::VideoMode(Constants::WindowSize),
              Constants::Strings::Title.str(core.gen()),
              successful);
    if (!successful) {
        return 1;
    }
    std::shared_ptr<Scenes::Title> pTitle{std::make_shared<Scenes::Title>()};
    core.setNextScene(pTitle);
    core.run();
    return 0;
}
