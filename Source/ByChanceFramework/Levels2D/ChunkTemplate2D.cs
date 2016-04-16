// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChunkTemplate2D.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2016 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Levels2D
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using ByChance.Core;

    /// <summary>
    /// Template that is used for creating similar chunks, which
    /// in turn make up a 2D level. Chunk templates define the chunk extents,
    /// the positions of all contexts and anchors of chunks, as well as
    /// attributes that are required for the level generation process such as
    /// the probability of a chunk being picked to be added next.
    /// </summary>
    public sealed class ChunkTemplate2D : ChunkTemplate
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width
        /// and height and a default weight. Chunks constructed with this
        /// template may not be rotated by the level generator.
        /// </summary>
        /// <param name="extents">Width and height of the chunks to construct a new template for.</param>
        /// <seealso cref="ChunkTemplate.DefaultWeight"/>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly",
            Justification = "Qualifier is redundant.")]
        public ChunkTemplate2D(Vector2F extents)
            : this(extents, DefaultWeight, string.Empty, false)
        {
        }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width
        /// and height and the passed weight. Chunks constructed with this
        /// template may not be rotated by the level generator.
        /// </summary>
        /// <param name="extents">Width and height of the chunks to construct a new template for.</param>
        /// <param name="weight">Relative weight of the new chunk template.</param>
        /// <seealso cref="ChunkTemplate.Weight"/>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="weight"/> is less than or equal to zero.</exception>
        public ChunkTemplate2D(Vector2F extents, int weight)
            : this(extents, weight, string.Empty, false)
        {
        }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width
        /// and height and a default weight. Chunks constructed with the new template will be of
        /// of the passed category and may not be rotated by the level generator.
        /// </summary>
        /// <param name="extents">Width and height of the chunks to construct a new template for.</param>
        /// <param name="tag">Category of the new chunk template.</param>
        /// <seealso cref="ChunkTemplate.DefaultWeight"/>
        /// <seealso cref="ChunkTemplate.Tag"/>
        /// <exception cref="ArgumentNullException"><paramref name="tag"/> is null.</exception>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly",
            Justification = "Qualifier is redundant.")]
        public ChunkTemplate2D(Vector2F extents, string tag)
            : this(extents, DefaultWeight, tag, false)
        {
        }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width,
        /// height and rotation permission. Chunks constructed with this template
        /// will have a default weight and may not be rotated by the level generator.
        /// </summary>
        /// <param name="extents">Width and height of the chunks to construct a new template for.</param>
        /// <param name="allowChunkRotation">Whether the level generator is allowed to rotate chunks created with the template by 90°, or not.</param>
        /// <seealso cref="ChunkTemplate.DefaultWeight"/>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly",
            Justification = "Qualifier is redundant.")]
        public ChunkTemplate2D(Vector2F extents, bool allowChunkRotation)
            : this(extents, DefaultWeight, string.Empty, allowChunkRotation)
        {
        }

        /// <summary>
        /// Constructs a new chunk template for chunks with the specified width,
        /// height, weight, category and rotation permission.
        /// </summary>
        /// <param name="extents">Width and height of the chunks to construct a new template for.</param>
        /// <param name="weight">Relative weight of the new chunk template.</param>
        /// <param name="tag">Category of the new chunk template.</param>
        /// <param name="allowChunkRotation">Whether the level generator is allowed to rotate chunks created with the template by 90°, or not.</param>
        /// <seealso cref="ChunkTemplate.Weight"/>
        /// <seealso cref="ChunkTemplate.Tag"/>
        /// <exception cref="ArgumentOutOfRangeException">Width, height or <paramref name="weight"/> is less then or equal to zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="tag"/> is null.</exception>
        public ChunkTemplate2D(Vector2F extents, int weight, string tag, bool allowChunkRotation)
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

            this.Extents = extents;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Width and height of the chunks constructed with this template.
        /// </summary>
        public Vector2F Extents { get; private set; }

        /// <summary>
        ///   Total size of chunks of this template.
        /// </summary>
        public override float Size
        {
            get
            {
                return this.Extents.X * this.Extents.Y;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a new anchor of the specified category at the passed
        /// position relative to the positions of the chunks constructed with
        /// this template.
        /// </summary>
        /// <param name="relativePosition">
        /// Position of the new anchor relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        /// <param name="tag">Category of the new anchor.</param>
        public void AddAnchor(Vector2F relativePosition, string tag)
        {
            this.AddAnchor(new Anchor2D(relativePosition, tag));
        }

        /// <summary>
        /// Adds a new context at the passed position relative to the
        /// positions of the chunks constructed with this template.
        /// </summary>
        /// <param name="relativePosition">
        /// Position of the new context relative to the
        /// positions of the chunks constructed with this template.
        /// </param>
        public void AddContext(Vector2F relativePosition)
        {
            this.AddContext(new Context2D(relativePosition));
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
        public void AddContext(Vector2F relativePosition, string tag)
        {
            this.AddContext(new Context2D(relativePosition, tag));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the passed anchor to the list of anchors of this chunk
        /// template and sets its chunk-wide unique index.
        /// </summary>
        /// <param name="anchor">Anchor to add to this template.</param>
        private void AddAnchor(Anchor anchor)
        {
            anchor.Index = this.AnchorCount;
            this.ChunkTemplateAnchors.Add(anchor);
        }

        /// <summary>
        /// Adds the passed context to the list of contexts of this chunk
        /// template and sets its chunk-wide unique index.
        /// </summary>
        /// <param name="context">Context to add to this template.</param>
        private void AddContext(Context context)
        {
            context.Index = this.ContextCount;
            this.ChunkTemplateContexts.Add(context);
        }

        #endregion
    }
}