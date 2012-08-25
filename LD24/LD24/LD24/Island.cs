﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LD24
{
    class Island
    {
        float scaleHorizontal = 4;
        float scaleVertical = 2;

        int treeminy = 40;
        int treemaxy = 150;
        int treemindist = 4;
        int treeAmount = 1000;

        private List<Tree> trees = new List<Tree>();

        private List<VertexBuffer> buffers = new List<VertexBuffer>();

        private VertexPositionNormalTexture[] pointList;
        private float[] heights;
        private VertexBuffer water;

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
            SmoothTerrain(9);

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

            CreateWater();
            CreateEnvironment();
        }

        private void CreateEnvironment()
        {

            for (int i = 0; i < treeAmount; i++)
            {
                var x = G.r.Next((int)(512 * scaleHorizontal));
                var z = G.r.Next((int)(512 * scaleHorizontal));
                var height = CheckHeightCollision(new Vector3(x, 0, z));
                while (height > treemaxy || height < treeminy)
                {
                    x = G.r.Next((int)(512 * scaleHorizontal));
                    z = G.r.Next((int)(512 * scaleHorizontal));
                    height = CheckHeightCollision(new Vector3(x, 0, z));
                }

                trees.Add(new Tree(new Vector3(x, height - 8, z)));
            }
        }

        private void CreateWater()
        {

            float waterheight = 9.5f;
            int size = 400;
            int iters = 150;

            VertexPositionNormalTexture[] waters = new VertexPositionNormalTexture[iters * iters * 6];
            int offset = 0;
            for (int x = 0; x < iters; x++)
            {
                for (int y = 0; y < iters; y++)
                {
                    int offX = size * (x - (iters / 2));
                    int offY = size * (y - (iters / 2));
                    waters[offset++] = new VertexPositionNormalTexture(new Vector3(offX, waterheight, offY), Vector3.Up, new Vector2(0, 0));
                    waters[offset++] = new VertexPositionNormalTexture(new Vector3(offX + size, waterheight, offY), Vector3.Up, new Vector2(size, 0));
                    waters[offset++] = new VertexPositionNormalTexture(new Vector3(offX, waterheight, offY + size), Vector3.Up, new Vector2(0, size));
                    waters[offset++] = new VertexPositionNormalTexture(new Vector3(offX, waterheight, offY + size), Vector3.Up, new Vector2(0, size));
                    waters[offset++] = new VertexPositionNormalTexture(new Vector3(offX + size, waterheight, offY), Vector3.Up, new Vector2(size, 0));
                    waters[offset++] = new VertexPositionNormalTexture(new Vector3(offX + size, waterheight, offY + size), Vector3.Up, new Vector2(size, size));                    
                }
            }

            water = new VertexBuffer(G.g.GraphicsDevice, typeof(VertexPositionNormalTexture), iters * iters * 6, BufferUsage.WriteOnly);
            water.SetData<VertexPositionNormalTexture>(waters);
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
            pointList = new VertexPositionNormalTexture[512 * 512 * 6];
            
            for (int x = 1; x < 512; x++)
            {
                for (int z = 1; z < 512; z++)
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

            return new Vector3((x * scaleHorizontal), heights[newx + (newz * 512)] * scaleVertical, (z * scaleHorizontal));
        }

        public void Draw()
        {
            G.g.e.VertexColorEnabled = false;
            G.g.e.TextureEnabled = true;
            G.g.e.Texture = RM.GetTexture("grass");
            
            G.g.e.LightingEnabled = true;
            G.g.e.DirectionalLight0.Enabled = true;
            G.g.e.DirectionalLight0.Direction = new Vector3(-1, -1, -1);
            G.g.e.DirectionalLight0.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f);
            G.g.e.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
            G.g.e.CurrentTechnique.Passes[0].Apply();
            foreach (var vb in buffers)
            {
                G.g.GraphicsDevice.SetVertexBuffer(vb);
                G.g.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vb.VertexCount / 3);
            }

            G.g.e.Texture = RM.GetTexture("water");
            G.g.e.CurrentTechnique.Passes[0].Apply();
            G.g.GraphicsDevice.SetVertexBuffer(water);
            G.g.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, water.VertexCount / 3);


            G.g.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            G.g.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            foreach (var t in trees.OrderBy(x => (x.Position - Camera.c.position).Length()).Reverse())
            {
                G.g.e.Texture = RM.GetTexture("treetrunk");
                G.g.e.CurrentTechnique.Passes[0].Apply();
                t.DrawTrunk();
                G.g.e.Texture = RM.GetTexture("treeleaves");
                G.g.e.CurrentTechnique.Passes[0].Apply();
                t.DrawLeaves();
            }
        }

        public float CheckHeightCollision(Vector3 location)
        {
            try
            {
                int x = (int)(location.X / scaleHorizontal);
                int z = (int)(location.Z / scaleHorizontal);
                if (x > 0 && x < 511 && z > 0 && z < 511)
                {
                    float tl = pointList[(6 * x) + ((6 * z) * 512)].Position.Y;
                    float tr = pointList[(6 * (x + 1)) + ((6 * z) * 512)].Position.Y;
                    float bl = pointList[(6 * x) + ((6 * (z + 1)) * 512)].Position.Y;
                    float br = pointList[(6 * (x + 1)) + ((6 * (z + 1)) * 512)].Position.Y;

                    float fHeight;
                    float fSqX = (location.X / scaleHorizontal) - x;
                    float fSqZ = (location.Z / scaleHorizontal) - z;
                    if ((fSqX + fSqZ) < 1)
                    {
                        fHeight = tl;
                        fHeight += (tr - tl) * fSqX;
                        fHeight += (bl - tl) * fSqZ;
                    }
                    else
                    {
                        fHeight = br;
                        fHeight += (tr - br) * (1.0f - fSqZ);
                        fHeight += (bl - br) * (1.0f - fSqX);
                    }

                    return fHeight+ 4;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
