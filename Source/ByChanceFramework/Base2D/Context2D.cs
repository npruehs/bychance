﻿/*
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

namespace ByChanceFramework.Base2D
{
    /// <summary>
    /// Describes a position a chunk can be aligned at to another one in a
    /// 2D level.
    /// </summary>
    sealed public class Context2D : Context
    {
        /// <summary>
        /// Gets or sets the x-coordinate of the position of this context relative to the
        /// position of the chunk it belongs to.
        /// </summary>
        public float RelativePosX { get; internal set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the position of this context relative to the
        /// position of the chunk it belongs to.
        /// </summary>
        public float RelativePosY { get; internal set; }

        /// <summary>
        /// Gets the x-coordinate of the absolute position of this context within the level.
        /// </summary>
        public float AbsolutePosX
        {
            get { return ((Chunk2D)Source).X + RelativePosX; }
        }

        /// <summary>
        /// Gets the y-coordinate of the absolute position of this context within the level.
        /// </summary>
        public float AbsolutePosY
        {
            get { return ((Chunk2D)Source).Y + RelativePosY; }
        }


        /// <summary>
        /// Constructs a new context with the passed position relative to the
        /// position of the chunk it belongs to.
        /// </summary>
        /// <param name="relativePosX">
        /// The x-coordinate of the position of the new context relative to the
        /// position of the chunk it belongs to.
        /// </param>
        /// <param name="relativePosY">
        /// The y-coordinate of the position of the new context relative to the
        /// position of the chunk it belongs to.
        /// </param>
        internal Context2D(float relativePosX, float relativePosY)
            : this(relativePosX, relativePosY, "")
        { }

        /// <summary>
        /// Constructs a new context of the specified category and with the passed
        /// position relative to the position of the chunk it belongs to.
        /// </summary>
        /// <param name="relativePosX">
        /// The x-coordinate of the position of the new context relative to the
        /// position of the chunk it belongs to.
        /// </param>
        /// <param name="relativePosY">
        /// The y-coordinate of the position of the new context relative to the
        /// position of the chunk it belongs to.
        /// </param>
        /// <param name="tag">The category of the new context.</param>
        internal Context2D(float relativePosX, float relativePosY, string tag)
            : base(tag)
        {
            this.RelativePosX = relativePosX;
            this.RelativePosY = relativePosY;
        }

        /// <summary>
        /// Constructs a new context with the same index and relative position
        /// as the passed one and attaches it to the specified chunk.
        /// </summary>
        /// <param name="template">The context whose attributes to copy.</param>
        /// <param name="source">The chunk to attach the new context to.</param>
        internal Context2D(Context2D template, Chunk2D source)
            : base(template.Tag)
        {
            this.Index = template.Index;
            this.source = source;
            this.RelativePosX = template.RelativePosX;
            this.RelativePosY = template.RelativePosY;
        }


        /// <summary>
        /// Checks whether this context is within <c>offset</c> to <c>other</c>
        /// in terms of the Euclidean norm.
        /// </summary>
        /// <param name="other">The context to check the adjacency to.</param>
        /// <param name="offset">The offset within this context is considered to be adjacent to <c>other</c>.</param>
        /// <returns><c>true</c>, if <c>other</c> is within <c>offset</c> to this one, and <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">The context to check the adjacency to is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The context to check the adjacency to is not of type <c>Context2D</c>.</exception>
        public override bool IsAdjacentTo(Context other, float offset)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else if (!(other is Context2D))
            {
                throw new ArgumentException("The passed context is not of type Context2D and therefore invalid.", "other");
            }

            Context2D context2D = (Context2D)other;

            return Math.Sqrt(Math.Pow(context2D.AbsolutePosX - AbsolutePosX, 2) + Math.Pow(context2D.AbsolutePosY - AbsolutePosY, 2)) <= offset;
        }

        /// <summary>
        /// Returns the absolute position of this context within the level
        /// as string.
        /// </summary>
        /// <returns>The absolute position of this context within the level.</returns>
        public override string ToString()
        {
            return AbsolutePosX + " | " + AbsolutePosY;
        }
    }
}
