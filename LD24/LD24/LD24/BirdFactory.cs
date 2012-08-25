﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LD24
{
    class BirdFactory
    {
        Texture2D sHead;
        Texture2D sLeg;
        Texture2D sTail;
        Texture2D sTorso;
        List<Texture2D> beaks = new List<Texture2D>();
        
        internal void LoadContent(ContentManager Content)
        {
            sHead = Content.Load<Texture2D>("Bird/Side/Head");
            sLeg = Content.Load<Texture2D>("Bird/Side/Leg");
            sTail = Content.Load<Texture2D>("Bird/Side/Tail");
            sTorso = Content.Load<Texture2D>("Bird/Side/Torso");
            for (int i = 1; i < 6; i++)
                beaks.Add(Content.Load<Texture2D>("Bird/Side/Beak" + i));
        }

        public Bird CreateBird(Island i, Vector3 pos)
        {
            Bird b = new Bird(i, pos);
            b.SetTexturesSide(sHead, sTorso, sTail, sLeg, beaks.First());
            Color baseColor = GenColor(Color.White);
            b.SetColors(GenColor(baseColor), GenColor(baseColor), baseColor);
            b.Finalize();
            return b;
        }

        private Color GenColor(Color color)
        {
            var r = G.r;
            if (r.Next(2) == 1) return color;
            int i = r.Next(20);
            switch (i)
            {
                case 0: 
                case 1:
                case 2:
                case 3:
                case 4: return Color.White;
                case 5: 
                case 6: return Color.LightBlue;
                case 7:
                case 8: return Color.Black;
                case 9: return Color.Yellow;
                case 10:
                case 11: return Color.LightGray;
                case 12: return Color.Salmon;
                case 13: return Color.LightGreen;
                case 14:
                case 15: return Color.Brown;
                case 16: return Color.Fuchsia;
                case 17: return Color.Red;
                case 18: return Color.Gold;
                case 19: return Color.DarkGray;
                default: return Color.White;
            }
        }
    }
}
