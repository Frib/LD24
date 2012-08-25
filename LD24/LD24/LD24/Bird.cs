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

        public Bird(Island i, Vector3 pos) : base(i, new Vector2(3, 3), pos)
        {

        }

        public void SetTexturesSide(Texture2D head, Texture2D torso, Texture2D tail, Texture2D leg, Texture2D beak)
        {
            this.sHead = head;
            this.sTorso = torso;
            this.sTail = tail;
            this.sLeg = leg;
            this.sBeak = beak;
        }

        public void SetColors(Color HeadColor, Color TailColor, Color TorsoColor)
        {
            this.cHead = HeadColor;
            this.cTail = TailColor;
            this.cTorso = TorsoColor;
        }

        public void Finalize()
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

        }

        public override void Draw()
        {
            var GraphicsDevice = G.g.GraphicsDevice;
            G.g.e.World = GetMatrixChain();

            G.g.e.Texture = sLeg;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(0, -1f, 0)) * Matrix.CreateRotationZ(legLeftAnim) * Matrix.CreateTranslation(new Vector3(0, 1f, -0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sTorso;
            G.g.e.World = GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sTail;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(-0.75f, -1.5f, 0)) * Matrix.CreateRotationZ(tailAnim / 2) * Matrix.CreateTranslation(new Vector3(0.75f, 1.5f, 0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sHead;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(0.75f, -1.75f, 0)) * Matrix.CreateRotationZ(headAnim / 2) * Matrix.CreateTranslation(new Vector3(-0.75f, 1.75f, 0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sBeak;
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sLeg;
            G.g.e.World = Matrix.CreateTranslation(new Vector3(0, -1f, 0)) * Matrix.CreateRotationZ(legRightAnim) * Matrix.CreateTranslation(new Vector3(0, 1f, 0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);
        }

        public override void Update()
        {
            tick++;
            animationTick++;
            if (tick % 180 == 0)
            {
                if (animation == Animations.idle)
                    animation = Animations.walking;
                else
                    animation = Animations.idle;
                animationTick = 0;
            }

            if (animation == Animations.idle)
            {
                legLeftAnim = 0f;
                legRightAnim = 0f;
            }
            if (animation == Animations.walking)
            {
                legLeftAnim = (float)Math.Sin(MathHelper.ToRadians(animationTick * 6));
                legRightAnim = (float)-Math.Sin(MathHelper.ToRadians(animationTick * 6));
                tailAnim = (float)Math.Sin(MathHelper.ToRadians(animationTick * 3));
                headAnim = (float)-Math.Cos(MathHelper.ToRadians(animationTick * 12)) + MathHelper.PiOver4;
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
    }

    public enum Animations
    {
        idle, walking, eating, flying
    }
}
