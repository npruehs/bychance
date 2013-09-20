// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextAlignmentRestriction.cs" company="Nick Pruehs, Denis Vaz Alves">
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
namespace ByChance.Configuration
{
    using ByChance.Core;

    /// <summary>
    /// Specifies which chunk contexts may be aligned.
    /// </summary>
    public class ContextAlignmentRestriction
    {
        #region Public Methods and Operators

        /// <summary>
        /// Checks if the two passed contexts can be aligned, or not.
        /// This relation is assumed to be symmetric i.e., if <c>first</c> and <c>second</c>
        /// can be aligned, then <c>second</c> and <c>first</c> can be aligned, too.
        /// </summary>
        /// <param name="first">First context to check.</param>
        /// <param name="second">Second context to check.</param>
        /// <returns><c>true</c>, if the two contexts can be aligned, and <c>false</c> otherwise.</returns>
        public virtual bool CanBeAligned(Context first, Context second)
        {
            return true;
        }

        #endregion
    }
}