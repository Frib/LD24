using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LD24
{
    class PhotoInfoScreen : Screen
    {
        private Photograph photograph;
        private PhotoAlbum photoAlbum;
        private string name;
        bool enteringName = false;

        public PhotoInfoScreen(Photograph photograph, PhotoAlbum photoAlbum)
        {
            this.photograph = photograph;
            this.photoAlbum = photoAlbum;
            name = Encyclopedia.GetName(photograph.Bird);
        }
        public override void Update()
        {
            if (!enteringName)
            {
                if (RM.IsPressed(InputAction.Back) || RM.IsPressed(InputAction.AltFire) || RM.IsPressed(InputAction.ShowAlbum))
                {
                    g.Showscreen(photoAlbum);
                }

                if (photograph.Bird != null && string.IsNullOrEmpty(name) && RM.IsPressed(InputAction.Fire) && new Rectangle(96, 324, 276, 24).Intersects(new Rectangle((int)IM.MousePos.X, (int)IM.MousePos.Y, 1, 1)))
                {
                    enteringName = true;
                }
            }
            else
            {
                foreach (var b in IM.GetPressedKeys())
                {
                    var letter = (b.ToString().Split('.').Last()).ToLower();
                    if (IM.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) || IM.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                    {
                        letter = letter.ToUpper();
                    }
                    if (letter.Length == 1)
                    {
                        char c = char.Parse(letter);
                        if (char.IsLetterOrDigit(c))
                        {
                            name += c;
                        }
                    }
                    if (b == Microsoft.Xna.Framework.Input.Keys.Space)
                    {
                        name += " ";
                    }
                    if (b == Microsoft.Xna.Framework.Input.Keys.Back)
                    {
                        name = new string(name.Take(name.Length - 1).ToArray());
                    }
                }

                if (IM.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter) || IM.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    enteringName = false;
                    Encyclopedia.NameBird(photograph.Bird, name);
                }
            }
        }

        public override void Draw()
        {
            var offset = 64;

            GraphicsDevice.Clear(new Color(48, 48, 48));
            spriteBatch.Begin();
            spriteBatch.Draw(photograph.Photo, new Rectangle(80, 60, 320, 240), Color.White);

            spriteBatch.DrawString(g.font, "Score: " + photograph.CalculateScore(), new Vector2(416, offset), Color.Yellow); offset += 24;
            spriteBatch.DrawString(g.font, "", new Vector2(416, offset), Color.Yellow); offset += 24;
            if (photograph.Bird != null)
            {
                spriteBatch.DrawString(g.font, "A" + photograph.animation.ToDescription() + " bird", new Vector2(416, offset), Color.Yellow); offset += 24;
                if (photograph.Splash)
                {
                    spriteBatch.DrawString(g.font, "fishing in the water", new Vector2(416, offset), Color.Yellow); offset += 24;
                }
                spriteBatch.DrawString(g.font, "", new Vector2(416, offset), Color.Yellow); offset += 24;
                spriteBatch.DrawString(g.font, "Taken from the " + photograph.Heading.ToDescription() + " " + (int)photograph.Distance + "m away", new Vector2(416, offset), Color.Yellow); offset += 24;
                spriteBatch.DrawString(g.font, "with a zoom level of " + (130 - photograph.Zoom), new Vector2(416, offset), Color.Yellow); offset += 24;
                spriteBatch.DrawString(g.font, "", new Vector2(416, offset), Color.Yellow); offset += 24;
                spriteBatch.DrawString(g.font, "The bird is " + photograph.Bird.cTorso.ToDescription(), new Vector2(416, offset), Color.Yellow); offset += 24;
                if (photograph.Bird.cHead != photograph.Bird.cTorso)
                {
                    spriteBatch.DrawString(g.font, "Its head is colored " + photograph.Bird.cHead.ToDescription(), new Vector2(416, offset), Color.Yellow); offset += 24;
                }
                if (photograph.Bird.cWing != photograph.Bird.cTorso)
                {
                    spriteBatch.DrawString(g.font, "It has " + photograph.Bird.cWing.ToDescription() + " wings", new Vector2(416, offset), Color.Yellow); offset += 24;
                }
                if (photograph.Bird.cTail != photograph.Bird.cTorso)
                {
                    spriteBatch.DrawString(g.font, "and it has a " + photograph.Bird.cTail.ToDescription() + " tail", new Vector2(416, offset), Color.Yellow); offset += 24;
                }

                string beakName = "big";
                if (photograph.Bird.BeakType == 1)
                {
                    beakName = "small";
                }
                if (photograph.Bird.BeakType == 2)
                {
                    beakName = "hooked";
                }
                if (photograph.Bird.BeakType == 3)
                {
                    beakName = "sharp";
                }
                if (photograph.Bird.BeakType == 4)
                {
                    beakName = "wide";
                }
                offset += 24;
                spriteBatch.DrawString(g.font, "It has a " + beakName + " beak", new Vector2(416, offset), Color.Yellow); offset += 24;


                if (!string.IsNullOrEmpty(name))
                {
                    spriteBatch.DrawString(g.font, "\'" + name + "\'", new Vector2(128, 324), enteringName ? Color.Green : Color.Yellow);
                }
                else
                {
                    spriteBatch.DrawString(g.font, "Click here to assign name", new Vector2(96, 324), enteringName ? Color.Green : Color.White);
                }
            }
            else
            {
                spriteBatch.DrawString(g.font, "A scenic shot", new Vector2(416, offset), Color.Yellow); offset += 24;
                spriteBatch.DrawString(g.font, "Taken with a zoom level of " + (130 - photograph.Zoom), new Vector2(416, offset), Color.Yellow); offset += 24;
            }

            spriteBatch.End();
        }
    }
}
