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
    /// Finds all contexts within the processed level and
    /// uses the predicate method <c>ShouldDiscardContext(Context)</c> to check
    /// whether to discard that context or not. Repeats that process until the
    /// first iteration in which no context is discarded.
    /// </para>
    /// <para>
    /// This policy discards all open contexts by default. Override
    /// <c>ShouldDiscardContext(Context)</c> to access every context's attributes like
    /// their position or tag and specify your own predicate.
    /// </para>
    /// <seealso cref="ShouldDiscardContext(Context)"/>
    /// </summary>
    public class DiscardOpenContextsPolicy : IPostProcessingPolicy
    {
        /// <summary>
        /// The logger used for logging all contexts discarded during the process.
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Gets or sets the level generator that build the processed level.
        /// </summary>
        public LevelGenerator LevelGenerator { get; set; }


        /// <summary>
        /// Finds all contexts within the processed level and
        /// uses the predicate method <c>ShouldDiscardContext(Context)</c> to check
        /// whether to discard that context or not. Repeats that process until the
        /// first iteration in which no context is discarded.
        /// </summary>
        /// <typeparam name="T">Type of the chunks the level consists of.</typeparam>
        /// <param name="level">The level to process.</param>
        /// <seealso cref="ShouldDiscardContext(Context)"/>
        /// <exception cref="ArgumentNullException"><c>level</c> is <c>null</c>.</exception>
        public void Process<T>(Level<T> level) where T : Chunk
        {
            List<Context> openContexts;

            if (level == null)
            {
                throw new ArgumentNullException("level");
            }

            openContexts = level.FindOpenContexts();
            foreach (Context context in openContexts)
            {
                if (ShouldDiscardContext(context))
                {
                    context.Source.RemoveContext(context);

                    logger.Info("+ Removed context at " + context + ".");
                }
            }
        }

        /// <summary>
        /// Used by this policy to check whether to discard the passed open
        /// context, or not. Returns <c>true</c> by default.
        /// </summary>
        /// <param name="context">The context to be discarded.</param>
        /// <returns><c>true</c> if the specified context should be discarded, and <c>false</c> otherwise.</returns>
        public virtual bool ShouldDiscardContext(Context context)
        {
            return false;
        }
    }
}
