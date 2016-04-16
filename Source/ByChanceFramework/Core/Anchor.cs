// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Anchor.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2016 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Core
{
    using System;

    /// <summary>
    /// Placeholder for game elements that can be filled after
    /// the level generation process.
    /// </summary>
    public abstract class Anchor
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new anchor of the specified category.
        /// </summary>
        /// <param name="tag">Category of the new anchor.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tag"/> is null.</exception>
        protected Anchor(string tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            this.Tag = tag;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Chunk-wide unique index of this anchor.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Chunk this anchor belongs to.
        /// </summary>
        public Chunk Source { get; protected set; }

        /// <summary>
        /// Category of this anchor.
        /// Use this category for determining how to fill this anchor after
        /// the level generation process is complete.
        /// </summary>
        public string Tag { get; private set; }

        #endregion
    }
}