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

        public GameScreen(G g) : base(g)
        {

        }

        public override void Update()
        {

        }

        public override void Draw()
        {
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            e.World = Matrix.Identity;
            e.View = Matrix.CreateLookAt(new Vector3(0, 0, -2), new Vector3(0, 0, 2), Vector3.Up);
            e.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90), 16f / 9f, 0.1f, 10000f);
            e.VertexColorEnabled = true;

            foreach (var p in e.CurrentTechnique.Passes)
            {
                p.Apply();
            }

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, new[] { 
            new VertexPositionColor(new Vector3(0, 0, 0), Color.Red),
            new VertexPositionColor(new Vector3(1, 0, 0), Color.Green),
            new VertexPositionColor(new Vector3(1, 1, 0), Color.Blue)
            }, 0, 1);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "test", Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
