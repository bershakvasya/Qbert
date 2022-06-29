﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace QBert.Classes
{
    class RedCircle : IEnemy
    {
        public RedCircle()
        {
            position = CountPositionByIndex();
            sprite_width = 44;
            sprite_height = 30;
            spriteIndex = 0;
        }
        public override void LoadContent(ContentManager manager)
        {
            texture = manager.Load<Texture2D>("redCircle");
        }
        public override void Draw(SpriteBatch brush)
        {
            brush.Draw(texture, position, sourceRectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public override Vector2 CountPositionByIndex()
        {
            return new Vector2(Game1.cubes[indexY][indexX].Rect_top.X + 25, Game1.cubes[indexY][indexX].Rect_top.Y + 5);
        }
    }
}
