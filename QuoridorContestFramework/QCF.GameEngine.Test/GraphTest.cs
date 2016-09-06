using FluentAssertions;
using QCF.GameEngine.Analysis;
using Xunit;

namespace QCF.GameEngine.Test
{
    
    public class GraphTest
    {
        [Fact]
        public void Graph_HasCorrectNodeCount_AfterGeneration()
        {
            var graph = new Graph();

            graph.Nodes.Count.Should().Be(81);
        }
    }
}
