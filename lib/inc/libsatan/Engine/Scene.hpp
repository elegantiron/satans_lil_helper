#ifndef LIBSATAN_ENGINE_SCENE_HPP
#define LIBSATAN_ENGINE_SCENE_HPP

#include <memory>

namespace libsatan::Engine {
    enum class SceneResult
    {
        SUCCESS,
        CONTINUE,
        FAILURE
    };

    class Scene {
        friend class Core;
        virtual SceneResult update()     = 0;
        virtual void        draw() const = 0;
        virtual SceneResult event()      = 0;
        virtual void        init();
        virtual void        onBury();
        virtual void        onReveal();
    };

    using ScenePtr = std::shared_ptr<Scene>;
}

#endif
