using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD24
{
    class WorldConfigScreen : Screen
    {
        private List<MenuOption> options = new List<MenuOption>();
        private MenuOption selected;
        private Screen previous;
        private bool needsUpdate;

        public WorldConfigScreen(Screen previous)
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
            options.Add(new MenuOption("Tree count: " + Island.treeAmount, RM.font) { Action = new Action(() => { Island.treeAmount += 100; if (Island.treeAmount > 2000) { Island.treeAmount = 0; } needsUpdate = true; }) });
            options.Add(new MenuOption("Flower count: " + Island.flowerAmount, RM.font) { Action = new Action(() => { Island.flowerAmount += 100; if (Island.flowerAmount > 2000) { Island.flowerAmount = 0; } needsUpdate = true; }) });
            options.Add(new MenuOption("Flower draw distance: " + Island.flowerRenderDist, RM.font) { Action = new Action(() => { Island.flowerRenderDist += 100; if (Island.flowerRenderDist > 2000) { Island.flowerRenderDist = 100; } needsUpdate = true; }) });
            options.Add(new MenuOption("Bird count: " + Island.birdCount, RM.font) { Action = new Action(() => { Island.birdCount += 25; if (Island.birdCount > 500) { Island.birdCount = 25; } needsUpdate = true; }) });
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
                offsetY += 64;
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
