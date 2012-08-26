using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LD24
{
    class TerrainBuffer
    {
        List<VertexBuffer> buffers = new List<VertexBuffer>();
        List<VertexPositionNormalTexture> points = new List<VertexPositionNormalTexture>();
        VertexBuffer current;
        int index = 0;

        public TerrainBuffer()
        {
            current = new VertexBuffer(G.g.GraphicsDevice, typeof(VertexPositionNormalTexture), 60000, BufferUsage.WriteOnly);
            buffers.Add(current);
        }

        public void AddVertices(VertexPositionNormalTexture[] vpnt)
        {
            points.AddRange(vpnt);
            index += 3;
            if (index >= 60000)
            {
                current.SetData<VertexPositionNormalTexture>(points.ToArray());
                current = new VertexBuffer(G.g.GraphicsDevice, typeof(VertexPositionNormalTexture), 60000, BufferUsage.WriteOnly);
                buffers.Add(current);
                index = 0;
                points.Clear();
            }
        }

        public void AddLast()
        {
            current = new VertexBuffer(G.g.GraphicsDevice, typeof(VertexPositionNormalTexture), index, BufferUsage.WriteOnly);
            current.SetData<VertexPositionNormalTexture>(points.ToArray());
            buffers.RemoveAt(buffers.Count - 1);
            buffers.Add(current);
        }

        internal void Draw()
        {
            for (int i = 0; i < buffers.Count - 1; i++)
            {
                G.g.GraphicsDevice.SetVertexBuffer(buffers[i]);
                G.g.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 20000);
            }
            
            G.g.GraphicsDevice.SetVertexBuffer(buffers[buffers.Count - 1]);
            G.g.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, index / 3);
        }
    }
}
