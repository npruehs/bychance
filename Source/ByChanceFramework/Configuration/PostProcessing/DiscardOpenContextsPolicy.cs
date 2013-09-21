﻿// --------------------------------------------------------------------------------------------------------------------
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
namespace ByChance.Configuration.PostProcessing
{
    using System;
    using System.Linq;

    using ByChance.Configuration.Parameters;
    using ByChance.Core;

    /// <summary>
    /// <para>
    /// Finds all contexts within the processed level and
    /// uses the predicate method <see cref="Parameters.IDiscardOpenContextsRestriction.ShouldDiscardContext"/> to check
    /// whether to discard that context or not. Repeats that process until the
    /// first iteration in which no context is discarded.
    /// </para>
    /// <para>
    /// This policy discards all open contexts by default. Set
    /// <see cref="DiscardOpenContextsRestriction"/> to specify your own predicate.
    /// </para>
    /// <seealso cref="Parameters.IDiscardOpenContextsRestriction.ShouldDiscardContext(Context)"/>
    /// </summary>
    public sealed class DiscardOpenContextsPolicy : PostProcessingPolicy
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new policy for discarding open contexts.
        /// </summary>
        public DiscardOpenContextsPolicy()
        {
            this.DiscardOpenContextsRestriction = new DiscardOpenContextsRestriction();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Which open contexts should be discarded.
        /// </summary>
        public IDiscardOpenContextsRestriction DiscardOpenContextsRestriction { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Finds all contexts within the processed level and
        /// uses the predicate <see cref="DiscardOpenContextsRestriction"/> to check
        /// whether to discard that context or not. Repeats that process until the
        /// first iteration in which no context is discarded.
        /// </summary>
        /// <typeparam name="T">Type of the chunks the level consists of.</typeparam>
        /// <param name="configuration">Configuration of the level generator that built the level to be processed.</param>
        /// <param name="level">Level to process.</param>
        /// <seealso cref="DiscardOpenContextsRestriction"/>
        /// <exception cref="ArgumentNullException"><paramref name="configuration"/> or <paramref name="level"/> is <c>null</c>.</exception>
        public override void Process<T>(LevelGeneratorConfiguration configuration, Level<T> level)
        {
            base.Process(configuration, level);

            var openContexts = level.FindOpenContexts();
            foreach (var context in openContexts.Where(this.DiscardOpenContextsRestriction.ShouldDiscardContext))
            {
                context.Source.RemoveContext(context);

                this.LogMessage(string.Format("+ Removed context at {0}.", context));
            }
        }

        #endregion
    }
}