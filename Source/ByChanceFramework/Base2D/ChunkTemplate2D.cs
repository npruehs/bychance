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

namespace ByChanceFramework.Base2D
{
    /// <summary>
    /// Represents a template that is used for creating similar chunks, which
    /// in turn make up a 2D level. Chunk templates define the chunk extents,
    /// the positions of all contexts and anchors of chunks, as well as
    /// attributes that are required for the level generation process such as
    /// the probability of a chunk being picked to be added next.
    /// </summary>
    sealed public class ChunkTemplate2D : ChunkTemplate
    {
        /// <summary>
        /// Gets or sets the width of the chunks constructed with this template.
        /// </summary>
        public float Width { get; private set; }

        /// <summary>
        /// Gets or sets the height of the chunks constructed with this template.
        /// </summary>
        public float Height { get; private set; }


        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width
        /// and height and a default weight. Chunks constructed with this
        /// template may not be rotated by the level generator.
        /// </summary>
        /// <param name="width">The width of the chunks to construct a new template for.</param>
        /// <param name="height">The height of the chunks to construct a new template for.</param>
        /// <seealso cref="ChunkTemplate.DEFAULT_WEIGHT"/>
        public ChunkTemplate2D(float width, float height)
            : this(width, height, DEFAULT_WEIGHT, "", false)
        { }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width
        /// and height and the passed weight. Chunks constructed with this
        /// template may not be rotated by the level generator.
        /// </summary>
        /// <param name="width">The width of the chunks to construct a new template for.</param>
        /// <param name="height">The height of the chunks to construct a new template for.</param>
        /// <param name="weight">The relative weight of the new chunk template.</param>
        /// <seealso cref="ChunkTemplate.Weight"/>
        /// <exception cref="ArgumentOutOfRangeException">The <c>weight</c> is less then or equal to zero.</exception>
        public ChunkTemplate2D(float width, float height, int weight)
            : this(width, height, weight, "", false)
        { }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width
        /// and height and a default weight. Chunks constructed with the new template will be of
        /// of the passed category and may not be rotated by the level generator.
        /// </summary>
        /// <param name="width">The width of the chunks to construct a new template for.</param>
        /// <param name="height">The height of the chunks to construct a new template for.</param>
        /// <param name="tag">The category of the new chunk template.</param>
        /// <seealso cref="ChunkTemplate.DEFAULT_WEIGHT"/>
        /// <seealso cref="ChunkTemplate.Tag"/>
        /// <exception cref="ArgumentNullException">The passed <c>tag</c> is null.</exception>
        public ChunkTemplate2D(float width, float height, string tag)
            : this(width, height, DEFAULT_WEIGHT, tag, false)
        { }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width,
        /// height and rotation permission. Chunks constructed with this template
        /// will have a default weight and may not be rotated by the level generator.
        /// </summary>
        /// <param name="width">The width of the chunks to construct a new template for.</param>
        /// <param name="height">The height of the chunks to construct a new template for.</param>
        /// <param name="allowChunkRotation">Whether the level generator is allowed to rotate chunks created with the template by 90°, or not.</param>
        /// <seealso cref="ChunkTemplate.DEFAULT_WEIGHT"/>
        public ChunkTemplate2D(float width, float height, bool allowChunkRotation)
            : this(width, height, DEFAULT_WEIGHT, "", allowChunkRotation)
        { }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width,
        /// height, weight, category and rotation permission.
        /// </summary>
        /// <param name="width">The width of the chunks to construct a new template for.</param>
        /// <param name="height">The height of the chunks to construct a new template for.</param>
        /// <param name="weight">The relative weight of the new chunk template.</param>
        /// <param name="tag">The category of the new chunk template.</param>
        /// <param name="allowChunkRotation">Whether the level generator is allowed to rotate chunks created with the template by 90°, or not.</param>
        /// <seealso cref="ChunkTemplate.Weight"/>
        /// <seealso cref="ChunkTemplate.Tag"/>
        /// <exception cref="ArgumentOutOfRangeException">The <c>width</c>, <c>height</c> or <c>weight</c> is less then or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">The passed <c>tag</c> is null.</exception>
        public ChunkTemplate2D(float width, float height, int weight, string tag, bool allowChunkRotation)
            : base(weight, tag, allowChunkRotation)
        {
            if (width <= 0f)
            {
                throw new ArgumentOutOfRangeException("width", width, "The width of the template must be greater than zero.");
            }

            if (height <= 0f)
            {
                throw new ArgumentOutOfRangeException("height", height, "The height of the template must be greater than zero.");
            }

            this.Width = width;
            this.Height = height;
        }


        /// <summary>
        /// Adds a new context at the passed position relative to the
        /// positions of the chunks constructed with this template.
        /// </summary>
        /// <param name="relativePositionX">
        /// The x-coordinate of the position of the new context relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        /// <param name="relativePositionY">
        /// The y-coordinate of the position of the new context relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        public void AddContext(float relativePositionX, float relativePositionY)
        {
            AddContext(new Context2D(relativePositionX, relativePositionY));
        }

        /// <summary>
        /// Adds a new context of the specified category at the passed
        /// position relative to the positions of the chunks constructed with
        /// this template.
        /// </summary>
        /// <param name="relativePositionX">
        /// The x-coordinate of the position of the new context relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        /// <param name="relativePositionY">
        /// The y-coordinate of the position of the new context relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        /// <param name="tag">The category of the new context.</param>
        public void AddContext(float relativePositionX, float relativePositionY, string tag)
        {
            AddContext(new Context2D(relativePositionX, relativePositionY, tag));
        }

        /// <summary>
        /// Adds the passed context to the list of contexts of this chunk
        /// template and sets its chunk-wide unique index.
        /// </summary>
        /// <param name="context">The context to add to this template.</param>
        private void AddContext(Context2D context)
        {
            context.Index = contexts.Count;
            contexts.Add(context);
        }

        /// <summary>
        /// Adds a new anchor at the passed position relative to the
        /// positions of the chunks constructed with this template.
        /// </summary>
        /// <param name="relativePositionX">
        /// The x-coordinate of the position of the new anchor relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        /// <param name="relativePositionY">
        /// The y-coordinate of the position of the new anchor relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        public void AddAnchor(float relativePositionX, float relativePositionY)
        {
            AddAnchor(relativePositionX, relativePositionY, "");
        }

        /// <summary>
        /// Adds a new anchor of the specified category at the passed
        /// position relative to the positions of the chunks constructed with
        /// this template.
        /// </summary>
        /// <param name="relativePositionX">
        /// The x-coordinate of the position of the new context relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        /// <param name="relativePositionY">
        /// The y-coordinate of the position of the new context relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        /// <param name="tag">The category of the new anchor.</param>
        public void AddAnchor(float relativePositionX, float relativePositionY, string tag)
        {
            AddAnchor(new Anchor2D(relativePositionX, relativePositionY, tag));
        }

        /// <summary>
        /// Adds the passed anchor to the list of anchors of this chunk
        /// template and sets its chunk-wide unique index.
        /// </summary>
        /// <param name="anchor">The anchor to add to this template.</param>
        private void AddAnchor(Anchor2D anchor)
        {
            anchor.Index = anchors.Count;
            anchors.Add(anchor);
        }
    }
}
