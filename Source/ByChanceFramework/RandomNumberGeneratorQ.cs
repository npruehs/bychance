/*
 * Copyright 2011 Nick Pruehs, Denis Vaz Alves.
 * 
 * This file is part of the ByChance Framework.
 *
 * The ByChance Framework is free software: you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 * 
 * The ByChance Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with the ByChance Framework.  If not, see
 * <http://www.gnu.org/licenses/>.
 */

using System;

namespace ByChanceFramework
{
    /// <summary>
    /// Represents a C# port of the Ranq1 struct found in 'Numerical Recipes in C : 3rd Edition' where 
    /// it is described as "Simplest and fastest generator we can readily recommend [...]".
    /// <para>
    /// It is a combined generator ( Ranq1 = D1 (A1(right-shift first)), see 'Numerical Recipes in C : 3rd Edition' ) 
    /// with a period of 1.8 x 10^19.
    /// </para>
    /// </summary>
    public class RandomNumberGeneratorQ
    {
        /// <summary>
        /// Initialization value.
        /// </summary>
        private const long m = 4101842887655102017L;

        /// <summary>
        /// First bitshift value.
        /// </summary>
        private const int a1 = 21;

        /// <summary>
        /// Second bitshift value.
        /// </summary>
        private const int a2 = 35;

        /// <summary>
        /// Third bitshift value.
        /// </summary>
        private const int a3 = 4;

        /// <summary>
        /// Recommended multiplier for D1 method.
        /// </summary>
        private const long a = 2685821657736338717L;


        /// <summary>
        /// Current state of the random number generation.
        /// </summary>
        private ulong v;

        /// <summary>
        /// The seed for the random number sequence.
        /// </summary>
        public ulong Seed { get; private set; }

        /// <summary>
        /// Constructs a new random number generator.
        /// This constructor takes the current system date and time represent as number of ticks.
        /// </summary>
        public RandomNumberGeneratorQ()
            : this((ulong) DateTime.Now.Ticks)
        { }

        /// <summary>
        /// Constructs a new random number generator with the given 64-bit unsigned integer as <c>seed</c>.
        /// </summary>
        /// <param name="seed">The 64-bit unsigned integer to be used as <c>seed</c></param>
        /// <seealso cref="Seed"/>
        public RandomNumberGeneratorQ(ulong seed)
        {
            Seed = seed;

            v = m ^ seed;
            v = RandomUInt64();
        }


        /// <summary>
        /// Gets the next random number in the sequence and returns it as a 64-bit unsigned integer.
        /// </summary>
        /// <returns>The next random number in the sequence as a 64-bit unsigned integer.</returns>
        public ulong RandomUInt64()
        {
            v ^= v >> a1;
            v ^= v << a2;
            v ^= v >> a3;

            return v * a;
        }

        /// <summary>
        /// Gets the next random number in the sequence and returns it as a 32-bit unsigned integer.
        /// </summary>
        /// <returns>The next random number in the sequence as a 32-bit unsigned integer.</returns>
        public uint RandomUInt32()
        {
            return (uint)RandomUInt64();
        }

        /// <summary>
        /// Gets the next random number in the sequence and returns it as a double-precision floating-point number 
        /// between 0.0f and 1.0f.
        /// </summary>
        /// <returns>
        /// The next random number in the sequence as a double-precision floating-point number 
        /// between 0.0f and 0.1f.
        /// </returns>
        public double RandomDouble()
        {
            return 5.42101086242752217E-20 * RandomUInt64();
        }

        /// <summary>
        /// Gets the next random number in the sequence and returns it as a single-precision floating-point number
        /// between 0.0f and 1.0f.
        /// </summary>
        /// <returns>
        /// The next random number in the sequence as a single-precision floating-point number 
        /// between 0.0f and 0.1f.
        /// </returns>
        public float RandomFloat()
        {
            return (float)RandomDouble();
        }

        /// <summary>
        /// Gets the next random number in the sequence and returns it as 32-bit unsigned integer in the range of <c>max</c>.
        /// </summary>
        /// <param name="max">The <c>max</c> value the random number should achieve.</param>
        /// <returns>The next random number in the sequence as a 32-bit unsigned integer with a maximum value of <c>max</c>.</returns>
        public uint RandomRangeUInt32(uint max)
        {
            return RandomUInt32() % max;
        }

        /// <summary>
        /// Gets the next random number in the sequence and returns it as 64-bit unsigned integer in the range of <c>max</c>.
        /// </summary>
        /// <param name="max">The <c>max</c> value the random number should achieve.</param>
        /// <returns>The next random number in the sequence as a 64-bit unsigned integer with a maximum value of <c>max</c>.</returns>
        public ulong RandomRangeUInt64(ulong max)
        {
            return RandomUInt64() % max;
        }
    }
}
