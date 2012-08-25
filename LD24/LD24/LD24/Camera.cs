using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD24
{
    class Camera
    {
        public static Camera c;

        public Vector3 position = new Vector3(128, 64, 128);
        public float leftRightRot = MathHelper.PiOver2 * -1.75f;
        public float upDownRot = MathHelper.PiOver2 * -0.30f;
        private int cameraZoom = 80;

        public Camera()
        {
            c = this;
        }

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

            if (RM.IsDown(InputAction.AltFire))
            {
                if (RM.IsDown(InputAction.ZoomIn) || IM.ScrollDelta > 0)
                {
                    cameraZoom--;
                    if (cameraZoom < 10)
                    {
                        cameraZoom = 10;
                    }
                }
                if (RM.IsDown(InputAction.ZoomOut) || IM.ScrollDelta < 0)
                {
                    cameraZoom++;
                    if (cameraZoom > 110)
                    {
                        cameraZoom = 110;
                    }
                }
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
            e.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(RM.IsDown(InputAction.AltFire) ? cameraZoom : 80), (float)G.g.Window.ClientBounds.Width / (float)G.g.Window.ClientBounds.Height, 0.2f, 10000f);
        }

        public Vector3 GetCameraDirection()
        {
            return new Vector3(-(float)Math.Sin(leftRightRot), (float)upDownRot, -(float)Math.Cos(leftRightRot));
        }
    }
}
