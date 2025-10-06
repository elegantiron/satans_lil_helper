#include "Engine/SceneManager.hpp"

void SceneManager::handleEvent(SDL_Event* event,
                               bool&      successful,
                               bool&      keepRunning) {
    successful  = true;
    keepRunning = true;
    if (_scenes.empty()) {
        keepRunning = false;
        return;
    }
    _scenes.top()->handleEvent(event, successful, keepRunning);
    if (!successful) {
        return;
    }
    if (!keepRunning) {
        _scenes.pop();
        if (_scenes.empty()) {
            return;
        }
        keepRunning = true;
        _scenes.top()->onReveal();
    }
}

void SceneManager::update(const GameTime& gameTime,
                          bool&           successful,
                          bool&           keepRunning) {
    successful  = true;
    keepRunning = true;
    if (_scenes.empty()) {
        keepRunning = false;
        return;
    }
    _scenes.top()->update(gameTime, successful, keepRunning);
    if (!successful) {
        return;
    }
    if (!keepRunning) {
        _scenes.pop();
        if (_scenes.empty()) {
            return;
        }
        keepRunning = true;
        _scenes.top()->onReveal();
    }
}

void SceneManager::setNextScene(ScenePtr pScene, bool immediate) {
    _nextScene = pScene;
    if (immediate || _scenes.empty()) {
        transitionScene();
    }
}

void SceneManager::transitionScene() {
    if (!_scenes.empty()) {
        _scenes.top()->onBury();
    }
    _nextScene->init();
    _scenes.push(std::move(_nextScene));
    _nextScene = nullptr;
}
