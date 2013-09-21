// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDiscardOpenContextsRestriction.cs" company="Nick Pruehs, Denis Vaz Alves">
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
    using ByChance.Configuration.PostProcessing;
    using ByChance.Core;

    /// <summary>
    /// Specifies which open contexts should be discarded.
    /// </summary>
    public interface IDiscardOpenContextsRestriction
    {
        #region Public Methods and Operators

        /// <summary>
        /// Used by <see cref="DiscardOpenContextsPolicy"/> to check whether to discard the passed open
        /// context, or not.
        /// </summary>
        /// <param name="context">Context to be discarded.</param>
        /// <returns><c>true</c> if the specified context should be discarded, and <c>false</c> otherwise.</returns>
        bool ShouldDiscardContext(Context context);

        #endregion
    }
}