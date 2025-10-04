#ifndef LIBSATAN_SYSTEM_GAMETIME_HPP
#define LIBSATAN_SYSTEM_GAMETIME_HPP

#include <chrono>

namespace libsatan::System {
    using Duration = std::chrono::duration<long double,
                                           std::chrono::milliseconds::period>;
    struct GameTime {
        Duration lastFrame;
        Duration totalElapsedTime;
    };
}

#endif
