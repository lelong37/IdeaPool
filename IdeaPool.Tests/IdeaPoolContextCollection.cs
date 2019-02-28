#region

using IdeaPool.Domain.Models;
using Xunit;

#endregion

namespace IdeaPool.Tests
{
    [CollectionDefinition(nameof(IdeaPoolContext))]
    public class IdeaPoolContextCollection: ICollectionFixture<IdeaPoolContextTextFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}