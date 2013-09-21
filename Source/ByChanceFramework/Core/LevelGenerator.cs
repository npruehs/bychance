// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelGenerator.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ByChance.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ByChance.Configuration;
    using ByChance.Configuration.Parameters;
    using ByChance.Configuration.PostProcessing;
    using ByChance.Levels2D;
    using ByChance.Levels3D;

    /// <summary>
    /// Generates a level based on a given chunk library.
    /// </summary>
    public abstract class LevelGenerator
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new level generator with default configuration.
        /// </summary>
        protected LevelGenerator()
        {
            this.Configuration = new LevelGeneratorConfiguration
                {
                    ChunkDistribution = new ChunkDistribution(), 
                    ContextAlignmentRestriction = new ContextAlignmentRestriction(), 
                    PostProcessingPolicies = new List<PostProcessingPolicy>()
                };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Configuration of this level generator.
        /// </summary>
        public LevelGeneratorConfiguration Configuration { get; set; }

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
        protected abstract Chunk ConstructChunkFromTemplate(ChunkTemplate chunkTemplate);

        /// <summary>
        /// Generates a level with the given chunk library, level and random number generator.
        /// </summary>
        /// <typeparam name="TChunkTemplate">Type of the chunk templates in the chunk library.</typeparam>
        /// <typeparam name="TChunk">Type of the chunks for the level.</typeparam>
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
        protected void GenerateLevel<TChunkTemplate, TChunk>(
            ChunkLibrary<TChunkTemplate> chunkLibrary, Level<TChunk> level, Random2 random)
            where TChunkTemplate : ChunkTemplate where TChunk : Chunk
        {
            if (chunkLibrary == null)
            {
                throw new ArgumentNullException("chunkLibrary");
            }

            if (chunkLibrary.Count <= 0)
            {
                throw new ArgumentException(
                    "The chunk library is empty. The level generation process requires at least one chunk template.", 
                    "chunkLibrary");
            }

            if (level == null)
            {
                throw new ArgumentNullException("level");
            }

            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            var chunkCandidates = new List<Chunk>();
            var candidateContexts = new List<int>();
            var effectiveWeights = new List<int>();

            Chunk possibleChunk;

            // Initialize log.
            this.LogMessage(
                string.Format("ByChance Framework version {0}.", Assembly.GetExecutingAssembly().GetName().Version));

            this.LogMessage(string.Format("Level has {0} chunk(s).", level.Count));
            this.LogMessage(string.Format("Chunk Library has {0} chunk template(s).", chunkLibrary.Count));
            this.LogMessage(string.Format("Random number generator uses a seed of {0}.", random.Seed));

            var startTime = DateTime.Now;
            var chunkQuantities = new int[chunkLibrary.Count];

            // Check if level has a starting chunk.
            if (level.Count <= 0)
            {
                // Set starting chunk randomly.
                var randomChunkIndex = random.NextInt32(chunkLibrary.Count);
                var chunkTemplate = chunkLibrary[randomChunkIndex];
                possibleChunk = this.ConstructChunkFromTemplate(chunkTemplate);
                level.SetRandomStartingChunk(possibleChunk, random);
            }

            // Start main level generation loop.
            while (true)
            {
                var freeContext = level.FindProcessibleContext();

                if (freeContext == null)
                {
                    var passedTime = DateTime.Now - startTime;

                    this.LogMessage(
                        string.Format(
                            "Level Generation took {0}.{1} seconds.", passedTime.Seconds, passedTime.Milliseconds));
                    this.LogMessage(string.Format("Generated level contains {0} chunks:", level.Count));

                    for (var i = 0; i < chunkQuantities.Length; i++)
                    {
                        this.LogMessage(
                            string.Format(
                                "\t{0}% of the chunks are instances of chunk {1}.", 
                                chunkQuantities[i] * 100 / level.Count, 
                                i));
                    }

                    // Start post processing.
                    if (this.Configuration.PostProcessingPolicies.Count > 0)
                    {
                        this.LogMessage("Beginning post-processing.");

                        startTime = DateTime.Now;

                        foreach (var policy in this.Configuration.PostProcessingPolicies)
                        {
                            policy.Process(this.Configuration, level);
                        }

                        passedTime = DateTime.Now - startTime;

                        this.LogMessage(
                            string.Format(
                                "Post-processing took {0}.{1} seconds.", passedTime.Seconds, passedTime.Milliseconds));
                    }

                    return;
                }

                var chunk2D = freeContext.Source as Chunk2D;

                if (chunk2D != null)
                {
                    this.LogMessage(string.Format("Expanding level at {0}.", chunk2D.Position));
                }
                else
                {
                    var chunk3D = freeContext.Source as Chunk3D;

                    if (chunk3D != null)
                    {
                        this.LogMessage(string.Format("Expanding level at {0}.", chunk3D.Position));
                    }
                }

                // Clear candidate lists after each iteration.
                chunkCandidates.Clear();
                candidateContexts.Clear();

                // Filter chunk library for compatible chunk candidates.
                foreach (var chunkTemplate in chunkLibrary)
                {
                    possibleChunk = this.ConstructChunkFromTemplate(chunkTemplate);

                    var keepTrying = true;

                    while (keepTrying)
                    {
                        foreach (
                            var possibleContext in
                                possibleChunk.Contexts.Where(
                                    possibleContext =>
                                    this.Configuration.ContextAlignmentRestriction.CanBeAligned(
                                        possibleContext, freeContext)
                                    && level.FitsLevelGeometry(freeContext, possibleContext)))
                        {
                            chunkCandidates.Add(possibleChunk);
                            candidateContexts.Add(possibleContext.Index);

                            keepTrying = false;
                            break;
                        }

                        if (keepTrying && possibleChunk.AllowChunkRotation)
                        {
                            this.LogMessage(string.Format("* Trying to rotate chunk with ID {0}.", possibleChunk.Index));
                            keepTrying = possibleChunk.Rotate();
                        }
                        else
                        {
                            keepTrying = false;
                        }
                    }
                }

                this.LogMessage(string.Format("Found {0} chunk candidates.", chunkCandidates.Count));

                // If no candidates are available for the selected context, block and ignore it in further iterations.
                if (chunkCandidates.Count == 0)
                {
                    freeContext.Blocked = true;
                    this.LogMessage("Blocked context for further iterations.");
                    continue;
                }

                // Compute effective weights for each chunk candidate.
                effectiveWeights.Clear();

                for (var i = 0; i < chunkCandidates.Count; i++)
                {
                    var chunkCandidate = chunkCandidates[i];

                    effectiveWeights.Add(
                        this.Configuration.ChunkDistribution.GetEffectiveWeight(
                            freeContext, 
                            chunkCandidate.GetContext(candidateContexts[i]), 
                            chunkQuantities[chunkCandidate.Index]));
                }

                // Compute the sum of all effective chunk weights.
                var totalWeight = effectiveWeights.Sum();
                this.LogMessage(string.Format("Calculated total weight: {0}.", totalWeight));

                // Pick a random chunk
                var randomWeight = random.NextInt32(totalWeight);
                this.LogMessage("Calculated random weight: " + randomWeight);

                for (var i = 0; i < chunkCandidates.Count; i++)
                {
                    randomWeight -= effectiveWeights[i];

                    if (randomWeight >= 0)
                    {
                        continue;
                    }

                    // Integrate selected chunk into level
                    var compatibleChunk = chunkCandidates[i];
                    var compatibleContext = compatibleChunk.GetContext(candidateContexts[i]);

                    level.AddChunk(freeContext, compatibleContext);
                    this.LogMessage(string.Format("Added chunk with ID {0} to the level.", compatibleChunk.Index));

                    chunkQuantities[compatibleContext.Source.Index]++;
                    break;
                }
            }
        }

        /// <summary>
        /// Writes the specified message to the level generation log.
        /// </summary>
        /// <param name="message">Message to log.</param>
        protected void LogMessage(string message)
        {
            if (this.Configuration.Logger != null)
            {
                this.Configuration.Logger.LogMessage(message);
            }
        }

        #endregion
    }
}