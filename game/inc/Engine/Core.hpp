#pragma once
#include "Engine/GameSettings.hpp"
#include "Engine/SceneManager.hpp"
#include "Utils/Clock.hpp"

#include <SDL3/SDL_render.h>
#include <memory>

class Core {
    friend std::unique_ptr<Core> std::make_unique<Core>();
    Core() = default;
    static std::unique_ptr<Core> _instance;
    SDL_Window*                  _window{nullptr};
    SDL_Renderer*                _renderer{nullptr};
    SceneManager                 _sceneMan;
    Clock                        _clock;
    GameSettings                 _settings;

public:
    static Core&        getInstance();
    void                init(bool& successful);
    SDL_Window* const   getWindow() const;
    SDL_Renderer* const getRenderer() const;
    void                draw();
    void handleEvent(SDL_Event* event, bool& successful, bool& keepRunning);
    void iterate(bool& successful, bool& keepRunning);
    void setNextScene(ScenePtr pScene, bool immediate = false);
    void setOptionValue(GameSettings::ID id, bool value);
};
