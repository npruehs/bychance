// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChunkDistribution.cs" company="Nick Pruehs, Denis Vaz Alves">
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
namespace ByChance.Configuration.Parameters
{
    using ByChance.Core;

    /// <summary>
    /// Specifies the distribution of chunk templates within a level.
    /// </summary>
    public class ChunkDistribution
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the effective weight of a chunk.
        /// <para>
        /// By default this method returns the weight of the chunk candidate to which <paramref name="secondContext"/> belongs to.
        /// <paramref name="firstContext"/> and <paramref name="occurrences"/> are passed additionally to provide means of comparisons 
        /// for custom implementations of the method by clients. 
        /// </para>
        /// </summary>
        /// <param name="firstContext">Open context of the existing chunk to which the new chunk candidate will be attached to.</param>
        /// <param name="secondContext">Open context of the chunk candidate.</param>
        /// <param name="occurrences">
        /// Number of times chunks similar to the chunk of <paramref name="secondContext"/> (i.e. based on the same template) 
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