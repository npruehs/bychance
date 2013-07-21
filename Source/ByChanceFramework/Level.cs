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

using System;
using System.Collections.Generic;

namespace ByChanceFramework
{
    /// <summary>
    /// Represents a level that consists of a number of chunks.
    /// <para>
    /// The concrete 2D and 3D implementations of this base class define the dimension of the level.
    /// </para>
    /// </summary>
    /// <typeparam name="T">Type of the chunks this level will consist of.</typeparam>
    public abstract class Level<T> where T : Chunk
    {
        /// <summary>
        /// The list of all chunks that make up the level. 
        /// </summary>
        protected List<T> chunks;

        /// <summary>
        /// Constructs a new, empty level.
        /// </summary>
        protected Level()
        {
            chunks = new List<T>();
        }


        /// <summary>
        /// Cycles through the existing chunks and returns the first context that is unused.
        /// </summary>
        /// <returns>An unused context of one of the chunks within the level.</returns>
        public Context FindProcessibleContext()
        {
            Context context;

            foreach (T chunk in chunks)
            {
                for (int i = 0; i < chunk.GetContextCount(); i++)
                {
                    context = chunk.GetContextByIndex(i);
                    if (!context.Blocked)
                    {
                        return context;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Cycles through the existing chunks and returns a list of all unaligned contexts within the level.
        /// </summary>
        /// <returns>A list of unaligned contexts within the level.</returns>
        public List<Context> FindOpenContexts()
        {
            Context context;
            List<Context> openContexts = new List<Context>();

            foreach (T chunk in chunks)
            {
                for (int i = 0; i < chunk.GetContextCount(); i++)
                {
                    context = chunk.GetContextByIndex(i);
                    if (context.Target == null)
                    {
                        openContexts.Add(context);
                    }
                }
            }

            return openContexts;
        }

        /// <summary>
        /// Cycles through the existing chunks and returns a list of all chunks with open contexts within the level.
        /// </summary>
        /// <returns>A list of chunks with open contexts within the level.</returns>
        public List<Chunk> FindOpenChunks()
        {
            List<Chunk> openChunks = new List<Chunk>();

            foreach (T chunk in chunks)
            {
                if (chunk.GetAlignedContextCount() < chunk.GetContextCount())
                {
                    openChunks.Add(chunk);
                }
            }

            return openChunks;
        }

        #region Virtual Methods
        /// <summary>
        /// Checks if a given chunk candidate (passed through its open context) fits within the level geometry i.e.
        /// if the chunk candidate fits within the level bounds and doesn't overlap with existing level chunks.
        /// </summary>
        /// <param name="freeContext">Open context of an existing level chunk.</param>
        /// <param name="possibleContext">Open context of a chunk candidate.</param>
        /// <returns><c>true</c>, if the passed chunk candidate fits within the level bounds 
        /// and doesn't overlap with existing chunks, and <c>false</c> otherwise.</returns>
        public virtual bool FitsLevelGeometry(Context freeContext, Context possibleContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the passed chunk to this level at a random position,
        /// without checking for any intersections with the level bounds or
        /// other chunks and without aligning it to any other chunk.
        /// </summary>
        /// <param name="chunk">The <c>chunk</c> that will be the starting point of the level generation process.</param>
        /// <param name="random">The instance of the random number generator to use for determining the random position
        /// of the starting chunk.</param>
        public virtual void SetRandomStartingChunk(Chunk chunk, RandomNumberGeneratorQ random)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a chunk to the existing list of chunks in the level.
        /// <para>
        /// This is done by firstly aligning the given open context of an existing level chunk with the open context
        /// of the new chunk and then adding the chunk to list.
        /// </para>
        /// </summary>
        /// <param name="freeContext">Open context of an existing level chunk.</param>
        /// <param name="compatibleContext">Open context of the new chunk to be added.</param>
        public virtual void AddChunk(Context freeContext, Context compatibleContext)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Removes passed chunk of the list of chunks of this level.
        /// </summary>
        /// <param name="chunk">The <c>chunk</c> to be removed.</param>
        /// <exception cref="ArgumentNullException">The passed <c>chunk</c> is null.</exception>
        /// <exception cref="ArgumentException">The passed <c>chunk</c> is not part of this level.</exception>
        public void RemoveChunk(Chunk chunk)
        {
            Context context;

            if (chunk == null)
            {
                throw new ArgumentNullException("chunk");
            }

            if (chunks.Remove((T)chunk))
            {
                // clean up newly freed contexts
                for (int i = 0; i < chunk.GetContextCount(); i++)
                {
                    context = chunk.GetContextByIndex(i);
                    context.ClearTarget();
                }
            }
            else
            {
                throw new ArgumentException("The passed chunk is not part of this level.", "chunk");
            }
        }

        /// <summary>
        /// Gets the chunk with the specified level-wide unique index.
        /// </summary>
        /// <param name="index">The <c>index</c> of the chunk to get.</param>
        /// <returns>The chunk with the specified <c>index</c> in the chunk list.</returns>
        public T GetLevelChunkByIndex(int index)
        {
            return chunks[index];
        }

        /// <summary>
        /// Gets the total number of chunks that make up this level.
        /// </summary>
        /// <returns>Non-negative integer that represents the total number of level chunks.</returns>
        public int GetLevelChunkCount()
        {
            return chunks.Count;
        }
    }
}
