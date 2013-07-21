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
    /// Aligns all open contexts in the processed level that are within the offset
    /// specified at the construction of this policy and are allowed to be aligned
    /// according to the level generator that build the processed level.
    /// </summary>
    /// <seealso cref="offset"/>
    /// <seealso cref="ByChanceFramework.LevelGenerator.CanBeAligned(Context, Context)"/>
    public class AlignAdjacentContextsPolicy : IPostProcessingPolicy
    {
        /// <summary>
        /// The logger used for logging all contexts aligned during the process.
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// The offset within two contexts are to be aligned.
        /// </summary>
        private float offset;


        /// <summary>
        /// Gets or sets the level generator that build the processed level and
        /// tells whether two contexts can be aligned or not.
        /// </summary>
        public LevelGenerator LevelGenerator { get; set; }


        /// <summary>
        /// Creates a new post-processing policy for aligning contexts that are
        /// within the specified offset.
        /// </summary>
        /// <param name="offset">The offset within two contexts are to be aligned.</param>
        public AlignAdjacentContextsPolicy(float offset)
        {
            this.offset = offset;
        }


        /// <summary>
        /// Aligns all open contexts in the processed level that are within the offset
        /// specified at the construction of this policy and are allowed to be aligned
        /// according to the level generator that build the processed level.
        /// </summary>
        /// <typeparam name="T">Type of the chunks the level consists of.</typeparam>
        /// <param name="level">The level to process.</param>
        /// <seealso cref="offset"/>
        /// <seealso cref="ByChanceFramework.LevelGenerator.CanBeAligned(Context, Context)"/>
        /// <exception cref="ArgumentNullException"><c>level</c> or <c>LevelGenerator</c> is <c>null</c>.</exception>
        public void Process<T>(Level<T> level) where T : Chunk
        {
            List<Context> openContexts;
            Context firstContext;
            Context secondContext;

            if (level == null)
            {
                throw new ArgumentNullException("level");
            }

            if (LevelGenerator == null)
            {
                throw new InvalidOperationException
                    ("The level generator associated with this policy has not been set. " +
                     "Please specify the level generator using this policy before processing the level.");
            }

            openContexts = level.FindOpenContexts();
            for (int i = 0; i < openContexts.Count - 1; i++)
            {
                firstContext = openContexts[i];
                if (firstContext.Target == null)
                {
                    for (int j = i + 1; j < openContexts.Count; j++)
                    {
                        secondContext = openContexts[j];

                        if (firstContext.IsAdjacentTo(secondContext, offset) &&
                            LevelGenerator.CanBeAligned(firstContext, secondContext))
                        {
                            firstContext.AlignTo(secondContext);

                            logger.Info("+ Aligned adjacent contexts at " + firstContext.ToString() + " with an offset of " + offset + ".");
                        }
                    }
                }
            }
        }
    }
}
