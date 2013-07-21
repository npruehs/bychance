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
using ByChanceFramework.Geom;

namespace ByChanceFramework.Base2D
{
    /// <summary>
    /// Represents a 2D level of a given width and height that consists of a number of chunks.
    /// </summary>
    sealed public class Level2D : Level<Chunk2D>
    {
        /// <summary>
        /// Gets or sets the width of this level.
        /// </summary>
        public float Width { get; private set; }

        /// <summary>
        /// Gets or sets the height of this level.
        /// </summary>
        public float Height { get; private set; }


        /// <summary>
        /// Constructs a new, empty level of the given width and height.
        /// </summary>
        /// <param name="width">The width of the new level.</param>
        /// <param name="height">The height of the new level.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <c>width</c> or <c>height</c> is less then or equal to zero.</exception>
        public Level2D(float width, float height)
            : base()
        {
            if (width <= 0f)
            {
                throw new ArgumentOutOfRangeException("width", width, "The width of the level must be greater than zero.");
            }

            if (height <= 0f)
            {
                throw new ArgumentOutOfRangeException("height", height, "The height of the level must be greater than zero.");
            }

            this.Width = width;
            this.Height = height;
        }


        /// <summary>
        /// Checks if a given chunk candidate (passed through its open context) fits within the level geometry i.e.
        /// if the chunk candidate fits within the level bounds and doesn't overlap with existing level chunks.
        /// </summary>
        /// <param name="existingContext">Open context of an existing level chunk.</param>
        /// <param name="possibleContext">Open context of a chunk candidate.</param>
        /// <returns><c>true</c>, if the passed chunk candidate fits within the level bounds 
        /// and doesn't overlap with existing chunks, and <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><c>existingContext</c> or <c>possibleContext</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><c>existingContext</c> or <c>possibleContext</c> is not of the type <c>Context2D</c>.</exception>
        public override bool FitsLevelGeometry(Context existingContext, Context possibleContext)
        {
            if (existingContext == null)
            {
                throw new ArgumentNullException("existingContext");
            }
            else if (!(existingContext is Context2D))
            {
                throw new ArgumentException("Passed existing context isn't of type Context2D and therefore invalid.", "existingContext");
            }

            if (possibleContext == null)
            {
                throw new ArgumentNullException("possibleContext");
            }
            else if (!(possibleContext is Context2D))
            {
                throw new ArgumentException("Passed possible context isn't of type Context2D and therefore invalid.", "possibleContext");
            }

            Rectangle levelRectangle;
            Rectangle chunkRectangle;
            Rectangle possibleChunkRectangle;

            levelRectangle = new Rectangle(0, 0, Width, Height);
            possibleChunkRectangle = CalculateRectangleByGivenContexts((Context2D)existingContext, (Context2D)possibleContext);

            // check if possible context stays within level boundaries
            if (!levelRectangle.Contains(possibleChunkRectangle))
            {
                return false;
            }

            // check if possible context overlaps with existing chunks
            foreach (Chunk2D chunk in chunks)
            {
                chunkRectangle = new Rectangle((int)chunk.X, (int)chunk.Y, (int)chunk.Width, (int)chunk.Height);
                if (chunkRectangle.IntersectsWith(possibleChunkRectangle))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Finds and returns the rectangle describing the position and the
        /// bounds of the chunk <c>newContext</c> belongs to aligned to the
        /// chunk <c>exisitingContext</c> belongs to.
        /// </summary>
        /// <param name="existingContext">The context to align the new chunk to.</param>
        /// <param name="newContext">The context of the new chunk that is to be aligned.</param>
        /// <returns>The position and extent of the new chunk aligned to the existing one.</returns>
        private Rectangle CalculateRectangleByGivenContexts(Context2D existingContext, Context2D newContext)
        {
            Chunk2D existingChunk;
            Chunk2D possibleChunk;
            float chunkPositionX;
            float chunkPositionY;

            existingChunk = (Chunk2D)existingContext.Source;
            possibleChunk = (Chunk2D)newContext.Source;

            chunkPositionX = existingChunk.X + existingContext.RelativePosX - newContext.RelativePosX;
            chunkPositionY = existingChunk.Y + existingContext.RelativePosY - newContext.RelativePosY;

            return new Rectangle(chunkPositionX, chunkPositionY, possibleChunk.Width, possibleChunk.Height);
        }

        /// <summary>
        /// Adds a chunk to the list of existing chunks in the level.
        /// <para>
        /// This is done by firstly aligning the given open context of an existing level chunk with the open context
        /// of the new chunk and then adding the chunk to list.
        /// </para>
        /// <para>
        /// <i>Note that the new chunk is assumed to fit the level geometry.
        /// <see cref="FitsLevelGeometry(Context, Context)"/> for further information on how to check this first.</i>
        /// </para>
        /// </summary>
        /// <param name="freeContext">The context to add the new chunk at.</param>
        /// <param name="newContext">The context of the new chunk to be aligned to the existing level.</param>
        /// <exception cref="ArgumentNullException"><c>freeContext</c> or <c>newContext</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><c>freeContext</c> or <c>newContext</c> is not of the type <c>Context2D</c>.</exception>
        public override void AddChunk(Context freeContext, Context newContext)
        {
            if (freeContext == null)
            {
                throw new ArgumentNullException("freeContext");
            }
            else if (!(freeContext is Context2D))
            {
                throw new ArgumentException("Passed free context isn't of type Context2D and therefore invalid.", "freeContext");
            }

            if (newContext == null)
            {
                throw new ArgumentNullException("newContext");
            }
            else if (!(newContext is Context2D))
            {
                throw new ArgumentException("Passed new context isn't of type Context2D and therefore invalid.", "newContext");
            }

            Chunk2D newChunk;
            Rectangle newChunkRectangle;

            newChunk = (Chunk2D)newContext.Source;

            Context2D freeContext2D = (Context2D)freeContext;
            Context2D newContext2D = (Context2D)newContext;

            // calculate and set position for the new chunk
            newChunkRectangle = CalculateRectangleByGivenContexts(freeContext2D, newContext2D);
            newChunk.SetPosition(newChunkRectangle.X, newChunkRectangle.Y);

            // block the affected contexts
            freeContext2D.Blocked = true;
            newContext2D.Blocked = true;
            freeContext2D.AlignTo(newContext2D);

            // add the new chunk to the level chunks
            chunks.Add(newChunk);
        }

        /// <summary>
        /// Adds the passed chunk to this level at the specified position,
        /// without checking for any intersections with the level bounds or
        /// other chunks and without aligning it to any other chunk.
        /// </summary>
        /// <param name="chunk">The chunk to add to this level.</param>
        /// <param name="startX">The x-coordinate of the new position of the chunk within this level.</param>
        /// <param name="startY">The y-coordinate of the new position of the chunk within this level.</param>
        /// <exception cref="ArgumentNullException"><c>chunk</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>startX</c> or <c>startY</c> is less than zero.</exception>
        public void SetStartingChunk(Chunk2D chunk, float startX, float startY)
        {
            if (chunk == null)
            {
                throw new ArgumentNullException("chunk");
            }

            if (startX < 0f)
            {
                throw new ArgumentOutOfRangeException("startX", startX, "Positions must be non-negative.");
            }

            if (startY < 0f)
            {
                throw new ArgumentOutOfRangeException("startY", startY, "Positions must be non-negative.");
            }

            chunk.SetPosition(startX, startY);

            chunks.Add(chunk);
        }

        /// <summary>
        /// Adds the passed chunk to this level at a random position,
        /// without checking for any intersections with the level bounds or
        /// other chunks and without aligning it to any other chunk.
        /// </summary>
        /// <param name="chunk">The <c>chunk</c> that will be the starting point of the level generation process.</param>
        /// <param name="random">
        /// The instance of the random number generator to use for determining the random position of the starting chunk.
        /// </param>
        /// <exception cref="ArgumentNullException"><c>chunk</c> or <c>random</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><c>chunk</c> is not of the type <c>Chunk2D</c>.</exception>
        public override void SetRandomStartingChunk(Chunk chunk, RandomNumberGeneratorQ random)
        {
            if (chunk == null)
            {
                throw new ArgumentNullException("chunk");
            }
            else if (!(chunk is Chunk2D))
            {
                throw new ArgumentException("Passed chunk isn't of type Chunk2D and is therefore invalid.", "chunk");
            }

            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            Chunk2D chunk2D = (Chunk2D)chunk;
            float startX = random.RandomFloat() * (this.Width - chunk2D.Width);
            float startY = random.RandomFloat() * (this.Height - chunk2D.Height);

            SetStartingChunk(chunk2D, startX, startY);
        }
    }
}
