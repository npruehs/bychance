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
namespace ByChance.Configuration.PostProcessing
{
    using System;
    using System.Linq;

    using ByChance.Configuration.Parameters;
    using ByChance.Core;

    /// <summary>
    /// <para>
    /// Finds all chunks within the processed level that have open contexts and
    /// uses the predicate method <see cref="Parameters.IDiscardOpenChunksRestriction.ShouldBeDiscarded"/> to check
    /// whether to discard that chunk or not. Repeats that process until the
    /// first iteration in which no chunk is discarded.
    /// </para>
    /// <para>
    /// This policy discards all chunks with open contexts by default. Set
    /// <see cref="DiscardOpenChunksRestriction"/> to specify your own predicate.
    /// </para>
    /// <seealso cref="Parameters.IDiscardOpenChunksRestriction.ShouldBeDiscarded(Chunk)"/>
    /// </summary>
    public sealed class DiscardOpenChunksPolicy : PostProcessingPolicy
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new policy for discarding open chunks.
        /// </summary>
        public DiscardOpenChunksPolicy()
        {
            this.DiscardOpenChunksRestriction = new DiscardOpenChunksRestriction();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Which chunks with open contexts should be discarded.
        /// </summary>
        public IDiscardOpenChunksRestriction DiscardOpenChunksRestriction { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Finds all chunks within the processed level that have open contexts and
        /// uses the predicate <see cref="DiscardOpenChunksRestriction"/> to check
        /// whether to discard that chunk or not. Repeats that process until the
        /// first iteration in which no chunk is discarded.
        /// </summary>
        /// <typeparam name="T">Type of the chunks the level consists of.</typeparam>
        /// <param name="configuration">Configuration of the level generator that built the level to be processed.</param>
        /// <param name="level">Level to process.</param>
        /// <seealso cref="DiscardOpenChunksRestriction"/>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> or <paramref name="level"/> is <c>null</c>.</exception>
        public override void Process<T>(LevelGeneratorConfiguration configuration, Level<T> level)
        {
            base.Process(configuration, level);

            var continueProcess = true;
            while (continueProcess)
            {
                continueProcess = false;

                var openChunks = level.FindOpenChunks();
                foreach (var chunk in openChunks.Where(this.DiscardOpenChunksRestriction.ShouldBeDiscarded))
                {
                    level.RemoveChunk(chunk);

                    this.LogMessage(string.Format("+ Removed chunk at {0}.", chunk));

                    continueProcess = true;
                }
            }
        }

        #endregion
    }
}