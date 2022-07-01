﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using QBert.Classes;
using QBert.Classes.Enemies;
using QBert.Classes.UI;

namespace QBert
{
    public class Game1 : Game
    {
        public static List<List<Cube>> cubes = new List<List<Cube>>();
        public static List<List<Cell>> Cells = new List<List<Cell>>();

        private static List<List<List<Color>>> colors = new List<List<List<Color>>>()
        {
            new List<List<Color>>()
            {
                new List<Color>() { new Color(0, 69, 222), new Color(239, 222, 119), /*new Color(33, 185, 49)*/ },
                new List<Color>() { new Color(102, 49, 0) },
                new List<Color>() { new Color(255, 119, 33) }
            },
            new List<List<Color>>()
            { 
                new List<Color>() { new Color(169, 185, 15), new Color(0, 102, 239), /*new Color(153, 0, 102)*/ },
                new List<Color>() { new Color(119, 135, 135) },
                new List<Color>() { new Color(15, 15, 153) }
            },
            new List<List<Color>>()
            { 
                new List<Color>() { new Color(135, 0, 119), new Color(0, 49, 153), /*new Color(33, 135, 206)*/ },
                new List<Color>() { new Color(185, 185, 33) },
                new List<Color>() { new Color(185, 49, 49) }
            },
            new List<List<Color>>()
            {
                new List<Color>() { new Color(0, 169, 222), new Color(69, 102, 85), /*new Color(255, 85, 85)*/ },
                new List<Color>() { new Color(185, 185, 33) },
                new List<Color>() { new Color(0, 49, 153) }
            }
        };
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private int level = 0;
        private int round = 0;
        private int cube_coord_x;
        private int cube_coord_y;
        private int cube_width = 100;
        private int cube_height = 100;
        private List<RedCircle> redCircles = new List<RedCircle>();
        private PurpleCircle purpleCircle;
        private CoolEnemy coolEnemy;
        private List<GreenCircle> greenCircles = new List<GreenCircle>();
        private Snake snake;
        private Player player;
        private Vector2 playerStartPosition = new Vector2(/*cubes[6][0].Rect_top.X + 25*/ 952, /*cubes[6][0].Rect_top.Y - 20*/ 399);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;

            // 1000 w  900 h
            player = new Player(playerStartPosition, 1, 7); // 952 399
            HUD.LeftBorderX = _graphics.PreferredBackBufferWidth / 2 - 500;
            HUD.TopBorderY = _graphics.PreferredBackBufferHeight / 2 - 450;
            HUD.Init();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            cube_coord_x = (_graphics.PreferredBackBufferWidth / 2 - cube_width / 2 - cube_width * 3) - cube_width - 8;
            cube_coord_y = _graphics.PreferredBackBufferHeight - 150;

            int amountCellsInLine = 9;
            for (int i = 0; amountCellsInLine >= i; i++)
            {
                Cells.Add(new List<Cell>());
                if (i != 0 && i != 9 && i != 8)
                {
                    cubes.Add(new List<Cube>());
                }

                for (int j = 0; j <= amountCellsInLine - i; j++)
                {
                    Cells[i].Add(new Cell(
                        new Rectangle(cube_coord_x + (cube_width / 2 - 2) * (i) + (cube_width - amountCellsInLine) * j, cube_coord_y - (i) * (cube_height - 27), 100, 100),
                         new Rectangle(cube_coord_x + (cube_width / 2 - 2) * (i) + (cube_width - amountCellsInLine) * j - 2, cube_coord_y - (i) * (cube_height - 27), 95, 50)
                    ));
                    if (j > 0 && j < amountCellsInLine - i && i != 0 && i != 9 && i != 8)
                    {
                        cubes[i - 1].Add(new Cube(
                         new Rectangle(Cells[i][j].X - 2, Cells[i][j].Y, 95, 50),
                         new Rectangle(Cells[i][j].X + 45, Cells[i][j].Y + 25, 47, 73),
                         new Rectangle(Cells[i][j].X - 3, Cells[i][j].Y + 25, 50, 73))
                        { Top_colors = colors[round][0], Left_color = colors[round][1][0], Right_color = colors[round][2][0] });
                    }

                }
            }

            redCircles.Add(new RedCircle());
            greenCircles.Add(new GreenCircle());
            purpleCircle = new PurpleCircle();
            snake = new Snake();
            coolEnemy = new CoolEnemy();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (List<Cube> l in cubes)
            {
                foreach (Cube cube in l) cube.LoadContent(Content);
            }

            foreach (List<Cell> l in Cells)
            {
                foreach (Cell cell in l) cell.LoadContent(Content);
            }


            foreach (RedCircle circle in redCircles) circle.LoadContent(Content);
            foreach (GreenCircle circle in greenCircles) circle.LoadContent(Content);
            purpleCircle.LoadContent(Content);
            snake.LoadContent(Content);
            coolEnemy.LoadContent(Content);

            player.LoadContent(Content);

            HUD.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            player.Update(gameTime);

            foreach (RedCircle circle in redCircles) circle.Update(gameTime);
            foreach (GreenCircle circle in greenCircles) circle.Update(gameTime);
            purpleCircle.Update(gameTime);
            snake.Update(new Vector2(player.IndexX, player.IndexY), gameTime);
            coolEnemy.Update(gameTime);

            if (AllCubesColored()) StartNewRound();

            HUD.Update(player.Score);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();



            foreach (List<Cell> l in Cells)
            {
                foreach (Cell cell in l) cell.Draw(_spriteBatch);
            }

            foreach (List<Cube> l in cubes)
            {
                foreach (Cube cube in l) cube.Draw(_spriteBatch);
            }

            foreach (RedCircle circle in redCircles) circle.Draw(_spriteBatch);
            foreach (GreenCircle circle in greenCircles) circle.Draw(_spriteBatch);
            purpleCircle.Draw(_spriteBatch);

            player.Draw(_spriteBatch);
            snake.Draw(_spriteBatch);
            coolEnemy.Draw(_spriteBatch);

            HUD.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool AllCubesColored()
        {
            foreach (List<Cube> c in cubes)
            {
                foreach (Cube cube in c)
                {
                    if (cube.Top_color_index != cube.Top_colors.Count - 1) return false;
                }
            }
            return true;
        }

        private void StartNewRound()
        {
            level += round == 3 ? 1 : 0;
            round = round == 3 ? 0 : round + 1;
            HUD.Level = level + 1;
            HUD.Round = round + 1;
            cubes.Clear();
            Cells.Clear();
            redCircles.Clear();
            greenCircles.Clear();

            Initialize();
            player.Position = playerStartPosition;
            player.IndexX = 1;
            player.IndexY = 7;
        }
    }
}