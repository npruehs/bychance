// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDiscardOpenContextsRestriction.cs" company="Nick Pruehs, Denis Vaz Alves">
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
    public interface IDiscardOpenContextsRestriction
    {
        #region Public Methods and Operators

        /// <summary>
        /// Used by <see cref="DiscardOpenContextsPolicy"/> to check whether to discard the passed open
        /// context, or not.
        /// </summary>
        /// <param name="context">Context to be discarded.</param>
        /// <returns><c>true</c> if the specified context should be discarded, and <c>false</c> otherwise.</returns>
        bool ShouldDiscardContext(Context context);

        #endregion
    }
}