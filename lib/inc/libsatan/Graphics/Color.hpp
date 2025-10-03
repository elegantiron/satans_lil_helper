#pragma once
#include <cstdint>
namespace libsatan {
    /**
     * @brief This struct represents a color for use with SDL.
     *
     */
    struct Color {
        /**
         * @brief Red channel value
         *
         */
        uint8_t r{0};
        /**
         * @brief Green channel value
         *
         */
        uint8_t g{0};
        /**
         * @brief Blue channel value
         *
         */
        uint8_t b{0};
        /**
         * @brief Alpha channel value
         *
         */
        uint8_t a{255};
    };
}
