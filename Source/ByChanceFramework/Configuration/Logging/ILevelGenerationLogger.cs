// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILevelGenerationLogger.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ByChance.Configuration.Logging
{
    /// <summary>
    /// Logger interface for writing log messages.
    /// </summary>
    public interface ILevelGenerationLogger
    {
        #region Public Methods and Operators

        /// <summary>
        /// Writes the specified message to the level generation log.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void LogMessage(string message);

        #endregion
    }
}