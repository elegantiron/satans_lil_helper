cmake_minimum_required(VERSION 3.30)
set(BUILD_SHARED_LIBS ON)
set(SDLTTF_VENDORED ON)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}/package)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
project(game)
include(FetchContent)
FetchContent_Declare(
    SFML
    GIT_REPOSITORY https://github.com/SFML/SFML.git
    GIT_TAG 3.0.2
    GIT_SHALLOW ON
    EXCLUDE_FROM_ALL
    SYSTEM
)
FetchContent_MakeAvailable(
    SFML
)
add_subdirectory("lib")
add_subdirectory("assets")
add_subdirectory("game")