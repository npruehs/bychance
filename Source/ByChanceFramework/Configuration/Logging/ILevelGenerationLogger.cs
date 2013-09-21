// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILevelGenerationLogger.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
//   
//   This file is part of the ByChance Framework.
//   
//   The ByChance Framework is free software: you can redistribute it and/or
//   modify it under the terms of the GNU Lesser General Public License as
//   published by the Free Software Foundation, either version 3 of the License,
//   or (at your option) any later version.
//   
//   The ByChance Framework is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
//   
//   You should have received a copy of the GNU Lesser General Public License
//   along with the ByChance Framework.  If not, see
//   <http://www.gnu.org/licenses/>.
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