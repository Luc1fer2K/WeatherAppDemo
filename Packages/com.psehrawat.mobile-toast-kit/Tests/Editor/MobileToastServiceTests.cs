using NUnit.Framework;
using MobileToastKit;

public class MobileToastServiceTests
{
    [Test]
    public void Show_DoesNotThrow_InEditor()
    {
        Assert.DoesNotThrow(() => MobileToastService.Show("Test toast"));
    }
}
