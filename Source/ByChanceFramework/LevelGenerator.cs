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
using System.Reflection;
using ByChanceFramework.Base2D;
using ByChanceFramework.Base3D;
using ByChanceFramework.PostProc;
using NLog;

namespace ByChanceFramework
{
    /// <summary>
    /// Represents the main hub for the level generation process.
    /// <para>
    /// It takes or creates everything needed for the level generation (chunk library, level and random number generator)
    /// and generates a level through a defined algorithm.
    /// </para>
    /// </summary>
    public class LevelGenerator
    {
        /// <summary>
        /// The list of post-processing policies that will be processed after the level generation.
        /// </summary>
        private List<IPostProcessingPolicy> postProcessingPolicies = new List<IPostProcessingPolicy>();
        
        private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Flag that shows if the generated level is supposed to be a 2D level or not.
        /// </summary>
        private bool is2DLevel = true;

        /// <summary>
        /// Generates a 2D level using the given chunk library and level object.
        /// </summary>
        /// <param name="chunkLibrary">
        /// The passed <c>chunkLibrary</c> that holds all chunk templates viable for the level generation.
        /// </param>
        /// <param name="level">The <c>level</c> object that will be filled during the level generation process.</param>
        public void GenerateLevel(ChunkLibrary<ChunkTemplate2D> chunkLibrary, Level2D level)
        {
            RandomNumberGeneratorQ random = new RandomNumberGeneratorQ();

            GenerateLevel(chunkLibrary, level, random);
        }

        /// <summary>
        /// Generates a 2D level using the given chunk library, level object and random number generator.
        /// </summary>
        /// <param name="chunkLibrary">
        /// The passed <c>chunkLibrary</c> that holds all chunk templates viable for the level generation.
        /// </param>
        /// <param name="level">The <c>level</c> object that will be filled during the level generation process.</param>
        /// <param name="random">The <c>random</c> number generator that will be used for the level generation.</param>
        public void GenerateLevel(ChunkLibrary<ChunkTemplate2D> chunkLibrary, Level2D level, RandomNumberGeneratorQ random)
        {
            GenerateLevel<ChunkTemplate2D, Chunk2D>(chunkLibrary, level, random);
        }

        /// <summary>
        /// Generates a 2D level using the given chunk library and desired dimensions for the level.
        /// </summary>
        /// <param name="chunkLibrary">
        /// The passed <c>chunkLibrary</c> that holds all chunk templates viable for the level generation.
        /// </param>
        /// <param name="levelWidth">The width the resulting level should have.</param>
        /// <param name="levelHeight">The height the resulting level should have.</param>
        /// <returns>The generated level object with the desired <c>levelWidth</c> and <c>levelHeight</c>.</returns>
        public Level2D GenerateLevel(ChunkLibrary<ChunkTemplate2D> chunkLibrary, float levelWidth, float levelHeight)
        {
            RandomNumberGeneratorQ random = new RandomNumberGeneratorQ();

            return GenerateLevel(chunkLibrary, levelWidth, levelHeight, random);
        }

        /// <summary>
        /// Generates a 2D level using the given chunk library, tdesired dimensions for the level and random number generator.
        /// </summary>
        /// <param name="chunkLibrary">
        /// The passed <c>chunkLibrary</c> that holds all chunk templates viable for the level generation.
        /// </param>
        /// <param name="levelWidth">The width the resulting level should have.</param>
        /// <param name="levelHeight">The height the resulting level should have.</param>
        /// <param name="random">The <c>random</c> number generator that will be used for the level generation.</param>
        /// <returns>The generated level with the desired <c>levelWidth</c> and <c>levelHeight</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The passed value <c>levelWidth</c> or <c>levelHeight</c> is smaller than or equal to zero.
        /// </exception>
        public Level2D GenerateLevel(ChunkLibrary<ChunkTemplate2D> chunkLibrary, float levelWidth, float levelHeight, RandomNumberGeneratorQ random)
        {
            if (levelWidth <= 0f)
            {
                throw new ArgumentOutOfRangeException("levelWidth", levelWidth, "The width of the level must be greater than 0.0f.");
            }

            if (levelHeight <= 0f)
            {
                throw new ArgumentOutOfRangeException("levelHeight", levelHeight, "The height of the level must be greater than 0.0f.");
            }

            Level2D level = new Level2D(levelWidth, levelHeight);

            GenerateLevel<ChunkTemplate2D, Chunk2D>(chunkLibrary, level, random);

            return level;
        }

