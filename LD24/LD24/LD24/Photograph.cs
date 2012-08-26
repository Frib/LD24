using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace LD24
{
    class Photograph
    {
        public Texture2D Photo { get; set; }

        public Photograph(Texture2D photo)
        {
            this.Photo = photo;
        }

        public Bird Bird { get; set; }
        public Animations animation { get; set; }

        public override string ToString()
        {
            if (Bird == null)
                return "Invalid photo. Blame Frib if you see a bird :(";
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Head: " + Bird.cHead);
            sb.AppendLine("Torso: " + Bird.cTorso);
            sb.AppendLine("Wings: " + Bird.cWing);
            sb.AppendLine("Tail: " + Bird.cTail);
            sb.AppendLine("Activity: " + animation);
            sb.AppendLine("Water splash: " + Splash);
            sb.AppendLine("Distance: " + Distance);
            sb.AppendLine("Heading: " + Heading);
            sb.AppendLine();
            sb.AppendLine("Score: " + CalculateScore());
            return sb.ToString();
        }

        public float Distance { get; set; }

        public bool Splash { get; set; }

        public int CalculateScore()
        {
            int score = 100;
            if (Bird == null)
                return 0;
            switch (animation)
            {
                case Animations.idle: score *= 2; break;
                case Animations.walking: score *= 3; break;
                case Animations.eating: score *= 4; break;
                case Animations.flying: score *= 2; break;
                case Animations.landing: score *= 1; break;
                default: break;
            }
            switch (Heading)
            {
                case LD24.Heading.Front: score *= 3; break;
                case LD24.Heading.Side: score *= 2; break;
                case LD24.Heading.Back: score *= 1; break;
            }

            if (Bird.cTorso != Color.White)
            {
                score *= 2;
            }
            if (Bird.cHead != Color.White)
            {
                if (Bird.cHead != Bird.cTorso)
                    score *= 3;
                score *= 2;
            }
            if (Bird.cTail != Color.White)
            {
                if (Bird.cTail != Bird.cTorso)
                    score *= 3;
                score *= 2;
            }
            if (Bird.cWing != Color.White)
            {
                if (Bird.cWing != Bird.cTorso)
                    score *= 3;
                score *= 2;
            }

            if (Splash)
            {
                score *= 3;
            }
            score *= (120 - Zoom) / 10;
            score /= (int)Distance;

            return score;
        }

        public Heading Heading { get; set; }

        public int Zoom { get; set; }
        public bool Saved { get; set; }

        internal void Save()
        {
            try
            {
                var now = DateTime.Now;
                string date = now.Month + "-" + now.Day + "_" + now.Hour + "-" + now.Minute + "-" + now.Second + "-" + G.r.Next(10000);
                if (!Directory.Exists("Photographs"))
                {
                    Directory.CreateDirectory("Photographs");
                }
                using (FileStream fs = new FileStream(@"Photographs\shot_" + date + ".png", FileMode.CreateNew))
                {
                    Photo.SaveAsPng(fs, Photo.Width, Photo.Height);
                }

                Saved = true;
            }
            catch
            {

            }
        }
    }
}
