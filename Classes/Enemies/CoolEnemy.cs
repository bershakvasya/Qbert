﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace QBert.Classes.Enemies
{
    class CoolEnemy : Enemy
    {
        private bool hasColoredCube = false;
        public CoolEnemy() : base()
        {
            position = CountPositionByIndex();
            sprite_width = 38;
            sprite_height = 40;
            spriteIndex = 4;
            textureName = "coolEnemy1";
        }
        public override Vector2 CountPositionByIndex()
        {
            return new Vector2(Game1.Cells[indexY][indexX].Rect_top.X + 30, Game1.Cells[indexY][indexX].Rect_top.Y);
        }

        public override void Update(GameTime gametime, Vector2 playerIndexes)
        {
            if (circleJump != null && circleJump.NowJumpState == JumpStates.inJump)
            {
                circleJump.Update(gametime);
                position = circleJump.position;

                if (circleJump.NowTime >= 0.13f && spriteIndex % 4 == 0) spriteIndex++;
            }

            if (circleJump.NowJumpState == JumpStates.readyToJump)
            {
                if (!hasColoredCube)
                {
                    Game1.cubes[indexY - 1][indexX - 1].ChangeTopColor(true);
                    hasColoredCube = true;
                }

                jumpTimer--;
                if (jumpTimer == 0)
                {
                    Game1.Cells[indexY][indexX].objectStatechanged("cube");
                    if (indexY == 1) return;
                    indexY--;
                    int direction = random.Next(0, 2);
                    spriteIndex = direction == 0 ? 3 : 5;
                    indexX += direction;
                    circleJump.UpdateTargetPosition(CountPositionByIndex(), position, JumpStates.inJump);
                    jumpTimer = 20;
                    hasColoredCube = false;
                    Game1.Cells[indexY][indexX].objectStatechanged(this);
                }
            }
            sourceRectangle = new Rectangle(sprite_width * spriteIndex, 0, sprite_width, sprite_height);
        }
    }
}
