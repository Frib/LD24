﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace LD24
{
    class OptionsScreen : Screen
    {
       private List<MenuOption> options = new List<MenuOption>();
        private MenuOption selected;
        private Screen previous;
        private bool needsUpdate;

        public OptionsScreen(Screen previous)
        {
            this.previous = previous;
        }

        public override void Show()
        {
            CreateControls();
            G.g.IsMouseVisible = true;
            IM.SnapToCenter = false;
        }

        private void CreateControls()
        {
            options.Clear();
            options.Add(new MenuOption("Invert Horizontal: " + IM.InvertedHorizontal, RM.font) { Action = new Action(() => { IM.InvertedHorizontal = !IM.InvertedHorizontal; needsUpdate = true; }) });
            options.Add(new MenuOption("Invert Vertical: " + IM.InvertedVertical, RM.font) { Action = new Action(() => { IM.InvertedVertical = !IM.InvertedVertical; needsUpdate = true; }) });
            options.Add(new MenuOption("Volume: " + ((RM.Volume == 0) ? "No sound" : (RM.Volume == 1) ? "Quieter" : "Full volume"), RM.font) { Action = new Action(() => { RM.Volume++; if (RM.Volume > 2) { RM.Volume = 0;} needsUpdate = true; }) });
            options.Add(new MenuOption("Autosave photos to disk: " + Photograph.Autosave, RM.font) { Action = new Action(() => { Photograph.Autosave = !Photograph.Autosave; needsUpdate = true; }) });
            options.Add(new MenuOption("", RM.font) { Action = new Action(() => { })});
            options.Add(new MenuOption("Toggle Fullscreen", RM.font) { Action = new Action(() => { g.graphics.ToggleFullScreen(); }) });

            options.Add(new MenuOption("Increase sensitivity: " + (int)(IM.MouseSensitivity * 10000), RM.font) { Action = new Action(() => { IM.MouseSensitivity += 0.0005f; if (IM.MouseSensitivity > 0.02f) { IM.MouseSensitivity = 0.0005f; } needsUpdate=true;}) });
            options.Add(new MenuOption("Decrease", RM.font) { Action = new Action(() => { IM.MouseSensitivity -= 0.0005f; if (IM.MouseSensitivity <= 0.0004f) { IM.MouseSensitivity = 0.02f; } needsUpdate = true; }) });
            
            CalculatePositions();
        }

        private void CalculatePositions()
        {
            int offsetY = 100;
            int quarter = G.Width / 5;
            var font = RM.font;
            Vector2 minSize = new Vector2(font.MeasureString("<EMPTY>").X, 0);

            foreach (var o in options)
            {
                o.Position = new Vector2(128, offsetY);
                offsetY += 48;
            }
        }

        public override void Update()
        {
            var m = IM.MousePos;
            var mpos = new Vector2(m.X, m.Y);

            foreach (var o in options)
            {
                if (o.Intersects(mpos))
                {
                    if (RM.IsPressed(InputAction.Accept) || RM.IsPressed(InputAction.Fire))
                    {
                        o.Action();
                    }
                    selected = o;
                }
            }

            if (needsUpdate)
            {
                CreateControls();
                needsUpdate = false;

            }

            if (RM.IsPressed(InputAction.Back))
            {
                g.Showscreen(previous);
            }
        }

        public override void Draw()
        {
            GraphicsDevice.Clear(new Color(48, 48, 48));
            SpriteBatch sb = G.g.spriteBatch;
            sb.Begin();
            foreach (var o in options)
            {
                o.Draw(selected == o ? Color.White : Color.Yellow);
            }
            sb.End();
        }
    }
}
