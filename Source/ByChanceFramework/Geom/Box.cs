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

namespace ByChanceFramework.Geom
{
    /// <summary>
    /// A box with a three-dimensional position and extent.
    /// </summary>
    internal class Box
    {
        /// <summary>
        /// Gets or sets the x-coordinate of the position of this box.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the position of this box.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the z-coordinate of the position of this box.
        /// </summary>
        public float Z { get; set; }

        /// <summary>
        /// Gets or sets the width of this box.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height of this box.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Gets or sets the depth of this box.
        /// </summary>
        public float Depth { get; set; }


        /// <summary>
        /// Constructs a new box with the specified position and extent.
        /// </summary>
        /// <param name="x">The x-coordinate of the position of the new box.</param>
        /// <param name="y">The y-coordinate of the position of the new box.</param>
        /// <param name="z">The z-coordinate of the position of the new box.</param>
        /// <param name="width">The width of the new box.</param>
        /// <param name="height">The height of the new box.</param>
        /// <param name="depth">The depth of the new box.</param>
        public Box(float x, float y, float z, float width, float height, float depth)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;

            this.Width = width;
            this.Height = height;
            this.Depth = depth;
        }


        /// <summary>
        /// Checks whether this box entirely encompasses the passed one.
        /// </summary>
        /// <param name="otherBox">The box to check.</param>
        /// <returns><c>true</c>, if this box contains <c>otherBox</c>, and <c>false</c> otherwise.</returns>
        public bool Contains(Box otherBox)
        {
            if ((X <= otherBox.X && X + Width >= otherBox.X + otherBox.Width) &&
                (Y <= otherBox.Y && Y + Height >= otherBox.Y + otherBox.Height) &&
                (Z <= otherBox.Z && Z + Depth >= otherBox.Z + otherBox.Depth))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether this box at least partially intersects the passed one.
        /// </summary>
        /// <param name="otherBox">The box to check.</param>
        /// <returns><c>true</c>, if this box intersetcts <c>otherBox</c>, and <c>false</c> otherwise.</returns>
        public bool IntersectsWith(Box otherBox)
        {
            if (X + Width <= otherBox.X || X >= otherBox.X + otherBox.Width)
            {
                return false;
            }
            if (Y + Height <= otherBox.Y || Y >= otherBox.Y + otherBox.Height)
            {
                return false;
            }
            if (Z + Depth <= otherBox.Z || Z >= otherBox.Z + otherBox.Depth)
            {
                return false;
            }

            return true;
        }
    }
}
