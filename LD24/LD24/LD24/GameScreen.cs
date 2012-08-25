﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LD24
{
    class GameScreen : Screen
    {
        private Camera camera;
        private Island island;
        
        public GameScreen(G g) : base(g)
        {
            camera = new Camera();
            island = new Island();
        }

        public override void Update()
        {
            camera.Update();
            island.Update();

           // camera.position.Y = island.CheckHeightCollision(camera.position);
        }

        public override void Draw()
        {
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            camera.Apply(e);

            foreach (var p in e.CurrentTechnique.Passes)
            {
                p.Apply();
            }

            island.Draw();


            
            spriteBatch.Begin();
            if (RM.IsDown(InputAction.AltFire))
            {
                spriteBatch.Draw(RM.GetTexture("cameraoverlay"), new Rectangle(0, 0, G.Width, G.Height), Color.White);
            }
            spriteBatch.DrawString(font, "test", Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public override void Show()
        {
            IM.SnapToCenter = true;
            
            base.Show();
        }
    }
}
