using SatansLilHelper.Constants;

namespace SatansLilHelper.Test.EntityTests;

public static class EntityTestData
{
    public static IEnumerable<AbilityID> ResourceIDs()
    {
        foreach (AbilityID id in Enum.GetValues(typeof(AbilityID)))
        {
            yield return id;
        }
    }

    public static IEnumerable<AbilityID> AbilityIDs()
    {
        foreach (AbilityID id in Enum.GetValues(typeof(AbilityID)))
        {
            yield return id;
        }
    }
}
