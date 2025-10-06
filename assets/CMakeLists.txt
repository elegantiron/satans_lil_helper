cmake_minimum_required(VERSION 3.30)
project(satanassets)
add_library(${PROJECT_NAME} SHARED)
target_compile_features(
    ${PROJECT_NAME}
    PRIVATE
    c_std_23
)
target_sources(
    ${PROJECT_NAME}
    PRIVATE
    SatanAssets.c
)
target_include_directories(
    ${PROJECT_NAME}
    PUBLIC
    "${CMAKE_CURRENT_SOURCE_DIR}/inc"
)