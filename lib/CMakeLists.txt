cmake_minimum_required(VERSION 3.30)
project(
    libslh
    VERSION 0.0.1
    LANGUAGES CXX
)
add_library(${PROJECT_NAME} STATIC)
target_compile_features(
    ${PROJECT_NAME}
    PRIVATE
    cxx_std_23
)
if(${PROJECT_IS_TOP_LEVEL})
    target_compile_options(
        ${PROJECT_NAME}
        PRIVATE
        -Wall -pedantic
    )
endif()
if(${CMAKE_BUILD_TYPE} STREQUAL "Debug")
target_compile_options(
    ${PROJECT_NAME}
    PRIVATE
    -glldb
)
target_compile_definitions(
    ${PROJECT_NAME}
    PRIVATE
    DEBUG=1
)
endif()
target_link_libraries(
    ${PROJECT_NAME}
    PUBLIC
    SFML::Graphics
)
target_include_directories(
    ${PROJECT_NAME}
    PUBLIC
    "inc"
)
target_sources(
    ${PROJECT_NAME}
    PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/src/Engine/Core.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/Engine/SceneManager.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/System/Clock.cpp"
)