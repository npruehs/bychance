// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Chunk.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2016 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// <para>
    /// Main building block that makes up a level.
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
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new, empty chunk with a reference to the passed chunk template.
        /// </summary>
        /// <param name="template">Chunk template the new chunk will be based on.</param>
        /// <seealso cref="ChunkTemplate"/>
        /// <exception cref="ArgumentNullException"><paramref name="template"/> is null.</exception>
        protected Chunk(ChunkTemplate template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            this.ChunkContexts = new List<Context>();
            this.ChunkAnchors = new List<Anchor>();

            this.ChunkTemplate = template;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Whether the level generator is allowed to rotate chunks created with
        /// this template by 90°.
        /// </summary>
        public bool AllowChunkRotation
        {
            get
            {
                return this.ChunkTemplate.AllowChunkRotation;
            }
        }

        /// <summary>
        /// Total number of anchors of this chunk.
        /// </summary>
        public int AnchorCount
        {
            get
            {
                return this.ChunkAnchors.Count;
            }
        }

        /// <summary>
        /// Anchors that can be filled in this chunk.
        /// </summary>
        public IEnumerable<Anchor> Anchors
        {
            get
            {
                return this.ChunkAnchors;
            }
        }

        /// <summary>
        /// Chunk template this chunk is based on.
        /// </summary>
        public ChunkTemplate ChunkTemplate { get; protected set; }

        /// <summary>
        /// Total number of contexts of this chunk.
        /// </summary>
        public int ContextCount
        {
            get
            {
                return this.ChunkContexts.Count;
            }
        }

        /// <summary>
        /// Contexts this chunk can be aligned at.
        /// </summary>
        public IEnumerable<Context> Contexts
        {
            get
            {
                return this.ChunkContexts;
            }
        }

        /// <summary>
        /// Chunk library-wide index of this chunk.
        /// </summary>
        public int Index
        {
            get
            {
                return this.ChunkTemplate.Index;
            }
        }

        /// <summary>
        /// Category of this chunk.
        /// Use this to tell post-processing algorithms whether chunks of this category should be discarded 
        /// if they have open contexts, or not, for example.
        /// </summary>
        public string Tag
        {
            get
            {
                return this.ChunkTemplate.Tag;
            }
        }

        /// <summary>
        /// Relative weight of this chunk.
        /// Chunks with <see cref="Weight"/> 2 are added twice as often to a level as
        /// chunks with <see cref="Weight"/> 1.
        /// </summary>
        public int Weight
        {
            get
            {
                return this.ChunkTemplate.Weight;
            }
        }

        /// <summary>
        /// Anchors that can be filled in this chunk.
        /// </summary>
        protected List<Anchor> ChunkAnchors { get; private set; }

        /// <summary>
        /// Contexts this chunk can be aligned at.
        /// </summary>
        protected List<Context> ChunkContexts { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the total number of aligned (used) contexts of this chunk.
        /// </summary>
        /// <returns>Non-negative integer that represents the total number of aligned contexts of this chunk.</returns>
        public int GetAlignedContextCount()
        {
            return this.ChunkContexts.Count(context => context.Target != null);
        }

        /// <summary>
        /// Gets the anchor with the specified chunk-wide unique index.
        /// </summary>
        /// <param name="index">Index of the anchor to get.</param>
        /// <returns>Anchor at the specified index in the anchor list.</returns>
        public Anchor GetAnchor(int index)
        {
            return this.ChunkAnchors[index];
        }

        /// <summary>
        /// Gets the context with the specified chunk-wide unique index.
        /// </summary>
        /// <param name="index">Index of the context to get.</param>
        /// <returns>Context at the specified index in the context list.</returns>
        public Context GetContext(int index)
        {
            return this.ChunkContexts[index];
        }

        /// <summary>
        /// Removes passed context from the chunk's list of contexts.
        /// </summary>
        /// <param name="context">Context to be removed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is null.</exception>
        /// <returns><c>true</c>, if the context has been removed, and <c>false</c> otherwise.</returns>
        public bool RemoveContext(Context context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (this.ChunkContexts.Remove(context))
            {
                context.ClearTarget();
                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// <para>
        /// Rotates the chunk by 90°.
        /// </para>
        /// <para>
        /// Actual rotation algorithm is implemented by the concrete 2D and 3D chunk classes.
        /// </para>
        /// </summary>
        /// <returns><c>true</c>, while the chunk hasn't been rotated four times (i.e. by 360°) in every direction and <c>false</c> otherwise.</returns>
        internal virtual bool Rotate()
        {
            return true;
        }

        #endregion
    }
}