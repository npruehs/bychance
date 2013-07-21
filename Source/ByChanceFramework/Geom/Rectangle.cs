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
    /// A rectangle with a two-dimensional position and extent.
    /// </summary>
    internal class Rectangle
    {
        /// <summary>
        /// Gets or sets the x-coordinate of the position of this rectangle.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the position of this rectangle.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the width of this rectangle.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height of this rectangle.
        /// </summary>
        public float Height { get; set; }


        /// <summary>
        /// Constructs a new rectangle with the specified position and extent.
        /// </summary>
        /// <param name="x">The x-coordinate of the position of the new rectangle.</param>
        /// <param name="y">The y-coordinate of the position of the new rectangle.</param>
        /// <param name="width">The width of the new rectangle.</param>
        /// <param name="height">The height of the new rectangle.</param>
        public Rectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;

            this.Width = width;
            this.Height = height;
        }


        /// <summary>
        /// Checks whether this rectangle entirely encompasses the passed one.
        /// </summary>
        /// <param name="otherRectangle">The rectangle to check.</param>
        /// <returns><c>true</c>, if this rectangle contains <c>otherRectangle</c>, and <c>false</c> otherwise.</returns>
        public bool Contains(Rectangle otherRectangle)
        {
            return (X <= otherRectangle.X && X + Width >= otherRectangle.X + otherRectangle.Width) &&
                    (Y <= otherRectangle.Y && Y + Height >= otherRectangle.Y + otherRectangle.Height);
        }

        /// <summary>
        /// Checks whether this rectangle at least partially intersects the passed one.
        /// </summary>
        /// <param name="otherRectangle">The rectangle to check.</param>
        /// <returns><c>true</c>, if this rectangle intersetcts <c>otherRectangle</c>, and <c>false</c> otherwise.</returns>
        public bool IntersectsWith(Rectangle otherRectangle)
        {
            return (X + Width > otherRectangle.X && X < otherRectangle.X + otherRectangle.Width) &&
                    (Y + Height > otherRectangle.Y && Y < otherRectangle.Y + otherRectangle.Height);
        }
    }
}
