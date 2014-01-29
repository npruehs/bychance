// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextAlignmentRestriction.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2014 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Configuration.Parameters
{
    using ByChance.Core;

    /// <summary>
    /// Specifies which chunk contexts may be aligned.
    /// </summary>
    public sealed class ContextAlignmentRestriction : IContextAlignmentRestriction
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns <c>true</c>.
        /// </summary>
        /// <param name="first">First context to check.</param>
        /// <param name="second">Second context to check.</param>
        /// <returns><c>true</c></returns>
        public bool CanBeAligned(Context first, Context second)
        {
            return true;
        }

        #endregion
    }
}