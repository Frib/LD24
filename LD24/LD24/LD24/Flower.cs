using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD24
{
    class Flower : Entity
    {
        private VertexPositionNormalTexture[] trunk;
        private VertexPositionNormalTexture[] core;
        private VertexPositionNormalTexture[] leaves;
        private float trunkwidth = 1;
        private float trunkheight = 2;
        private float leafwidth = 1;
        private float leafheight = 1;

        private Texture2D tstem;
        private Texture2D tcore;
        private Texture2D tleaves;
        private Color cLeaves;
        private Color cCore;

        public Flower(Island i, Vector3 vector3)
            : base(i, Vector2.Zero, vector3)
        {
            this.Position = vector3;
            trunk = new VertexPositionNormalTexture[6];
            core = new VertexPositionNormalTexture[6];
            leaves = new VertexPositionNormalTexture[6];
            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f);

            Vector3 e = new Vector3(-trunkwidth, trunkheight, 0);
            Vector3 f = new Vector3(trunkwidth, trunkheight, 0);
            Vector3 g = new Vector3(trunkwidth, 0, 0);
            Vector3 h = new Vector3(-trunkwidth, 0, 0);

            trunk[0] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            trunk[1] = new VertexPositionNormalTexture(e, frontNormal, new Vector2(0, 0));
            trunk[2] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            trunk[3] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            trunk[4] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            trunk[5] = new VertexPositionNormalTexture(g, frontNormal, new Vector2(1, 1));

            e = new Vector3(-leafwidth, trunkheight + leafheight ,0);
            f = new Vector3(leafwidth, trunkheight + leafheight,0);
            g = new Vector3(leafwidth, trunkheight - leafheight, 0);
            h = new Vector3(-leafwidth, trunkheight - leafheight,0);

            leaves[0] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            leaves[1] = new VertexPositionNormalTexture(e, frontNormal, new Vector2(0, 0));
            leaves[2] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            leaves[3] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            leaves[4] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            leaves[5] = new VertexPositionNormalTexture(g, frontNormal, new Vector2(1, 1));

            e = new Vector3(-leafwidth, trunkheight + leafheight + 0.1f, 0);
            f = new Vector3(leafwidth, trunkheight + leafheight + 0.1f, 0);
            g = new Vector3(leafwidth, (trunkheight - leafheight) + 0.1f, 0);
            h = new Vector3(-leafwidth, (trunkheight - leafheight) + 0.1f, 0);

            core[0] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            core[1] = new VertexPositionNormalTexture(e, frontNormal, new Vector2(0, 0));
            core[2] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            core[3] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            core[4] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            core[5] = new VertexPositionNormalTexture(g, frontNormal, new Vector2(1, 1));

            tstem = RM.GetTexture("flowerstem");
            tcore = RM.GetTexture("flowercore");
            tleaves = RM.GetTexture("flowerleaves");

            cCore = GenColor(Color.Yellow);
            cLeaves = GenColor(Color.White);

            ColorFlower();
        }

        private Color GenColor(Color color)
        {
            var r = G.r;
            if (r.Next(2) == 1) return color;
            int i = r.Next(30);
            switch (i)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4: return Color.White;
                case 5:
                case 6: return Color.LightBlue;
                case 7:
                case 8: return Color.Black;
                case 9: return Color.Yellow;
                case 10:
                case 11: return Color.LightGray;
                case 12: return Color.Salmon;
                case 13: return Color.LightGreen;
                case 14:
                case 15: return Color.Brown;
                case 16: return Color.Fuchsia;
                case 17: return Color.Red;
                case 18: return Color.Gold;
                case 19: return Color.DarkGray;
                default: return Color.White;
            }
        }

        public void ColorFlower()
        {
            Color[] data = new Color[tcore.Bounds.Width * tcore.Bounds.Height];
            tcore.GetData<Color>(data);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Color(cCore.R, cCore.G, cCore.B, data[i].A);
            }
            tcore = new Texture2D(G.g.GraphicsDevice, tcore.Width, tcore.Height);
            tcore.SetData<Color>(data);

            data = new Color[tleaves.Bounds.Width * tleaves.Bounds.Height];
            tleaves.GetData<Color>(data);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Color(cLeaves.R, cLeaves.G, cLeaves.B, data[i].A);
            }
            tleaves = new Texture2D(G.g.GraphicsDevice, tleaves.Width, tleaves.Height);
            tleaves.SetData<Color>(data);
        }

        public override void Draw()
        {
            if ((this.position - island.player.Position).Length() < Island.flowerRenderDist)
            {

                DrawTrunk();
                DrawLeaves();
            }
        }

        public override void Update()
        {
        }
        internal void DrawTrunk()
        {
            G.g.e.Texture = tstem;
            G.g.e.World = Matrix.CreateRotationY(Camera.c.leftRightRot) * Matrix.CreateTranslation(Position);
            G.g.e.CurrentTechnique.Passes[0].Apply();
            G.g.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, trunk, 0, 2);
        }

        internal void DrawLeaves()
        {
            G.g.e.Texture = tleaves;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(0, -trunkheight, 0)) * Matrix.CreateRotationX(Camera.c.upDownRot) * Matrix.CreateTranslation(new Vector3(0, trunkheight, 0)) * Matrix.CreateRotationY(Camera.c.leftRightRot) * Matrix.CreateTranslation(Position);
            G.g.e.CurrentTechnique.Passes[0].Apply();
            G.g.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, leaves, 0, 2);

            G.g.e.Texture = tcore;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(0, -trunkheight, 0)) * Matrix.CreateRotationX(Camera.c.upDownRot) * Matrix.CreateTranslation(new Vector3(0, trunkheight, 0)) * Matrix.CreateRotationY(Camera.c.leftRightRot) * Matrix.CreateTranslation(Position);
            G.g.e.CurrentTechnique.Passes[0].Apply();
            G.g.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, core, 0, 2);
        }
    }
}
