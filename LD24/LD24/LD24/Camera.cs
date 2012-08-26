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
        public int cameraZoom = 80;

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
            cameraFinalTarget = position + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            e.View = View;
            e.Projection = Projection;
        }

        public Vector3 GetCameraDirection()
        {
            return new Vector3(-(float)Math.Sin(leftRightRot), (float)upDownRot, -(float)Math.Cos(leftRightRot));
        }

        private Vector3 cameraFinalTarget;
        private Vector3 cameraRotatedUpVector;

        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(position, cameraFinalTarget, cameraRotatedUpVector);
            }
        }

        public Matrix Projection
        {
            get { return Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(RM.IsDown(InputAction.AltFire) ? cameraZoom : 80), (float)G.g.Window.ClientBounds.Width / (float)G.g.Window.ClientBounds.Height, 0.2f, 10000f); }
        }

        public Matrix ZoomProjection
        {
             get { return Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(cameraZoom/2.5f), (float)G.g.Window.ClientBounds.Width / (float)G.g.Window.ClientBounds.Height, 0.2f, 10000f); }
        }
    }
}
