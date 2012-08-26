using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD24
{
    class Clouds
    {
        Vector3 offsetA;
        Vector3 offsetB;
        Vector3 offsetC;
        float degreesA = 0;
        float degreesB = 0;
        float degreesC = 0;

        Texture2D CloudA;
        Texture2D CloudB;
        Texture2D CloudC;
        Vector3 orbitA;
        Vector3 orbitB;
        Vector3 orbitC;
        private VertexPositionNormalTexture[] billboardVertices;

        public Clouds(Vector2 orbit)
        {

            var size = new Vector2(9192, 4096);

            billboardVertices = new VertexPositionNormalTexture[6];
            Vector3 e = new Vector3(-size.X / 2, size.Y, 0);
            Vector3 f = new Vector3(size.X / 2, size.Y, 0);
            Vector3 g = new Vector3(size.X / 2, 0, 0);
            Vector3 h = new Vector3(-size.X / 2, 0, 0);

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f);
            billboardVertices[0] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            billboardVertices[1] = new VertexPositionNormalTexture(e, frontNormal, new Vector2(0, 0));
            billboardVertices[2] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            billboardVertices[3] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            billboardVertices[4] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            billboardVertices[5] = new VertexPositionNormalTexture(g, frontNormal, new Vector2(1, 1));

            offsetA = new Vector3(-orbit.X * 4, 0, -orbit.Y * 4);
            offsetB = new Vector3(-orbit.X * 3, 0, -orbit.Y * 3);
            offsetC = new Vector3(-orbit.X * 5f, 0, -orbit.Y * 5f);

            orbitA = new Vector3(orbit.X, 256, orbit.Y);
            orbitB = new Vector3(orbit.X, 5120, orbit.Y);
            orbitC = new Vector3(orbit.X, 2048, orbit.Y);

            degreesA = G.r.Next(360);
            degreesB = G.r.Next(360);
            degreesC = G.r.Next(360);

            CloudA = RM.GetTexture("cloud1");
            CloudB = RM.GetTexture("cloud2");
            CloudC = RM.GetTexture("cloud3");
        }

        public void Update()
        {
            degreesA += 0.12f;
            degreesB += -0.1f;
            degreesC += 0.06f;
        }

        public void Draw()
        {
            var GraphicsDevice = G.g.GraphicsDevice;
            G.g.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            G.g.e.World = Matrix.CreateRotationX(MathHelper.PiOver4) * Matrix.CreateRotationY(MathHelper.PiOver4) * Matrix.CreateTranslation(offsetB) * Matrix.CreateRotationY(MathHelper.ToRadians(degreesB)) * Matrix.CreateTranslation(orbitB);
            G.g.e.Texture = CloudB;
            G.g.e.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);


            G.g.e.World = Matrix.CreateRotationX(MathHelper.PiOver4) * Matrix.CreateRotationY(MathHelper.PiOver4) * Matrix.CreateTranslation(offsetC) * Matrix.CreateRotationY(MathHelper.ToRadians(degreesC)) * Matrix.CreateTranslation(orbitC);
            G.g.e.Texture = CloudC;
            G.g.e.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);



            G.g.e.World = Matrix.CreateRotationY(MathHelper.PiOver4) * Matrix.CreateTranslation(offsetA) * Matrix.CreateRotationY(MathHelper.ToRadians(degreesA)) * Matrix.CreateTranslation(orbitA);
            G.g.e.Texture = CloudA;
            G.g.e.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);
        }
    }
}
