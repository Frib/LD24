using System;
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
        Texture2D sWing;
        List<Texture2D> sBeaks = new List<Texture2D>();
        
        Texture2D fHead;
        Texture2D fLeg;
        Texture2D fTorso;
        Texture2D fWing;
        List<Texture2D> fBeaks = new List<Texture2D>();
        private Texture2D bWing;
        private Texture2D bTorso;
        private Texture2D bTail;
        private Texture2D bLeg;
        private Texture2D bHead;
        
        internal void LoadContent(ContentManager Content)
        {
            sHead = Content.Load<Texture2D>("Bird/Side/Head");
            sLeg = Content.Load<Texture2D>("Bird/Side/Leg");
            sTail = Content.Load<Texture2D>("Bird/Side/Tail");
            sTorso = Content.Load<Texture2D>("Bird/Side/Torso");
            sWing = Content.Load<Texture2D>("Bird/Side/Wing");
            for (int i = 1; i < 6; i++)
                sBeaks.Add(Content.Load<Texture2D>("Bird/Side/Beak" + i));

            fHead = Content.Load<Texture2D>("Bird/Front/Head");
            fLeg = Content.Load<Texture2D>("Bird/Front/Leg");
            fTorso = Content.Load<Texture2D>("Bird/Front/Torso");
            fWing = Content.Load<Texture2D>("Bird/Front/Wing");
            for (int i = 1; i < 2; i++)
                fBeaks.Add(Content.Load<Texture2D>("Bird/Front/Beak" + i));

            bHead = Content.Load<Texture2D>("Bird/Back/Head");
            bLeg = Content.Load<Texture2D>("Bird/Back/Leg");
            bTail = Content.Load<Texture2D>("Bird/Back/Tail");
            bTorso = Content.Load<Texture2D>("Bird/Back/Torso");
            bWing = Content.Load<Texture2D>("Bird/Back/Wing");
        }

        public Bird CreateBird(Island i, Vector3 pos)
        {
            Bird b = new Bird(i, pos);
            b.SetTexturesSide(sHead, sTorso, sTail, sLeg, sBeaks.Random(), sWing);
            b.SetTexturesFront(fHead, fTorso, fLeg, fBeaks.Random(), fWing);
            b.SetTexturesBack(bHead, bTorso, bLeg, bWing, bTail);
            Color baseColor = GenColor(Color.White);
            b.SetColors(GenColor(baseColor), GenColor(baseColor), baseColor, GenColor(baseColor));
            b.FinalizeBird();
            return b;
        }

        private Color GenColor(Color color)
        {
            var r = G.r;
            if (r.Next(2) == 1) return color;
            int i = r.Next(30);
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
