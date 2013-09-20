// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Anchor.cs" company="Nick Pruehs, Denis Vaz Alves">
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
namespace ByChance.Core
{
    using System;

    /// <summary>
    /// Placeholder for game elements that can be filled after
    /// the level generation process.
    /// </summary>
    public abstract class Anchor
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new anchor of the specified category.
        /// </summary>
        /// <param name="tag">Category of the new anchor.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tag"/> is null.</exception>
        protected Anchor(string tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            this.Tag = tag;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Chunk-wide unique index of this anchor.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Chunk this anchor belongs to.
        /// </summary>
        public Chunk Source { get; protected set; }

        /// <summary>
        /// Category of this anchor.
        /// Use this category for determining how to fill this anchor after
        /// the level generation process is complete.
        /// </summary>
        public string Tag { get; private set; }

        #endregion
    }
}