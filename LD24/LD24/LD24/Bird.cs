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
            animation = Animations.walking;
            
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
                if (animation == Animations.flying)
                {
                    if (G.r.Next(10) == 2)
                    {
                        animation = Animations.landing;
                    }
                }
                else if (animation != Animations.landing)
                {
                    if (G.r.Next(3) == 1 || animation == Animations.eating)
                    {
                        int i= G.r.Next(4);
                        switch(i)
                        {
                            case 0: animation = Animations.idle; break;
                            case 1: animation = Animations.eating; break;
                            case 2: animation = Animations.walking; direction = G.r.Next(360); break;
                            case 3: animation = Animations.flying; flying = true; direction = G.r.Next(360); break;                            
                        }
                    }
                }
                animationTick = 0;
            }

            if (animation == Animations.landing && Math.Abs(position.Y - island.CheckHeightCollision(position)) < 1f)
            {
                flying = false;
                animation = Animations.idle;
            }

            if (flying)
            {
                if (animation == Animations.flying)
                {
                    if (position.Y < 64)
                        velocity.Y = 0.2f + (float)(G.r.NextDouble() / 2f);
                    else if (position.Y < 1024)
                        velocity.Y = 0.2f + (float)(G.r.NextDouble());
                    else
                        velocity.Y = 0f;
                }
                else if (animation == Animations.landing)
                {
                    velocity.Y = -0.5f;
                }
            }

            if (animation == Animations.flying || animation == Animations.walking || animation == Animations.landing)
            {
                direction += MathHelper.ToRadians(G.r.Next(11) - 5);
                
                Matrix cameraRotation = Matrix.CreateRotationY(direction);
                Vector3 rotatedVector = Vector3.Transform(Vector3.Forward, cameraRotation);
                if (rotatedVector.Length() > 0)
                {
                    rotatedVector.Normalize();
                    if (animation != Animations.flying)
                    {
                        rotatedVector /= 3f;
                    }
                    velocity.X = rotatedVector.X;
                    velocity.Z = rotatedVector.Z;
                }
            }
            else
            {
                velocity.X = 0;
                velocity.Z = 0;

                if ((position - island.player.Position).Length() < 12)
                {
                    if (G.r.Next(3) == 0)
                        animation = Animations.flying;
                    else
                        animation = Animations.walking;
                }
            }

            if (position.X < 0 || position.Z < 0 || position.X > 512 * island.scaleHorizontal || position.Z > 512 * island.scaleVertical)
            {
                var d = new Vector3(256 * island.scaleHorizontal, 0, 256 * island.scaleVertical) - position;
                d.Normalize();
                direction = (float)Math.Atan2(d.X, d.Z) + MathHelper.PiOver2;
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
            if (animation == Animations.landing)
            {
                legLeftAnim = 1f;
                legRightAnim = 1f;
            }

            position += velocity;
            if (!flying)
                position.Y = island.CheckHeightCollision(position);

            if (position.Y < 8.5f && !flying)
            {
                animation = Animations.flying;
                flying = true;

                island.AddEntity(new SplashEffect(island, position));
            }
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
        private float direction = MathHelper.ToRadians(G.r.Next(360));
    }

    public enum Animations
    {
        idle, walking, eating, flying,
        landing
    }
}
