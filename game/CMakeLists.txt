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
    libsatan
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
    cxx_std_23
)
target_sources(
    ${PROJECT_NAME}
    PRIVATE
    "src/main.cpp"
    "src/Scenes/Title.cpp"
)