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

using ByChanceFramework.Geom;
using System;

namespace ByChanceFramework.Base3D
{
    /// <summary>
    /// Represents a 3D level of a given width, height and depth that consists of a number of chunks.
    /// </summary>
    sealed public class Level3D : Level<Chunk3D>
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
        /// Gets or sets the depth of this level.
        /// </summary>
        public float Depth { get; private set; }


        /// <summary>
        /// Constructs a new, empty level of the given width, height and depth.
        /// </summary>
        /// <param name="width">The width of the new level.</param>
        /// <param name="height">The height of the new level.</param>
        /// <param name="depth">The depth of the new level.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <c>width</c>, <c>height</c> or <c>depth</c>is less then or equal to zero.</exception>
        public Level3D(float width, float height, float depth)
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

            if (depth <= 0f)
            {
                throw new ArgumentOutOfRangeException("depth", depth, "The depth of the level must be greater than zero.");
            }

            this.Width = width;
            this.Height = height;
            this.Depth = depth;
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
        /// <exception cref="ArgumentException"><c>existingContext</c> or <c>possibleContext</c> is not of the type <c>Context3D</c>.</exception>
        public override bool FitsLevelGeometry(Context existingContext, Context possibleContext)
        {
            if (existingContext == null)
            {
                throw new ArgumentNullException("existingContext");
            }
            else if (!(existingContext is Context3D))
            {
                throw new ArgumentException("Passed existing context isn't of type Context3D and therefore invalid.", "existingContext");
            }

            if (possibleContext == null)
            {
                throw new ArgumentNullException("possibleContext");
            }
            else if (!(possibleContext is Context3D))
            {
                throw new ArgumentException("Passed possible context isn't of type Context3D and therefore invalid.", "possibleContext");
            }

            Box levelBox;
            Box existingChunkBox;
            Box possibleChunkBox;

            levelBox = new Box(0, 0, 0, Width, Height, Depth);
            possibleChunkBox = CalculateBoxByGivenContexts((Context3D)existingContext, (Context3D)possibleContext);

            // check if possible context stays within level boundaries
            if (!levelBox.Contains(possibleChunkBox))
            {
                return false;
            }

            // check if possible context overlaps with existing Chunks
            foreach (Chunk3D chunk in chunks)
            {
                existingChunkBox = new Box(chunk.X, chunk.Y, chunk.Z, chunk.Width, chunk.Height, chunk.Depth);
                if (existingChunkBox.IntersectsWith(possibleChunkBox))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Finds and returns the box describing the position and the
        /// bounds of the chunk <c>newContext</c> belongs to aligned to the
        /// chunk <c>exisitingContext</c> belongs to.
        /// </summary>
        /// <param name="existingContext">The context to align the new chunk to.</param>
        /// <param name="newContext">The context of the new chunk that is to be aligned.</param>
        /// <returns>The position and extent of the new chunk aligned to the existing one.</returns>
        private Box CalculateBoxByGivenContexts(Context3D existingContext, Context3D newContext)
        {
            Chunk3D existingChunk;
            Chunk3D possibleChunk;
            float chunkPositionX;
            float chunkPositionY;
            float chunkPositionZ;

            existingChunk = (Chunk3D)existingContext.Source;
            possibleChunk = (Chunk3D)newContext.Source;

            chunkPositionX = existingChunk.X + existingContext.RelativePosX - newContext.RelativePosX;
            chunkPositionY = existingChunk.Y + existingContext.RelativePosY - newContext.RelativePosY;
            chunkPositionZ = existingChunk.Z + existingContext.RelativePosZ - newContext.RelativePosZ;

            return new Box(chunkPositionX, chunkPositionY, chunkPositionZ, possibleChunk.Width, possibleChunk.Height, possibleChunk.Depth);
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
        /// <exception cref="ArgumentException"><c>freeContext</c> or <c>newContext</c> is not of the type <c>Context3D</c>.</exception>
        public override void AddChunk(Context freeContext, Context newContext)
        {
            if (freeContext == null)
            {
                throw new ArgumentNullException("freeContext");
            }
            else if (!(freeContext is Context3D))
            {
                throw new ArgumentException("Passed free context isn't of type Context3D and therefore invalid.", "freeContext");
            }

            if (newContext == null)
            {
                throw new ArgumentNullException("newContext");
            }
            else if (!(newContext is Context3D))
            {
                throw new ArgumentException("Passed new context isn't of type Context3D and therefore invalid.", "newContext");
            }

            Chunk3D newChunk;
            Box newChunkBox;

            newChunk = (Chunk3D)newContext.Source;

            Context3D freeContext3D = (Context3D)freeContext;
            Context3D newContext3D = (Context3D)newContext;

            // calculate and set position for the new chunk
            newChunkBox = CalculateBoxByGivenContexts(freeContext3D, newContext3D);
            newChunk.SetPosition(newChunkBox.X, newChunkBox.Y, newChunkBox.Z);

            // block the affected contexts
            freeContext3D.Blocked = true;
            newContext3D.Blocked = true;
            freeContext3D.AlignTo(newContext3D);

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
        /// <param name="startZ">The z-coordinate of the new position of the chunk within this level.</param>
        /// <exception cref="ArgumentNullException"><c>chunk</c> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>startX</c>, <c>startY</c> or <c>startZ</c> is less than zero.</exception>
        public void SetStartingChunk(Chunk3D chunk, float startX, float startY, float startZ)
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

            if (startZ < 0f)
            {
                throw new ArgumentOutOfRangeException("startZ", startZ, "Positions must be non-negative.");
            }

            chunk.SetPosition(startX, startY, startZ);

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
        /// <exception cref="ArgumentException"><c>chunk</c> is not of the type <c>Chunk3D</c>.</exception>
        public override void SetRandomStartingChunk(Chunk chunk, RandomNumberGeneratorQ random)
        {
            if (chunk == null)
            {
                throw new ArgumentNullException("chunk");
            }
            else if (!(chunk is Chunk3D))
            {
                throw new ArgumentException("Passed chunk isn't of type Chunk3D and is therefore invalid.", "chunk");
            }

            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            Chunk3D chunk3D = (Chunk3D)chunk;
            float startX = random.RandomFloat() * (this.Width - chunk3D.Width);
            float startY = random.RandomFloat() * (this.Height - chunk3D.Height);
            float startZ = random.RandomFloat() * (this.Depth - chunk3D.Depth);

            SetStartingChunk(chunk3D, startX, startY, startZ);
        }
    }
}
