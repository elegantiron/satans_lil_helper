cmake_minimum_required(VERSION 3.30)
set(BUILD_SHARED_LIBS ON)
set(SDLTTF_VENDORED ON)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/package)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
project(game)
include(FetchContent)
FetchContent_Declare(
    SDL3
    GIT_REPOSITORY https://github.com/libsdl-org/SDL.git
    GIT_TAG release-3.2.22
    GIT_SHALLOW ON
    EXCLUDE_FROM_ALL
    SYSTEM
)
FetchContent_Declare(
    SDL3_ttf
    GIT_REPOSITORY https://github.com/libsdl-org/SDL_ttf.git
    GIT_TAG release-3.2.2
    GIT_SHALLOW ON
    EXCLUDE_FROM_ALL
    SYSTEM
)
FetchContent_MakeAvailable(
    SDL3
    SDL3_ttf
)
add_subdirectory("lib")
add_subdirectory("assets")
add_subdirectory("game")