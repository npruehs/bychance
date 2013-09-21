// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Math2.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ByChance.Core
{
    using System;

    /// <summary>
    /// Extension of the .NET <see cref="Math"/> class. Provides useful
    /// constants and method overloads for different value types.
    /// </summary>
    public static class Math2
    {
        #region Constants

        /// <summary>
        /// Math constant Pi.
        /// </summary>
        public const float Pi = 3.1415926535897932384626433832795f;

        /// <summary>
        /// Math constant Pi divided by four.
        /// </summary>
        public const float PiOverFour = Pi * 0.25f;

        /// <summary>
        /// Math constant Pi divided by two.
        /// </summary>
        public const float PiOverTwo = Pi * 0.5f;

        /// <summary>
        /// Sine of 45 degrees.
        /// </summary>
        public const float Sin45 = 0.70710678118654752440084436210485f;

        /// <summary>
        /// Math constant Pi times two.
        /// </summary>
        public const float TwoPi = Pi * 2.0f;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the smallest integer greater than or equal to the specified number.
        /// </summary>
        /// <param name="d">
        /// Number to ceil.
        /// </param>
        /// <returns>
        /// Smallest integer that is greater than or equal to the specified number.
        /// </returns>
        public static int Ceil(double d)
        {
            return (int)Math.Ceiling(d);
        }

        /// <summary>
        /// Clamps the passed value to the specified bounds.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the value to clamp.
        /// </typeparam>
        /// <param name="value">
        /// Value to clamp.
        /// </param>
        /// <param name="min">
        /// Lower bound.
        /// </param>
        /// <param name="max">
        /// Upper bound.
        /// </param>
        /// <returns>
        /// Clamped value.
        /// </returns>
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }

            return value.CompareTo(max) > 0 ? max : value;
        }

        /// <summary>
        /// Converts the specified angle in degrees to radians for passing them
        /// to the .NET <see cref="Math"/> trigonometric functions.
        /// </summary>
        /// <param name="degrees">
        /// Degrees to convert to radians.
        /// </param>
        /// <returns>
        /// Passed angle in radians.
        /// </returns>
        public static float DegreesToRadians(float degrees)
        {
            return degrees * (Pi / 180.0f);
        }

        /// <summary>
        /// Checks whether the two passed numbers are equal, with respect
        ///     to possible loss of precision caused by rounding values.
        /// </summary>
        /// <param name="f">
        /// First number to compare.
        /// </param>
        /// <param name="g">
        /// Second number to compare.
        /// </param>
        /// <returns>
        /// <c>true</c>, if both numbers are equal, and <c>false</c> otherwise.
        /// </returns>
        public static bool Equals(float f, float g)
        {
            return Math.Abs(f - g) < float.Epsilon;
        }

        /// <summary>
        /// Checks whether the two passed numbers are equal, with respect
        ///     to possible loss of precision caused by rounding values.
        /// </summary>
        /// <param name="d">
        /// First number to compare.
        /// </param>
        /// <param name="e">
        /// Second number to compare.
        /// </param>
        /// <returns>
        /// <c>true</c>, if both numbers are equal, and <c>false</c> otherwise.
        /// </returns>
        public static bool Equals(double d, double e)
        {
            return Math.Abs(d - e) < double.Epsilon;
        }

        /// <summary>
        /// Returns the largest integer less than or equal to the specified number.
        /// </summary>
        /// <param name="d">
        /// Number to floor.
        /// </param>
        /// <returns>
        /// Largest integer less than or equal to the specified number.
        /// </returns>
        public static int Floor(double d)
        {
            return (int)Math.Floor(d);
        }

        /// <summary>
        /// Gets the k-th bit of the passed <see cref="int"/> value.
        /// </summary>
        /// <param name="i">
        /// Value to get the k-th bit of.
        /// </param>
        /// <param name="k">
        /// Bit to get.
        /// </param>
        /// <returns>
        /// 0 or 1.
        /// </returns>
        public static int GetBit(int i, int k)
        {
            return (i >> k) & 1;
        }

        /// <summary>
        /// Linearly interpolates between the two passed values.
        /// </summary>
        /// <param name="f">
        /// First value to interpolate.
        /// </param>
        /// <param name="g">
        /// Second value to interpolate.
        /// </param>
        /// <param name="l">
        /// Interpolation parameter. 0 returns <paramref name="f"/>, 1 returns <paramref name="g"/>.
        /// </param>
        /// <returns>
        /// Linear interpolation between the two passed values.
        /// </returns>
        public static float Lerp(float f, float g, float l)
        {
            if (l <= 0.0f)
            {
                return f;
            }

            if (l >= 1.0f)
            {
                return g;
            }

            return f + (l * (g - f));
        }

        /// <summary>
        /// Rounds the specified number to the nearest integer.
        /// </summary>
        /// <param name="d">
        /// Number to round.
        /// </param>
        /// <returns>
        /// Integer nearest to the specified number.
        /// </returns>
        public static int Round(double d)
        {
            return (int)Math.Round(d);
        }

        /// <summary>
        /// Returns the square root of the specified number.
        /// </summary>
        /// <param name="f">
        /// Number to get the square root of.
        /// </param>
        /// <returns>
        /// Square root of the specified number
        /// </returns>
        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt(f);
        }

        /// <summary>
        /// Returns the integral part of the specified number.
        /// </summary>
        /// <param name="d">
        /// Number to truncate.
        /// </param>
        /// <returns>
        /// Integral part of the specified number
        /// </returns>
        public static int Truncate(double d)
        {
            return (int)Math.Truncate(d);
        }

        #endregion
    }
}