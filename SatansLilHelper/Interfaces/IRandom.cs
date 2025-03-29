namespace SatansLilHelper.Interfaces;

internal interface IRandom
{
    void SetState(byte[] data);
    byte[] GetState();
    uint Next();
    void Seed(uint seed);
    void Seed(int seed);
    uint Next(uint maxValue);
    uint Next(uint minValue, uint maxValue);
    double NextDouble();
}
