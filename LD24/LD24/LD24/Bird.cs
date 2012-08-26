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

        public Bird(Island i, Vector3 pos)
            : base(i, new Vector2(1.5f + (float)G.r.NextDouble() * 6), pos)
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
        
        internal void SetTexturesFront(Texture2D head, Texture2D torso, Texture2D leg, Texture2D beak, Texture2D wing)
        {
            this.fHead = head;
            this.fTorso = torso;
            this.fLeg = leg;
            this.fBeak = beak;
            this.fWing = wing;
        }

        internal void SetTexturesBack(Texture2D head, Texture2D torso, Texture2D leg, Texture2D wing, Texture2D tail)
        {
            this.bHead = head;
            this.bTorso = torso;
            this.bLeg = leg;
            this.bTail = tail;
            this.bWing = wing;
        }

        public void SetColors(Color HeadColor, Color TailColor, Color TorsoColor, Color WingColor)
        {
            this.cHead = HeadColor;
            this.cTail = TailColor;
            this.cTorso = TorsoColor;
            this.cWing = WingColor;
        }

        public Texture2D repaint(Texture2D input, Color c)
        {
            Color[] data = new Color[input.Width * input.Height];
            input.GetData<Color>(data);
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].R != 0)
                {
                    data[i] = new Color(c.R, c.G, c.B, data[i].A);
                }
            }
            var result = new Texture2D(G.g.GraphicsDevice, input.Width, input.Height);
            result.SetData<Color>(data);

            return result;
        }

        public void FinalizeBird()
        {
            sHead = repaint(sHead, cHead);
            sTorso = repaint(sTorso, cTorso);
            sTail = repaint(sTail, cTail);
            sWing = repaint(sWing, cWing);

            fHead = repaint(fHead, cHead);
            fTorso = repaint(fTorso, cTorso);
            fWing = repaint(fWing, cWing);

            bHead = repaint(bHead, cHead);
            bTorso = repaint(bTorso, cTorso);
            bWing = repaint(bWing, cWing);
            bTail = repaint(bTail, cTail);
        }

        public override void Draw()
        {
            Vector3 dir = velocity;
            if (velocity.Length() == 0)
            {
                dir = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(direction));
            }
            dir.Normalize();
            Vector3 fb = Vector3.Transform(Vector3.Left, Matrix.CreateRotationY(direction));
            Vector3 playerDir = island.player.Position - position;
            playerDir.Y = 0;
            if (playerDir.Length() == 0) return;
            playerDir.Normalize();

            var leftRight = Vector3.Dot(fb, playerDir);
            var frontBack = Vector3.Dot(dir, playerDir);
            if (leftRight <= -0.6f)
                DrawRightSide();
            else if (leftRight >= 0.6f)
                DrawLeftSide();
            else if (frontBack <= -0.5f)
                DrawBackSide();
            else
                DrawFrontSide();
        }

        private void DrawFrontSide()
        {
            G.g.e.World = Matrix.CreateTranslation(0, -size.Y / 2, 0) * Matrix.CreateRotationX(legRightAnim) * Matrix.CreateTranslation(0, size.Y / 2, 0) * Matrix.CreateTranslation(new Vector3(-size.Y / 8, 0, 0)) * GetMatrixChain();
            DrawTex(fLeg);
            G.g.e.World = Matrix.CreateTranslation(0, -size.Y / 2, 0) * Matrix.CreateRotationX(legLeftAnim) * Matrix.CreateTranslation(0, size.Y / 2, 0) * Matrix.CreateTranslation(new Vector3(size.Y / 8, 0, 0)) * GetMatrixChain();
            DrawTex(fLeg);

            G.g.e.World = GetMatrixChain();
            DrawTex(fTorso);

            G.g.e.World = Matrix.CreateTranslation(-size.Y / 8, -size.Y / 1.5f, 0) * Matrix.CreateRotationZ(wingAnim) * Matrix.CreateTranslation(size.Y / 8, size.Y / 1.5f, 0) * GetMatrixChain();
            DrawTex(fWing);
            G.g.e.World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(size.Y / 8, -size.Y / 1.5f, 0) * Matrix.CreateRotationZ(-wingAnim) * Matrix.CreateTranslation(-size.Y / 8, size.Y / 1.5f, 0) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            G.g.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, reverseBillboardVertices, 0, 2);

            G.g.e.World = Matrix.CreateTranslation(0, headAnim / 8, 0) * GetMatrixChain();
            DrawTex(fHead);
            DrawTex(fBeak);
        }

        private void DrawTex(Texture2D tex)
        {
            G.g.e.Texture = tex;
            G.g.e.CurrentTechnique.Passes[0].Apply();
            G.g.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);
        }

        private void DrawBackSide()
        {
            G.g.e.World = Matrix.CreateTranslation(0, headAnim / 12, 0) * GetMatrixChain();
            DrawTex(bHead);

            G.g.e.World = Matrix.CreateTranslation(-size.Y / 8, -size.Y / 1.5f, 0) * Matrix.CreateRotationZ(wingAnim) * Matrix.CreateTranslation(size.Y / 8, size.Y / 1.5f, 0) * GetMatrixChain();
            DrawTex(bWing);
            G.g.e.World = Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(size.Y / 8, -size.Y / 1.5f, 0) * Matrix.CreateRotationZ(-wingAnim) * Matrix.CreateTranslation(-size.Y / 8, size.Y / 1.5f, 0) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            G.g.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, reverseBillboardVertices, 0, 2);

            G.g.e.World = GetMatrixChain();
            DrawTex(bTorso);

            G.g.e.World = Matrix.CreateTranslation(0, -size.Y / 2, 0) * Matrix.CreateRotationX(-legRightAnim) * Matrix.CreateTranslation(0, size.Y / 2, 0) * Matrix.CreateTranslation(new Vector3(-size.Y / 8, 0, 0)) * GetMatrixChain();
            DrawTex(bLeg);
            G.g.e.World = Matrix.CreateTranslation(0, -size.Y / 2, 0) * Matrix.CreateRotationX(-legLeftAnim) * Matrix.CreateTranslation(0, size.Y / 2, 0) * Matrix.CreateTranslation(new Vector3(size.Y / 8, 0, 0)) * GetMatrixChain();
            DrawTex(bLeg);

            G.g.e.World = Matrix.CreateTranslation(0, tailAnim / 6, 0) * GetMatrixChain();
            DrawTex(bTail);
        }

        private void DrawRightSide()
        {
            var GraphicsDevice = G.g.GraphicsDevice;
            G.g.e.World = GetMatrixChain();
            var reverse = Matrix.CreateRotationY(MathHelper.Pi);

            G.g.e.Texture = sLeg;
            G.g.e.World = reverse * Matrix.CreateTranslation(new Vector3(0, -size.Y / 3f, 0)) * Matrix.CreateRotationZ(-legLeftAnim) * Matrix.CreateTranslation(new Vector3(0, size.Y / 3f, -0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sTorso;
            G.g.e.World = reverse * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sTail;
            G.g.e.World = reverse * Matrix.CreateTranslation(new Vector3(size.Y / 4f, -size.Y / 2f, 0)) * Matrix.CreateRotationZ(tailAnim / -2) * Matrix.CreateTranslation(new Vector3(-size.Y / 4f, size.Y / 2f, 0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sHead;
            G.g.e.World = reverse * Matrix.CreateTranslation(new Vector3(-size.Y / 4f, -size.Y / 1.6f, 0)) * Matrix.CreateRotationZ(-headAnim / (animation == Animations.eating ? 0.75f : 2)) * Matrix.CreateTranslation(new Vector3(size.Y / 4f, size.Y / 1.6f, 0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sBeak;
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sLeg;
            G.g.e.World = reverse * Matrix.CreateTranslation(new Vector3(0, -size.Y / 3f, 0)) * Matrix.CreateRotationZ(-legRightAnim) * Matrix.CreateTranslation(new Vector3(0, size.Y / 3f, 0.1f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);

            G.g.e.Texture = sWing;
            G.g.e.World = reverse * Matrix.CreateTranslation(new Vector3(-size.Y / 7f, -size.Y / 2f, 0)) * Matrix.CreateRotationZ(-wingAnim * 1.5f) * Matrix.CreateTranslation(new Vector3(size.Y / 7f, size.Y / 2f, 0.25f)) * GetMatrixChain();
            G.g.e.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);
        }
        private void DrawLeftSide()
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
            if (tick > 120 && G.r.Next(20) == 0)
            {
                tick = 0;
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
                        int i = G.r.Next(4);
                        switch (i)
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
            }
            if ((position - island.player.Position).Length() < (RM.IsDown(InputAction.Run) ? 50 : 20))
            {
                animation = Animations.flying;
                flying = true;
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

        private Texture2D fHead;
        private Texture2D fTorso;
        private Texture2D fLeg;
        private Texture2D fBeak;
        private Texture2D fWing;
        private Texture2D bWing;
        private Texture2D bTail;
        private Texture2D bLeg;
        private Texture2D bTorso;
        private Texture2D bHead;

    }

    public enum Animations
    {
        idle, walking, eating, flying,
        landing
    }
}
