using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LD24
{
    class SplashEffect : Entity
    {
        private int tick;
        public SplashEffect(Island island, Vector3 position) : base(island, new Vector2(8, 8), new Vector3(position.X, island.waterheight + 2f, position.Z))
        {
            // TODO: Complete member initialization
            this.island = island;
            this.position = new Vector3(position.X, island.waterheight, position.Z);
            textureFront = RM.GetTexture("splash1");
        }

        public override void Update()
        {
            tick++;
            if (tick == 8)
            {
                textureFront = RM.GetTexture("splash2");
            }
            else if (tick == 16)
            {
                textureFront = RM.GetTexture("splash3");
            }
            else if (tick == 24)
            {
                removeMe = true;
            }
        }
    }
}