        /// <summary>
        /// Generates a 3D level with the given chunk library and level object.
        /// </summary>
        /// <param name="chunkLibrary">
        /// The passed <c>chunkLibrary</c> that holds all chunk templates viable for the level generation.
        /// </param>
        /// <param name="level">The <c>level</c> object that will be filled during the level generation process.</param>
        public void GenerateLevel(ChunkLibrary<ChunkTemplate3D> chunkLibrary, Level3D level)
        {
            RandomNumberGeneratorQ random = new RandomNumberGeneratorQ();

            GenerateLevel(chunkLibrary, level, random);
        }

        /// <summary>
        /// Generates a 3D level with the given chunk library, level object and random number generator.
        /// </summary>
        /// <param name="chunkLibrary">
        /// The passed <c>chunkLibrary</c> that holds all chunk templates viable for the level generation.
        /// </param>
        /// <param name="level">The <c>level</c> object that will be filled during the level generation process.</param>
        /// <param name="random">The <c>random</c> number generator that will be used for the level generation.</param>
        public void GenerateLevel(ChunkLibrary<ChunkTemplate3D> chunkLibrary, Level3D level, RandomNumberGeneratorQ random)
        {
            GenerateLevel<ChunkTemplate3D, Chunk3D>(chunkLibrary, level, random);
        }

        /// <summary>
        /// Generates a 3D level with the given chunk library and desired dimensions for the level.
        /// </summary>
        /// <param name="chunkLibrary">
        /// The passed <c>chunkLibrary</c> that holds all chunk templates viable for the level generation.
        /// </param>
        /// <param name="levelWidth">The width the resulting level should have.</param>
        /// <param name="levelHeight">The height the resulting level should have.</param>
        /// <param name="levelDepth">The depth the resulting level should have.</param>
        /// <returns>The generated level with the desired <c>levelWidth</c>, <c>levelHeight</c> and <c>levelDepth</c>.</returns>
        public Level3D GenerateLevel(ChunkLibrary<ChunkTemplate3D> chunkLibrary, float levelWidth, float levelHeight, float levelDepth)
        {
            RandomNumberGeneratorQ random = new RandomNumberGeneratorQ();

            return GenerateLevel(chunkLibrary, levelWidth, levelHeight, levelDepth, random);
        }

        /// <summary>
        /// Generates a 3D level with the given chunk library, desired dimensions for the level and random number generator.
        /// </summary>
        /// <param name="chunkLibrary">
        /// The passed <c>chunkLibrary</c> that holds all chunk templates viable for the level generation.
        /// </param>
        /// <param name="levelWidth">The width the resulting level should have.</param>
        /// <param name="levelHeight">The height the resulting level should have.</param>
        /// <param name="levelDepth">The depth the resulting level should have.</param>
        /// <param name="random">The <c>random</c> number generator that will be used for the level generation.</param>
        /// <returns>The generated level with the desired <c>levelWidth</c>, <c>levelHeight</c> and <c>levelDepth</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The passed value for <c>levelWidth</c> or <c>levelHeight</c> or <c>levelDepth</c> is smaller than or equal to zero.
        /// </exception>
        public Level3D GenerateLevel(ChunkLibrary<ChunkTemplate3D> chunkLibrary, float levelWidth, float levelHeight, float levelDepth, RandomNumberGeneratorQ random)
        {
            if (levelWidth <= 0f)
            {
                throw new ArgumentOutOfRangeException("levelWidth", levelWidth, "The width of the level must be greater than 0.0f.");
            }

            if (levelHeight <= 0f)
            {
                throw new ArgumentOutOfRangeException("levelHeight", levelHeight, "The height of the level must be greater than 0.0f.");
            }

            if (levelDepth <= 0f)
            {
                throw new ArgumentOutOfRangeException("levelDepth", levelDepth, "The depth of the level must be greater than 0.0f.");
            }

            Level3D level = new Level3D(levelWidth, levelHeight, levelDepth);

            GenerateLevel<ChunkTemplate3D, Chunk3D>(chunkLibrary, level, random);

            return level;
        }

