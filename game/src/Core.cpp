#include "Engine/Core.hpp"

#include <SDL3/SDL_init.h>
#include <SDL3/SDL_log.h>
#include <SDL3_image/SDL_image.h>
#include <SDL3_ttf/SDL_ttf.h>

void Core::init(bool& successful) {
    successful = true;
    SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_NAME_STRING,
                               Constants::Title);
    SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_CREATOR_STRING,
                               Constants::Company);
    SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_COPYRIGHT_STRING,
                               Constants::Copyright);
    SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_IDENTIFIER_STRING,
                               Constants::Identifier);
    SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_VERSION_STRING,
                               Constants::Version);

    if (!SDL_Init(SDL_INIT_VIDEO)) {
        SDL_Log("Couldn't initialize SDL: %s", SDL_GetError());
        successful = false;
        return;
    }
    _window = SDL_CreateWindow(Constants::Title,
                               Constants::WindowWidth,
                               Constants::WindowHeight,
                               0);
    if (_window == nullptr) {
        successful = false;
        SDL_Log("Couldn't create a window: %s", SDL_GetError());
        return;
    }
    _renderer = SDL_CreateRenderer(_window, nullptr);
    if (_renderer == nullptr) {
        successful = false;
        SDL_Log("Couldn't initialize the renderer: %s", SDL_GetError());
        return;
    }
    if (!TTF_Init()) {
        successful = false;
        SDL_Log("Couldn't initialize sdl_ttf: %s", SDL_GetError());
    }
}

void Core::handleEvent(SDL_Event* event, bool& successful, bool& keepRunning) {
    successful  = true;
    keepRunning = true;
    if (event->type == SDL_EVENT_QUIT) {
        keepRunning = false;
        return;
    }
    if (_settings.testOptionValue(GameSettings::ID::EXIT_ON_ESCAPE)
        && event->type == SDL_EVENT_KEY_DOWN) {
        if (event->key.scancode == SDL_SCANCODE_ESCAPE) {
            keepRunning = false;
            return;
        }
    } else {
        _sceneMan.handleEvent(event, successful, keepRunning);
    }
}

void Core::iterate(bool& successful, bool& keepRunning) {
    _sceneMan.update(_clock.newFrame(), successful, keepRunning);
}

std::unique_ptr<Core> Core::_instance{std::make_unique<Core>()};

Core& Core::getInstance() {
    return *_instance;
}

void Core::setNextScene(ScenePtr pScene, bool immediate) {
    _sceneMan.setNextScene(pScene, immediate);
}

void Core::setOptionValue(GameSettings::ID id, bool value) {
    _settings.setOptionValue(id, value);
}
