// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPostProcessingPolicy.cs" company="Nick Pruehs, Denis Vaz Alves">
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
namespace ByChance.PostProc
{
    using ByChance.Core;

    using ByChanceFramework;

    /// <summary>
    /// <para>
    /// Encapsulates some way of processing a level after it has been
    /// successfully generated.
    /// </para>
    /// <para>
    /// Post-processing policies are optional
    /// and by no means necessary to create a level. They are provided
    /// as a convinient and uniform way to automatically run post-generation
    /// algorithms that are tracked and logged to the same log file in which 
    /// the ByChance framework summarizes the level generation process. The
    /// framework uses the strategy pattern to define a family of processing
    /// algorithms that can be called after the level has been generated.
    /// </para>
    /// <para>
    /// Each post-processing policy has access to the level generator that
    /// build the level that is to be processed, in order to determine which
    /// contexts can be aligned, for example.
    /// <see cref="ByChanceFramework.LevelGenerator.AddPostProcessingPolicy(IPostProcessingPolicy)"/>
    /// in order to learn more about how to teach the level generator which
    /// policies it should apply.
    /// </para>
    /// <para>
    /// As part of the contract, post-processing policies are entitled to be
    /// able to process 2D and 3D levels alike.
    /// </para>
    /// </summary>
    public interface IPostProcessingPolicy
    {
        #region Public Methods and Operators

        /// <summary>
        /// Processes the passed finished level following the rules defined by
        /// the level generator that build that level.
        /// </summary>
        /// <typeparam name="T">Type of the chunks the level consists of.</typeparam>
        /// <param name="levelGenerator">Level generator that built the level to be processed.</param>
        /// <param name="level">Level to process.</param>
        void Process<T>(LevelGenerator levelGenerator, Level<T> level) where T : Chunk;

        #endregion
    }
}