// Adam Dernis © 2022

using ClusterF_ck.Spaces.Properties;
using System.Collections.Generic;

namespace ClusterF_ck.Intermediate.Boxing;

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
    /// <summary>
    /// Clusters a set of points into cell clusters.
    /// </summary>
    /// <typeparam name="T">The type of points to cluster.</typeparam>
    /// <typeparam name="TCell">The type of cells to </typeparam>
    /// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
    /// <param name="points">The points to cluster.</param>
    /// <param name="window">The window size for the cells.</param>
    /// <param name="shape">The shape to use on the points to cluster.</param>
    /// <remarks>
    /// <see cref="IGridSpace{T, TCell}.Window"/> of <paramref name="shape"/> will be overriden to <paramref name="window"/> as a side effect.
    /// </remarks>
    /// <returns>A sparse matrix of cell clusters.</returns>
    public static Dictionary<TCell, BoxingCluster<T, TCell, TShape>> Cluster<T, TCell, TShape>(
        T[] points,
        double window,
        TShape shape = default)
        where T : unmanaged
        where TCell : unmanaged
        where TShape : struct, IGridSpace<T, TCell>, IAverageSpace<T>
    {
        shape.Window = window;

        var cells = new Dictionary<TCell, BoxingCluster<T, TCell, TShape>>();
        foreach (var point in points)
        {
            // Get point's cell.
            TCell cell = shape.GetCell(point);

            // Create new cell cluster if cell not found.
            if (!cells.ContainsKey(cell))
            {
                var origin = shape.GetCellOrigin(cell);
                cells.Add(cell, new BoxingCluster<T, TCell, TShape>(cell, origin, shape));
            }

            // Add point to cell cluster
            cells[cell].Points.Add(point);
        }

        return cells;
    }
}