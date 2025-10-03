#include "libcakes/Engine/Core.hpp"

#include "libcakes/Engine/Core.impl.hpp"

namespace libcakes::Engine {
    void Core::run(const char*     title,
                   int             width,
                   int             height,
                   SDL_WindowFlags flags,
                   ScenePtr        pScene)
    {
        pImpl->run(title, width, height, flags, pScene);
    }

    void Core::setNextScene(ScenePtr pScene)
    {
        pImpl->setNextScene(pScene);
    }

    void Core::setBackgroundColor(Color color)
    {
        pImpl->setBackgroundColor(color);
    }

    std::unique_ptr<Core::impl> Core::pImpl{std::make_unique<Core::impl>()};

    void Core::enableSceneMultiset()
    {
        pImpl->enableSceneMultiset();
    }
}
