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
    /// Describes a position a chunk can be aligned at to another one.
    /// </summary>
    public abstract class Context
    {
        /// <summary>
        /// The chunk this context belongs to.
        /// </summary>
        protected Chunk source;

        /// <summary>
        /// The chunk the source chunk is aligned to.
        /// </summary>
        protected Context target;


        /// <summary>
        /// Gets the chunk this context belongs to.
        /// </summary>
        public Chunk Source { get { return source; } }

        /// <summary>
        /// Gets the chunk the source chunk is aligned to.
        /// </summary>
        public Context Target { get { return target; } }

        /// <summary>
        /// The chunk-wide unique index of this context.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets or sets whether this context can be aligned to another one.
        /// </summary>
        public bool Blocked { get; internal set; }

        /// <summary>
        /// Gets or sets the category of this context.
        /// Use this category to modify the probability of aligning this
        /// context to another one, or to tell the level generator whether
        /// two contexts can be aligned at all.
        /// </summary>
        public string Tag { get; private set; }


        /// <summary>
        /// Constructs a new context of the specified category.
        /// </summary>
        /// <param name="tag">The category of the new context.</param>
        /// <exception cref="ArgumentNullException">The tag is null.</exception>
        protected Context(string tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            Tag = tag;
        }


        /// <summary>
        /// Checks whether this context is within <c>offset</c> to <c>other</c>.
        /// </summary>
        /// <param name="other">The context to check the adjacency to.</param>
        /// <param name="offset">The offset within this context is considered to be adjacent to <c>other</c>.</param>
        /// <returns><c>true</c>, if <c>other</c> is within <c>offset</c> to this one, and <c>false</c> otherwise.</returns>
        public virtual bool IsAdjacentTo(Context other, float offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aligns this context to <c>other</c>, making <c>target</c> of both contexts point to each other.
        /// </summary>
        /// <param name="other">The context to align this one to.</param>
        /// <seealso cref="Target"/>
        /// <exception cref="ArgumentNullException">The context to align this one to is null.</exception>
        /// <exception cref="InvalidOperationException">This context or <c>other</c> is already aligned to another context.</exception>
        /// <exception cref="ArgumentException"><c>other</c> has a different type than this context.</exception>
        public void AlignTo(Context other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (target != null)
            {
                throw new InvalidOperationException
                    ("The source context is already aligned to another context. " +
                     "Make sure to clear the target of the source context before aligning it to another one.");
            }

            if (other.target != null)
            {
                throw new InvalidOperationException
                    ("The target context is already aligned to another context. " +
                     "Make sure to clear the target of the target context before aligning it to another one.");
            }

            if (this.GetType().Equals(other.GetType()))
            {
                target = other;
                other.target = this;
            }
            else
            {
                throw new ArgumentException
                    ("The target context has a different type than the source context. " +
                     "Aligned contexts have to be of the same type.", "other");
            }
        }

        /// <summary>
        /// Breaks the connection between this context and its target, setting <c>target</c> of both contexts to <c>null</c>.
        /// </summary>
        public void ClearTarget()
        {
            if (target != null)
            {
                target.target = null;
                target = null;
            }
        }
    }
}