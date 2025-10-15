cmake_minimum_required(VERSION 3.30)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)
set(SFML_USE_SYSTEM_DEPS OFF)
project(
    SatansLilHelper
    VERSION 0.0.1
    LANGUAGES C CXX
)
set(CMAKE_PREFIX_PATH "c:/msys64/ucrt64")
if(${PROJECT_IS_TOP_LEVEL})
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_LIST_DIR}/bin)
endif()
find_package(Boost 1.80 COMPONENTS locale REQUIRED)
find_package(Gettext REQUIRED)
include(FetchContent)
FetchContent_Declare(
    SFML
    GIT_REPOSITORY https://github.com/SFML/SFML.git
    GIT_TAG 3.0.2
    GIT_SHALLOW ON
    EXCLUDE_FROM_ALL
    SYSTEM
)
FetchContent_Declare(
    EnTT
    GIT_REPOSITORY https://github.com/skypjack/entt.git
    GIT_TAG v3.15.0
    GIT_SHALLOW ON
    EXCLUDE_FROM_ALL
    SYSTEM
)
FetchContent_MakeAvailable(
    SFML
    EnTT
)
add_executable(${PROJECT_NAME})
set_target_properties(
    ${PROJECT_NAME}
    PROPERTIES
    CXX_EXTENSIONS OFF
)
target_link_libraries(
    ${PROJECT_NAME}
    PRIVATE
    SFML::Graphics
    Boost::locale
    libslh
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
    -Wall -pedantic-errors
)
if("${CMAKE_BUILD_TYPE}" STREQUAL "Debug")
target_compile_options(
    ${PROJECT_NAME}
    PRIVATE
    -ggdb
)
elseif("${CMAKE_BUILD_TYPE}" STREQUAL "Release")
target_compile_options(
    ${PROJECT_NAME}
    PRIVATE
    -O3 -DNDEBUG
)
endif()
target_precompile_headers(
    ${PROJECT_NAME}
    PRIVATE
    "${CMAKE_CURRENT_SOURCE_DIR}/inc/Constants.hpp"
    "${CMAKE_CURRENT_SOURCE_DIR}/inc/Assets.hpp"
)
set(
    SOURCES
    "src/Constants/Numbers.cpp"
    "src/Constants/Strings.cpp"
    "src/Scenes/MainMenu.cpp"
    "src/Scenes/Title.cpp"
    "src/main.cpp"
)
set(
    ASSETS
    "src/Assets/Fonts.cpp"
    "src/Assets/Textures/Satan.cpp"
)
target_sources(
    ${PROJECT_NAME}
    PRIVATE
    ${SOURCES}
    ${ASSETS}
)

include(cmake/Gettext_helpers.cmake)
set(DOMAIN_NAME "slh")
configure_gettext(
    DOMAIN ${DOMAIN_NAME}
    TARGET_NAME "slh_i18n"
    SOURCES ${SOURCES}
    POTFILE_DESTINATION "i18n"
    GMOFILE_DESTINATION "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}/l10n"
    XGETTEXT_ARGS
        "--keyword=_" "--keyword=C_:1c,2"
        "--keyword=P_:1,2" "-F" "--sentence-end=single-space"
        "--package-name=${PROJECT_NAME}" "--package-version=${PROJECT_VERSION}"
        "--copyright-holder=SmuB Design" "--msgid-bugs-address=elegant.iron3167@fastmail.com"
    LANGUAGES "en_US.UTF-8"
)

add_subdirectory("deps/libslh")