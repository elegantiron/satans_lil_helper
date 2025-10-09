cmake_minimum_required(VERSION 3.30)
set(BUILD_SHARED_LIBS ON)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_LIST_DIR}/bin)
set(FETCHCONTENT_BASE_DIR ${CMAKE_CURRENT_LIST_DIR}/deps)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
set(BOOST_INCLUDE_LIBRARIES locale json)
set(BOOST_ENABLE_CMAKE ON)
project(game)
include(FetchContent)
# FetchContent_Declare(
#     SFML
#     GIT_REPOSITORY https://github.com/SFML/SFML.git
#     GIT_TAG 3.0.2
#     GIT_SHALLOW ON
#     EXCLUDE_FROM_ALL
#     SYSTEM
# )
FetchContent_Declare(
    SFML
    GIT_REPOSITORY https://github.com/elegantiron/SFML.git
    GIT_SHALLOW ON
    EXCLUDE_FROM_ALL
    SYSTEM
)
FetchContent_Declare(
    TinyXML
    GIT_REPOSITORY https://github.com/leethomason/tinyxml2.git
    GIT_TAG 11.0.0
    GIT_SHALLOW ON
    EXCLUDE_FROM_ALL
    SYSTEM
)
FetchContent_Declare(
    boost
    GIT_REPOSITORY https://github.com/boostorg/boost.git
    GIT_TAG boost-1.89.0
    GIT_SHALLOW ON
    EXCLUDE_FROM_ALL
    SYSTEM
)
FetchContent_MakeAvailable(
    SFML
    TinyXML
    boost
)
add_subdirectory("assets")
add_subdirectory("game")