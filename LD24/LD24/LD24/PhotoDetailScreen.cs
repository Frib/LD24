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
            if (RM.IsPressed(InputAction.Fire) || RM.IsPressed(InputAction.Back) || RM.IsPressed(InputAction.AltFire))
            {
                g.Showscreen(photoAlbum);
            }
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(new Color(48,48,48));
            spriteBatch.Begin();
            spriteBatch.Draw(photograph.Photo, new Vector2(80, 60), Color.White);
            spriteBatch.End();
        }
    }
}
