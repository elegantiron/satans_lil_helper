#include "Scenes/Title.hpp"

#include <libslh/Engine/Core.hpp>

int main()
{
    using namespace SatansLilHelper;
    using Core = libslh::Engine::Core;
    std::shared_ptr<Scenes::Title> pTitle{std::make_shared<Scenes::Title>()};
    Core&                          core = Core::getInstance();
    core.run(Constants::Title, Constants::WindowSize, pTitle);

    return 0;
}
