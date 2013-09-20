// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscardOpenChunksPolicy.cs" company="Nick Pruehs, Denis Vaz Alves">
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
namespace ByChance.PostProcessing
{
    using System;
    using System.Linq;

    using ByChance.Core;

    /// <summary>
    /// <para>
    /// Finds all chunks within the processed level that have open contexts and
    /// uses the predicate method <see cref="ShouldBeDiscarded"/> to check
    /// whether to discard that chunk or not. Repeats that process until the
    /// first iteration in which no chunk is discarded.
    /// </para>
    /// <para>
    /// This policy discards all chunks with open contexts by default. Override
    /// <see cref="ShouldBeDiscarded"/> to access the chunk's attributes like
    /// their position or tag and specify your own predicate.
    /// </para>
    /// <seealso cref="ShouldBeDiscarded(Chunk)"/>
    /// </summary>
    public class DiscardOpenChunksPolicy : IPostProcessingPolicy
    {
        #region Public Methods and Operators

        /// <summary>
        /// Finds all chunks within the processed level that have open contexts and
        /// uses the predicate method <see cref="ShouldBeDiscarded"/> to check
        /// whether to discard that chunk or not. Repeats that process until the
        /// first iteration in which no chunk is discarded.
        /// </summary>
        /// <typeparam name="T">Type of the chunks the level consists of.</typeparam>
        /// <param name="levelGenerator">Level generator that built the level to be processed.</param>
        /// <param name="level">Level to process.</param>
        /// <seealso cref="ShouldBeDiscarded(Chunk)"/>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> or <paramref name="level"/> is <c>null</c>.</exception>
        public void Process<T>(LevelGenerator levelGenerator, Level<T> level) where T : Chunk
        {
            if (levelGenerator == null)
            {
                throw new ArgumentNullException("levelGenerator");
            }

            if (level == null)
            {
                throw new ArgumentNullException("level");
            }

            var continueProcess = true;
            while (continueProcess)
            {
                continueProcess = false;

                var openChunks = level.FindOpenChunks();
                foreach (var chunk in openChunks.Where(this.ShouldBeDiscarded))
                {
                    level.RemoveChunk(chunk);

                    levelGenerator.LogMessage(string.Format("+ Removed chunk at {0}.", chunk));

                    continueProcess = true;
                }
            }
        }

        /// <summary>
        /// Used by this policy to check whether to discard the passed chunk
        /// with open contexts, or not. Returns <c>true</c> by default.
        /// </summary>
        /// <param name="chunk">Chunk to be discarded.</param>
        /// <returns><c>true</c> if the specified chunk should be discarded, and <c>false</c> otherwise.</returns>
        public virtual bool ShouldBeDiscarded(Chunk chunk)
        {
            return true;
        }

        #endregion
    }
}