// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChunkDistribution.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ByChance.Configuration.Parameters
{
    using ByChance.Core;

    /// <summary>
    /// Specifies the distribution of chunk templates within a level.
    /// </summary>
    public sealed class ChunkDistribution : IChunkDistribution
    {
        #region Public Methods and Operators

        /// <summary>
        /// <para>
        /// Returns the weight of the chunk candidate to which <paramref name="secondContext"/> belongs to.
        /// </para>
        /// </summary>
        /// <param name="firstContext">Open context of the existing chunk to which the new chunk candidate will be attached to.</param>
        /// <param name="secondContext">Open context of the chunk candidate.</param>
        /// <param name="occurrences">
        /// Number of times chunks similar to the chunk of <paramref name="secondContext"/> (i.e. based on the same template) 
        /// already exist in the level.
        /// </param>
        /// <returns>Non-negative integer that represents the effective weight of the chunk candidate.</returns>
        public int GetEffectiveWeight(Context firstContext, Context secondContext, int occurrences)
        {
            return secondContext.Source.Weight;
        }

        #endregion
    }
}