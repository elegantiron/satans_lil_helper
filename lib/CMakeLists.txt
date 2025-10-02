cmake_minimum_required(VERSION 3.30)
project(
    libsatan
    VERSION 0.0.1
    LANGUAGES CXX
)
add_library(${PROJECT_NAME})
target_compile_features(
    ${PROJECT_NAME}
    PRIVATE
    cxx_std_23
)
target_link_libraries(
    ${PROJECT_NAME}
    PUBLIC
    SDL3::SDL3
    SDL3_ttf::SDL3_ttf
)
target_include_directories(
    ${PROJECT_NAME}
    PUBLIC
    "${CMAKE_CURRENT_SOURCE_DIR}/inc"
    PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/inc-priv"
)
target_sources(
    ${PROJECT_NAME}
    PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/src/Engine/Core.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/Engine/Core.impl.cpp"
)