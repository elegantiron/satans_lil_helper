namespace SatansLilHelper.Interfaces;

internal interface IKill
{
    int Alpha { get; }
    int Normal { get; }
    void AddKill(bool isAlpha);
    void RemoveKill(bool isAlpha);
}
