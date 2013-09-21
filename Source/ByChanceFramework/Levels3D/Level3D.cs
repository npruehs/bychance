// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Level3D.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ByChance.Levels3D
{
    using System;

    using ByChance.Core;

    /// <summary>
    /// 3D level of a given width, height and depth that consists of a number of chunks.
    /// </summary>
    public sealed class Level3D : Level<Chunk3D>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new, empty level of the given width, height and depth.
        /// </summary>
        /// <param name="extents">Width, height and depth of the new level.</param>
        /// <exception cref="ArgumentOutOfRangeException">Width, height or depth is less then or equal to zero.</exception>
        public Level3D(Vector3F extents)
        {
            if (extents.X <= 0f)
            {
                throw new ArgumentOutOfRangeException("extents", "Width of the level must be greater than zero.");
            }

            if (extents.Y <= 0f)
            {
                throw new ArgumentOutOfRangeException("extents", "Height of the level must be greater than zero.");
            }

            if (extents.Z <= 0f)
            {
                throw new ArgumentOutOfRangeException("extents", "Depth of the level must be greater than zero.");
            }

            this.Extents = extents;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Width, height and depth of this level.
        /// </summary>
        public Vector3F Extents { get; private set; }

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
        /// <exception cref="ArgumentException"><paramref name="freeContext"/> or <paramref name="newContext"/> is not of the type <see cref="Context3D"/>.</exception>
        public override void AddChunk(Context freeContext, Context newContext)
        {
            if (freeContext == null)
            {
                throw new ArgumentNullException("freeContext");
            }

            if (!(freeContext is Context3D))
            {
                throw new ArgumentException("Free context isn't of type Context3D.", "freeContext");
            }

            if (newContext == null)
            {
                throw new ArgumentNullException("newContext");
            }

            if (!(newContext is Context3D))
            {
                throw new ArgumentException("New context isn't of type Context3D.", "newContext");
            }

            var newChunk = (Chunk3D)newContext.Source;

            var freeContext3D = (Context3D)freeContext;
            var newContext3D = (Context3D)newContext;

            // Calculate and set position for the new chunk.
            var newChunkBox = CalculateBoxByGivenContexts(freeContext3D, newContext3D);
            newChunk.SetPosition(new Vector3F(newChunkBox.X, newChunkBox.Y, newChunkBox.Z));

            // Block the affected contexts.
            freeContext3D.Blocked = true;
            newContext3D.Blocked = true;
            freeContext3D.AlignTo(newContext3D);

            // Add new chunk to the level chunks.
            this.Chunks.Add(newChunk);
        }

        /// <summary>
        /// Checks if a given chunk candidate (passed through its open context) fits within the level geometry, i.e.
        /// if the chunk candidate fits within the level bounds and doesn't overlap with existing level chunks.
        /// </summary>
        /// <param name="existingContext">Open context of an existing level chunk.</param>
        /// <param name="possibleContext">Open context of a chunk candidate.</param>
        /// <returns><c>true</c>, if the passed chunk candidate fits within the level bounds 
        /// and doesn't overlap with existing chunks, and <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="existingContext"/> or <paramref name="possibleContext"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="existingContext"/> or <paramref name="possibleContext"/> is not of the type <see cref="Context3D"/>.</exception>
        public override bool FitsLevelGeometry(Context existingContext, Context possibleContext)
        {
            if (existingContext == null)
            {
                throw new ArgumentNullException("existingContext");
            }

            if (!(existingContext is Context3D))
            {
                throw new ArgumentException("Existing context isn't of type Context3D.", "existingContext");
            }

            if (possibleContext == null)
            {
                throw new ArgumentNullException("possibleContext");
            }

            if (!(possibleContext is Context3D))
            {
                throw new ArgumentException("Possible context isn't of type Context3D.", "possibleContext");
            }

            var levelBox = new BoxF(0, 0, 0, this.Extents);
            var possibleChunkBox = CalculateBoxByGivenContexts((Context3D)existingContext, (Context3D)possibleContext);

            // Check if possible context stays within level boundaries.
            if (!levelBox.Contains(possibleChunkBox))
            {
                return false;
            }

            // Check if possible context overlaps with existing Chunks.
            foreach (var chunk in this.Chunks)
            {
                var existingChunkBox = new BoxF(chunk.Position, chunk.Extents);
                if (existingChunkBox.Intersects(possibleChunkBox))
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
        /// <exception cref="ArgumentException"><paramref name="chunk"/> is not of the type <seealso cref="Chunk3D"/>.</exception>
        public override void SetRandomStartingChunk(Chunk chunk, Random2 random)
        {
            if (chunk == null)
            {
                throw new ArgumentNullException("chunk");
            }

            Chunk3D chunk3D = chunk as Chunk3D;

            if (chunk3D == null)
            {
                throw new ArgumentException("Passed chunk isn't of type Chunk3D.", "chunk");
            }

            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            var startX = random.NextFloat() * (this.Extents.X - chunk3D.Extents.X);
            var startY = random.NextFloat() * (this.Extents.Y - chunk3D.Extents.Y);
            var startZ = random.NextFloat() * (this.Extents.Z - chunk3D.Extents.Z);

            this.SetStartingChunk(chunk3D, new Vector3F(startX, startY, startZ));
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
        public void SetStartingChunk(Chunk3D chunk, Vector3F startPosition)
        {
            if (chunk == null)
            {
                throw new ArgumentNullException("chunk");
            }

            if (startPosition.X < 0f || startPosition.Y < 0f || startPosition.Z < 0f)
            {
                throw new ArgumentOutOfRangeException("startPosition", "Positions must be non-negative.");
            }

            chunk.SetPosition(startPosition);

            this.Chunks.Clear();
            this.Chunks.Add(chunk);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds and returns the box describing the position and the
        /// bounds of the chunk <paramref name="newContext"/> belongs to aligned to the
        /// chunk <paramref name="existingContext"/> belongs to.
        /// </summary>
        /// <param name="existingContext">Context to align the new chunk to.</param>
        /// <param name="newContext">Context of the new chunk that is to be aligned.</param>
        /// <returns>Position and extent of the new chunk aligned to the existing one.</returns>
        private static BoxF CalculateBoxByGivenContexts(Context3D existingContext, Context3D newContext)
        {
            Chunk3D existingChunk = (Chunk3D)existingContext.Source;
            Chunk3D possibleChunk = (Chunk3D)newContext.Source;

            var chunkPosition = existingChunk.Position + existingContext.RelativePosition - newContext.RelativePosition;

            return new BoxF(chunkPosition, possibleChunk.Extents);
        }

        #endregion
    }
}