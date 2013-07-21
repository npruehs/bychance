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

namespace ByChanceFramework.Base3D
{
    /// <summary>
    /// <para>
    /// Represents the main building block that make up a 3D level, providing
    /// extents, position and rotation.
    /// </para>
    /// <para>
    /// Chunks are created based on pre-defined chunk templates and define the positions 
    /// of all contexts and anchors associated with the chunk, as well as attributes that are 
    /// required for the level generation process such as the probability of a chunk
    /// being picked to be added next.
    /// </para>
    /// </summary>
    sealed public class Chunk3D : Chunk
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
        /// Gets or sets the depth of this chunk.
        /// </summary>
        public float Depth { get; private set; }

        /// <summary>
        /// Gets or sets the x-coordinate of the position of this chunk within the level.
        /// </summary>
        public float X { get; private set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the position of this chunk within the level.
        /// </summary>
        public float Y { get; private set; }

        /// <summary>
        /// Gets or sets the z-coordinate of the position of this chunk within the level.
        /// </summary>
        public float Z { get; private set; }

        /// <summary>
        /// Gets or sets the rotation of this chunk around the x-axis, in degrees.
        /// </summary>
        public float RotationX { get; private set; }

        /// <summary>
        /// Gets or sets the rotation of this chunk around the y-axis, in degrees.
        /// </summary>
        public float RotationY { get; private set; }

        /// <summary>
        /// Gets or sets the rotation of this chunk around the z-axis, in degrees.
        /// </summary>
        public float RotationZ { get; private set; }


        /// <summary>
        /// Constructs a new chunk with a reference to the passed chunk template,
        /// using that template's width, height and depth and performing deep copies of
        /// the lists of contexts and anchors of that template.
        /// </summary>
        /// <param name="chunkTemplate">The chunk template the new chunk will be based on.</param>
        /// <seealso cref="Chunk.ChunkTemplate"/>
        /// <exception cref="ArgumentNullException">The passed <c>template</c> is null.</exception>
        /// <exception cref="ArgumentException">The passed template is not of the type <c>ChunkTemplate3D</c>.</exception>
        public Chunk3D(ChunkTemplate chunkTemplate)
            : base(chunkTemplate)
        {
            // check the type of the passed template
            if (!(chunkTemplate is ChunkTemplate3D))
            {
                throw new ArgumentException("Passed template isn't of type ChunkTemplate3D and therefore invalid.", "chunkTemplate");
            }

            Context3D context;
            Anchor3D anchor;

            // copy template extents
            Width = ((ChunkTemplate3D)chunkTemplate).Width;
            Height = ((ChunkTemplate3D)chunkTemplate).Height;
            Depth = ((ChunkTemplate3D)chunkTemplate).Depth;

            // perform deep copies of the context and anchor lists of the template
            for (int i = 0; i < chunkTemplate.GetContextCount(); i++)
            {
                context = (Context3D)chunkTemplate.GetContextByIndex(i);
                contexts.Add(new Context3D(context, this));
            }

            for (int j = 0; j < chunkTemplate.GetAnchorCount(); j++)
            {
                anchor = (Anchor3D)chunkTemplate.GetAnchorByIndex(j);
                anchors.Add(new Anchor3D(anchor, this));
            }
        }


        /// <summary>
        /// Rotates this chunk according to the following rules:
        /// 
        /// <list type="bullet">
        ///     <item>
        ///         <description>The chunk is rotated by 90° around the y-axis.</description>
        ///     </item>
        ///     <item>
        ///         <description>If the chunk has been rotated by 360° around the y-axis, it is rotated by 90° around the x-axis after.</description>
        ///     </item>
        ///     <item>
        ///         <description>If the chunk has been rotated by 360° around the x-axis, it is rotated by 90° around the z-axis after.</description>
        ///     </item>
        /// </list>
        /// 
        /// The positions of all contexts are changed accordingly, and width, height and depth are switched as appropriate.
        /// </summary>
        /// <returns><c>true</c>, while the chunk hasn't been rotated by 360° in every direction, and <c>false</c> otherwise.</returns>
        internal override bool Rotate()
        {
            return RotateY();
        }

        /// <summary>
        /// Rotates this chunk by 90° around the y-axis, changing the positions of
        /// all contexts and switching width and depth. If the chunk has been
        /// rotated by 360° around the y-axis, it is rotated by 90° around the
        /// x-axis after.
        /// </summary>
        /// <returns><c>true</c>, while the chunk hasn't been rotated by 360° in every direction, and <c>false</c> otherwise.</returns>
        private bool RotateY()
        {
            float centerX;
            float centerZ;

            float originX;
            float originZ;

            float tempWidth;

            centerX = Width / 2;
            centerZ = Depth / 2;

            // change the positions of all contexts
            foreach (Context3D context in contexts)
            {
                originX = context.RelativePosX - centerX;
                originZ = context.RelativePosZ - centerZ;

                context.RelativePosX = originZ + centerZ;
                context.RelativePosZ = -originX + centerX;
            }

            // switch width and depth
            tempWidth = Width;
            Width = Depth;
            Depth = tempWidth;

            RotationY += 90;
            if (RotationY >= 360)
            {
                RotationY = 0;

                return RotateX();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Rotates this chunk by 90° around the x-axis, changing the positions of
        /// all contexts and switching height and depth. If the chunk has been
        /// rotated by 360° around the x-axis, it is rotated by 90° around the
        /// z-axis after.
        /// </summary>
        /// <returns><c>true</c>, while the chunk hasn't been rotated by 360° in every direction, and <c>false</c> otherwise.</returns>
        private bool RotateX()
        {
            float centerY;
            float centerZ;

            float originY;
            float originZ;

            float tempHeight;

            centerY = Height / 2;
            centerZ = Depth / 2;

            // change the positions of all contexts
            foreach (Context3D context in contexts)
            {
                originY = context.RelativePosY - centerY;
                originZ = context.RelativePosZ - centerZ;

                context.RelativePosY = -originZ + centerZ;
                context.RelativePosZ = originY + centerY;
            }

            // switch height and depth
            tempHeight = Height;
            Height = Depth;
            Depth = tempHeight;

            RotationX += 90;
            if (RotationX >= 360)
            {
                RotationX = 0;

                return RotateZ();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Rotates this chunk by 90° around the z-axis, changing the positions of
        /// all contexts and switching width and height.
        /// </summary>
        /// <returns><c>true</c>, while the chunk hasn't been rotated by 360° in every direction, and <c>false</c> otherwise.</returns>
        private bool RotateZ()
        {
            float centerX;
            float centerY;

            float originX;
            float originY;

            float tempHeight;

            centerX = Width / 2;
            centerY = Height / 2;

            // change the positions of all contexts
            foreach (Context3D context in contexts)
            {
                originX = context.RelativePosX - centerX;
                originY = context.RelativePosY - centerY;

                context.RelativePosX = -originY + centerY;
                context.RelativePosY = originX + centerX;
            }

            // switch height and depth
            tempHeight = Height;
            Height = Width;
            Width = tempHeight;

            RotationZ += 90;
            return RotationZ < 360;
        }

        /// <summary>
        /// Sets the position of this chunk within the level and changes the
        /// relative positions of all anchors of this chunk to match its
        /// rotation.
        /// </summary>
        /// <param name="x">The new x-coordinate of the position of this chunk within the level.</param>
        /// <param name="y">The new y-coordinate of the position of this chunk within the level.</param>
        /// <param name="z">The new z-coordinate of the position of this chunk within the level.</param>
        internal void SetPosition(float x, float y, float z)
        {
            float centerX;
            float centerY;
            float centerZ;

            float originX;
            float originY;
            float originZ;

            // set new position
            this.X = x;
            this.Y = y;
            this.Z = z;

            // rotate anchors if necessary
            centerX = Width / 2;
            centerY = Height / 2;
            centerZ = Depth / 2;

            if (RotationZ > 0)
            {
                foreach (Anchor3D anchor in anchors)
                {
                    originX = anchor.RelativePosX - centerX;
                    originY = anchor.RelativePosY - centerY;

                    anchor.RelativePosX = originX * (float)Math.Cos(RotationZ) - originY * (float)Math.Sin(RotationZ) + centerY;
                    anchor.RelativePosY = originX * (float)Math.Sin(RotationZ) + originY * (float)Math.Cos(RotationZ) + centerX;
                }
            }

            if (RotationX > 0)
            {
                foreach (Anchor3D anchor in anchors)
                {
                    originY = anchor.RelativePosY - centerY;
                    originZ = anchor.RelativePosZ - centerZ;

                    anchor.RelativePosY = originY * (float)Math.Cos(RotationX) - originZ * (float)Math.Sin(RotationX) + centerZ;
                    anchor.RelativePosZ = originY * (float)Math.Sin(RotationX) + originZ * (float)Math.Cos(RotationX) + centerY;
                }
            }

            if (RotationY > 0)
            {
                foreach (Anchor3D anchor in anchors)
                {
                    originX = anchor.RelativePosX - centerX;
                    originZ = anchor.RelativePosZ - centerZ;

                    anchor.RelativePosZ = originZ * (float)Math.Cos(RotationY) - originX * (float)Math.Sin(RotationY) + centerX;
                    anchor.RelativePosX = originZ * (float)Math.Sin(RotationY) + originX * (float)Math.Cos(RotationY) + centerZ;
                }
            }
        }

        /// <summary>
        /// Returns the position of this chunk within the level as string.
        /// </summary>
        /// <returns>The position of this chunk within the level.</returns>
        public override string ToString()
        {
            return X + " | " + Y + " | " + Z;
        }
    }
}
