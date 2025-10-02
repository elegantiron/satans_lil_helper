cmake_minimum_required(VERSION 3.30)
project(satanassets)
add_library(${PROJECT_NAME})
target_compile_features(
    ${PROJECT_NAME}
    PRIVATE
    c_std_23
)
target_sources(
    ${PROJECT_NAME}
    PUBLIC
    SatanAssets.h
    PRIVATE
    SatanAssets.c
)