/*
 * Copyright 2011 Nick Pruehs, Denis Vaz Alves.
 * 
 * This file is part of the ByChance Framework.
 *
 * The ByChance Framework is free software: you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 * 
 * The ByChance Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with the ByChance Framework.  If not, see
 * <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using NLog;
using System;

namespace ByChanceFramework.PostProc
{
    /// <summary>
    /// <para>
    /// Finds all chunks within the processed level that have open contexts and
    /// uses the predicate method <c>ShouldBeDiscarded(Chunk)</c> to check
    /// whether to discard that chunk or not. Repeats that process until the
    /// first iteration in which no chunk is discarded.
    /// </para>
    /// <para>
    /// This policy discards all chunks with open contexts by default. Override
    /// <c>ShouldBeDiscarded(Chunk)</c> to access every chunk's attributes like
    /// their position or tag and specify your own predicate.
    /// </para>
    /// <seealso cref="ShouldBeDiscarded(Chunk)"/>
    /// </summary>
    public class DiscardOpenChunksPolicy : IPostProcessingPolicy
    {
        /// <summary>
        /// The logger used for logging all chunks discarded during the process.
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Gets or sets the level generator that build the processed level.
        /// </summary>
        public LevelGenerator LevelGenerator { get; set; }


        /// <summary>
        /// Finds all chunks within the processed level that have open contexts and
        /// uses the predicate method <c>ShouldBeDiscarded(Chunk)</c> to check
        /// whether to discard that chunk or not. Repeats that process until the
        /// first iteration in which no chunk is discarded.
        /// </summary>
        /// <typeparam name="T">Type of the chunks the level consists of.</typeparam>
        /// <param name="level">The level to process.</param>
        /// <seealso cref="ShouldBeDiscarded(Chunk)"/>
        /// <exception cref="ArgumentNullException"><c>level</c> is <c>null</c>.</exception>
        public void Process<T>(Level<T> level) where T : Chunk
        {
            List<Chunk> openChunks;
            bool continueProcess;

            if (level == null)
            {
                throw new ArgumentNullException("level");
            }

            continueProcess = true;
            while (continueProcess)
            {
                continueProcess = false;

                openChunks = level.FindOpenChunks();
                foreach (Chunk chunk in openChunks)
                {
                    if (ShouldBeDiscarded(chunk))
                    {
                        level.RemoveChunk(chunk);

                        logger.Info("+ Removed chunk at " + chunk + ".");

                        continueProcess = true;
                    }
                }
            }
        }

        /// <summary>
        /// Used by this policy to check whether to discard the passed chunk
        /// with open contexts, or not. Returns <c>true</c> by default.
        /// </summary>
        /// <param name="chunk">The chunk to be discarded.</param>
        /// <returns><c>true</c> if the specified chunk should be discarded, and <c>false</c> otherwise.</returns>
        public virtual bool ShouldBeDiscarded(Chunk chunk)
        {
            return true;
        }
    }
}
