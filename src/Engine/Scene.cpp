#include "Engine/Scene.hpp"

#include <utility>

namespace SatansLilHelper {
    void Scene::init() {}

    void Scene::onBury() {}

    void Scene::onReveal() {}

    Scene::Scene(ScenePtr parent) : _parent(std::move(parent)) {}

    Scene::~Scene() {}
}
