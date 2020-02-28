using Reinforcement_Learning;

using System;
using Xunit;

namespace TestLibrary
{
    public class UnitTest
    {
        [Fact]
        public void Test1()
        {
            Tile tile0 = new Tile("04");
            Assert.Equal(4, tile0.tile_integer);
            //Assert.Equal(true, tile0.discardedBy == Reinforcement_Learning.DiscardBy.NOBODY);
        }

        [Theory]
        [InlineData(4, "04")]
        [InlineData(24, "24")]
        [InlineData(30, "30")]
        public void CountInstancesCorrectly(int answer, string tile_code)
        {
            Tile tile0 = new Tile(tile_code);
            Assert.NotEqual(answer, tile0.tile_integer);
        }
    }
}
