namespace SatansLilHelper.Interfaces;

internal interface IRandom
{
    void SetState(byte[] data);
    byte[] GetState();
    uint Next(uint maxValue);
    uint Next(uint minValue, uint maxValue);
}
