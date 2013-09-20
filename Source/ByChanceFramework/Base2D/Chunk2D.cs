// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Chunk2D.cs" company="Nick Pruehs, Denis Vaz Alves">
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
namespace ByChance.Base2D
{
    using System;

    using ByChance.Core;

    using Npruehs.GrabBag.Math.Vectors;

    /// <summary>
    /// <para>
    /// Main building block that makes up a 2D level, providing
    /// extents, position and rotation.
    /// </para>
    /// <para>
    /// Chunks are created based on pre-defined chunk templates and define the positions 
    /// of all contexts and anchors associated with the chunk, as well as attributes that are 
    /// required for the level generation process such as the probability of a chunk
    /// being picked to be added next.
    /// </para>
    /// </summary>
    public sealed class Chunk2D : Chunk
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new chunk with a reference to the passed chunk template,
        /// using that template's width and height and performing deep copies of
        /// the lists of contexts and anchors of that template.
        /// </summary>
        /// <param name="chunkTemplate">Chunk template the new chunk will be based on.</param>
        /// <seealso cref="Chunk.ChunkTemplate"/>
        /// <exception cref="ArgumentNullException"><paramref name="chunkTemplate"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="chunkTemplate"/> is not of the type <see cref="ChunkTemplate2D"/>.</exception>
        public Chunk2D(ChunkTemplate chunkTemplate)
            : base(chunkTemplate)
        {
            ChunkTemplate2D chunkTemplate2D = chunkTemplate as ChunkTemplate2D;

            // Check the type of the passed template.
            if (chunkTemplate2D == null)
            {
                throw new ArgumentException("Passed template isn't of type ChunkTemplate2D.", "chunkTemplate");
            }

            // Copy template extents.
            this.Extents = chunkTemplate2D.Extents;

            // Perform deep copies of the context and anchor lists of the template.
            foreach (Context2D context in chunkTemplate2D.Contexts)
            {
                this.Contexts.Add(new Context2D(context, this));
            }

            foreach (Anchor2D anchor in chunkTemplate2D.Anchors)
            {
                this.Anchors.Add(new Anchor2D(anchor, this));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Width and height of this chunk.
        /// </summary>
        public Vector2F Extents { get; private set; }

        /// <summary>
        ///     Position of this chunk within the level.
        /// </summary>
        public Vector2F Position { get; private set; }

        /// <summary>
        /// Clock-wise rotation of this chunk within the level, in degrees.
        /// </summary>
        public float Rotation { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the position of this chunk within the level as string.
        /// </summary>
        /// <returns>Position of this chunk within the level.</returns>
        public override string ToString()
        {
            return this.Position.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Rotates this chunk clock-wise by 90°, changing the positions of all
        /// contexts and switching width and height.
        /// </summary>
        /// <returns><c>true</c>, while the chunk hasn't been rotated by 360° in every direction, and <c>false</c> otherwise.</returns>
        internal override bool Rotate()
        {
            var center = this.Extents / 2;

            foreach (Context2D context in this.Contexts)
            {
                // Translate context position to chunk origin.
                var origin = context.RelativePosition - center;

                /*
                 * Each point (x, y) in a cartesian coordinate system can be written as
                 * 
                 * x = r * cos q
                 * y = r * sin q
                 * 
                 * with an initial angle q.
                 * 
                 * Rotating that point around the origin by the angle f results in
                 * 
                 * x' = r * cos (q + f) = r * cos q * cos f - r * sin q * sin f
                 * y' = r * sin (q + w) = r * sin q * cos f + r * cos q * sin f
                 * 
                 * Applying the original definition of x and y leads to
                 * 
                 * x' = x * cos f - y * sin f
                 * y' = y * cos f + x * sin f
                 * 
                 * Rotating by 90° gives us
                 * 
                 * x' = x * 0 - y * 1 = -y
                 * y' = y * 0 + x * 1 = x
                 * 
                 * After that, we have to translate the context position back in the rotated chunk's coordinate system.
                 */
                var contextRelativePosX = -origin.Y + center.Y;
                var contextRelativePosY = origin.X + center.X;

                context.RelativePosition = new Vector2F(contextRelativePosX, contextRelativePosY);
            }

            // Switch width and height.
            this.Extents = new Vector2F(this.Extents.Y, this.Extents.X);

            // Update rotation.
            this.Rotation += 90;
            return this.Rotation < 360;
        }

        /// <summary>
        /// Sets the position of this chunk within the level and changes the
        /// relative positions of all anchors of this chunk to match its
        /// rotation.
        /// </summary>
        /// <param name="position">New position of this chunk within the level.</param>
        internal void SetPosition(Vector2F position)
        {
            // Set new position.
            this.Position = position;

            // Rotate anchors if necessary.
            if (this.Rotation <= 0)
            {
                return;
            }

            var center = this.Extents / 2;

            foreach (Anchor2D anchor in this.Anchors)
            {
                var origin = anchor.RelativePosition - center;

                var anchorRelativePositionX = (origin.X * (float)Math.Cos(this.Rotation))
                                              - (origin.Y * (float)Math.Sin(this.Rotation)) + center.Y;
                var anchorRelativePositionY = (origin.Y * (float)Math.Cos(this.Rotation))
                                              + (origin.X * (float)Math.Sin(this.Rotation)) + center.X;

                anchor.RelativePosition = new Vector2F(anchorRelativePositionX, anchorRelativePositionY);
            }
        }

        #endregion
    }
}