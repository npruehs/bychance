// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostProcessingPolicy.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ByChance.Configuration.PostProcessing
{
    using System;

    using ByChance.Core;

    /// <summary>
    /// <para>
    /// Encapsulates some way of processing a level after it has been
    /// successfully generated.
    /// </para>
    /// <para>
    /// Post-processing policies are optional
    /// and by no means necessary to create a level. They are provided
    /// as a convenient and uniform way to automatically run post-generation
    /// algorithms that are tracked and logged to the same log file in which 
    /// the ByChance framework summarizes the level generation process. The
    /// framework uses the strategy pattern to define a family of processing
    /// algorithms that can be called after the level has been generated.
    /// </para>
    /// <para>
    /// Each post-processing policy has access to the level generator configuration
    /// in order to determine which contexts can be aligned, for example.
    /// </para>
    /// <para>
    /// As part of the contract, post-processing policies are entitled to be
    /// able to process 2D and 3D levels alike.
    /// </para>
    /// </summary>
    public abstract class PostProcessingPolicy
    {
        #region Fields

        /// <summary>
        /// Level generation parameters, such as which chunk contexts may be aligned, or the distribution of chunk templates within a level.
        /// </summary>
        private LevelGeneratorConfiguration configuration;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Processes the passed finished level following the rules defined by
        /// the specified level generator generation.
        /// </summary>
        /// <typeparam name="T">Type of the chunks the level consists of.</typeparam>
        /// <param name="levelGeneratorConfiguration">Configuration of the level generator that built the level to be processed.</param>
        /// <param name="level">Level to process.</param>
        /// <exception cref="ArgumentNullException"><paramref name="levelGeneratorConfiguration"/> or <paramref name="level"/> is <c>null</c>.</exception>
        public virtual void Process<T>(LevelGeneratorConfiguration levelGeneratorConfiguration, Level<T> level)
            where T : Chunk
        {
            if (levelGeneratorConfiguration == null)
            {
                throw new ArgumentNullException("levelGeneratorConfiguration");
            }

            if (level == null)
            {
                throw new ArgumentNullException("level");
            }

            this.configuration = levelGeneratorConfiguration;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Writes the specified message to the level generation log.
        /// </summary>
        /// <param name="message">Message to log.</param>
        protected void LogMessage(string message)
        {
            if (this.configuration.Logger != null)
            {
                this.configuration.Logger.LogMessage(message);
            }
        }

        #endregion
    }
}