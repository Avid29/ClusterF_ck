// Adam Dernis © 2022

using ClusterF_ck.Spaces.Interfaces;

namespace ClusterF_ck.Spaces.Interfaces.Properties;

/// <summary>
/// An <see cref="ISpace{T}"/> where points can be split into a grid.
/// </summary>
/// <typeparam name="T">The type of points in the space.</typeparam>
/// <typeparam name="TCell">A type identifying the cell in the grid.</typeparam>
public interface IGridSpace<T, TCell> : ISpace<T>
{
    /// <summary>
    /// Gets or sets the cell window size for the space.
    /// </summary>
    public double Window { get; set; }

    /// <summary>
    /// Gets the cell of a point in the space.
    /// </summary>
    /// <param name="value">The point.</param>
    /// <returns>The cell of the point in the space.</returns>
    TCell GetCell(T value);

    /// <summary>
    /// Gets the origin point of a cell.
    /// </summary>
    /// <param name="cell">The cell to get the center of.</param>
    /// <returns>The center of the cell given the window size.</returns>
    T GetCellOrigin(TCell cell);
}