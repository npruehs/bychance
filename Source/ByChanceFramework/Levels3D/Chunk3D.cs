// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Chunk3D.cs" company="Nick Pruehs, Denis Vaz Alves">
//   Copyright 2011-2013 Nick Pruehs, Denis Vaz Alves.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ByChance.Levels3D
{
    using System;

    using ByChance.Core;

    /// <summary>
    /// <para>
    /// Main building block that makes up a 3D level, providing
    /// extents, position and rotation.
    /// </para>
    /// <para>
    /// Chunks are created based on pre-defined chunk templates and define the positions 
    /// of all contexts and anchors associated with the chunk, as well as attributes that are 
    /// required for the level generation process such as the probability of a chunk
    /// being picked to be added next.
    /// </para>
    /// </summary>
    public sealed class Chunk3D : Chunk
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new chunk with a reference to the passed chunk template,
        /// using that template's width, height and depth and performing deep copies of
        /// the lists of contexts and anchors of that template.
        /// </summary>
        /// <param name="chunkTemplate">Chunk template the new chunk will be based on.</param>
        /// <seealso cref="Chunk.ChunkTemplate"/>
        /// <exception cref="ArgumentNullException"><paramref name="chunkTemplate"/> is null.</exception>
        /// <exception cref="ArgumentException">Passed template is not of the type <see cref="ChunkTemplate3D"/>.</exception>
        internal Chunk3D(ChunkTemplate chunkTemplate)
            : base(chunkTemplate)
        {
            // Check the type of the passed template.
            if (!(chunkTemplate is ChunkTemplate3D))
            {
                throw new ArgumentException("Passed template isn't of type ChunkTemplate3D.", "chunkTemplate");
            }

            // Copy template extents.
            this.Extents = ((ChunkTemplate3D)chunkTemplate).Extents;

            // Perform deep copies of the context and anchor lists of the template.
            foreach (Context3D context in chunkTemplate.ChunkTemplateContexts)
            {
                this.ChunkContexts.Add(new Context3D(context, this));
            }

            foreach (Anchor3D anchor in chunkTemplate.ChunkTemplateAnchors)
            {
                this.ChunkAnchors.Add(new Anchor3D(anchor, this));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Width, height and depth of this chunk.
        /// </summary>
        public Vector3F Extents { get; private set; }

        /// <summary>
        /// Position of this chunk within the level.
        /// </summary>
        public Vector3F Position { get; private set; }

        /// <summary>
        /// Rotation of this chunk around the x-, y- and z-axis, in degrees.
        /// </summary>
        public Vector3F Rotation { get; private set; }

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
        /// Rotates this chunk according to the following rules:
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
        /// The positions of all contexts are changed accordingly, and width, height and depth are switched as appropriate.
        /// </summary>
        /// <returns><c>true</c>, while the chunk hasn't been rotated by 360° in every direction, and <c>false</c> otherwise.</returns>
        internal override bool Rotate()
        {
            return this.RotateY();
        }

        /// <summary>
        /// Sets the position of this chunk within the level and changes the
        /// relative positions of all anchors of this chunk to match its
        /// rotation.
        /// </summary>
        /// <param name="position">New position of this chunk within the level.</param>
        internal void SetPosition(Vector3F position)
        {
            // Set new position.
            this.Position = position;

            // Rotate anchors if necessary.
            var center = this.Extents / 2;

            if (this.Rotation.Z > 0)
            {
                foreach (Anchor3D anchor in this.ChunkAnchors)
                {
                    var origin = anchor.RelativePosition - center;

                    var anchorRelativePositionX = (origin.X * (float)Math.Cos(this.Rotation.Z))
                                                  - (origin.Y * (float)Math.Sin(this.Rotation.Z)) + center.Y;
                    var anchorRelativePositionY = (origin.X * (float)Math.Sin(this.Rotation.Z))
                                                  + (origin.Y * (float)Math.Cos(this.Rotation.Z)) + center.X;

                    anchor.RelativePosition = new Vector3F(
                        anchorRelativePositionX, anchorRelativePositionY, anchor.RelativePosition.Z);
                }
            }

            if (this.Rotation.X > 0)
            {
                foreach (Anchor3D anchor in this.ChunkAnchors)
                {
                    var origin = anchor.RelativePosition - center;

                    var anchorRelativePositionY = (origin.Y * (float)Math.Cos(this.Rotation.X))
                                                  - (origin.Z * (float)Math.Sin(this.Rotation.X)) + center.Z;
                    var anchorRelativePositionZ = (origin.Y * (float)Math.Sin(this.Rotation.X))
                                                  + (origin.Z * (float)Math.Cos(this.Rotation.X)) + center.Y;

                    anchor.RelativePosition = new Vector3F(
                        anchor.RelativePosition.X, anchorRelativePositionY, anchorRelativePositionZ);
                }
            }

            if (this.Rotation.Y > 0)
            {
                foreach (Anchor3D anchor in this.ChunkAnchors)
                {
                    var origin = anchor.RelativePosition - center;

                    var anchorRelativePositionZ = (origin.Z * (float)Math.Cos(this.Rotation.Y))
                                                  - (origin.X * (float)Math.Sin(this.Rotation.Y)) + center.X;
                    var anchorRelativePositionX = (origin.Z * (float)Math.Sin(this.Rotation.Y))
                                                  + (origin.X * (float)Math.Cos(this.Rotation.Y)) + center.Z;

                    anchor.RelativePosition = new Vector3F(
                        anchorRelativePositionX, anchor.RelativePosition.Y, anchorRelativePositionZ);
                }
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
            var center = this.Extents / 2;

            // Change the positions of all contexts.
            foreach (Context3D context in this.ChunkContexts)
            {
                var origin = context.RelativePosition - center;

                var contextRelativePositionY = -origin.Z + center.Z;
                var contextRelativePositionZ = origin.Y + center.Y;

                context.RelativePosition = new Vector3F(
                    context.RelativePosition.X, contextRelativePositionY, contextRelativePositionZ);
            }

            // Switch height and depth.
            this.Extents = new Vector3F(this.Extents.X, this.Extents.Z, this.Extents.Y);

            var rotationX = this.Rotation.X + 90;

            if (rotationX >= 360)
            {
                this.Rotation = new Vector3F(0, this.Rotation.X, this.Rotation.Z);
                return this.RotateZ();
            }

            this.Rotation = new Vector3F(rotationX, this.Rotation.Y, this.Rotation.Z);
            return true;
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
            var center = this.Extents / 2;

            // Change the positions of all contexts.
            foreach (Context3D context in this.ChunkContexts)
            {
                var origin = context.RelativePosition - center;

                var contextRelativePositionX = origin.Z + center.Z;
                var contextRelativePositionZ = -origin.X + center.X;

                context.RelativePosition = new Vector3F(
                    contextRelativePositionX, context.RelativePosition.Y, contextRelativePositionZ);
            }

            // Switch width and depth.
            this.Extents = new Vector3F(this.Extents.Z, this.Extents.Y, this.Extents.X);

            var rotationY = this.Rotation.Y + 90;

            if (rotationY >= 360)
            {
                this.Rotation = new Vector3F(this.Rotation.X, 0, this.Rotation.Z);
                return this.RotateX();
            }

            this.Rotation = new Vector3F(this.Rotation.X, rotationY, this.Rotation.Z);
            return true;
        }

        /// <summary>
        /// Rotates this chunk by 90° around the z-axis, changing the positions of
        /// all contexts and switching width and height.
        /// </summary>
        /// <returns><c>true</c>, while the chunk hasn't been rotated by 360° in every direction, and <c>false</c> otherwise.</returns>
        private bool RotateZ()
        {
            var center = this.Extents / 2;

            // Change the positions of all contexts.
            foreach (Context3D context in this.ChunkContexts)
            {
                var origin = context.RelativePosition - center;

                var contextRelativePositionX = -origin.Y + center.Y;
                var contextRelativePositionY = origin.X + center.X;

                context.RelativePosition = new Vector3F(
                    contextRelativePositionX, contextRelativePositionY, context.RelativePosition.Z);
            }

            // Switch width and height.
            this.Extents = new Vector3F(this.Extents.Y, this.Extents.X, this.Extents.Z);

            this.Rotation = new Vector3F(this.Rotation.X, this.Rotation.Y, this.Rotation.Z + 90);

            return this.Rotation.Z < 360;
        }

        #endregion
    }
}