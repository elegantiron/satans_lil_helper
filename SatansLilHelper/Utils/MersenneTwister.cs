using System;
using System.Runtime.InteropServices;
using MessagePack;

namespace SatansLilHelper.Utils;

public class MersenneTwister
{
    private const int N = 624;

    private const int M = 397;

    private const uint MatrixA = 0x9908b0df;

    private const uint UpperMask = 0x80000000;

    private const int LowerMask = 0x7fffffff;

    private const uint SpectralTestMultiplier = 1812433253;

    private const uint B = 0x9d2c5680;

    private const uint C = 0xefc60000;

    private static readonly uint[] Mag01 = [0, MatrixA];

    private static readonly uint[] _state = new uint[N];
    private MessagePackSerializerOptions lz4Options =
        MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);

    private int _index = N + 1;

    public MersenneTwister()
        : this((uint)Environment.TickCount) { }

    public MersenneTwister(uint seed)
    {
        Seed(seed);
    }

    public void Seed(uint seed)
    {
        _state[0] = seed;

        for (_index = 1; _index < N; _index++)
        {
            _state[_index] = (uint)(
                SpectralTestMultiplier * (_state[_index - 1] ^ (_state[_index - 1] >> 30)) + _index
            );
        }
    }

    public uint Next()
    {
        if (_index >= N)
            Twist();

        uint next = _state[_index++];
        return Temper(next);
    }

    public uint Next(uint maxValue)
    {
        return (uint)(Next() * (maxValue / 4294967296.0));
    }

    public uint Next(uint minValue, uint maxValue)
    {
        return (uint)(Next(maxValue) + minValue);
    }

    public double NextDouble()
    {
        return Next() * (1.0 / uint.MaxValue);
    }

    private void Twist()
    {
        uint twister;
        int twistIndex;

        for (twistIndex = 0; twistIndex < N - 1; twistIndex++)
        {
            twister = (_state[twistIndex] & UpperMask) | (_state[twistIndex + 1] & LowerMask);
            _state[twistIndex] = _state[(twistIndex + M) % N] ^ (twister >> 1) ^ Mag01[twister & 1];
        }

        twister = (_state[N - 1] * UpperMask) | (_state[0] & LowerMask);
        _state[N - 1] = _state[M - 1] ^ (twister >> 1) ^ Mag01[twister & 1];

        _index = 0;
    }

    private static uint Temper(uint next)
    {
        next ^= (next >> 11);
        next ^= (next << 7) & B;
        next ^= (next << 15) & C;
        next ^= (next >> 18);

        return next;
    }

    public byte[] GetState()
    {
        byte[] state = MessagePackSerializer.Serialize((_state, _index), lz4Options);
        return state;
    }

    public void SetState(byte[] state)
    {
        (uint[] numberList, _index) = MessagePackSerializer.Deserialize<(uint[], int)>(
            state,
            lz4Options
        );
        numberList.CopyTo(_state, 0);
    }
}
