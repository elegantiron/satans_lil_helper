cmake_minimum_required(VERSION 3.30)
project(
    SatansLilHelper
    VERSION 0.0.1
    LANGUAGES CXX
)
add_executable(${PROJECT_NAME})
target_compile_features(
    ${PROJECT_NAME}
    PRIVATE
    cxx_std_23
)
target_compile_options(
    ${PROJECT_NAME}
    PRIVATE
    -Wall -pedantic
)
if(${CMAKE_BUILD_TYPE} STREQUAL "Debug")
target_compile_options(
    ${PROJECT_NAME}
    PRIVATE
    -glldb
)
endif()
target_link_libraries(
    ${PROJECT_NAME}
    PRIVATE
    SDL3::SDL3
    SDL3_image::SDL3_image
    SDL3_ttf::SDL3_ttf
    # SDL3_mixer::SDL3_mixer
)
target_precompile_headers(
    ${PROJECT_NAME}
    PRIVATE
    "inc/Constants.hpp"
)
target_include_directories(
    ${PROJECT_NAME}
    PRIVATE
    "inc"
)
target_sources(
    ${PROJECT_NAME}
    PRIVATE
    "src/main.cpp"
    "src/Core.cpp"
    "src/Engine/GameSettings.cpp"
    "src/Engine/SceneManager.cpp"
    "src/Graphics/Sprite.cpp"
    "src/Scenes/Title.cpp"
    "src/Utils/Clock.cpp"
)