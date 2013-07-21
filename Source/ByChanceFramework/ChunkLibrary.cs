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
using System;

namespace ByChanceFramework
{
    /// <summary>
    /// Holds references to all user-defined chunk templates.
    /// </summary>
    /// <typeparam name="T">Type of the chunk templates this chunk library will hold.</typeparam>
    public class ChunkLibrary<T> where T : ChunkTemplate
    {
        /// <summary>
        /// The list of available chunk templates.
        /// </summary>
        private List<T> chunkTemplates;

        /// <summary>
        /// Constructs a new, empty chunk library.
        /// </summary>
        public ChunkLibrary()
        {
            chunkTemplates = new List<T>();
        }

        /// <summary>
        /// Adds a given chunk template to the existing list of chunk templates.
        /// </summary>
        /// <param name="chunkTemplate">The new chunk template that should be added.</param>
        /// <seealso cref="chunkTemplates"/>
        /// <exception cref="ArgumentNullException">The passed <c>chunkTemplate</c> is null.</exception>
        public void AddChunkTemplate(T chunkTemplate)
        {
            if (chunkTemplate == null)
            {
                throw new ArgumentNullException("chunkTemplate");
            }

            chunkTemplate.Index = chunkTemplates.Count;
            chunkTemplates.Add(chunkTemplate);
        }

        /// <summary>
        /// Gets the total number of available chunk templates.
        /// </summary>
        /// <returns>Non-negative integer that represents the total number of available chunk templates.</returns>
        public int GetChunkTemplateCount()
        {
            return chunkTemplates.Count;
        }

        /// <summary>
        /// Gets the chunk template with the specified unique index.
        /// </summary>
        /// <param name="index">The index of the chunk template to get.</param>
        /// <returns>Chunk template at the specified index in the list.</returns>
        public T GetChunkTemplateByIndex(int index)
        {
            return chunkTemplates[index];
        }
    }
}
