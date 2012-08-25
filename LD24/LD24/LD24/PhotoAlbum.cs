﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LD24
{
    class PhotoAlbum : Screen
    {
        private GameScreen gameScreen;
        int offset = 0;
        public PhotoAlbum(GameScreen gameScreen)
        {
            this.gameScreen = gameScreen;
        }
        public override void Update()
        {
            if (RM.IsPressed(InputAction.Back))
            {
                g.Showscreen(gameScreen);
            }

            if (RM.IsDown(InputAction.Left))
                offset += 10;
            if (RM.IsDown(InputAction.Right))
                offset -= 10;

            if (RM.IsPressed(InputAction.Fire))
            {
                if (IM.MousePos.Y > 160 && IM.MousePos.Y < 400)
                {
                    int xPos = ((int)IM.MousePos.X - offset) - 64;
                    if (xPos % 330 < 320)
                    {
                        int pic = (int)Math.Floor((float)(xPos / 330));
                        if (pic >= 0 && pic < g.photos.Count)
                        {
                            g.Showscreen(new PhotoDetailScreen(g.photos[pic], this));
                        }
                    }
                }
            }
        }

        public override void Show()
        {
            IM.SnapToCenter = false;
            g.IsMouseVisible = true;
            base.Show();
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(new Color(48,48,48));
            
            spriteBatch.Begin();
            for (int i = 0; i < g.photos.Count; i++)
            {
                spriteBatch.Draw(g.photos[i].Photo, new Rectangle(64 + (i * 330) + offset, 160, 320, 240), Color.White);
            }
            spriteBatch.End();
        }
    }
}
