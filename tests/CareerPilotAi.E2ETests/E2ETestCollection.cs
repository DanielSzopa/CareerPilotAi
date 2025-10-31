namespace CareerPilotAi.E2ETests;

/// <summary>
/// Collection definition for E2E tests.
/// This allows multiple test classes to share the same fixture instance.
/// </summary>
[CollectionDefinition("E2E collection")]
public class E2ETestCollection : ICollectionFixture<E2ETestFixture>
{
    // This class has no code, and is never created.
    // Its purpose is simply to be the place to apply [CollectionDefinition] and all the ICollectionFixture<> interfaces.
}

