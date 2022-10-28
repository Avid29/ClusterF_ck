// Adam Dernis © 2022

namespace ClusterF_ck.Spaces.Properties
{
    /// <summary>
    /// A class containing extensions for the <see cref="IDistanceSpace{T}"/> interface.
    /// </summary>
    public static class DistanceSpaceExtensions
    {
        /// <summary>
        /// Checks if two points are equal using only their distance.
        /// </summary>
        /// <typeparam name="T">The type of a point.</typeparam>
        /// <param name="space">The space the points exist in.</param>
        /// <param name="it1">The first point.</param>
        /// <param name="i2">The second point.</param>
        /// <returns>True if the distance between the points is zero.</returns>
        public static bool AreEqual<T>(this IDistanceSpace<T> space, T it1, T i2)
        {
            return space.FindDistanceSquared(it1, i2) == 0;
        }

        /// <summary>
        /// Checks if two points are equal using only their distance, with a margin of error.
        /// </summary>
        /// <typeparam name="T">The type of a point.</typeparam>
        /// <param name="space">The space the points exist in.</param>
        /// <param name="it1">The first point.</param>
        /// <param name="i2">The second point.</param>
        /// <param name="error">The acceptable margin of error to still consider the points equal.</param>
        /// <returns>True if the distance between the points is zero.</returns>
        public static bool AreEqual<T>(this IDistanceSpace<T> space, T it1, T i2, double error)
        {
            return space.FindDistanceSquared(it1, i2) < error;
        }
    }
}
