// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChunkLibrary.cs" company="Nick Pruehs, Denis Vaz Alves">
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
    using System.Collections.Generic;

    /// <summary>
    /// Collection of user-defined chunk templates.
    /// </summary>
    /// <typeparam name="T">Type of the chunk templates of this chunk library.</typeparam>
    public abstract class ChunkLibrary<T>
        where T : ChunkTemplate
    {
        #region Fields

        /// <summary>
        /// List of available chunk templates.
        /// </summary>
        private readonly List<T> chunkTemplates = new List<T>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Total number of available chunk templates.
        /// </summary>
        public int Count
        {
            get
            {
                return this.chunkTemplates.Count;
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        /// Gets the chunk template with the specified unique index.
        /// </summary>
        /// <param name="index">Index of the chunk template to get.</param>
        /// <returns>Chunk template at the specified index.</returns>
        public T this[int index]
        {
            get
            {
                return this.chunkTemplates[index];
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds the passed chunk template to this chunk library.
        /// </summary>
        /// <param name="chunkTemplate">Chunk template to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="chunkTemplate"/> is <c>null</c>.</exception>
        public void AddChunkTemplate(T chunkTemplate)
        {
            if (chunkTemplate == null)
            {
                throw new ArgumentNullException("chunkTemplate");
            }

            chunkTemplate.Index = this.chunkTemplates.Count;
            this.chunkTemplates.Add(chunkTemplate);
        }

        #endregion
    }
}