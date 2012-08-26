using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LD24
{
    class Sun : Entity
    {
        Vector3 offset;
        float degrees = 0;
        private int tick;

        public Sun(Island island, Vector2 orbit)
            : base(island, new Vector2(4096, 4096), new Vector3(orbit.X, 5120, orbit.Y))
        {
            this.island = island;
            textureFront = RM.GetTexture("sun");
            offset = new Vector3(-orbit.X * 5, 0, -orbit.Y * 5);
        }

        public override void Update()
        {
            degrees += 0.04f;
        }

        public Vector3 GetDirection()
        {
            Vector3 result = -1 * Vector3.Normalize(Vector3.Transform(Vector3.Zero, Matrix.CreateTranslation(offset) * Matrix.CreateRotationY(MathHelper.ToRadians(degrees)) * Matrix.CreateTranslation(Position)) - new Vector3(position.X, 0, position.Z));
            return result;
        }

        protected override Matrix GetMatrixChain()
        {
            return Matrix.CreateRotationX(MathHelper.PiOver4) * Matrix.CreateRotationY(MathHelper.PiOver4) * Matrix.CreateTranslation(offset) * Matrix.CreateRotationY(MathHelper.ToRadians(degrees)) * Matrix.CreateTranslation(Position);
        }
    }
}
