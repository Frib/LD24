﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD24
{
    class Camera
    {
        public Vector3 position = new Vector3(0, 0, -20);
        public float leftRightRot = MathHelper.PiOver2 * -1.75f;
        public float upDownRot = MathHelper.PiOver2 * -0.10f;

        public void Update()
        {
            float mouseTime = 0.016f;

            float xDifference = IM.MouseDelta.X;
            float yDifference = IM.MouseDelta.Y;
            
            leftRightRot -= xDifference * mouseTime;
            upDownRot -= yDifference * mouseTime;

            if (leftRightRot > MathHelper.TwoPi)
                leftRightRot -= MathHelper.TwoPi;
            else if (leftRightRot < MathHelper.TwoPi * -1)
                leftRightRot += MathHelper.TwoPi;

            if (upDownRot > 1.40f)
                upDownRot = 1.40f;
            else if (upDownRot < -1.34f)
                upDownRot = -1.34f;

            if (IM.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                position += Vector3.Normalize(GetCameraDirection());
            }
        }

        public void Apply(BasicEffect e)
        {
            e.World = Matrix.Identity;
            Matrix cameraRotation = Matrix.CreateRotationX(upDownRot) * Matrix.CreateRotationY(leftRightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = position + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            e.View = Matrix.CreateLookAt(position, cameraFinalTarget, cameraRotatedUpVector);
            e.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), (float)G.g.Window.ClientBounds.Width / (float)G.g.Window.ClientBounds.Height, 0.1f, 10000f);
        }

        public Vector3 GetCameraDirection()
        {
            return new Vector3(-(float)Math.Sin(leftRightRot), (float)upDownRot, -(float)Math.Cos(leftRightRot));
        }
    }
}
