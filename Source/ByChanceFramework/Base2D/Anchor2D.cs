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

namespace ByChanceFramework.Base2D
{
    /// <summary>
    /// Represents a placeholder for game elements in 2D levels that can be
    /// filled after the level generation process.
    /// </summary>
    sealed public class Anchor2D : Anchor
    {
        /// <summary>
        /// Gets or sets the x-coordinate of the position of this anchor relative to the
        /// position of the chunk it belongs to.
        /// </summary>
        public float RelativePosX { get; internal set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the position of this anchor relative to the
        /// position of the chunk it belongs to.
        /// </summary>
        public float RelativePosY { get; internal set; }

        /// <summary>
        /// Gets the x-coordinate of the absolute position of this anchor within the level.
        /// </summary>
        public float AbsolutePosX
        {
            get { return ((Chunk2D)Source).X + RelativePosX; }
        }

        /// <summary>
        /// Gets the y-coordinate of the absolute position of this anchor within the level.
        /// </summary>
        public float AbsolutePosY
        {
            get { return ((Chunk2D)Source).Y + RelativePosY; }
        }


        /// <summary>
        /// Constructs a new anchor of the specified category and with the passed
        /// position relative to the position of the chunk it belongs to.
        /// </summary>
        /// <param name="relativePosX">
        /// The x-coordinate of the position of the new anchor relative to the
        /// position of the chunk it belongs to.
        /// </param>
        /// <param name="relativePosY">
        /// The y-coordinate of the position of the new anchor relative to the
        /// position of the chunk it belongs to.
        /// </param>
        /// <param name="tag">The category of the new anchor.</param>
        internal Anchor2D(float relativePosX, float relativePosY, string tag)
            : base(tag)
        {
            this.RelativePosX = relativePosX;
            this.RelativePosY = relativePosY;
        }

        /// <summary>
        /// Constructs a new anchor with the same index and relative position
        /// as the passed one and attaches it to the specified chunk.
        /// </summary>
        /// <param name="template">The anchor whose attributes to copy.</param>
        /// <param name="source">The chunk to attach the new anchor to.</param>
        internal Anchor2D(Anchor2D template, Chunk2D source)
            : base(template.Tag)
        {
            this.Index = template.Index;
            this.source = source;
            this.RelativePosX = template.RelativePosX;
            this.RelativePosY = template.RelativePosY;
        }
    }
}
