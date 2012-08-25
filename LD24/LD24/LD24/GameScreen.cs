using System;
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
        
        public GameScreen(G g)
        {
            camera = new Camera();
            island = new Island();
        }

        public override void Update()
        {
            if (RM.IsPressed(InputAction.Back))
            {
                g.Showscreen(new PhotoAlbum(this));
            }

            camera.Update();
            island.Update();

           // camera.position.Y = island.CheckHeightCollision(camera.position);
        }

        public override void Draw()
        {
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true, DepthBufferWriteEnable = true };
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            camera.Apply(e);

            foreach (var p in e.CurrentTechnique.Passes)
            {
                p.Apply();
            }

            island.Draw();

            spriteBatch.Begin();
            if (RM.IsDown(InputAction.AltFire) && !RM.IsPressed(InputAction.Fire))
            {
                spriteBatch.Draw(RM.GetTexture("cameraoverlay"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            }
            spriteBatch.End();
        }

        public override void Show()
        {
            IM.SnapToCenter = true;
            g.IsMouseVisible = false;
            base.Show();
        }
    }
}
