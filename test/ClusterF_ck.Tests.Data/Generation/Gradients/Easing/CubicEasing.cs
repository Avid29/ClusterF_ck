// Adam Dernis © 2022

using ClusterF_ck.Tests.Data.Generation.Gradients.Easing.Abstract;
using ClusterF_ck.Tests.Data.Generation.Gradients.Easing.Enums;
using System;

namespace ClusterF_ck.Tests.Data.Generation.Gradients.Easing
{
    public class CubicEasing : AsymetricEasingBase
    {
        public CubicEasing(EasingMode mode) :
            base(mode)
        {
            EaseFunc = mode switch
            {
                EasingMode.EaseIn => CubicEaseIn,
                EasingMode.EaseOut => CubicEaseOut,
                EasingMode.EaseInOut or _ => CubicEaseInOut,
            };
        }

        private Func<double, double> EaseFunc { get; }

        public override double Ease(double pos)
        {
            return EaseFunc(pos);
        }

        private double CubicEaseIn(double x)
        {
            return x * x * x;
        }

        private double CubicEaseOut(double x)
        {
            x--;
            return x * x * x + 1;
        }

        private double CubicEaseInOut(double x)
        {
            if (x < .5)
                return 4 * x * x * x;

            x *= 2;
            x -= 2;
            return (x * x * x / 2) + 1;
        }
    }
}
