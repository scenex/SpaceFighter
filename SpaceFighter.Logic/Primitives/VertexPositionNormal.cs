﻿//-----------------------------------------------------------------------------
// VertexPositionNormal.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


namespace SpaceFighter.Logic.Primitives
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Custom vertex type for vertices that have just a
    /// position and a normal, without any texture coordinates.
    /// </summary>
    public struct VertexPositionNormal : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;


        /// <summary>
        /// Constructor.
        /// </summary>
        public VertexPositionNormal(Vector3 position, Vector3 normal)
        {
            this.Position = position;
            this.Normal = normal;
        }

        /// <summary>
        /// A VertexDeclaration object, which contains information about the vertex
        /// elements contained within this struct.
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexPositionNormal.VertexDeclaration; }
        }

    }
}
