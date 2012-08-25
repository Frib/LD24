using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LD24
{
    class Island
    {
        private List<VertexBuffer> buffers = new List<VertexBuffer>();

        private VertexPositionNormalTexture[] pointList;
        private float[] heights;

        public Island()
        {
            var tex = RM.GetTexture("island");
            
            var width = tex.Width;
            var height = tex.Height;

            heights = new float[512 * 512];
            for (int x = 0; x < 512; x++)
            {
                for (int z = 0; z < 512; z++)
                {
                    Color[] myColor = new Color[1];
                    tex.GetData<Color>(0, new Rectangle(x, z, 1, 1), myColor, 0, 1);
                    heights[x + (z * 512)] = myColor[0].B;
                }
            }

            WritePointList();
            SmoothTerrain(7);

            int previous = 0;
            int herp = 60000;
            for (int i = 0; i < pointList.Length; i += herp)
            {
                if (i == 0)
                {
                    continue;
                }

                var vb = new VertexBuffer(G.g.GraphicsDevice, typeof(VertexPositionNormalTexture), herp, BufferUsage.WriteOnly);
                vb.SetData<VertexPositionNormalTexture>(pointList.Skip(previous).Take(herp).ToArray());
                buffers.Add(vb);
                previous = i;
            }

            var vbe = new VertexBuffer(G.g.GraphicsDevice, typeof(VertexPositionNormalTexture), pointList.Length - previous, BufferUsage.WriteOnly);
            vbe.SetData<VertexPositionNormalTexture>(pointList.Skip(previous).Take(pointList.Length - previous).ToArray());
            buffers.Add(vbe);
        }

        public void SmoothTerrain(int times)
        {
            for (int h = 0; h <= times; h++)
            {
                for (int i = 1; i < 512 - 2; i++)
                {
                    for (int j = 1; j < 512 - 2; j++)
                    {
                        float point = heights[GetIndex(i, j)];

                        float up = heights[GetIndex(i, j - 1)];
                        float down = heights[GetIndex(i, j + 1)];
                        float left = heights[GetIndex(i - 1, j)];
                        float right = heights[GetIndex(i + 1, j)];

                        heights[GetIndex(i, j)] = (point + ((up + down + left + right) / 4)) / 2;
                    }
                }
            }
            WritePointList();
        }

        public int GetIndex(int x, int z)
        {
            return x + (z * 512);
        }

        public void WritePointList()
        {
            pointList = new VertexPositionNormalTexture[513 * 513 * 6];
            for (int x = 1; x < 513; x++)
            {
                for (int z = 1; z < 513; z++)
                {
                    Vector3 point1 = GetVectorForHeightMapThingyEnzo(x, z);
                    Vector3 point2 = GetVectorForHeightMapThingyEnzo(x + 1, z);
                    Vector3 point3 = GetVectorForHeightMapThingyEnzo(x, z + 1);
                    Vector3 point4 = GetVectorForHeightMapThingyEnzo(x + 1, z + 1);
                    
                    float texScale = 1;

                    pointList[(6 * x) + ((6 * z) * 512)] = new VertexPositionNormalTexture(point1, GetNormal(x, z), new Vector2(x / texScale, z / texScale));
                    pointList[(6 * x) + 1 + ((6 * z) * 512)] = new VertexPositionNormalTexture(point2, GetNormal(x + 1, z), new Vector2((x + 1) / texScale, z / texScale));
                    pointList[(6 * x) + 2 + ((6 * z) * 512)] = new VertexPositionNormalTexture(point3, GetNormal(x, z + 1), new Vector2(x / texScale, (z + 1) / texScale));

                    pointList[(6 * x) + 4 + ((6 * z) * 512)] = new VertexPositionNormalTexture(point2, GetNormal(x + 1, z), new Vector2((x + 1) / texScale, z / texScale));
                    pointList[(6 * x) + 3 + ((6 * z) * 512)] = new VertexPositionNormalTexture(point3, GetNormal(x, z + 1), new Vector2(x / texScale, (z + 1) / texScale));
                    pointList[(6 * x) + 5 + ((6 * z) * 512)] = new VertexPositionNormalTexture(point4, GetNormal(x + 1, z + 1), new Vector2((x + 1) / texScale, (z + 1) / texScale));
                }
            }
        }

        public Vector3 GetNormal(int x, int z)
        {
            Vector3 point1 = GetVectorForHeightMapThingyEnzo(x - 1, z + 1);
            Vector3 point2 = GetVectorForHeightMapThingyEnzo(x, z - 1);
            Vector3 point3 = GetVectorForHeightMapThingyEnzo(x + 1, z);

            Vector3 U = point2 - point1;
            Vector3 V = point3 - point1;

            Vector3 normal = new Vector3(U.Y * V.Z - U.Z * V.Y, U.Z * V.X - U.X * V.Z, U.X * V.Y - U.Y * V.X) * -1;
            normal.Normalize();

            return normal;
        }

        public Vector3 GetVectorForHeightMapThingyEnzo(int x, int z)
        {
            int newz = z;
            int newx = x;
            if (z >= 512)
            {
                newz -= 512;
            }
            if (x >= 512)
            {
                newx -= 512;
            }

            return new Vector3((x), heights[newx + (newz * 512)], (z));
        }

        public void Draw()
        {
            G.g.e.VertexColorEnabled = false;
            G.g.e.TextureEnabled = true;
            G.g.e.Texture = RM.GetTexture("grass");
            G.g.e.CurrentTechnique.Passes[0].Apply();

            G.g.e.LightingEnabled = true;
            G.g.e.DirectionalLight0.Enabled = true;
            G.g.e.DirectionalLight0.Direction = new Vector3(-1, -1, -1);
            G.g.e.DirectionalLight0.DiffuseColor = new Vector3(0.4f, 0.5f, 0.4f);
            G.g.e.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);

            foreach (var vb in buffers)
            {
                G.g.GraphicsDevice.SetVertexBuffer(vb);
                G.g.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vb.VertexCount / 3);

            }

        }
    }
}