        /// <summary>
        /// Generates a level with the given chunk library, level object and random number generator.
        /// The specified Type parameters define if the resulting level will be 2D or 3D.
        /// </summary>
        /// <typeparam name="T">Type of the chunk templates in the chunk library.</typeparam>
        /// <typeparam name="U">Type of the chunks for the level.</typeparam>
        /// <param name="chunkLibrary">
        /// The <c>chunkLibrary</c> that holds the chunk templates to be considered for the level generation.
        /// </param>
        /// <param name="level">The <c>level</c> that will be filled during the level generation.</param>
        /// <param name="random">The <c>random</c> number generator that will be used for the level generation.</param>
        /// <exception cref="ArgumentNullException">
        ///     <list type="bullet">
        ///         <item><description>The passed <c>chunkLibrary</c> is <c>null</c>.</description></item>
        ///         <item><description>The passed <c>level</c> object is <c>null</c>.</description></item>
        ///         <item><description>The passed <c>random</c> number generator object is <c>null</c>.</description></item>
        ///     </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <list type="bullet">
        ///         <item><description>The passed <c>chunkLibrary</c> is empty.</description></item>
        ///         <item><description>The chunk templates in the <c>chunklibrary</c> have an invalid type.</description></item>
        ///     </list>
        /// </exception>
        private void GenerateLevel<T, U>(ChunkLibrary<T> chunkLibrary, Level<U> level, RandomNumberGeneratorQ random)
            where T : ChunkTemplate
            where U : Chunk
        {
            DateTime startTime;
            TimeSpan passedTime;
            List<Chunk> chunkCandidates = new List<Chunk>();
            List<Int32> candidateContexts = new List<Int32>();
            List<Int32> effectiveWeights = new List<Int32>();
            int[] chunkQuantities;
            int totalWeight;
            int randomWeight;
            int randomChunkIndex;

            ChunkTemplate chunkTemplate;
            Chunk possibleChunk;
            Chunk compatibleChunk = null;
            Context freeContext;
            Context possibleContext;
            Context compatibleContext;

            bool keepTrying;


            if (chunkLibrary == null)
            {
                throw new ArgumentNullException("chunkLibrary");
            }
            else if (chunkLibrary.GetChunkTemplateCount() <= 0)
            {
                throw new ArgumentException("The chunk library is empty. The level generation process requires at least one chunk template.", "chunkLibrary");
            }

            if (level == null)
            {
                throw new ArgumentNullException("level");
            }

            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            chunkTemplate = chunkLibrary.GetChunkTemplateByIndex(0);

            if (chunkTemplate is ChunkTemplate2D)
            {
                is2DLevel = true;
            }
            else if (chunkTemplate is ChunkTemplate3D)
            {
                is2DLevel = false;
            }
            else
            {
                throw new ArgumentException("The chunk templates in the chunk library have an invalid type. " +
                                            "Make sure that chunk templates are of type ChunkTemplate2D or ChunkTemplate3D.", "chunkLibrary");
            }

            // initialize log
            logger.Info("ByChance Framework version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ".");
            if (is2DLevel)
            {
                logger.Info("Level is a 2D level.");
            }
            else
            {
                logger.Info("Level is a 3D level.");
            }

            logger.Info("Level has " + level.GetLevelChunkCount() + " chunk(s).");
            logger.Info("Chunk Library has " + chunkLibrary.GetChunkTemplateCount() + " chunk template(s).");
            logger.Info("RNG uses a seed of " + random.Seed + ".");

            startTime = DateTime.Now;
            chunkQuantities = new int[chunkLibrary.GetChunkTemplateCount()];

            // check if level has a starting chunk
            if (level.GetLevelChunkCount() <= 0)
            {
                // if not set one randomly
                randomChunkIndex = (int)random.RandomRangeUInt32((uint)chunkLibrary.GetChunkTemplateCount());

                chunkTemplate = chunkLibrary.GetChunkTemplateByIndex(randomChunkIndex);

                possibleChunk = ConstructChunkFromTemplate(chunkTemplate);
                
                level.SetRandomStartingChunk(possibleChunk, random);
            }

            // main level generation loop
            while (true)
            {
                freeContext = level.FindProcessibleContext();
                if (freeContext == null)
                {
                    passedTime = DateTime.Now - startTime;

                    logger.Info("Level Generation took " + passedTime.Seconds + "." + passedTime.Milliseconds + " seconds.");
                    logger.Info("Generated level contains " + level.GetLevelChunkCount() + " chunks:");
                    for (int i = 0; i < chunkQuantities.Length; i++)
                    {
                        logger.Info("\t" + chunkQuantities[i] * 100 / level.GetLevelChunkCount() + "% of the chunks are instances of chunk " + i + ".");
                    }

                    // after finishing the level generation, start the post-processing
                    if (postProcessingPolicies.Count > 0)
                    {
                        logger.Info("Beginning post-processing.");

                        startTime = DateTime.Now;

                        foreach (IPostProcessingPolicy policy in postProcessingPolicies)
                        {
                            policy.Process<U>(level);
                        }

                        passedTime = DateTime.Now - startTime;

                        logger.Info("Post-processing took " + passedTime.Seconds + "." + passedTime.Milliseconds + " seconds.");
                    }

                    return;
                }

                if (freeContext.Source is Chunk2D)
                {
                    Chunk2D chunk2D = (Chunk2D) freeContext.Source;
                    logger.Info("Expanding level at " + chunk2D.X + " | " + chunk2D.Y);
                }
                else if (freeContext.Source is Chunk3D)
                {
                    Chunk3D chunk3D = (Chunk3D) freeContext.Source;
                    logger.Info("Expanding level at " + chunk3D.X + " | " + chunk3D.Y + " | " + chunk3D.Z);
                }

                // clear candidate lists after each iteration
                chunkCandidates.Clear();
                candidateContexts.Clear();

                // filter chunk library for compatible chunk candidates
                for (int i = 0; i < chunkLibrary.GetChunkTemplateCount(); i++)
                {
                    chunkTemplate = chunkLibrary.GetChunkTemplateByIndex(i);

                    possibleChunk = ConstructChunkFromTemplate(chunkTemplate);

                    keepTrying = true;
                    while (keepTrying)
                    {
                        for (int j = 0; j < possibleChunk.GetContextCount(); j++)
                        {
                            possibleContext = possibleChunk.GetContextByIndex(j);
                            if (CanBeAligned(possibleContext, freeContext) &&
                                level.FitsLevelGeometry(freeContext, possibleContext))
                            {
                                chunkCandidates.Add(possibleChunk);
                                candidateContexts.Add(possibleContext.Index);

                                keepTrying = false;
                                break;
                            }
                        }

                        if (keepTrying && possibleChunk.AllowChunkRotation)
                        {
                            logger.Info("* Trying to rotate chunk with ID " + possibleChunk.Index + ".");
                            keepTrying = possibleChunk.Rotate();
                        }
                        else
                        {
                            keepTrying = false;
                        }
                    }
                }
                logger.Info("Found " + chunkCandidates.Count + " chunk candidates.");

                // if no candidates are available for the selected context, block and ignore it in further iterations
                if (chunkCandidates.Count == 0)
                {
                    freeContext.Blocked = true;
                    logger.Info("Blocked context for further iterations.");
                    continue;
                }

                // compute effective weights for each chunk candidate
                effectiveWeights.Clear();
                for (int i = 0; i < chunkCandidates.Count; i++)
                {
                    Chunk chunkCandidate;

                    chunkCandidate = chunkCandidates[i];

                    effectiveWeights.Add(GetEffectiveWeight(freeContext, chunkCandidate.GetContextByIndex(candidateContexts[i]), chunkQuantities[chunkCandidate.Index]));
                }

                // compute the sum of all effective chunk weights
                totalWeight = 0;
                foreach (int effectiveWeight in effectiveWeights)
                {
                    totalWeight += effectiveWeight;
                }
                logger.Info("Calculated total weight: " + totalWeight);

                // pick a random chunk
                randomWeight = (int)random.RandomRangeUInt32((uint)totalWeight);
                logger.Info("Calculated random weight: " + randomWeight);
                for (int i = 0; i < chunkCandidates.Count; i++)
                {
                    randomWeight -= effectiveWeights[i];

                    if (randomWeight < 0)
                    {
                        // integrate selected chunk into level
                        compatibleChunk = chunkCandidates[i];
                        compatibleContext = compatibleChunk.GetContextByIndex(candidateContexts[i]);
                        
                        level.AddChunk(freeContext, compatibleContext);
                        logger.Info("Added chunk with ID " + compatibleChunk.Index + " to the level.");

                        chunkQuantities[compatibleContext.Source.Index]++;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a new chunk based on the given chunk template and in a type that fits the type of the chunk template 
        /// and the chunks in the level respectively.
        /// </summary>
        /// <param name="chunkTemplate">The <c>chunk template</c> the returned chunk should be based on.</param>
        /// <returns>A chunk based on the chunk template with the desired chunk type.</returns>
        /// <exception cref="ArgumentException">
        /// The type of the passed <c>chunktemplate</c> doesn't match the desired chunk type.
        /// </exception>
        private Chunk ConstructChunkFromTemplate(ChunkTemplate chunkTemplate)
        {
            if (is2DLevel)
            {
                try
                {
                    return new Chunk2D(chunkTemplate);
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException("Can't construct 2D levels with chunk templates that aren't of type ChunkTemplate2D.", "chunkTemplate", e);
                }
            }
            else
            {
                try
                {
                    return new Chunk3D(chunkTemplate);
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException("Can't construct 3D levels with chunk templates that aren't of type ChunkTemplate3D.", "chunkTemplate", e);
                }
            }
        }

        /// <summary>
        /// Adds a given policy to the existing list of post-processing policies.
        /// </summary>
        /// <param name="policy">The <c>policy</c> to be added.</param>
        /// <seealso cref="postProcessingPolicies"/>
        /// <exception cref="ArgumentNullException">The passed <c>policy</c> is null.</exception>
        public void AddPostProcessingPolicy(IPostProcessingPolicy policy)
        {
            if (policy == null)
            {
                throw new ArgumentNullException("policy");
            }

            postProcessingPolicies.Add(policy);
            policy.LevelGenerator = this;
        }

        /// <summary>
        /// Clears the list of post-processing policies.
        /// </summary>
        /// <seealso cref="postProcessingPolicies"/>
        public void ClearPostProcessingPolicies()
        {
            postProcessingPolicies.Clear();
        }

        #region Virtual Methods
        /// <summary>
        /// Checks if the two passed contexts can be aligned, or not.
        /// This relation is assumed to be symmetric i.e., if <c>first</c> and <c>second</c>
        /// can be aligned, then <c>second</c> and <c>first</c> can be aligned, too.
        /// </summary>
        /// <param name="first">The <c>first</c> context to check.</param>
        /// <param name="second">The <c>second</c> context to check.</param>
        /// <returns><c>true</c>, if the two contexts can be aligned, and <c>false</c> otherwise.</returns>
        public virtual bool CanBeAligned(Context first, Context second)
        {
            return true;
        }

        /// <summary>
        /// Gets the effective weight of a chunk.
        /// <para>
        /// By default this method returns the weight of the chunk candidate to which <c>secondContext</c> belongs to.
        /// <c>firstContext</c> and <c>occurences</c> are passed additionally to provide means of comparisons 
        /// for custom implementations of the method by clients. 
        /// </para>
        /// </summary>
        /// <param name="firstContext">Open context of the existing chunk to which the new chunk candidate will be attached to.</param>
        /// <param name="secondContext">Open context of the chunk candidate.</param>
        /// <param name="occurrences">
        /// The number of times chunks similar to the chunk of <c>secondContext</c> (i.e. based on the same template) 
        /// already exist in the level.
        /// </param>
        /// <returns>Non-negative integer that represents the effective weight of the chunk candidate.</returns>
        public virtual int GetEffectiveWeight(Context firstContext, Context secondContext, int occurrences)
        {
            return secondContext.Source.Weight;
        }
        #endregion
    }
}
