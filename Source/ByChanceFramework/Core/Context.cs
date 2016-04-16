// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Context.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2016 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Core
{
    using System;

    /// <summary>
    /// Describes a position a chunk can be aligned at to another one.
    /// </summary>
    public abstract class Context
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new context of the specified category.
        /// </summary>
        /// <param name="tag">Category of the new context.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
        protected Context(string tag)
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
        /// Whether this context can be aligned to another one.
        /// </summary>
        public bool Blocked { get; internal set; }

        /// <summary>
        /// Chunk-wide unique index of this context.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Chunk this context belongs to.
        /// </summary>
        public Chunk Source { get; protected set; }

        /// <summary>
        /// Category of this context.
        /// Use this category to modify the probability of aligning this
        /// context to another one, or to tell the level generator whether
        /// two contexts can be aligned at all.
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// Chunk the source chunk is aligned to.
        /// </summary>
        public Context Target { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Aligns this context to <paramref name="other"/>, making <see cref="Target"/> of both contexts point to each other.
        /// </summary>
        /// <param name="other">Context to align this one to.</param>
        /// <seealso cref="Target"/>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">This context or <paramref name="other"/> is already aligned to another context.</exception>
        /// <exception cref="ArgumentException"><paramref name="other"/> has a different type than this context.</exception>
        public void AlignTo(Context other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (this.Target != null)
            {
                throw new InvalidOperationException(
                    "The source context is already aligned to another context. "
                    + "Make sure to clear the target of the source context before aligning it to another one.");
            }

            if (other.Target != null)
            {
                throw new InvalidOperationException(
                    "The target context is already aligned to another context. "
                    + "Make sure to clear the target of the target context before aligning it to another one.");
            }

            if (this.GetType() != other.GetType())
            {
                throw new ArgumentException(
                    "The target context has a different type than the source context. "
                    + "Aligned contexts have to be of the same type.",
                    "other");
            }

            this.Target = other;
            other.Target = this;
        }

        /// <summary>
        /// Breaks the connection between this context and its target, setting <see cref="Target"/> of both contexts to <c>null</c>.
        /// </summary>
        public void ClearTarget()
        {
            if (this.Target == null)
            {
                return;
            }

            this.Target.Target = null;
            this.Target = null;
        }

        /// <summary>
        /// Checks whether this context is within <paramref name="offset"/> to <paramref name="other"/>.
        /// </summary>
        /// <param name="other">Context to check the adjacency to.</param>
        /// <param name="offset">Offset within this context is considered to be adjacent to <paramref name="other"/>.</param>
        /// <returns><c>true</c>, if <paramref name="other"/> is within <paramref name="offset"/> to this one, and <c>false</c> otherwise.</returns>
        public abstract bool IsAdjacentTo(Context other, float offset);

        #endregion
    }
}