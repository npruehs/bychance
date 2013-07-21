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
using System.Collections.Generic;

namespace ByChanceFramework
{
    /// <summary>
    /// Represents a template that is used for creating similar chunks, which
    /// in turn make up a level. Chunk templates define the positions of all
    /// contexts and anchors of chunks, as well as attributes that are required
    /// for the level generation process such as the probability of a chunk
    /// being picked to be added next.
    /// </summary>
    public abstract class ChunkTemplate
    {
        /// <summary>
        /// The default relative weight of this chunk template.
        /// </summary>
        /// <seealso cref="Weight"/>
        protected const int DEFAULT_WEIGHT = 1;


        /// <summary>
        /// The list of all contexts chunks created with this template can be aligned at.
        /// </summary>
        protected List<Context> contexts;

        /// <summary>
        /// The list of all anchors that can be filled in chunks created with this template.
        /// </summary>
        protected List<Anchor> anchors;

        /// <summary>
        /// The relative weight of this chunk template.
        /// </summary>
        protected int weight;


        /// <summary>
        /// Gets or sets the unique chunk library-wide index of this chunk template.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets the relative weight of this chunk template.
        /// Chunks of templates with <c>weight</c> 2 are added about twice as often to a level as
        /// chunks of templates with <c>weight</c> 1.
        /// </summary>
        public int Weight { get { return weight; } }

        /// <summary>
        /// Gets or sets the category of this chunk template.
        /// Use this category to tell post-processing algorithms whether chunks created with this
        /// template should be discarded if they have open contexts, or not, for example.
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// Gets or sets whether the level generator is allowed to rotate chunks created with
        /// this template by 90°.
        /// </summary>
        public bool AllowChunkRotation { get; private set; }


        /// <summary>
        /// Constructs a new chunk template with the specified weight, category
        /// and rotation permission.
        /// </summary>
        /// <param name="weight">The relative weight of the new chunk template.</param>
        /// <param name="tag">The category of the new chunk template.</param>
        /// <param name="allowChunkRotation">Whether the level generator is allowed to rotate chunks created with the template by 90°, or not.</param>
        /// <seealso cref="Weight"/>
        /// <seealso cref="Tag"/>
        /// <exception cref="ArgumentOutOfRangeException">The <c>weight</c> is less then or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">The passed <c>tag</c> is null.</exception>
        protected ChunkTemplate(int weight, string tag, bool allowChunkRotation)
        {
            contexts = new List<Context>();
            anchors = new List<Anchor>();

            if (weight <= 0)
            {
                throw new ArgumentOutOfRangeException("weight", weight, "Weight has to be greater than zero.");
            }

            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            this.weight = weight;

            Tag = tag;
            AllowChunkRotation = allowChunkRotation;
        }


        /// <summary>
        /// Gets the context with the specified template-wide unique index.
        /// </summary>
        /// <param name="index">The index of the context to get.</param>
        /// <returns>The context with the specified template-wide unique index.</returns>
        public Context GetContextByIndex(int index)
        {
            return contexts[index];
        }

        /// <summary>
        /// Gets the anchor with the specified template-wide unique index.
        /// </summary>
        /// <param name="index">The index of the anchor to get.</param>
        /// <returns>The anchor with the specified template-wide unique index.</returns>
        public Anchor GetAnchorByIndex(int index)
        {
            return anchors[index];
        }

        /// <summary>
        /// Gets the total number of contexts of this chunk template.
        /// </summary>
        /// <returns>The non-negative total number of contexts of this chunk template.</returns>
        public int GetContextCount()
        {
            return contexts.Count;
        }

        /// <summary>
        /// Gets the total number of anchors of this chunk template.
        /// </summary>
        /// <returns>The non-negative total number of anchors of this chunk template.</returns>
        public int GetAnchorCount()
        {
            return anchors.Count;
        }
    }
}
