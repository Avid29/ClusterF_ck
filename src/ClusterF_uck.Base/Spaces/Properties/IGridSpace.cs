// Adam Dernis © 2022

namespace ClusterF_ck.Spaces.Properties
{
    /// <summary>
    /// An <see cref="ISpace{T}"/> where points can be split into a grid.
    /// </summary>
    /// <typeparam name="T">The type of points in the space.</typeparam>
    /// <typeparam name="TCell">A type identifying the cell in the grid.</typeparam>
    public interface IGridSpace<T, TCell> : ISpace<T>
    {
        /// <summary>
        /// Gets the cell of a point in the space.
        /// </summary>
        /// <param name="value">The point.</param>
        /// <param name="window">The size of the cells.</param>
        /// <returns>The cell of the point in the space.</returns>
        TCell GetCell(T value, double window);

        /// <summary>
        /// Gets the center point of a cell.
        /// </summary>
        /// <param name="cell">The cell to get the center of.</param>
        /// <param name="window">The size of the cells.</param>
        /// <returns>The center of the cell given the window size.</returns>
        T GetCellCenter(TCell cell, double window);
    }
}
