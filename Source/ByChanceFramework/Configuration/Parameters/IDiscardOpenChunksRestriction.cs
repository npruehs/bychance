// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDiscardOpenChunksRestriction.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ByChance.Configuration.Parameters
{
    using ByChance.Configuration.PostProcessing;
    using ByChance.Core;

    /// <summary>
    /// Specifies which chunks with open contexts should be discarded.
    /// </summary>
    public interface IDiscardOpenChunksRestriction
    {
        #region Public Methods and Operators

        /// <summary>
        /// Used by <see cref="DiscardOpenChunksPolicy"/> to check whether to discard the passed chunk
        /// with open contexts, or not.
        /// </summary>
        /// <param name="chunk">Chunk to be discarded.</param>
        /// <returns><c>true</c> if the specified chunk should be discarded, and <c>false</c> otherwise.</returns>
        bool ShouldBeDiscarded(Chunk chunk);

        #endregion
    }
}