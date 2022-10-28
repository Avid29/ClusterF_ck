// Adam Dernis © 2022

using System;

namespace ClusterF_ck.Kernels
{
    /// The shape of a Gaussian Distribution
    /// 
    ///                *
    ///              *   *
    ///             *     *
    ///            *       *
    ///           *         *
    ///        *               *
    ///  *                           *
    ///  -----------------------------

    /// <summary>
    /// A Kernel with a gaussian falloff.
    /// </summary>
    public struct GaussianKernel : IKernel
    {
        private double _denominatorBandwidth;
        private double _bandwidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianKernel"/> struct.
        /// </summary>
        /// <param name="bandwidth">The bandwidth of the <see cref="GaussianKernel"/>.</param>
        public GaussianKernel(double bandwidth)
        {
            // These will be set in WindowSize
            _bandwidth = 0;
            _denominatorBandwidth = 0;

            WindowSize = bandwidth;
        }

        /// <inheritdoc/>
        public double WindowSize
        {
            get => _bandwidth;
            set
            {
                _bandwidth = value;
                _denominatorBandwidth = value * value * -2;
            }
        }

        /// <inheritdoc/>
        public double WeightDistance(double distanceSquared)
        {
            // Unoptimized equivalent.
            // return Math.Pow(Math.E, -.5 * distanceSquared / (_bandwidth * _bandwidth);

            return Math.Exp(distanceSquared / _denominatorBandwidth);
        }
    }
}
