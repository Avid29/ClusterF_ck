﻿// Adam Dernis © 2022

using ClusterF_ck.Spaces.Interfaces.Properties;

namespace ClusterF_ck.Spaces.Interfaces;

/// <summary>
/// An <see cref="ISpace{T}"/> for geometric points in a cluster.
/// </summary>
/// <remarks>
/// Geometric points are defined by their absolute positions in space.
/// </remarks>
/// <typeparam name="T">The type being wrapped by the implementation.</typeparam>
/// <typeparam name="TCell">The type indicating the cell a point belongs to in a grid space.</typeparam>
public interface IGeometricSpace<T, TCell> : ISpace<T>, IMetricSpace<T>, IAverageSpace<T>, IWeightedAverageSpace<T>, IGridSpace<T, TCell>
{
}