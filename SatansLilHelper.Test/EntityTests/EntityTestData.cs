using System;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Test.EntityTests;

public static class EntityTestData
{
    public static IEnumerable<ResourceID> ResourceIDs()
    {
        foreach (ResourceID id in Enum.GetValues(typeof(ResourceID)))
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
