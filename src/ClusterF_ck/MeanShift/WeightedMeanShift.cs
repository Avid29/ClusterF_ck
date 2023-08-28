// Adam Dernis © 2022

using ClusterF_ck.Kernels;
using ClusterF_ck.Spaces.Properties;
using CommunityToolkit.HighPerformance;
using System;
using System.Collections.Generic;

namespace ClusterF_ck.MeanShift;

/// <summary>
/// A static class containing Weighted Mean Shift methods.
/// </summary>
public static partial class WeightedMeanShift
{
    #region Weighted Points

    #region ReadOnlySpan

    /// <summary>
    /// Clusters a set of weighted points using MeanShift over a weighted field.
    /// </summary>
    /// <remarks>
    /// If all points are unique, it is wise to use <see cref="MeanShift.Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/> instead since there's no duplicates to merge.
    /// </remarks>
    /// <typeparam name="T">The type of points to cluster.</typeparam>
    /// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
    /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
    /// <param name="points">The weighted points to shift until convergence.</param>
    /// <param name="field">The field of weighted points to converge over.</param>
    /// <param name="kernel">The kernel to use for clustering.</param>
    /// <param name="shape">The shape to use on the points to cluster.</param>
    /// <returns>A list of clusters weighted by the contributing points.</returns>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        ReadOnlySpan<(T, double)> points,
        ReadOnlySpan<(T, double)> field,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Wrap(ClusterRaw(points, field, kernel, shape));


    /// <inheritdoc cref="WeightedMeanShift.Cluster"/>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        ReadOnlySpan<(T, double)> points,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster(points, points, kernel, shape);

    #endregion

    #region Array
        
    /// <inheritdoc cref="WeightedMeanShift.Cluster"/>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        (T, double)[] points,
        (T, double)[] field,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster<T, TShape, TKernel>(points.AsSpan(), field.AsSpan(), kernel, shape);
        
    /// <inheritdoc cref="WeightedMeanShift.Cluster"/>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        (T, double)[] points,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster(points, points, kernel, shape);

    #endregion

    #region List

#if NET6_0_OR_GREATER
        
    /// <inheritdoc cref="WeightedMeanShift.Cluster"/>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        List<(T, double)> points,
        List<(T, double)> field,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster<T, TShape, TKernel>(points.AsSpan(), field.AsSpan(), kernel, shape);
        
    /// <inheritdoc cref="WeightedMeanShift.Cluster"/>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        List<(T, double)> points,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster(points, points, kernel, shape);

#endif

    #endregion

    #endregion

    #region Unweighted Points

    #region ReadOnlySpan

    /// <summary>
    /// Clusters a set of points using a weighted version of MeanShift over a field by merging equal points.
    /// </summary>
    /// <remarks>
    /// If all points are unique, it is wise to use <see cref="MeanShift.Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/> instead since there's no duplicates to merge.
    /// </remarks>
    /// <typeparam name="T">The type of points to cluster.</typeparam>
    /// <typeparam name="TShape">The type of shape to use on the points to cluster.</typeparam>
    /// <typeparam name="TKernel">The type of kernel to use on the cluster.</typeparam>
    /// <param name="points">The points to shift until convergence.</param>
    /// <param name="field">The field of points to converge over.</param>
    /// <param name="kernel">The kernel to use for clustering.</param>
    /// <param name="shape">The shape to use on the points to cluster.</param>
    /// <returns>A list of clusters weighted by the contributing points.</returns>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        ReadOnlySpan<T> points,
        ReadOnlySpan<T> field,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster(MakeWeighted(points), MakeWeighted(field), kernel, shape);

    /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        ReadOnlySpan<T> points,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster(MakeWeighted(points), kernel, shape);

    #endregion

    #region Array
        
    /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        T[] points,
        T[] field,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster<T, TShape, TKernel>(points.AsSpan(), field.AsSpan(), kernel, shape);

    /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        T[] points,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster(points, points, kernel, shape);

    #endregion

    #region List

#if NET6_0_OR_GREATER

    /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        List<T> points,
        List<T> field,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster<T, TShape, TKernel>(points.AsSpan(), field.AsSpan(), kernel, shape);

    /// <inheritdoc cref="Cluster{T, TShape, TKernel}(ReadOnlySpan{T}, ReadOnlySpan{T}, TKernel, TShape)"/>
    public static List<MeanShiftCluster<T>> Cluster<T, TShape, TKernel>(
        List<T> points,
        TKernel kernel,
        TShape shape = default)
        where T : unmanaged, IEquatable<T>
        where TShape : struct, IDistanceSpace<T>, IWeightedAverageSpace<T>
        where TKernel : struct, IKernel => Cluster(points, points, kernel, shape);

#endif

    #endregion

    #endregion
}