// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscardOpenChunksRestriction.cs" company="Nick Pruehs, Denis Vaz Alves">
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
    public sealed class DiscardOpenChunksRestriction : IDiscardOpenChunksRestriction
    {
        #region Public Methods and Operators

        /// <summary>
        /// Used by <see cref="DiscardOpenChunksPolicy"/> to check whether to discard the passed chunk
        /// with open contexts, or not. Returns <c>true</c>.
        /// </summary>
        /// <param name="chunk">Chunk to be discarded.</param>
        /// <returns><c>true</c></returns>
        public bool ShouldBeDiscarded(Chunk chunk)
        {
            return true;
        }

        #endregion
    }
}