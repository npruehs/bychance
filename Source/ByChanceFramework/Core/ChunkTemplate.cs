// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChunkTemplate.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
//   
//   This file is part of the ByChance Framework.
//   
//   The ByChance Framework is free software: you can redistribute it and/or
//   modify it under the terms of the GNU Lesser General Public License as
//   published by the Free Software Foundation, either version 3 of the License,
//   or (at your option) any later version.
//   
//   The ByChance Framework is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
//   
//   You should have received a copy of the GNU Lesser General Public License
//   along with the ByChance Framework.  If not, see
//   <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ByChance.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Template that is used for creating similar chunks, which
    /// in turn make up a level. Chunk templates define the positions of all
    /// contexts and anchors of chunks, as well as attributes that are required
    /// for the level generation process such as the probability of a chunk
    /// being picked to be added next.
    /// </summary>
    public abstract class ChunkTemplate
    {
        #region Constants

        /// <summary>
        /// Default relative weight of this chunk template.
        /// </summary>
        /// <seealso cref="Weight"/>
        protected const int DefaultWeight = 1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new chunk template with the specified weight, category
        /// and rotation permission.
        /// </summary>
        /// <param name="weight">Relative weight of the new chunk template.</param>
        /// <param name="tag">Category of the new chunk template.</param>
        /// <param name="allowChunkRotation">Whether the level generator is allowed to rotate chunks created with the template by 90°, or not.</param>
        /// <seealso cref="Weight"/>
        /// <seealso cref="Tag"/>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="weight"/> is less then or equal to zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="tag"/> is null.</exception>
        protected ChunkTemplate(int weight, string tag, bool allowChunkRotation)
        {
            if (weight <= 0)
            {
                throw new ArgumentOutOfRangeException("weight", weight, "Weight has to be greater than zero.");
            }

            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            this.Contexts = new List<Context>();
            this.Anchors = new List<Anchor>();

            this.Weight = weight;
            this.Tag = tag;
            this.AllowChunkRotation = allowChunkRotation;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Whether the level generator is allowed to rotate chunks created with
        /// this template by 90°.
        /// </summary>
        public bool AllowChunkRotation { get; private set; }

        /// <summary>
        /// Total number of anchors of this chunk template.
        /// </summary>
        public int AnchorCount
        {
            get
            {
                return this.Anchors.Count;
            }
        }

        /// <summary>
        /// Total number of contexts of this chunk template.
        /// </summary>
        public int ContextCount
        {
            get
            {
                return this.Contexts.Count;
            }
        }

        /// <summary>
        /// Unique chunk library-wide index of this chunk template.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Category of this chunk template.
        /// Use this category to tell post-processing algorithms whether chunks created with this
        /// template should be discarded if they have open contexts, or not, for example.
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// Relative weight of this chunk template.
        /// Chunks of templates with <see cref="Weight"/> 2 are added about twice as often to a level as
        /// chunks of templates with <see cref="Weight"/> 1.
        /// </summary>
        public int Weight { get; protected set; }

        #endregion

        #region Properties

        /// <summary>
        /// Anchors that can be filled in chunks created with this template.
        /// </summary>
        protected internal List<Anchor> Anchors { get; private set; }

        /// <summary>
        /// Contexts chunks created with this template can be aligned at.
        /// </summary>
        protected internal List<Context> Contexts { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the anchor with the specified template-wide unique index.
        /// </summary>
        /// <param name="index">Index of the anchor to get.</param>
        /// <returns>Anchor with the specified template-wide unique index.</returns>
        public Anchor GetAnchorByIndex(int index)
        {
            return this.Anchors[index];
        }

        /// <summary>
        /// Gets the context with the specified template-wide unique index.
        /// </summary>
        /// <param name="index">Index of the context to get.</param>
        /// <returns>Context with the specified template-wide unique index.</returns>
        public Context GetContext(int index)
        {
            return this.Contexts[index];
        }

        #endregion
    }
}