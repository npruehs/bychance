// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelGenerator2D.cs" company="Nick Pruehs, Denis Vaz Alves">
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

    using Npruehs.GrabBag.Math.Vectors;
    using Npruehs.GrabBag.Util;

    /// <summary>
    /// Generates a 3D level based on a given chunk library.
    /// </summary>
    public class LevelGenerator2D : LevelGenerator
    {
        #region Public Methods and Operators

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