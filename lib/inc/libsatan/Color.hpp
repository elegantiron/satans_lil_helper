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
        uint8_t r;
        /**
         * @brief Green channel value
         *
         */
        uint8_t g;
        /**
         * @brief Blue channel value
         *
         */
        uint8_t b;
        /**
         * @brief Alpha channel value
         *
         */
        uint8_t a{255};
    };
}
