using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace LD24
{
    static class Encyclopedia
    {
        private static List<Bird> birds = new List<Bird>();

        static Encyclopedia()
        {
            try
            {
                var lines = File.ReadAllLines("encyclopedia.txt");
                foreach (var line in lines)
                {
                    try
                    {
                        var s = line.Split(':');
                        var head = ToColor(s[0]);
                        var tail = ToColor(s[1]);
                        var torso = ToColor(s[2]);
                        var wing = ToColor(s[3]);
                        var beak = int.Parse(s[4]);
                        var name = s.Length == 6 ? s[5] : "";

                        Bird b = new Bird(null, Vector3.Zero);
                        b.cHead = head;
                        b.cTail = tail;
                        b.cTorso = torso;
                        b.cWing = wing;
                        b.BeakType = beak;
                        b.Name = name;

                        birds.Add(b);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }

        private static Color ToColor(string p)
        {
            if (p == "white")
                return Color.White;
            if (p == "light blue")
                return Color.LightBlue;
            if (p == "black")
                return Color.Black;
            if (p == "yellow")
                return Color.Yellow;
            if (p == "light gray")
                return Color.LightGray;
            if (p == "brownish")
                return Color.Salmon;
            if (p == "light green")
                return Color.LightGreen;
            if (p == "brown")
                return Color.Brown;
            if (p == "pink")
                return Color.Fuchsia;
            if (p == "red")
                return Color.Red;
            if (p == "gold")
                return Color.Gold;
            if (p == "gray")
                return Color.DarkGray;
            return Color.White;
        }

        public static void AddBird(Bird b)
        {
            foreach (var old in birds)
            {
                if (old.cHead == b.cHead && old.cTail == b.cTail && old.cTorso == b.cTorso && old.cWing == b.cWing && old.BeakType == b.BeakType)
                    return;
            }
            birds.Add(b);

            WriteStuff();
        }

        public static void WriteStuff()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var bird in birds)
            {
                sb.Append(bird.cHead.ToDescription() + ":");
                sb.Append(bird.cTail.ToDescription() + ":");
                sb.Append(bird.cTorso.ToDescription() + ":");
                sb.Append(bird.cWing.ToDescription() + ":");
                sb.Append(bird.BeakType);
                if (bird.Name != "")
                    sb.Append(":" + bird.Name);
                sb.AppendLine();
            }

            try
            {
                File.WriteAllText("encyclopedia.txt", sb.ToString());
            }
            catch { }
        }

        public static int UniquesCount { get { return birds.Count; } }

        internal static string GetName(Bird b)
        {
            if (b == null) { return ""; }
            foreach (var old in birds)
            {
                if (old.cHead == b.cHead && old.cTail == b.cTail && old.cTorso == b.cTorso && old.cWing == b.cWing && old.BeakType == b.BeakType)
                    return old.Name;
            }
            return "";
        }

        internal static void NameBird(Bird b, string name)
        {            
            foreach (var old in birds)
            {
                if (old.cHead == b.cHead && old.cTail == b.cTail && old.cTorso == b.cTorso && old.cWing == b.cWing && old.BeakType == b.BeakType)
                {
                    old.Name = name;
                    WriteStuff();
                }
            }
        }
    }
}
