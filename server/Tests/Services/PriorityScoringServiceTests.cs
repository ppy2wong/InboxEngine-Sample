using Xunit;
using InboxEngine.Models;
using InboxEngine.Services;

namespace InboxEngine.Tests.Services;

public class PriorityScoringServiceTests
{
    private readonly IPriorityScoringService _service;

    public PriorityScoringServiceTests()
    {
        _service = new PriorityScoringService();
    }

    [Fact]
    public void TestSetup_ShouldPass()
    {
        // This is a dummy test to verify the test infrastructure is working.
        // If this test fails after you add your own tests, you know it's something
        // in your code that broke the build.
        Assert.True(true);
    }
    
}

