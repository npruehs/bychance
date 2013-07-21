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

namespace ByChanceFramework.Base2D
{
    /// <summary>
    /// <para>
    /// Represents the main building block that make up a 2D level, providing
    /// extents, position and rotation.
    /// </para>
    /// <para>
    /// Chunks are created based on pre-defined chunk templates and define the positions 
    /// of all contexts and anchors associated with the chunk, as well as attributes that are 
    /// required for the level generation process such as the probability of a chunk
    /// being picked to be added next.
    /// </para>
    /// </summary>
    sealed public class Chunk2D : Chunk
    {
        /// <summary>
        /// Gets or sets the width of this chunk.
        /// </summary>
        public float Width { get; private set; }

        /// <summary>
        /// Gets or sets the height of this chunk.
        /// </summary>
        public float Height { get; private set; }

        /// <summary>
        /// Gets or sets the x-coordinate of the position of this chunk within the level.
        /// </summary>
        public float X { get; private set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the position of this chunk within the level.
        /// </summary>
        public float Y { get; private set; }

        /// <summary>
        /// Gets or sets the clock-wise rotation of this chunk within the level, in degrees.
        /// </summary>
        public float Rotation { get; private set; }


        /// <summary>
        /// Constructs a new chunk with a reference to the passed chunk template,
        /// using that template's width and height and performing deep copies of
        /// the lists of contexts and anchors of that template.
        /// </summary>
        /// <param name="chunkTemplate">The chunk template the new chunk will be based on.</param>
        /// <seealso cref="Chunk.ChunkTemplate"/>
        /// <exception cref="ArgumentNullException">The passed <c>template</c> is null.</exception>
        /// <exception cref="ArgumentException">The passed template is not of the type <c>ChunkTemplate2D</c>.</exception>
        public Chunk2D(ChunkTemplate chunkTemplate)
            : base(chunkTemplate)
        {
            // check the type of the passed template
            if (!(chunkTemplate is ChunkTemplate2D))
            {
                throw new ArgumentException("Passed template isn't of type ChunkTemplate2D and therefore invalid.", "chunkTemplate");
            }

            Context2D context;
            Anchor2D anchor;

            // copy template extents
            Width = ((ChunkTemplate2D)chunkTemplate).Width;
            Height = ((ChunkTemplate2D)chunkTemplate).Height;

            // perform deep copies of the context and anchor lists of the template
            for (int i = 0; i < chunkTemplate.GetContextCount(); i++)
            {
                context = (Context2D)chunkTemplate.GetContextByIndex(i);
                contexts.Add(new Context2D(context, this));
            }

            for (int j = 0; j < chunkTemplate.GetAnchorCount(); j++)
            {
                anchor = (Anchor2D)chunkTemplate.GetAnchorByIndex(j);
                anchors.Add(new Anchor2D(anchor, this));
            }
        }

        /// <summary>
        /// Rotates this chunk clock-wise by 90°, changing the positions of all
        /// contexts and switching width and height.
        /// </summary>
        /// <returns><c>true</c>, while the chunk hasn't been rotated by 360° in every direction, and <c>false</c> otherwise.</returns>
        internal override bool Rotate()
        {
            float centerX;
            float centerY;

            float originX;
            float originY;

            float tempWidth;

            centerX = Width / 2;
            centerY = Height / 2;

            foreach (Context2D context in contexts)
            {
                // translate context position to chunk origin
                originX = context.RelativePosX - centerX;
                originY = context.RelativePosY - centerY;

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
                context.RelativePosX = -originY + centerY; 
                context.RelativePosY = originX + centerX;
            }

            // switch width and height
            tempWidth = Width;
            Width = Height;
            Height = tempWidth;

            Rotation += 90;
            return Rotation < 360;
        }

        /// <summary>
        /// Sets the position of this chunk within the level and changes the
        /// relative positions of all anchors of this chunk to match its
        /// rotation.
        /// </summary>
        /// <param name="x">The new x-coordinate of the position of this chunk within the level.</param>
        /// <param name="y">The new y-coordinate of the position of this chunk within the level.</param>
        internal void SetPosition(float x, float y)
        {
            float centerX;
            float centerY;

            float originX;
            float originY;

            // set new position
            this.X = x;
            this.Y = y;

            // rotate anchors if necessary
            centerX = Width / 2;
            centerY = Height / 2;

            if (Rotation > 0)
            {
                foreach (Anchor2D anchor in anchors)
                {
                    originX = anchor.RelativePosX - centerX;
                    originY = anchor.RelativePosY - centerY;

                    anchor.RelativePosX = originX * (float)Math.Cos(Rotation) - originY * (float)Math.Sin(Rotation) + centerY;
                    anchor.RelativePosY = originY * (float)Math.Cos(Rotation) + originX * (float)Math.Sin(Rotation) + centerX;
                }
            }
        }

        /// <summary>
        /// Returns the position of this chunk within the level as string.
        /// </summary>
        /// <returns>The position of this chunk within the level.</returns>
        public override string ToString()
        {
            return X + " | " + Y;
        }
    }
}
