// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChunkDistribution.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2016 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Configuration.Parameters
{
    using ByChance.Core;

    /// <summary>
    /// Specifies the distribution of chunk templates within a level.
    /// </summary>
    public interface IChunkDistribution
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the effective weight of a chunk.
        /// <para>
        /// Most simple implementation is to just return the weight of the chunk candidate to which <paramref name="chunkCandidateContext"/> belongs to.
        /// <paramref name="freeLevelContext"/> and <paramref name="level"/> are passed for custom implementations by clients. 
        /// </para>
        /// </summary>
        /// <param name="freeLevelContext">Open context of the existing chunk to which the new chunk candidate will be attached to.</param>
        /// <param name="chunkCandidateContext">Open context of the chunk candidate.</param>
        /// <param name="level">Current generated level.</param>
        /// <returns>Non-negative integer that represents the effective weight of the chunk candidate.</returns>
        int GetEffectiveWeight(Context freeLevelContext, Context chunkCandidateContext, object level);

        #endregion
    }
}