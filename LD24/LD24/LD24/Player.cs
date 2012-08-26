using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LD24
{
    class Player : Entity
    {
        public Player(Island i, Vector2 size, Vector3 pos)
            : base(i,size, pos)
        {
            eyes = new Vector3(0, size.Y * 0.8125f, 0);
            textureFront = RM.GetTexture("player");
        }

        public override void Update()
        {
            Vector3 moveVector = Vector3.Zero;

            if (RM.IsDown(InputAction.Left))
                moveVector += Vector3.Left;
            if (RM.IsDown(InputAction.Right))
                moveVector += Vector3.Right;
            if (RM.IsDown(InputAction.Up))
                moveVector += Vector3.Forward;
            if (RM.IsDown(InputAction.Down))
                moveVector += Vector3.Backward;
            
            MovePlayer(moveVector);

            var newYa = island.CheckHeightCollision(position + new Vector3(velocity.X, 0, 0));
            var newYb = island.CheckHeightCollision(position + new Vector3(0, 0, velocity.Z));
            float level = island.waterheight - 2;
            if (newYa < level)
                velocity.X = 0;
            if (newYb < level)
                velocity.Z = 0;

            base.Update();

            rotation = Matrix.CreateRotationX(Camera.c.upDownRot) * Matrix.CreateRotationY(Camera.c.leftRightRot);
            Camera.c.position = this.position + eyes;
            velocity = Vector3.Zero;

        }

        public override Vector3 getLookAt()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(Camera.c.upDownRot) * Matrix.CreateRotationY(Camera.c.leftRightRot);
            Vector3 rotatedVector = Vector3.Transform(Vector3.Forward, cameraRotation);
            rotatedVector.Normalize();
            return rotatedVector;
        }

        protected override Matrix GetMatrixChain()
        {
            return base.GetMatrixChain() * Matrix.CreateTranslation(-getLookAt());
        }

        private void MovePlayer(Vector3 vector)
        {
            Matrix cameraRotation = Matrix.CreateRotationY(Camera.c.leftRightRot);
            Vector3 rotatedVector = Vector3.Transform(vector, cameraRotation);
            if (rotatedVector.Length() > 0)
            {
                rotatedVector.Normalize();
                if (RM.IsDown(InputAction.Run))
                {
                    rotatedVector *= 4;
                }
                else
                {
                    rotatedVector /= 4;
                }
                velocity.X = rotatedVector.X;
                velocity.Z = rotatedVector.Z;
            }
        }
    }
}
