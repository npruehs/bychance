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
    /// Represents a placeholder for game elements that can be filled after
    /// the level generation process.
    /// </summary>
    public abstract class Anchor
    {
        /// <summary>
        /// The chunk this anchor belongs to.
        /// </summary>
        protected Chunk source;


        /// <summary>
        /// Gets or sets the chunk-wide unique index of this anchor.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets the chunk this anchor belongs to.
        /// </summary>
        public Chunk Source { get { return source; } }

        /// <summary>
        /// Gets or sets the category of this anchor.
        /// Use this category for determining how to fill this anchor after
        /// the level generation process is complete.
        /// </summary>
        public string Tag { get; private set; }


        /// <summary>
        /// Constructs a new anchor of the specified category.
        /// </summary>
        /// <param name="tag">The category of the new anchor.</param>
        /// <exception cref="ArgumentNullException">The tag is null.</exception>
        protected Anchor(string tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            this.Tag = tag;
        }
    }
}
