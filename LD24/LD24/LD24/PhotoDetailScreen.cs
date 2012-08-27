using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LD24
{
    class PhotoDetailScreen : Screen
    {
        private Photograph photograph;
        private PhotoAlbum photoAlbum;

        public PhotoDetailScreen(Photograph photograph, PhotoAlbum photoAlbum)
        {
            this.photograph = photograph;
            this.photoAlbum = photoAlbum;
        }
        public override void Update()
        {
            if (RM.IsPressed(InputAction.Back) || RM.IsPressed(InputAction.AltFire) || RM.IsPressed(InputAction.ShowAlbum))
            {
                g.Showscreen(photoAlbum);
            }
            if (!photograph.Saved && RM.IsPressed(InputAction.Fire) && new Rectangle((G.Width / 2) - 64, G.Height - 24, 120, 30).Intersects(new Rectangle((int)IM.MousePos.X, (int)IM.MousePos.Y, 1, 1)))
            {
                photograph.Save();
            }
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(new Color(48,48,48));
            spriteBatch.Begin();
            spriteBatch.Draw(photograph.Photo, new Rectangle(32, 32, G.Width - 64, G.Height - 64), Color.White);
            if (!photograph.Saved)
            {
                spriteBatch.DrawString(g.font, "Save to disk", new Vector2((G.Width / 2) - 64, G.Height - 24), Color.Yellow);
            }
            spriteBatch.End();
        }
    }
}
