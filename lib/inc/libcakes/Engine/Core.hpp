#pragma once
#include "libcakes/Color.hpp"
#include "libcakes/Engine/Scene.hpp"

#include <SDL3/SDL_video.h>
#include <memory>

/**
 * @brief Contains classes and types for running and managing a game.
 *
 */
namespace libcakes::Engine {
    /**
     * @brief Main class for running a game.
     * @details All members are static, so they can be called from anywhere.
     */
    class Core {
        class impl;
        static std::unique_ptr<impl> pImpl;
        Core() {}

    public:
        /**
         * @brief Starts a game
         *
         * @param title Title for the window
         * @param width Width of the window
         * @param height Height of the window
         * @param flags Flags to use when creating the window
         * @param pScene First scene for the game to have on load
         */
        static void run(const char*     title,
                        int             width,
                        int             height,
                        SDL_WindowFlags flags,
                        ScenePtr        pScene = nullptr);

        /**
         * @brief Set the Next Scene to use for the game
         *
         * @param pScene
         */
        static void setNextScene(ScenePtr pScene);

        /**
         * @brief Set the Background Color to use when clearing the screen
         *
         * @param color
         */
        static void setBackgroundColor(Color color);

        /// @brief Enables adding multiple scenes in one frame
        /// @details After calling this, you will be able to add multiple
        ///   scenes to the stack. Before calling this, adding a new scene to
        ///   the stack after adding another scene, but before the frame is
        ///   over, the older scene is forgotten. After calling this, adding the
        ///   second scene will immediately cause the first scene to be made the
        ///   active scene. The second scene will be made the active scene
        ///   before the next frame, as normal.
        static void enableSceneMultiset();
    };
}
