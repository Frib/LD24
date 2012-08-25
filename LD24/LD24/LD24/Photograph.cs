using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LD24
{
    class Photograph
    {
        public Texture2D Photo { get;set;}

        public Photograph(Texture2D photo)
        {
            this.Photo = photo;
        }
    }
}
