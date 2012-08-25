using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LD24
{
    class Bird : Entity
    {
        private Texture2D sHead;
        private Texture2D sTorso;
        private Texture2D sTail;
        private Texture2D sLeg;
        private Texture2D sBeak;
        private Color cHead;
        private Color cTail;
        private Color cTorso;

        public Bird(Island i, Vector3 pos) : base(i, new Vector2(1.5f + (float)G.r.NextDouble() *4), pos)
        {

        }

        public void SetTexturesSide(Texture2D head, Texture2D torso, Texture2D tail, Texture2D leg, Texture2D beak, Texture2D wing)
        {
            this.sHead = head;
            this.sTorso = torso;
            this.sTail = tail;
            this.sLeg = leg;
            this.sBeak = beak;
            this.sWing = wing;
        }

        public void SetColors(Color HeadColor, Color TailColor, Color TorsoColor, Color WingColor)
        {
            this.cHead = HeadColor;
            this.cTail = TailColor;
            this.cTorso = TorsoColor;
            this.cWing = WingColor;
        }

        public void FinalizeBird()
        {
            Color[] data = new Color[sHead.Width * sHead.Height];
            sHead.GetData<Color>(data);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Color(Math.Min(cHead.R, data[i].R), Math.Min(cHead.G, data[i].G), Math.Min(cHead.B, data[i].B), data[i].A);
            }
            sHead = new Texture2D(G.g.GraphicsDevice, sHead.Width, sHead.Height);
            sHead.SetData<Color>(data);

            sTorso.GetData<Color>(data);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Color(cTorso.R, cTorso.G, cTorso.B, data[i].A);
            }
            sTorso = new Texture2D(G.g.GraphicsDevice, sTorso.Width, sTorso.Height);
            sTorso.SetData<Color>(data);

            sTail.GetData<Color>(data);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Color(cTail.R, cTail.G, cTail.B, data[i].A);
            }
            sTail = new Texture2D(G.g.GraphicsDevice, sTail.Width, sTail.Height);
            sTail.SetData<Color>(data);

            sWing.GetData<Color>(data);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Color(cWing.R, cWing.G, cWing.B, data[i].A);
            }
            sWing = new Texture2D(G.g.GraphicsDevice, sWing.Width, sWing.Height);
            sWing.SetData<Color>(data);
        }

        public override void Draw()
        {
            var GraphicsDevice = G.g.GraphicsDevice;
            G.g.e.World = GetMatrixChain();

            G.g.e.Texture = sLeg;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(0, -size.Y / 3f, 0)) * Matrix.CreateRotationZ(legLeftAnim) * Matrix.CreateTranslation(new Vector3(0, size.Y / 3f, -0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sTorso;
            G.g.e.World = GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sTail;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(-size.Y / 4f, -size.Y / 2f, 0)) * Matrix.CreateRotationZ(tailAnim / 2) * Matrix.CreateTranslation(new Vector3(size.Y / 4f, size.Y / 2f, 0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sHead;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(size.Y / 4f, -size.Y / 1.6f, 0)) * Matrix.CreateRotationZ(headAnim / (animation == Animations.eating ? 0.75f : 2)) * Matrix.CreateTranslation(new Vector3(-size.Y / 4f, size.Y / 1.6f, 0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sBeak;
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sLeg;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(0, -size.Y / 3f, 0)) * Matrix.CreateRotationZ(legRightAnim) * Matrix.CreateTranslation(new Vector3(0, size.Y / 3f, 0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sWing;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(size.Y / 7f, -size.Y / 2f, 0)) * Matrix.CreateRotationZ(wingAnim * 1.5f) * Matrix.CreateTranslation(new Vector3(-size.Y / 7f, size.Y / 2f, 0.25f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);
        }

        public override void Update()
        {
            tick++;
            animationTick++;
            if (tick % 180 == 0)
            {
                switch (animation)
                {
                    case Animations.eating: animation = Animations.flying; break;
                    case Animations.flying: animation = Animations.idle; break;
                    case Animations.idle: animation = Animations.walking; break;
                    case Animations.walking: animation = Animations.eating; break;
                }
                animationTick = 0;
            }

            legLeftAnim = 0f;
            legRightAnim = 0f;
            tailAnim = 0f;
            headAnim = 0f;
            wingAnim = 0f;
            if (animation == Animations.walking)
            {
                legLeftAnim = (float)Math.Sin(MathHelper.ToRadians(animationTick * 6));
                legRightAnim = (float)-Math.Sin(MathHelper.ToRadians(animationTick * 6));
                tailAnim = (float)Math.Sin(MathHelper.ToRadians(animationTick * 3));
                headAnim = (float)-Math.Cos(MathHelper.ToRadians(animationTick * 12)) + MathHelper.PiOver4;
            }
            if (animation == Animations.eating)
            {
                headAnim = (float)-Math.Cos(MathHelper.ToRadians(animationTick * 4)) + MathHelper.PiOver4;
            }
            if (animation == Animations.flying)
            {
                headAnim = (float)-Math.Cos(MathHelper.ToRadians(animationTick * 6)) + MathHelper.PiOver4;
                tailAnim = (float)Math.Sin(MathHelper.ToRadians(animationTick * 12));
                legLeftAnim = 1f;
                legRightAnim = 1f;
                wingAnim = (float)Math.Sin(MathHelper.ToRadians(animationTick * 24));
            }

            base.Update();
        }

        Animations animation = Animations.idle;
        int tick;
        int animationTick;

        private float legLeftAnim;
        private float legRightAnim;
        private float tailAnim;
        private float headAnim;
        private Texture2D sWing;
        private Color cWing;
        private float wingAnim;
    }

    public enum Animations
    {
        idle, walking, eating, flying
    }
}
