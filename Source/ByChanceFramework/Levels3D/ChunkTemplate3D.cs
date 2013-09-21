// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChunkTemplate3D.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ByChance.Levels3D
{
    using System;

    using ByChance.Core;

    using Npruehs.GrabBag.Math.Vectors;

    /// <summary>
    /// Template that is used for creating similar chunks, which
    /// in turn make up a 3D level. Chunk templates define the chunk extents,
    /// the positions of all contexts and anchors of chunks, as well as
    /// attributes that are required for the level generation process such as
    /// the probability of a chunk being picked to be added next.
    /// </summary>
    public sealed class ChunkTemplate3D : ChunkTemplate
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width,
        /// height and depth and a default weight. Chunks constructed with this
        /// template may not be rotated by the level generator.
        /// </summary>
        /// <param name="extents">Width, height and depth of the chunks to construct a new template for.</param>
        /// <seealso cref="ChunkTemplate.DefaultWeight"/>
        public ChunkTemplate3D(Vector3F extents)
            : this(extents, DefaultWeight, string.Empty, false)
        {
        }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width,
        /// height and depth and the passed weight. Chunks constructed with this
        /// template may not be rotated by the level generator.
        /// </summary>
        /// <param name="extents">Width, height and depth of the chunks to construct a new template for.</param>
        /// <param name="weight">Relative weight of the new chunk template.</param>
        /// <seealso cref="ChunkTemplate.Weight"/>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="weight"/> is less then or equal to zero.</exception>
        public ChunkTemplate3D(Vector3F extents, int weight)
            : this(extents, weight, string.Empty, false)
        {
        }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width,
        /// height and depth and a default weight. Chunks constructed with the new template will be of
        /// of the passed category and may not be rotated by the level generator.
        /// </summary>
        /// <param name="extents">Width, height and depth of the chunks to construct a new template for.</param>
        /// <param name="tag">Category of the new chunk template.</param>
        /// <seealso cref="ChunkTemplate.DefaultWeight"/>
        /// <seealso cref="ChunkTemplate.Tag"/>
        /// <exception cref="ArgumentNullException"><paramref name="tag"/> is null.</exception>
        public ChunkTemplate3D(Vector3F extents, string tag)
            : this(extents, DefaultWeight, tag, false)
        {
        }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width,
        /// height, depth and rotation permission. Chunks constructed with this template
        /// will have a default weight and may not be rotated by the level generator.
        /// </summary>
        /// <param name="extents">Width, height and depth of the chunks to construct a new template for.</param>
        /// <param name="allowChunkRotation">Whether the level generator is allowed to rotate chunks created with the template by 90°, or not.</param>
        /// <seealso cref="ChunkTemplate.DefaultWeight"/>
        public ChunkTemplate3D(Vector3F extents, bool allowChunkRotation)
            : this(extents, DefaultWeight, string.Empty, allowChunkRotation)
        {
        }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width,
        /// height, depth, weight, category and rotation permission.
        /// </summary>
        /// <param name="extents">Width, height and depth of the chunks to construct a new template for.</param>
        /// <param name="weight">Relative weight of the new chunk template.</param>
        /// <param name="tag">Category of the new chunk template.</param>
        /// <param name="allowChunkRotation">Whether the level generator is allowed to rotate chunks created with the template by 90°, or not.</param>
        /// <seealso cref="ChunkTemplate.Weight"/>
        /// <seealso cref="ChunkTemplate.Tag"/>
        /// <exception cref="ArgumentOutOfRangeException">Width, height, depth or weight is less then or equal to zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="tag"/> is null.</exception>
        public ChunkTemplate3D(Vector3F extents, int weight, string tag, bool allowChunkRotation)
            : base(weight, tag, allowChunkRotation)
        {
            if (extents.X <= 0f)
            {
                throw new ArgumentOutOfRangeException("extents", "Width of the template must be greater than zero.");
            }

            if (extents.Y <= 0f)
            {
                throw new ArgumentOutOfRangeException("extents", "Height of the template must be greater than zero.");
            }

            if (extents.Z <= 0f)
            {
                throw new ArgumentOutOfRangeException("extents", "Depth of the template must be greater than zero.");
            }

            this.Extents = extents;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Width, height and depth of the chunks constructed with this template.
        /// </summary>
        public Vector3F Extents { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a new anchor at the passed position relative to the
        /// positions of the chunks constructed with this template.
        /// </summary>
        /// <param name="relativePosition">
        /// Position of the new anchor relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        public void AddAnchor(Vector3F relativePosition)
        {
            this.AddAnchor(relativePosition, string.Empty);
        }

        /// <summary>
        /// Adds a new anchor of the specified category at the passed
        /// position relative to the positions of the chunks constructed with
        /// this template.
        /// </summary>
        /// <param name="relativePosition">
        /// Position of the new context relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        /// <param name="tag">Category of the new anchor.</param>
        public void AddAnchor(Vector3F relativePosition, string tag)
        {
            this.AddAnchor(new Anchor3D(relativePosition, tag));
        }

        /// <summary>
        /// Adds a new context at the passed position relative to the
        /// positions of the chunks constructed with this template.
        /// </summary>
        /// <param name="relativePosition">
        /// Position of the new context relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        public void AddContext(Vector3F relativePosition)
        {
            this.AddContext(new Context3D(relativePosition));
        }

        /// <summary>
        /// Adds a new context of the specified category at the passed
        /// position relative to the positions of the chunks constructed with
        /// this template.
        /// </summary>
        /// <param name="relativePosition">
        /// Position of the new context relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        /// <param name="tag">Category of the new context.</param>
        public void AddContext(Vector3F relativePosition, string tag)
        {
            this.AddContext(new Context3D(relativePosition, tag));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the passed anchor to the list of anchors of this chunk
        /// template and sets its chunk-wide unique index.
        /// </summary>
        /// <param name="anchor">Anchor to add to this template.</param>
        private void AddAnchor(Anchor3D anchor)
        {
            anchor.Index = this.AnchorCount;
            this.ChunkTemplateAnchors.Add(anchor);
        }

        /// <summary>
        /// Adds the passed context to the list of contexts of this chunk
        /// template and sets its chunk-wide unique index.
        /// </summary>
        /// <param name="context">Context to add to this template.</param>
        private void AddContext(Context3D context)
        {
            context.Index = this.ContextCount;
            this.ChunkTemplateContexts.Add(context);
        }

        #endregion
    }
}