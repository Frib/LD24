using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LD24
{
    public static class Ext
    {
        public static T Random<T>(this List<T> list)
        {
            return list[G.r.Next(list.Count)];
        }

        public static string ToDescription(this Animations anim)
        {
            switch (anim)
            {
                case Animations.idle: return "n idle";
                case Animations.walking: return " walking";
                case Animations.eating: return " foraging";
                case Animations.flying: return " flying";
                case Animations.landing: return " hovering";
                default: return " idle";
            }
        }

        public static string ToDescription(this Heading h)
        {
            switch (h)
            {
                case Heading.Back:return "rear";
                case Heading.Front: return "front";
                case Heading.Side: return "side";
            }
            return "front";
        }

        public static string ToDescription(this Color c)
        {
            if (c == Color.White)
                return "white";
            if (c == Color.LightBlue)
                return "light blue";
            if (c == Color.Black)
                return "black";
            if (c == Color.Yellow)
                return "yellow";
            if (c == Color.LightGray)
                return "light gray";
            if (c == Color.Salmon)
                return "brownish";
            if (c == Color.LightGreen)
                return "light green";
            if (c == Color.Brown)
                return "brown";
            if (c == Color.Fuchsia)
                return "pink";
            if (c == Color.Red)
                return "red";
            if (c == Color.Gold)
                return "gold";
            if (c == Color.DarkGray)
                return "gray";
            return "omg bug :(";
        }

        public static string GetName(this InputAction ia)
        {
            switch (ia)
            {
                case InputAction.Up: return "Move forward";
                case InputAction.Down: return "Move backward";
                case InputAction.Left: return "Move left";
                case InputAction.Right: return "Move right";
                case InputAction.Accept: return "Confirm";
                case InputAction.Back: return "Back";
                case InputAction.Fire: return "Take photo";
                case InputAction.AltFire: return "Ready camera";
                case InputAction.Run: return "Run";
                case InputAction.ShowAlbum: return "Show album";
                case InputAction.ZoomIn: return "Zoom in";
                case InputAction.ZoomOut: return "Zoom out";
                default: return ia.ToString();
            }
        }
    }
}
