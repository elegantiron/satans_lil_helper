using SatansLilHelper.Utils;
namespace SatansLilHelper.Test;

internal class EntityCalcTests
{
    [Test]
    public async Task Mytest()
    {
        var result = EntityCalcs.DoNothing();
        await Assert.That(result).IsEqualTo(0);
    }
}
