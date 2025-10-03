#pragma once
#include "libsatan/GameTime.hpp"

#include <SDL3/SDL.h>
#include <memory>
namespace libsatan::Engine {
    /**
     * @brief Indicates the result of various Scene functions.
     *
     */
    enum class SceneResult
    {
        /**
         * @brief Indicates the scene has finished
         *
         */
        SUCCESS,
        /**
         * @brief Indicates the scene will remain active
         *
         */
        CONTINUE,
        /**
         * @brief Indicates a non-recoverable error.
         * @details Causes the Core to initiate a dump.
         */
        FAILURE
    };

    /**
     * @brief Represents an arrangement of game parts.
     *
     */
    class Scene {
        friend class Core;

    protected:
        /**
         * @brief Handles a game event.
         *
         * @param pEvent Pointer to a raw SDL_Event
         * @return SceneResult Result of handling the event
         */
        virtual SceneResult event(SDL_Event* pEvent) = 0;
        /**
         * @brief Updates the scene.
         *
         * @param gameTime Struct containing time information about the start of
         * the frame
         * @return SceneResult Result of performing the update
         */
        virtual SceneResult update(const GameTime& gameTime) = 0;
        virtual ~Scene()                                     = default;
        /**
         * @brief Initializes the Scene.
         * @details This is **guaranteed** to be called *after* a Window
         * and Renderer have been created.
         */
        virtual void init() {}
        /**
         * @brief Called just before the scene is buried by the next one.
         *
         */
        virtual void onBury() {}
        /**
         * @brief Called just after the scene is brought to the top of the
         * stack.
         *
         */
        virtual void onReveal() {}
    };

    /**
     * @brief A pointer to a scene
     *
     */
    using ScenePtr = std::shared_ptr<Scene>;
} // namespace libsatan::Engine
