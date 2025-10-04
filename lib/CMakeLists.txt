cmake_minimum_required(VERSION 3.30)
project(
    libsatan
    VERSION 0.0.1
    LANGUAGES CXX
)
add_library(${PROJECT_NAME})

find_package(Doxygen)
if(Doxygen_FOUND)
    set(DOXYGEN_CREATE_SUBDIRS YES)
    set(DOXYGEN_BUILTIN_STL_SUPPORT YES)
    set(DOXYGEN_TIMESTAMP DATETIME)
    set(DOXYGEN_RESOLVE_UNNAMED_PARAMS YES)
    set(DOXYGEN_SORT_MEMBERS_CTORS_1ST YES)
    set(DOXYGEN_RECURSIVE YES)
    set(DOXYGEN_INLINE_SOURCES NO)
    set(DOXYGEN_REFERENCED_BY_RELATION YES)
    set(DOXYGEN_REFERENCES_RELATION YES)
    set(DOXYGEN_HTML_COLORSTYLE DARK)
    set(DOXYGEN_HTML_DYNAMIC_SECTIONS YES)
    set(DOXYGEN_HTML_PROJECT_COOKIE hdfsauhiewalfahfsdjleuawibhvjdsafueiwovhjcbkz)
    set(DOXYGEN_STRIP_FROM_INC_PATH ${CMAKE_CURRENT_SOURCE_DIR}/inc)
    set(DOXYGEN_STRIP_FROM_PATH ${CMAKE_CURRENT_SOURCE_DIR})
    doxygen_add_docs(
        ${PROJECT_NAME}_docs
    )
endif()

target_compile_features(
    ${PROJECT_NAME}
    PRIVATE
    cxx_std_23
)
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
target_compile_options(
    ${PROJECT_NAME}
    PRIVATE
    -Wall -pedantic
)
target_sources(
    ${PROJECT_NAME}
    PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/src/Engine/Game.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/Engine/Scene.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/Engine/SceneManager.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/Graphics/WindowManager.cpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/src/System/Clock.cpp"
)