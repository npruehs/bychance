// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Level.cs" company="Nick Pruehs, Denis Vaz Alves">
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
    using System.Linq;

    using Npruehs.GrabBag.Util;

    /// <summary>
    /// Level that consists of a number of chunks.
    /// <para>
    /// The concrete 2D and 3D implementations of this base class define the dimension of the level.
    /// </para>
    /// </summary>
    /// <typeparam name="T">Type of the chunks this level consists of.</typeparam>
    public abstract class Level<T>
        where T : Chunk
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new, empty level.
        /// </summary>
        protected Level()
        {
            this.Chunks = new List<T>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Total number of chunks that make up this level.
        /// </summary>
        public int LevelChunkCount
        {
            get
            {
                return this.Chunks.Count;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Chunks that make up this level. 
        /// </summary>
        protected List<T> Chunks { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a chunk to this level.
        /// <para>
        /// This is done by aligning the given open context of an existing level chunk with the open context
        /// of the new chunk and then adding the chunk to the chunk list.
        /// </para>
        /// <para>
        /// <i>Note that the new chunk is assumed to fit the level geometry. See 
        /// <see cref="FitsLevelGeometry(Context, Context)"/> for further information on how to check this first.</i>
        /// </para>
        /// </summary>
        /// <param name="freeContext">Context to add the new chunk at.</param>
        /// <param name="newContext">Context of the new chunk to be aligned to the existing level.</param>
        public abstract void AddChunk(Context freeContext, Context newContext);

        /// <summary>
        /// Returns a list of all chunks with open contexts within this level.
        /// </summary>
        /// <returns>List of chunks with open contexts within this level.</returns>
        public List<T> FindOpenChunks()
        {
            return this.Chunks.Where(chunk => chunk.GetAlignedContextCount() < chunk.ContextCount).ToList();
        }

        /// <summary>
        /// Returns a list of all unaligned contexts within this level.
        /// </summary>
        /// <returns>List of unaligned contexts within this level.</returns>
        public List<Context> FindOpenContexts()
        {
            List<Context> openContexts = new List<Context>();

            foreach (var chunk in this.Chunks)
            {
                for (var i = 0; i < chunk.ContextCount; i++)
                {
                    var context = chunk.GetContext(i);

                    if (context.Target == null)
                    {
                        openContexts.Add(context);
                    }
                }
            }

            return openContexts;
        }

        /// <summary>
        /// Returns the first context that is not blocked.
        /// </summary>
        /// <returns>Non-blocked context of one of the chunks within this level, if there is one, and <c>null</c> otherwise.</returns>
        public Context FindProcessibleContext()
        {
            foreach (var chunk in this.Chunks)
            {
                for (var i = 0; i < chunk.ContextCount; i++)
                {
                    var context = chunk.GetContext(i);

                    if (!context.Blocked)
                    {
                        return context;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if a given chunk candidate (passed through its open context) fits within the level geometry, i.e.
        /// if the chunk candidate fits within the level bounds and doesn't overlap with existing level chunks.
        /// </summary>
        /// <param name="freeContext">Open context of an existing level chunk.</param>
        /// <param name="possibleContext">Open context of a chunk candidate.</param>
        /// <returns><c>true</c>, if the passed chunk candidate fits within the level bounds 
        /// and doesn't overlap with existing chunks, and <c>false</c> otherwise.</returns>
        public abstract bool FitsLevelGeometry(Context freeContext, Context possibleContext);

        /// <summary>
        /// Gets the chunk with the specified level-wide unique index.
        /// </summary>
        /// <param name="index">Index of the chunk to get.</param>
        /// <returns>Chunk with the specified index in the chunk list.</returns>
        public T GetLevelChunk(int index)
        {
            return this.Chunks[index];
        }

        /// <summary>
        /// Removes passed chunk from this level.
        /// </summary>
        /// <param name="chunk">Chunk to be removed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="chunk"/> is null.</exception>
        /// <returns><c>true</c>, if the chunk has been removed, and <c>false</c> otherwise.</returns>
        public bool RemoveChunk(Chunk chunk)
        {
            if (chunk == null)
            {
                throw new ArgumentNullException("chunk");
            }

            if (this.Chunks.Remove((T)chunk))
            {
                // Clean up newly freed contexts.
                for (var i = 0; i < chunk.ContextCount; i++)
                {
                    var context = chunk.GetContext(i);
                    context.ClearTarget();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds the passed chunk to this level at a random position,
        /// without checking for any intersections with the level bounds or
        /// other chunks and without aligning it to any other chunk.
        /// </summary>
        /// <param name="chunk">Chunk that will be the starting point of the level generation process.</param>
        /// <param name="random">Instance of the random number generator to use for determining the random position
        /// of the starting chunk.</param>
        public abstract void SetRandomStartingChunk(Chunk chunk, Random2 random);

        #endregion
    }
}