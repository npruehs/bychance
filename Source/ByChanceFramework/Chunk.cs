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
    /// <para>
    /// Represents the main building block that make up a level.
    /// </para>
    /// <para>
    /// Chunks are created based on pre-defined chunk templates and define the positions 
    /// of all contexts and anchors associated with the chunk, as well as attributes that are 
    /// required for the level generation process such as the probability of a chunk
    /// being picked to be added next.
    /// </para>
    /// <para>
    /// The concrete 2D and 3D implementations of this base class hold additional information
    /// regarding their absolute position within the level and their size.
    /// </para>
    /// </summary>
    public abstract class Chunk
    {
        /// <summary>
        /// The list of all contexts this chunk can be aligned at.
        /// </summary>
        protected List<Context> contexts;

        /// <summary>
        /// The list of all anchors that can be filled in this chunk.
        /// </summary>
        protected List<Anchor> anchors;

        /// <summary>
        /// Reference to the chunk template this chunk is based on.
        /// </summary>
        protected ChunkTemplate chunkTemplate;


        /// <summary>
        /// Gets the reference to the chunk template this chunk is based on.
        /// </summary>
        public ChunkTemplate ChunkTemplate { get { return chunkTemplate; } }

        /// <summary>
        /// Gets the unique chunk library-wide index of this chunk (through the index of the chunk template).
        /// </summary>
        public int Index { get { return chunkTemplate.Index; } }

        /// <summary>
        /// Gets the relative weight of this chunk.
        /// Chunks with <c>weight</c> 2 are added twice as often to a level as
        /// chunks with <c>weight</c> 1.
        /// </summary>
        public int Weight { get { return chunkTemplate.Weight; } }

        /// <summary>
        /// Gets or sets the category of this chunk.
        /// Use this e.g. to tell post-processing algorithms whether chunks of this category should be discarded 
        /// if they have open contexts, or not.
        /// </summary>
        public string Tag { get { return chunkTemplate.Tag; } }

        /// <summary>
        /// Gets or sets whether the level generator is allowed to rotate chunks created with
        /// this template by 90°.
        /// </summary>
        public bool AllowChunkRotation { get { return chunkTemplate.AllowChunkRotation; } }

        /// <summary>
        /// Constructs a new, empty chunk with a reference to the passed chunk template.
        /// </summary>
        /// <param name="template">The chunk template the new chunk will be based on.</param>
        /// <seealso cref="chunkTemplate"/>
        /// <exception cref="ArgumentNullException">The passed <c>template</c> is null.</exception>
        protected Chunk(ChunkTemplate template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            contexts = new List<Context>();
            anchors = new List<Anchor>();

            this.chunkTemplate = template;
        }

        /// <summary>
        /// <para>
        /// Rotates the chunk by 90°.
        /// </para>
        /// <para>
        /// The actual rotation algorithm is implemented by the concrete 2D and 3D chunk classes.
        /// </para>
        /// </summary>
        /// <returns><c>true</c>, while the chunk hasn't been rotated four times (i.e. by 360°) in every direction and <c>false</c> otherwise.</returns>
        internal virtual bool Rotate()
        {
            return true;
        }

        /// <summary>
        /// Gets the context with the specified chunk-wide unique index.
        /// </summary>
        /// <param name="index">The <c>index</c> of the context to get.</param>
        /// <returns>The context at the specified <c>index</c> in the context list.</returns>
        public Context GetContextByIndex(int index)
        {
            return contexts[index];
        }

        /// <summary>
        /// Removes passed context from the chunk's list of contexts.
        /// </summary>
        /// <param name="context">The <c>context</c> to be removed.</param>
        /// <exception cref="ArgumentNullException">The passed <c>context</c> is null.</exception>
        /// <exception cref="ArgumentException">The passed <c>context</c> is not part of this chunk.</exception>
        public void RemoveContext(Context context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (contexts.Remove(context))
            {
                context.ClearTarget();
            }
            else
            {
                throw new ArgumentException("The passed context is not part of this chunk.", "context");
            }
        }

        /// <summary>
        /// Gets the anchor with the specified chunk-wide unique index.
        /// </summary>
        /// <param name="index">The <c>index</c> of the anchor to get.</param>
        /// <returns>The anchor at the specified <c>index</c> in the anchor list.</returns>
        public Anchor GetAnchorByIndex(int index)
        {
            return anchors[index];
        }

        /// <summary>
        /// Gets the total number of contexts of this chunk.
        /// </summary>
        /// <returns>Non-negative integer that represents the total number of existing contexts of this chunk.</returns>
        public int GetContextCount()
        {
            return contexts.Count;
        }

        /// <summary>
        /// Gets the total number of anchors of this chunk.
        /// </summary>
        /// <returns>Non-negative integer that represents the total number of existing anchors of this chunk.</returns>
        public int GetAnchorCount()
        {
            return anchors.Count;
        }

        /// <summary>
        /// Gets the total number of aligned (used) contexts of this chunk.
        /// </summary>
        /// <returns>Non-negative integer that represents the total number of aligned contexts of this chunk.</returns>
        public int GetAlignedContextCount()
        {
            int targetCount = 0;

            foreach (Context context in contexts)
            {
                if (context.Target != null)
                {
                    targetCount++;
                }
            }

            return targetCount;
        }
    }
}
