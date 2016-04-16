// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContextAlignmentRestriction.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2016 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Configuration.Parameters
{
    using ByChance.Core;

    /// <summary>
    /// Specifies which chunk contexts may be aligned.
    /// </summary>
    public interface IContextAlignmentRestriction
    {
        #region Public Methods and Operators

        /// <summary>
        /// Checks if the two passed contexts can be aligned, or not.
        /// This relation is assumed to be symmetric i.e., if <c>first</c> and <c>second</c>
        /// can be aligned, then <c>second</c> and <c>first</c> can be aligned, too.
        /// </summary>
        /// <param name="first">First context to check.</param>
        /// <param name="second">Second context to check.</param>
        /// <param name="level">Current generated level.</param>
        /// <returns><c>true</c>, if the two contexts can be aligned, and <c>false</c> otherwise.</returns>
        bool CanBeAligned(Context first, Context second, object level);

        #endregion
    }
}