// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscardOpenContextsRestriction.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2016 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Configuration.Parameters
{
    using ByChance.Configuration.PostProcessing;
    using ByChance.Core;

    /// <summary>
    /// Specifies which open contexts should be discarded.
    /// </summary>
    public sealed class DiscardOpenContextsRestriction : IDiscardOpenContextsRestriction
    {
        #region Public Methods and Operators

        /// <summary>
        /// Used by <see cref="DiscardOpenContextsPolicy"/> to check whether to discard the passed open
        /// context, or not. Returns <c>true</c>.
        /// </summary>
        /// <param name="context">Context to be discarded.</param>
        /// <returns><c>true</c></returns>
        public bool ShouldDiscardContext(Context context)
        {
            return true;
        }

        #endregion
    }
}