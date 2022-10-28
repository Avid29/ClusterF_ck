// Adam Dernis © 2022

using ClusterF_ck.Spaces;
using ClusterF_ck.Spaces.Properties;
using System.Collections.Generic;
using System.Linq;

namespace ClusterF_ck.Intermediate.Boxing
{
    /// Boxing, or Box Clustering, is my name for this algorithm since I couldn't find one.
    /// Box Clustering is an extremely naive grid based clustering method.
    /// 
    /// The area is split into a grid of evenly sized segments. The average of all points in each segment is taken.
    /// The resulting average of each segment is treated as a cluster.
    /// 
    /// For example, let's imagine this grid:
    /// + - - - - - - - - - - - +
    /// |     x     x           |
    /// |          x x          |
    /// | x         x     x     |
    /// |                       |
    /// | x       x             |
    /// |                       |
    /// |     x     x     x x x |
    /// |                       |
    /// |    x    xxx           |
    /// |         xxx           |
    /// |    x    xxx           |
    /// + - - - - - - - - - - - +
    /// 
    /// The grid could then be split into segments like this:
    /// + - - - + - - - + - - - +
    /// |     x |   x   |       |
    /// |       |  x x  |       |
    /// | x     |   x   | x     |
    /// + - - - + - - - + - - - +
    /// | x     | x     |       |
    /// |       |       |       |
    /// |     x |   x   | x x x |
    /// + - - - + - - - + - - - +
    /// |    x  | xxx   |       |
    /// |       | xxx   |       |
    /// |    x  | xxx   |       |
    /// + - - - + - - - + - - - +
    /// 
    /// Which could be clustered into a single value per segment, with a weight of the number of clusters:
    /// + - - - + - - - + - - - +
    /// |       |       |       |
    /// |   2   |   4   |       |
    /// |       |       | 1     |
    /// + - - - + - - - + - - - +
    /// |       |       |       |
    /// |   2   |  2    |       |
    /// |       |       |   3   |
    /// + - - - + - - - + - - - +
    /// |       |       |       |
    /// |    2  |  9    |       |
    /// |       |       |       |
    /// + - - - + - - - + - - - +

    /// <summary>
    /// A static class containing Box Clustering methods.
    /// </summary>
    public static class Boxing
    {
        public static List<BoxingCluster<T, TCell, TShape>> Cluster<T, TCell, TShape>(
            T[] points,
            double window,
            TShape shape = default)
            where T : unmanaged
            where TCell : unmanaged
            where TShape : struct, IGridSpace<T, TCell>, IAverageSpace<T>
        {
            var cells = new Dictionary<TCell, BoxingCluster<T, TCell, TShape>>();
            foreach (var point in points)
            {
                TCell cell = shape.GetCell(point, window);
                if (!cells.ContainsKey(cell))
                {
                    var center = shape.GetCellCenter(cell, window);
                    cells.Add(cell, new BoxingCluster<T, TCell, TShape>(cell, center, shape));
                }

                cells[cell].Points.Add(point);
            }

            var clusters = new List<BoxingCluster<T, TCell, TShape>>();
            foreach(var cluster in cells.Values)
            {
                clusters.Add(cluster);
            }

            return clusters;
        }
    }
}
