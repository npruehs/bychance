// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscardOpenContextsPolicy.cs" company="Nick Pruehs, Denis Vaz Alves">
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
    /// Finds all contexts within the processed level and
    /// uses the predicate method <see cref="ShouldDiscardContext"/> to check
    /// whether to discard that context or not. Repeats that process until the
    /// first iteration in which no context is discarded.
    /// </para>
    /// <para>
    /// This policy discards all open contexts by default. Override
    /// <see cref="ShouldDiscardContext"/> to access the context's attributes like
    /// their position or tag and specify your own predicate.
    /// </para>
    /// <seealso cref="ShouldDiscardContext(Context)"/>
    /// </summary>
    public class DiscardOpenContextsPolicy : IPostProcessingPolicy
    {
        #region Public Methods and Operators

        /// <summary>
        /// Finds all contexts within the processed level and
        /// uses the predicate method <c>ShouldDiscardContext(Context)</c> to check
        /// whether to discard that context or not. Repeats that process until the
        /// first iteration in which no context is discarded.
        /// </summary>
        /// <typeparam name="T">Type of the chunks the level consists of.</typeparam>
        /// <param name="levelGenerator">Level generator that built the level to be processed.</param>
        /// <param name="level">Level to process.</param>
        /// <seealso cref="ShouldDiscardContext(Context)"/>
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
            foreach (var context in openContexts.Where(this.ShouldDiscardContext))
            {
                context.Source.RemoveContext(context);

                levelGenerator.LogMessage(string.Format("+ Removed context at {0}.", context));
            }
        }

        /// <summary>
        /// Used by this policy to check whether to discard the passed open
        /// context, or not. Returns <c>true</c> by default.
        /// </summary>
        /// <param name="context">Context to be discarded.</param>
        /// <returns><c>true</c> if the specified context should be discarded, and <c>false</c> otherwise.</returns>
        public virtual bool ShouldDiscardContext(Context context)
        {
            return false;
        }

        #endregion
    }
}