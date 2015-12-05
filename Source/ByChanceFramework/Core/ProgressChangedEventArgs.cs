// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelGenerator.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2015 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ByChance.Core
{
    using System;

    /// <summary>
    ///   Level generation progress changed data.
    /// </summary>
    public class ProgressChangedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        ///   Current level generation progress.
        /// </summary>
        public float Progress { get; internal set; }

        #endregion
    }
}