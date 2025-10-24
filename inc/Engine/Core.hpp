#pragma once
#include "Engine/SceneManager.hpp"
#include "Graphics/WindowManager.hpp"

namespace SatansLilHelper {
    class Core {
        static Core* _instance;
        Core()  = default;
        ~Core() = default;

    public:
        static Core& getInstance();
        void         init(sf::VideoMode mode, const sf::String& windowTitle);
        void         run();
        void         run(ScenePtr firstScene);
#pragma region Window
    private:
        WindowManager _winMan;

    public:
        void         setWindowSize(sf::Vector2u size);
        sf::Vector2u getWindowSize() const;
        void         setWindowTitle(const sf::String& title);
        sf::String   getWindowTitle();
#pragma endregion

#pragma region Scene
    private:
        SceneManager _sceneMan;

    public:
        void     setNextScene(ScenePtr nextScene);
        ScenePtr getCurrentScene() const;
#pragma endregion
    };
} // namespace SatansLilHelper
