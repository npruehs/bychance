// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlignAdjacentContextsPolicy.cs" company="Nick Pruehs, Denis Vaz Alves">
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
namespace ByChance.PostProc
{
    using System;

    using ByChanceFramework;

    /// <summary>
    /// Aligns all open contexts in the processed level that are within the offset
    /// specified at the construction of this policy and are allowed to be aligned
    /// according to the level generator that build the processed level.
    /// </summary>
    /// <seealso cref="Offset"/>
    /// <seealso cref="ByChanceFramework.LevelGenerator.CanBeAligned(Context, Context)"/>
    public class AlignAdjacentContextsPolicy : IPostProcessingPolicy
    {
        #region Constructors and Destructors

        /// <summary>
        /// Creates a new post-processing policy for aligning contexts that are
        /// within the specified offset.
        /// </summary>
        /// <param name="offset">Offset within two contexts are to be aligned.</param>
        public AlignAdjacentContextsPolicy(float offset)
        {
            this.Offset = offset;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Offset within two contexts are to be aligned.
        /// </summary>
        public float Offset { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Aligns all open contexts in the processed level that are within the offset
        /// specified at the construction of this policy and are allowed to be aligned
        /// according to the level generator that build the processed level.
        /// </summary>
        /// <typeparam name="T">Type of the chunks the level consists of.</typeparam>
        /// <param name="levelGenerator">Level generator that built the level to be processed.</param>
        /// <param name="level">Level to process.</param>
        /// <seealso cref="Offset"/>
        /// <seealso cref="ByChanceFramework.LevelGenerator.CanBeAligned(Context, Context)"/>
        /// <exception cref="ArgumentNullException"><paramref name="levelGenerator"/> or <paramref name="level"/> is <c>null</c>.</exception>
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

            var openContexts = level.FindOpenContexts();
            for (var i = 0; i < openContexts.Count - 1; i++)
            {
                var firstContext = openContexts[i];
                if (firstContext.Target != null)
                {
                    continue;
                }

                for (var j = i + 1; j < openContexts.Count; j++)
                {
                    var secondContext = openContexts[j];

                    if (firstContext.IsAdjacentTo(secondContext, this.Offset)
                        && levelGenerator.CanBeAligned(firstContext, secondContext))
                    {
                        firstContext.AlignTo(secondContext);

                        levelGenerator.LogMessage(
                            string.Format(
                                "+ Aligned adjacent contexts at {0} with an offset of {1}.", firstContext, this.Offset));
                    }
                }
            }
        }

        #endregion
    }
}