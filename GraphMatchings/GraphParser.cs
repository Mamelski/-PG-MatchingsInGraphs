using System;

namespace GraphMatchings
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    
    public class GraphParser
    {
        public int[,] Parse(string pathToFile)
        {
            CheckIfFileExists(pathToFile);

            int[,] graph;

            using (var reader = File.OpenText(pathToFile))
            {
                var numberOfNodes = ParseNumberOfNodesFromFile(reader, pathToFile);

                graph = new AdjacencyMatrixGraph(numberOfNodes);

                for (var i = 0; i < numberOfNodes; ++i)
                {
                    var line = ReadLineFromFile(reader, i, numberOfNodes, pathToFile);

                    var nodeId = ParseNodeIdFromLine(line, pathToFile);

                    var neighbours = ParseNeighboursFromFile(line, pathToFile);               

                    foreach (var neighbourId in neighbours)
                    {
                        graph[nodeId, neighbourId] = 1;
                        graph[neighbourId, nodeId] = 1;
                    }
                }
            }

            return graph;
        }
        
        private void CheckIfFileExists(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File \"{path}\" does not exist.");
            }
        }
        
        private int ParseNumberOfNodesFromFile(StreamReader reader, string path)
        {
            var numberOfNodesLine = reader.ReadLine();
            if (string.IsNullOrEmpty(numberOfNodesLine))
            {
                throw new GraphMatchingsException($"First line in file \"{path}\" is empty. It should contain number of Nodes");
            }

            var isParseSuccessfull = int.TryParse(numberOfNodesLine, out var numberOfNodes);

            if (!isParseSuccessfull)
            {
                throw new GraphMatchingsException($"Could not parse first line \"{numberOfNodesLine}\" from file \"{path}\" to int. This line should represent number of Nodes.");
            }

            return numberOfNodes;
        }

        /// <summary>
        /// The read line from file.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="i">
        /// The i.
        /// </param>
        /// <param name="numberOfNodes">
        /// The number of Nodes.
        /// </param>
        /// <param name="pathToFile">
        /// The path to file.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ReadLineFromFile(StreamReader reader, int i, int numberOfNodes, string pathToFile)
        {
            var line = reader.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
                throw new GraphMatchingsException($"Line {i} in file {pathToFile} is null or empty. Expecting {numberOfNodes} lines.");
            }

            return line;
        }

        /// <summary>
        /// The parse node id from line.
        /// </summary>
        /// <param name="line">
        /// The line.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int ParseNodeIdFromLine(string line, string path)
        {
            var nodes = line.Split(' ');
            var nodeNumberString = nodes.First();

            var isParseSuccessfull = int.TryParse(nodeNumberString, out var nodeId);

            if (!isParseSuccessfull)
            {
                throw new GraphMatchingsException($"Could not parse first line \"{line}\" from file \"{path}\" to int. This line should represent number of Nodes.");
            }

            return nodeId;
        }

        /// <summary>
        /// The parse neighbours from file.
        /// </summary>
        /// <param name="line">
        /// The line.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        private List<int> ParseNeighboursFromFile(string line, string path)
        {
            var splitedLine = line.Split(' ');
            var neighbours = new List<int>();

            // We are starting from 1 because first number in the line is the node and neighbours start from 1.
            for (int i = 1; i < splitedLine.Length; ++i)
            {
                var isParseSuccessfull = int.TryParse(splitedLine[i], out var nodeId);

                if (!isParseSuccessfull)
                {
                    throw new GraphMatchingsException($"Could not parse character \"{splitedLine[i]}\" from file \"{path}\" to int. This string should represent node.");
                }

                neighbours.Add(nodeId);
            }

            return neighbours;
        }
    }
}