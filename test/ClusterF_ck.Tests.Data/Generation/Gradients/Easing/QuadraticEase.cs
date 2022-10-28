// Adam Dernis © 2022

using ClusterF_ck.Tests.Data.Generation.Gradients.Easing.Abstract;
using ClusterF_ck.Tests.Data.Generation.Gradients.Easing.Enums;
using System;

namespace ClusterF_ck.Tests.Data.Generation.Gradients.Easing
{
    public class QuadraticEase : AsymetricEasingBase
    {
        public QuadraticEase(EasingMode mode) :
            base(mode)
        {
            EaseFunc = mode switch
            {
                EasingMode.EaseIn => QuadEaseIn,
                EasingMode.EaseOut => QuadEaseOut,
                EasingMode.EaseInOut or _ => QuadEaseInOut,
            };
        }

        private Func<double, double> EaseFunc { get; }

        public override double Ease(double pos)
        {
            return EaseFunc(pos);
        }

        private double QuadEaseIn(double x)
        {
            return x * x;
        }

        private double QuadEaseOut(double x)
        {
            return x * (2 - x);
        }

        private double QuadEaseInOut(double x)
        {
            if (x < .5)
                return 2 * x * x;
            return 2 * (2 - x) * x - 1;
        }
    }
}
