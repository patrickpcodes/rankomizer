using NetArchTest.Rules;
using Shouldly;


namespace Rankomizer.Tests.Architecture.Layers;

public class LayerTests : BaseTest
{
    [Fact]
    public void Domain_Should_NotHaveDependencyOnApplication()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
                                 .Should()
                                 .NotHaveDependencyOn("Application")
                                 .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}