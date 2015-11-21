// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelGeneratorConfiguration.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2015 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Configuration
{
    using System.Collections.Generic;

    using ByChance.Configuration.Logging;
    using ByChance.Configuration.Parameters;
    using ByChance.Configuration.PostProcessing;

    /// <summary>
    /// Specified level generation parameters, such as which chunk contexts may be aligned, or the distribution of chunk templates within a level.
    /// </summary>
    public class LevelGeneratorConfiguration
    {
        #region Properties

        /// <summary>
        /// Distribution of chunk templates within a level.
        /// </summary>
        public IChunkDistribution ChunkDistribution { get; set; }

        /// <summary>
        /// Which chunk contexts may be aligned.
        /// </summary>
        public IContextAlignmentRestriction ContextAlignmentRestriction { get; set; }

        /// <summary>
        /// Logger interface for writing log messages.
        /// </summary>
        public ILevelGenerationLogger Logger { get; set; }

        /// <summary>
        /// Post-processing policies that will be applied after the level generation.
        /// </summary>
        public IList<PostProcessingPolicy> PostProcessingPolicies { get; set; }

        /// <summary>
        ///   When to stop level generation. The process is stopped if <i>any</i> of
        ///   these conditions if met.
        /// </summary>
        public IList<ITerminationCondition> TerminationConditions { get; set; }

        #endregion
    }
}