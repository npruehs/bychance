// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Level2D.cs" company="Nick Pruehs, Denis Vaz Alves">
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
namespace ByChance.Levels2D
{
    using System;

    using ByChance.Core;

    using Npruehs.GrabBag.Math.Geometry;
    using Npruehs.GrabBag.Math.Vectors;
    using Npruehs.GrabBag.Util;

    /// <summary>
    /// 2D level of a given width and height that consists of a number of chunks.
    /// </summary>
    public sealed class Level2D : Level<Chunk2D>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new, empty level of the given width and height.
        /// </summary>
        /// <param name="extents">Width and height of the new level.</param>
        /// <exception cref="ArgumentOutOfRangeException">Width or height is less than or equal to zero.</exception>
        public Level2D(Vector2F extents)
        {
            if (extents.X <= 0f)
            {
                throw new ArgumentOutOfRangeException("extents", "Width of the level must be greater than zero.");
            }

            if (extents.Y <= 0f)
            {
                throw new ArgumentOutOfRangeException("extents", "Height of the level must be greater than zero.");
            }

            this.Extents = extents;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Width and height of this level.
        /// </summary>
        public Vector2F Extents { get; private set; }

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
        /// <exception cref="ArgumentNullException"><paramref name="freeContext"/> or <paramref name="newContext"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="freeContext"/> or <paramref name="newContext"/> is not of the type <seealso cref="Context2D"/>.</exception>
        public override void AddChunk(Context freeContext, Context newContext)
        {
            if (freeContext == null)
            {
                throw new ArgumentNullException("freeContext");
            }

            if (newContext == null)
            {
                throw new ArgumentNullException("newContext");
            }

            Context2D freeContext2D = freeContext as Context2D;
            Context2D newContext2D = newContext as Context2D;

            if (freeContext2D == null)
            {
                throw new ArgumentException("Free context isn't of type Context2D.", "freeContext");
            }

            if (newContext2D == null)
            {
                throw new ArgumentException("New context isn't of type Context2D.", "newContext");
            }

            Chunk2D newChunk = (Chunk2D)newContext.Source;

            // Calculate and set position for the new chunk.
            var newChunkRectangle = CalculateRectangleByGivenContexts(freeContext2D, newContext2D);
            newChunk.SetPosition(newChunkRectangle.Position);

            // Block the affected contexts.
            freeContext2D.Blocked = true;
            newContext2D.Blocked = true;
            freeContext2D.AlignTo(newContext2D);

            // Add the new chunk to the level chunks.
            this.Chunks.Add(newChunk);
        }

        /// <summary>
        /// Checks if a given chunk candidate (passed through its open context) fits within the level geometry i.e.
        /// if the chunk candidate fits within the level bounds and doesn't overlap with existing level chunks.
        /// </summary>
        /// <param name="existingContext">Open context of an existing level chunk.</param>
        /// <param name="possibleContext">Open context of a chunk candidate.</param>
        /// <returns><c>true</c>, if the passed chunk candidate fits within the level bounds 
        /// and doesn't overlap with existing chunks, and <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="existingContext"/> or <paramref name="possibleContext"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="existingContext"/> or <paramref name="possibleContext"/> is not of the type <see cref="Context2D"/>.</exception>
        public override bool FitsLevelGeometry(Context existingContext, Context possibleContext)
        {
            if (existingContext == null)
            {
                throw new ArgumentNullException("existingContext");
            }

            if (possibleContext == null)
            {
                throw new ArgumentNullException("possibleContext");
            }

            var existingContext2D = existingContext as Context2D;
            var possibleContext2D = possibleContext as Context2D;

            if (existingContext2D == null)
            {
                throw new ArgumentException("Existing context isn't of type Context2D.", "existingContext");
            }

            if (possibleContext2D == null)
            {
                throw new ArgumentException("Possible context isn't of type Context2D.", "possibleContext");
            }

            RectangleF levelRectangle = new RectangleF(Vector2F.Zero, this.Extents);
            var possibleChunkRectangle = CalculateRectangleByGivenContexts(existingContext2D, possibleContext2D);

            // Check if possible context stays within level boundaries.
            if (!levelRectangle.Contains(possibleChunkRectangle))
            {
                return false;
            }

            // Check if possible context overlaps with existing chunks.
            foreach (var chunk in this.Chunks)
            {
                RectangleF chunkRectangle = new RectangleF(chunk.Position, chunk.Extents);
                if (chunkRectangle.Intersects(possibleChunkRectangle))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds the passed chunk to this level at a random position,
        /// without checking for any intersections with the level bounds or
        /// other chunks and without aligning it to any other chunk.
        /// </summary>
        /// <param name="chunk">Chunk that will be the starting point of the level generation process.</param>
        /// <param name="random">
        /// Instance of the random number generator to use for determining the random position of the starting chunk.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="chunk"/> or <paramref name="random"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="chunk"/> is not of the type <seealso cref="Chunk2D"/>.</exception>
        public override void SetRandomStartingChunk(Chunk chunk, Random2 random)
        {
            if (chunk == null)
            {
                throw new ArgumentNullException("chunk");
            }

            Chunk2D chunk2D = chunk as Chunk2D;

            if (chunk2D == null)
            {
                throw new ArgumentException("Passed chunk isn't of type Chunk2D.", "chunk");
            }

            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            var startX = random.NextFloat() * (this.Extents.X - chunk2D.Extents.X);
            var startY = random.NextFloat() * (this.Extents.Y - chunk2D.Extents.Y);

            this.SetStartingChunk(chunk2D, new Vector2F(startX, startY));
        }

        /// <summary>
        /// Adds the passed chunk to this level at the specified position,
        /// without checking for any intersections with the level bounds or
        /// other chunks and without aligning it to any other chunk.
        /// </summary>
        /// <param name="chunk">Chunk to add to this level.</param>
        /// <param name="startPosition">New position of the chunk within this level.</param>
        /// <exception cref="ArgumentNullException"><paramref name="chunk"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Start position is negative.</exception>
        public void SetStartingChunk(Chunk2D chunk, Vector2F startPosition)
        {
            if (chunk == null)
            {
                throw new ArgumentNullException("chunk");
            }

            if (startPosition.X < 0f || startPosition.Y < 0f)
            {
                throw new ArgumentOutOfRangeException("startPosition", "Positions must be non-negative.");
            }

            chunk.SetPosition(startPosition);

            this.Chunks.Add(chunk);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds and returns the rectangle describing the position and the
        /// bounds of the chunk <paramref name="newContext"/> belongs to aligned to the
        /// chunk <paramref name="existingContext"/> belongs to.
        /// </summary>
        /// <param name="existingContext">Context to align the new chunk to.</param>
        /// <param name="newContext">Context of the new chunk that is to be aligned.</param>
        /// <returns>Position and extent of the new chunk aligned to the existing one.</returns>
        private static RectangleF CalculateRectangleByGivenContexts(Context2D existingContext, Context2D newContext)
        {
            Chunk2D existingChunk = (Chunk2D)existingContext.Source;
            Chunk2D possibleChunk = (Chunk2D)newContext.Source;

            var chunkPosition = existingChunk.Position + existingContext.RelativePosition - newContext.RelativePosition;

            return new RectangleF(chunkPosition, possibleChunk.Extents);
        }

        #endregion
    }
}