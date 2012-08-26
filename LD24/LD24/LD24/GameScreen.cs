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
        private int timer;
        private int HiScore;
        
        public GameScreen(G g)
        {
            camera = new Camera();
            island = new Island();
            CanTakePhoto = true;
        }

        public override void Update()
        {
            if (timer > 0)
            {
                timer--;
                if (timer == 0)
                    CanTakePhoto = true;
            }
            if (RM.IsPressed(InputAction.ShowAlbum))
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
            spriteBatch.DrawString(g.font, HiScore.ToString(), new Vector2(16, 16), Color.Yellow);
            if (RM.IsDown(InputAction.AltFire))
            {
                if (RM.IsPressed(InputAction.Fire) && CanTakePhoto)
                {

                }
                else
                {
                    spriteBatch.Draw(RM.GetTexture("cameraoverlay"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                }
            }
            spriteBatch.End();
        }

        public override void Show()
        {
            IM.SnapToCenter = true;
            g.IsMouseVisible = false;
            base.Show();
        }

        internal override void AddPhotoData(Photograph pg)
        {
            CanTakePhoto = false;
            RM.PlaySound("snap");
            timer = 60;

            BoundingFrustum bf = new BoundingFrustum(camera.View * camera.ZoomProjection);
            var entities = island.GetEntitiesInView(bf);
            Console.WriteLine(entities.Count());
            var bird = entities.OfType<Bird>().OrderBy(x => (x.Position - island.player.Position).Length()).FirstOrDefault();
            if (bird != null)
            {
                pg.Bird = bird;
                pg.animation = bird.animation;
                pg.Distance = (bird.Position - island.player.Position).Length();
                pg.Splash = entities.OfType<SplashEffect>().Any();
                pg.Heading = bird.GetHeading();
                pg.Zoom = camera.cameraZoom;
            }

            if (pg.CalculateScore() > HiScore)
            {
                HiScore = pg.CalculateScore();
            }
            Console.Write(pg.ToString());

        }
    }
}
