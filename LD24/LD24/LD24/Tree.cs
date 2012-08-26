using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD24
{
    class Tree : Entity, IHasRect
    {
        private VertexPositionNormalTexture[] trunk;
        private VertexPositionNormalTexture[] leaves;

        float trunkheight = 25f;
        float trunkwidth = 4f;
        float leafwidth = 12f;
        float leafheight = 12f;
        private RectangleF rect;

        public Tree(Island i, Vector3 vector3)
            : base(i, Vector2.Zero, vector3)
        {
            trunkheight += (float)G.r.NextDouble() * 4f;
            trunkwidth += (float)G.r.NextDouble() * 1f;
            leafwidth += (float)G.r.NextDouble() * 3f;
            leafheight += (float)G.r.NextDouble() * 3f;

            this.Position = vector3;
            trunk = new VertexPositionNormalTexture[6];
            leaves = new VertexPositionNormalTexture[6];
            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f);

            Vector3 e = new Vector3(-trunkwidth, trunkheight, 0);
            Vector3 f = new Vector3(trunkwidth, trunkheight, 0);
            Vector3 g = new Vector3(trunkwidth, 0, 0);
            Vector3 h = new Vector3(-trunkwidth, 0, 0);

            rect = new RectangleF(new Vector2(Position.X - trunkwidth, Position.Z - trunkwidth), new Vector2(Position.X + trunkwidth, Position.Z + trunkwidth));

            trunk[0] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            trunk[1] = new VertexPositionNormalTexture(e, frontNormal, new Vector2(0, 0));
            trunk[2] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            trunk[3] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            trunk[4] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            trunk[5] = new VertexPositionNormalTexture(g, frontNormal, new Vector2(1, 1));

            e = new Vector3(-leafwidth, trunkheight + leafheight, 0.02f);
            f = new Vector3(leafwidth, trunkheight + leafheight, 0.02f);
            g = new Vector3(leafwidth, trunkheight - leafheight, 0.02f);
            h = new Vector3(-leafwidth, trunkheight - leafheight, 0.02f);

            leaves[0] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            leaves[1] = new VertexPositionNormalTexture(e, frontNormal, new Vector2(0, 0));
            leaves[2] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            leaves[3] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            leaves[4] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            leaves[5] = new VertexPositionNormalTexture(g, frontNormal, new Vector2(1, 1));

        }

        public override void Draw()
        {
            DrawTrunk();
            DrawLeaves();
        }

        public override void Update()
        {
        }
        internal void DrawTrunk()
        {
            G.g.e.Texture = RM.GetTexture("treetrunk");
            G.g.e.World = Matrix.CreateRotationY(Camera.c.leftRightRot) * Matrix.CreateTranslation(Position);
            G.g.e.CurrentTechnique.Passes[0].Apply();
            G.g.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, trunk, 0, 2);
        }

        internal void DrawLeaves()
        {
            G.g.e.Texture = RM.GetTexture("treeleaves");
            G.g.e.World = Matrix.CreateRotationY(Camera.c.leftRightRot) * Matrix.CreateTranslation(Position);
            G.g.e.CurrentTechnique.Passes[0].Apply();
            G.g.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, leaves, 0, 2);
        }

        public RectangleF Rectangle
        {
            get { return rect; }
        }
    }
}
