// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelGenerator2D.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2015 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Levels2D
{
    using System;

    using ByChance.Core;

    /// <summary>
    /// Generates a 3D level based on a given chunk library.
    /// </summary>
    public sealed class LevelGenerator2D : LevelGenerator
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Expands the passed level at the specified free context.
        /// </summary>
        /// <param name="chunkLibrary">
        /// Chunk library that holds all chunk templates to use for the level generation.
        /// </param>
        /// <param name="level">Level to fill during the level generation process.</param>
        /// <param name="random">Random number generator to use for the level generation.</param>
        /// <param name="freeContext">Context to expland the level at.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="chunkLibrary"/>, <paramref name="level"/> or <paramref name="random"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="chunkLibrary"/> is empty, or the types of <paramref name="chunkLibrary"/> and <paramref name="level"/> don't match.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="freeContext"/> is already blocked or aligned.
        /// </exception>
        /// <returns>
        ///   <c>true</c>, if another chunk can be added to the level, and
        ///   <c>false</c>, otherwise.
        /// </returns>
        public bool AddChunk(ChunkLibrary2D chunkLibrary, Level2D level, Random2 random, Context2D freeContext)
        {
            return this.AddChunk<ChunkTemplate2D, Chunk2D>(chunkLibrary, level, random, freeContext);
        }

        /// <summary>
        ///   Expands the passed level at any free context.
        /// </summary>
        /// <param name="chunkLibrary">
        /// Chunk library that holds all chunk templates to use for the level generation.
        /// </param>
        /// <param name="level">Level to fill during the level generation process.</param>
        /// <param name="random">Random number generator to use for the level generation.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="chunkLibrary"/>, <paramref name="level"/> or <paramref name="random"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="chunkLibrary"/> is empty, or the types of <paramref name="chunkLibrary"/> and <paramref name="level"/> don't match.
        /// </exception>
        /// <returns>
        ///   <c>true</c>, if another chunk can be added to the level, and
        ///   <c>false</c>, otherwise.
        /// </returns>
        public bool AddChunk(ChunkLibrary2D chunkLibrary, Level2D level, Random2 random)
        {
            return this.AddChunk<ChunkTemplate2D, Chunk2D>(chunkLibrary, level, random);
        }

        /// <summary>
        /// Adds a random chunk to the passed level at a random position,
        /// without checking for any intersections with the level bounds or
        /// other chunks and without aligning it to any other chunk.
        /// </summary>
        /// <param name="chunkLibrary">
        /// Chunk library that holds all chunk templates to use for the level generation.
        /// </param>
        /// <param name="level">Level to fill during the level generation process.</param>
        /// <param name="random">Random number generator to use for the level generation.</param>
        public void AddRandomChunk(ChunkLibrary2D chunkLibrary, Level2D level, Random2 random)
        {
            this.AddRandomChunk<ChunkTemplate2D, Chunk2D>(chunkLibrary, level, random);
        }

        /// <summary>
        /// Generates a 2D level using the given chunk library and level.
        /// </summary>
        /// <param name="chunkLibrary">
        /// Chunk library that holds all chunk templates to use for the level generation.
        /// </param>
        /// <param name="level">Level to fill during the level generation process.</param>
        public void GenerateLevel(ChunkLibrary2D chunkLibrary, Level2D level)
        {
            Random2 random = new Random2();

            this.GenerateLevel(chunkLibrary, level, random);
        }

        /// <summary>
        /// Generates a 2D level using the given chunk library, level and random number generator.
        /// </summary>
        /// <param name="chunkLibrary">
        /// Chunk library that holds all chunk templates to use for the level generation.
        /// </param>
        /// <param name="level">Level to fill during the level generation process.</param>
        /// <param name="random">Random number generator to use for the level generation.</param>
        public void GenerateLevel(ChunkLibrary2D chunkLibrary, Level2D level, Random2 random)
        {
            this.GenerateLevel<ChunkTemplate2D, Chunk2D>(chunkLibrary, level, random);
        }

        /// <summary>
        /// Generates a 2D level using the given chunk library and desired dimensions for the level.
        /// </summary>
        /// <param name="chunkLibrary">
        /// Chunk library that holds all chunk templates to use for the level generation.
        /// </param>
        /// <param name="levelExtents">Width and height the resulting level should have.</param>
        /// <returns>Generated level with the desired width and height.</returns>
        public Level2D GenerateLevel(ChunkLibrary2D chunkLibrary, Vector2F levelExtents)
        {
            Random2 random = new Random2();

            return this.GenerateLevel(chunkLibrary, levelExtents, random);
        }

        /// <summary>
        /// Generates a 2D level using the given chunk library, desired dimensions for the level and random number generator.
        /// </summary>
        /// <param name="chunkLibrary">
        /// Chunk library that holds all chunk templates to use for the level generation.
        /// </param>
        /// <param name="levelExtents">Width and height the resulting level should have.</param>
        /// <param name="random">Random number generator to use for the level generation.</param>
        /// <returns>Generated level with the desired width and height.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Level width or height is smaller than or equal to zero.
        /// </exception>
        public Level2D GenerateLevel(ChunkLibrary2D chunkLibrary, Vector2F levelExtents, Random2 random)
        {
            if (levelExtents.X <= 0f)
            {
                throw new ArgumentOutOfRangeException("levelExtents", "Level width must be greater than zero.");
            }

            if (levelExtents.Y <= 0f)
            {
                throw new ArgumentOutOfRangeException("levelExtents", "Level height must be greater than zero.");
            }

            Level2D level = new Level2D(levelExtents);

            this.GenerateLevel(chunkLibrary, level, random);

            return level;
        }

        /// <summary>
        ///   Applies all available post-processing policies to the passed level.
        /// </summary>
        /// <param name="level">Level to process.</param>
        public void PostProcessLevel(Level2D level)
        {
            this.PostProcessLevel<Chunk2D>(level);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Factory method that returns a new chunk based on the given chunk template that the type of the chunk template 
        /// and of the chunks in the level.
        /// </summary>
        /// <param name="chunkTemplate">Chunk template the returned chunk should be based on.</param>
        /// <returns>Chunk based on the chunk template with the desired chunk type.</returns>
        /// <exception cref="ArgumentException">
        /// The type of the passed <c>chunktemplate</c> doesn't match the desired chunk type.
        /// </exception>
        protected override Chunk ConstructChunkFromTemplate(ChunkTemplate chunkTemplate)
        {
            return new Chunk2D(chunkTemplate);
        }

        #endregion
    }
}