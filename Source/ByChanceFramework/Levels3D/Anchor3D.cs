﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Anchor3D.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2016 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Levels3D
{
    using ByChance.Core;

    /// <summary>
    /// Placeholder for game elements in 3D levels that can be
    /// filled after the level generation process.
    /// </summary>
    public sealed class Anchor3D : Anchor
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new anchor of the specified category and with the passed
        /// position relative to the position of the chunk it belongs to.
        /// </summary>
        /// <param name="relativePosition">
        /// Position of the new anchor relative to the
        /// position of the chunk it belongs to.
        /// </param>
        /// <param name="tag">Category of the new anchor.</param>
        internal Anchor3D(Vector3F relativePosition, string tag)
            : base(tag)
        {
            this.RelativePosition = relativePosition;
        }

        /// <summary>
        /// Constructs a new anchor with the same index and relative position
        /// as the passed one and attaches it to the specified chunk.
        /// </summary>
        /// <param name="template">Anchor whose attributes to copy.</param>
        /// <param name="source">Chunk to attach the new anchor to.</param>
        internal Anchor3D(Anchor3D template, Chunk source)
            : base(template.Tag)
        {
            this.Index = template.Index;
            this.Source = source;
            this.RelativePosition = template.RelativePosition;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Absolute position of this anchor within the level.
        /// </summary>
        public Vector3F AbsolutePosition
        {
            get
            {
                return ((Chunk3D)this.Source).Position + this.RelativePosition;
            }
        }

        /// <summary>
        /// Position of this anchor relative to the
        /// position of the chunk it belongs to.
        /// </summary>
        public Vector3F RelativePosition { get; internal set; }

        #endregion
    }
}