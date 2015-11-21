// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITerminationCondition.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2015 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Configuration.Parameters
{
    /// <summary>
    ///   Specifies when to stop the level generation process.
    /// </summary>
    public interface ITerminationCondition
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Returns <c>true</c>, if the level generation should stop, and
        ///   <c>false</c>, otherwise.
        /// </summary>
        /// <param name="chunkLibrary">Chunk library used for the level generation.</param>
        /// <param name="level">Current generated level.</param>
        /// <param name="configuration">Current level generation configuration.</param>
        /// <returns>
        ///   <c>true</c>, if the level generation should stop, and
        ///   <c>false</c>, otherwise.
        /// </returns>
        bool ConditionIsMet(object chunkLibrary, object level, LevelGeneratorConfiguration configuration);

        #endregion
    }
}