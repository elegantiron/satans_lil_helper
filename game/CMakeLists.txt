cmake_minimum_required(VERSION 3.30)
project(
    SatansLilHelper
    VERSION 0.0.1
    LANGUAGES C CXX
)
add_executable(${PROJECT_NAME})
target_link_libraries(
    ${PROJECT_NAME}
    PRIVATE
    SFML::Graphics
)
target_include_directories(
    ${PROJECT_NAME}
    PRIVATE
    inc
)
target_compile_features(
    ${PROJECT_NAME}
    PRIVATE
    c_std_23
    cxx_std_26
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
elseif(${CMAKE_BUILD_TYPE} STREQUAL "Release")
target_compile_options(
    ${PROJECT_NAME}
    PRIVATE
    -O3 -DNDEBUG
)
endif()
target_precompile_headers(
    ${PROJECT_NAME}
    PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/inc/Assets.hpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/inc/Constants.hpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/inc/Engine/Core.hpp"
)
target_sources(
    ${PROJECT_NAME}
    PRIVATE
    "src/Constants/Numbers.cpp"
    "src/Constants/Strings.cpp"
    "src/Engine/Core.cpp"
    "src/Engine/SceneManager.cpp"
    "src/Scenes/MainMenu.cpp"
    "src/Scenes/Title.cpp"
    "src/Clock.cpp"
    "src/Menu.cpp"
    "src/main.cpp"
)