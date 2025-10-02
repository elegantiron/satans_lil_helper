#pragma once
#include "libcakes/Engine/Scene.hpp"

#include <SDL3/SDL_video.h>
#include <memory>

namespace libcakes::Engine {
    class Core {
        class impl;
        static std::unique_ptr<impl> pIpml;
        Core() {}

    public:
        static void run(const char*     title,
                        int             width,
                        int             height,
                        SDL_WindowFlags flags);
        static void setNextScene(ScenePtr pScene);
    };
}
