using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD24
{
    class Entity
    {
        public Matrix Rotation { get { return rotation; } }
        protected Matrix rotation = Matrix.Identity;

        public Vector3 eyes = new Vector3(0, 1.5f, 0);
        protected Vector3 position;
        public Vector3 Position { get { return position; } set { position = value; } }
        public Vector3 velocity;

        public Texture2D textureFront;

        public Vector3 Center { get { return new Vector3(position.X, position.Y + (size.Y / 2), position.Z); } }

        protected Vector3 size;

        public Entity(Island i, Vector2 size, Vector3 pos)
        {
            this.island = i;
            billboardVertices = new VertexPositionNormalTexture[6];
            Vector3 e = new Vector3(-size.X / 2, size.Y, 0);
            Vector3 f = new Vector3(size.X / 2, size.Y, 0);
            Vector3 g = new Vector3(size.X / 2, 0, 0);
            Vector3 h = new Vector3(-size.X / 2, 0, 0);

            this.size = new Vector3(size.X, size.Y, size.X);

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f);
            billboardVertices[0] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            billboardVertices[1] = new VertexPositionNormalTexture(e, frontNormal, new Vector2(0, 0));
            billboardVertices[2] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            billboardVertices[3] = new VertexPositionNormalTexture(h, frontNormal, new Vector2(0, 1));
            billboardVertices[4] = new VertexPositionNormalTexture(f, frontNormal, new Vector2(1, 0));
            billboardVertices[5] = new VertexPositionNormalTexture(g, frontNormal, new Vector2(1, 1));
            this.Position = pos;
        }


        public virtual void Update()
        {
            var expectedPos = Position + velocity;

            if (island.WorldCollision(GetRectF(expectedPos)).Any())
            {
                var posX = Position + new Vector3(velocity.X, 0, 0);
                var posZ = Position + new Vector3(0, 0, velocity.Z);
                if (island.WorldCollision(GetRectF(posX)).Any())
                {
                    velocity.X = 0;
                }
                if (island.WorldCollision(GetRectF(posZ)).Any())
                {
                    velocity.Z = 0;
                }

            }

            position += velocity;
            if (!flying)
            position.Y = island.CheckHeightCollision(position);
        }

        protected virtual void ProcessCollision(Entity e)
        {

        }

        public virtual void Draw()
        {
            var GraphicsDevice = G.g.GraphicsDevice;
            G.g.e.World = GetMatrixChain();
            G.g.e.Texture = textureFront;
            G.g.e.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, billboardVertices, 0, 2);
        }

        protected VertexPositionNormalTexture[] billboardVertices;
        protected Island island;
        protected bool flying;
        
        protected virtual Matrix GetMatrixChain()
        {
            return Matrix.CreateRotationY(Camera.c.leftRightRot) * Matrix.CreateTranslation(Position);
        }

        public virtual Vector3 getLookAt()
        {
            return Vector3.Forward;
        }
        
        protected RectangleF GetRectF(Vector3 pos)
        {
            return new RectangleF(new Vector2(pos.X - (size.X / 2), pos.Z - (size.Z / 2)), new Vector2(pos.X + (size.X / 2), pos.Z + (size.Z / 2)));
        }
    }
}
