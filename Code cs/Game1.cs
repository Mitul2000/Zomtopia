using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Revenge_of_the_Non_Living
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        

        int optionNum = 0;

        enum Gamestate
        {
            MainMenu,
            Options,
            Playing,
            Exit,
        }
        Gamestate CurrentGameState = Gamestate.MainMenu;

        classButton btnply;
        classButton btnOpt;
        classButton btnEsc;

        SpriteFont font;

        GamePadState[] pad = new GamePadState[4];
        GamePadState[] oldpad = new GamePadState[4];

        Character[] player = new Character[4];

        List<Enemy> zombo = new List<Enemy>();
        List<Enemy> zombo2 = new List<Enemy>();
        List<Enemy> zombo3 = new List<Enemy>();
        Enemy Titanus;
        Tobu bossTobu;
        HiveMind bossHiveMind;

        bool[] zomboMoving;
        bool[] zomboMoving2;
        bool[] zomboMoving3;
        bool boxContact;
        bool spinning;
        bool countdown;
        bool[] refresh = new bool[4];
        bool reset = true;
        bool lvlChange;

        Texture2D potatoProjpic;
        Texture2D firecrackerProjpic;
        Texture2D watersprayerProjpic;
        Texture2D backgroundPic;
        Texture2D magicBoxPic;
        Texture2D zomboPic;
       

        double[] player1Distance;
        double[] player2Distance;
        double[] player3Distance;
        double[] player4Distance;

        string[] currentProj = new string[4];
        string[] proj = new string[3];
        string currentLevel = "Level 2";
        string gamestate = "Playing";
        string boxMsg = "Press X for a random projectile (Cost: 1000 Points)";

        float boxMsgWidth;
        float boxMsgHeight;

        int[] currentPoints = new int[4];
        int maxCount = 10;
        int tick;
        int zomboCounter;
        int boxTimer = 240;
        int noisetimer;
        int[] zomboPosX = new int[2];
        int[] zomboPosY = new int[2];
        int[] playerHealth = new int[4];
        int loadingScreenTimer = 240;
        int currentPlayer;

        Rectangle[] playerRec = new Rectangle[4];
        Rectangle backgroundRec;
        Rectangle magicBox;

        SoundEffect zomboNoise;
        Song backgroundMusic;


        Random rnd = new Random();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            //sets the screen size to 720p
            for (int l = 0; l < pad.Length; l++)
            {
                playerHealth[l] = 100;
                currentProj[l] = "Potato";
            }//set the player health and the all the player's current projectiles
            proj[0] = "The WaterSprayer";
            proj[1] = "The Firecracker";
            proj[2] = "Potato";

            //gives the name to each projectile
           
            //loads the pictures of the players
            player[0] = new Character(0, playerHealth[0], new Rectangle(0, 20, 50, 50), Content.Load<Texture2D>("player"));
            player[1] = new Character(0, playerHealth[1], new Rectangle(GraphicsDevice.Viewport.Width - playerRec[1].Width - 50, 20, 50, 50), Content.Load<Texture2D>("player"));
            player[2] = new Character(0, playerHealth[2], new Rectangle(0, GraphicsDevice.Viewport.Height - playerRec[2].Height - 20, 50, 50), Content.Load<Texture2D>("player"));
            player[3] = new Character(0, playerHealth[3], new Rectangle(GraphicsDevice.Viewport.Width - playerRec[3].Width, GraphicsDevice.Viewport.Height - playerRec[3].Height - 20, 50, 50), Content.Load<Texture2D>("player"));
            //instantiates 4 different players that spawn in the corners of the screen

            backgroundRec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            backgroundPic = Content.Load<Texture2D>("grassland");//sets the background rectangle and the picture 

            magicBox = new Rectangle(GraphicsDevice.Viewport.Width - 50, GraphicsDevice.Viewport.Height / 2, 50, 50);
            magicBoxPic = Content.Load<Texture2D>("mysteryBox");

            zomboPic = Content.Load<Texture2D>("zomboRight");
            zomboPosX[0] = GraphicsDevice.Viewport.Width / 2;
            zomboPosY[0] = -50;
            zomboPosX[1] = GraphicsDevice.Viewport.Width / 2;
            zomboPosY[1] = GraphicsDevice.Viewport.Height + 50;//allows the zombie to spawn an the top or bottom of the screen
            for (int j = 0; j < 5; j++)
            {
                zombo.Add(new Enemy(25, 200, new Rectangle(zomboPosX[0], zomboPosY[0], 50, 50), zomboPic));
                zombo.Add(new Enemy(25, 200, new Rectangle(zomboPosX[1], zomboPosY[1], 50, 50), zomboPic));
            }//instatiates 10 zombies that spawn at the middle top and bottom of the screen

            for (int k = 0; k < zombo.Count; k++)
            {
                zomboMoving = new bool[zombo.Count];
                zomboMoving[k] = false;
            }//creates a new bool to determine if the zombos are moving for each zombo, assumes they are not moving to start

            player1Distance = new double[zombo.Count];
            player2Distance = new double[zombo.Count];
            player3Distance = new double[zombo.Count];
            player4Distance = new double[zombo.Count];
            //allows the distance to be tested from each zombo to each character

            Titanus = new Enemy(10, 25, new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 100, 100), Content.Load<Texture2D>("enemy"));
            bossTobu = new Tobu(new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 100, 100), Content.Load<Texture2D>("Tobu"));
            bossHiveMind = new HiveMind(new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 100, 100), Content.Load<Texture2D>("HiveMindMain"));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            btnply = new classButton(Content.Load<Texture2D>("startButton"), graphics.GraphicsDevice);
            btnply.setBtntype("Active");
            btnOpt = new classButton(Content.Load<Texture2D>("optionsButton"), graphics.GraphicsDevice);
            btnEsc = new classButton(Content.Load<Texture2D>("quitButton"), graphics.GraphicsDevice);

            btnply.setPosition(new Vector2(50, 400));
            btnOpt.setPosition(new Vector2(50, 450));
            btnEsc.setPosition(new Vector2(50, 500));

            potatoProjpic = Content.Load<Texture2D>("PotatoProj");
            firecrackerProjpic = Content.Load<Texture2D>("FireCrackerProj");
            watersprayerProjpic = Content.Load<Texture2D>("WaterSprayerProj");

            //loads the pictures for all projectiles
            zomboNoise = Content.Load<SoundEffect>("zombieSound");
            backgroundMusic = Content.Load<Song>("toxic avenger");
            MediaPlayer.Play(backgroundMusic);
            font = Content.Load<SpriteFont>("textFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
           
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
        
            pad[0] = GamePad.GetState(PlayerIndex.One);
            pad[1] = GamePad.GetState(PlayerIndex.Two);
            pad[2] = GamePad.GetState(PlayerIndex.Three);
            pad[3] = GamePad.GetState(PlayerIndex.Four);
            //gets the state of all gamepads
            switch (CurrentGameState)
            {
                case Gamestate.MainMenu:
                    if (btnply.isClicked == true) CurrentGameState = Gamestate.Playing;
                    if (btnOpt.isClicked == true) CurrentGameState = Gamestate.Options;
                    if (btnEsc.isClicked == true) CurrentGameState = Gamestate.Exit;
                    if (pad[0].DPad.Down == ButtonState.Pressed && oldpad[0].DPad.Down == ButtonState.Released)
                    {
                        optionNum++;
                        if (optionNum == 3)
                        {
                            optionNum = 0;
                        }
                        if (optionNum == 0)
                        {
                            btnply.setBtntype("Active");
                            btnOpt.setBtntype("other");
                            btnEsc.setBtntype("other");
                        }

                        else if (optionNum == 1)
                        {
                            btnply.setBtntype("other");
                            btnOpt.setBtntype("Active");
                            btnEsc.setBtntype("other");
                        }
                        else if (optionNum == 2)
                        {
                            btnply.setBtntype("other");
                            btnOpt.setBtntype("other");
                            btnEsc.setBtntype("Active");
                        }



                    }
                    oldpad[0] = pad[0];
                    btnply.Update(pad[0]);
                    btnOpt.Update(pad[0]);
                    btnEsc.Update(pad[0]);


                    break;

                case Gamestate.Playing:
                    if (currentLevel == "Level 1")
                    {
                        if (gamestate == "Playing")
                        {
                            if (pad[0].Buttons.Start == ButtonState.Pressed && oldpad[0].Buttons.Start == ButtonState.Released)
                            {
                                gamestate = "Paused";
                            }//if the start button is pressed the game is paused
                            if (MediaPlayer.State == MediaState.Stopped)
                            {
                                MediaPlayer.Play(backgroundMusic);
                            }//replays the music when it ends
                            noisetimer++;
                            if (noisetimer % 600 == 0)
                            {
                                zomboNoise.Play();
                            }//plays a zombo noise every 10 seconds

                            for (int i = 0; i < player.Length; i++)
                            {
                                if (!pad[i].IsConnected)
                                {
                                    player[i] = null;
                                }
                            }//if a controller is not connected the player for the controller will be set to null
                            for (int i = 0; i < player.Length; i++)
                            {
                                if (pad[i].IsConnected)
                                {
                                    if (player[i].getHealth() > 0)
                                    {
                                        player[i].Move(pad[i], GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                                        if (currentProj[i] == "Potato")
                                        {
                                            maxCount = 10;
                                            player[i].Update(pad[i], oldpad[i], new Rectangle(player[i].getRec().X, player[i].getRec().Y, 20, 20), potatoProjpic, maxCount, 10, 10);
                                            player[i].Damage(zombo, 15);
                                            player[i].bossDamage(Titanus, 15);
                                        }
                                        if (currentProj[i] == "The WaterSprayer")
                                        {
                                            maxCount = 30;
                                            player[i].Update(pad[i], oldpad[i], new Rectangle(player[i].getRec().X, player[i].getRec().Y, 10, 10), watersprayerProjpic, maxCount, 25, 25);
                                            player[i].Damage(zombo, 5);
                                            player[i].bossDamage(Titanus, 5);
                                        }
                                        if (currentProj[i] == "The Firecracker")
                                        {
                                            maxCount = 5;
                                            player[i].Update(pad[i], oldpad[i], new Rectangle(player[i].getRec().X, player[i].getRec().Y, 35, 35), firecrackerProjpic, maxCount, 5, 5);
                                            player[i].Damage(zombo, 35);
                                            player[i].bossDamage(Titanus, 35);
                                        }//fires a certain type of projectile and deals damage based on their stats
                                    }
                                    if (player[i].getHealth() <= 0)
                                    {
                                        player[i] = new Character(0, 0, new Rectangle(5000, 5000, 0, 0), Content.Load<Texture2D>("player"));
                                    }//if the player health reaches 0 or below the player is moved off screen
                                    oldpad[i] = pad[i];//sets the old gamepad state to the new gamepad state
                                    if (player[i].getCount() == maxCount)
                                    {
                                        refresh[i] = true;
                                    }//if the player's current projectile has reached its maximum count then that player's refresh option is set to true
                                    if (pad[i].Buttons.X == ButtonState.Pressed)
                                    {
                                        refresh[i] = false;
                                    }//if the player presses X the refresh option is set to false
                                    if (player[i].getRec().Intersects(magicBox))
                                    {
                                        boxContact = true;
                                    }
                                    else
                                    {
                                        boxContact = false;
                                    }//determines if the player is in contact with the box
                                    if (player[i].getRec().Intersects(magicBox) && pad[i].Buttons.X == ButtonState.Pressed && player[i].getPoints() >= 50)
                                    {
                                        spinning = true;
                                        countdown = true;
                                        currentPlayer = i;
                                        player[i].losePoints();
                                    }//starts the timer and randomly giving the player a projectile, the player loses 1000 points 

                                    if (spinning)
                                    {
                                        currentProj[currentPlayer] = proj[rnd.Next(0, 3)];
                                    }//gives the player using the box a random projectile
                                    currentPoints[i] = player[i].getPoints();
                                }
                            }
                            boxMsgWidth = font.MeasureString(boxMsg).X;
                            boxMsgHeight = font.MeasureString(boxMsg).Y;
                            //measures the width and height of the message that appears when a player intersects with the magic box
                            if (countdown)
                            {
                                boxTimer--;
                            }//starts the countdown of the box timer
                            if (boxTimer == 0)
                            {

                                spinning = false;
                                countdown = false;
                                boxTimer = 240;
                            }//when the box timer is 0 the player receives their random projectile and the timer is refreshed until its next use
                            tick++;//adds one to the tick every update to keep track fo every update
                            if (tick % 120 == 0 && zomboCounter < 6)
                            {
                                zomboMoving[zomboCounter] = true;
                                zomboCounter++;
                            }//every 2 seconds a new zombo starts to move
                            for (int j = 0; j < zombo.Count; j++)
                            {
                                if (pad[0].IsConnected)
                                    player1Distance[j] = Math.Sqrt((zombo[j].getRec().Y - player[0].getRec().Y) * (zombo[j].getRec().Y - player[0].getRec().Y) + (zombo[j].getRec().X - player[0].getRec().X) * (zombo[j].getRec().X - player[0].getRec().X));
                                if (pad[1].IsConnected)
                                    player2Distance[j] = Math.Sqrt((zombo[j].getRec().Y - player[1].getRec().Y) * (zombo[j].getRec().Y - player[1].getRec().Y) + (zombo[j].getRec().X - player[1].getRec().X) * (zombo[j].getRec().X - player[1].getRec().X));
                                if (pad[2].IsConnected)
                                    player3Distance[j] = Math.Sqrt((zombo[j].getRec().Y - player[2].getRec().Y) * (zombo[j].getRec().Y - player[2].getRec().Y) + (zombo[j].getRec().X - player[2].getRec().X) * (zombo[j].getRec().X - player[2].getRec().X));
                                if (pad[3].IsConnected)
                                    player4Distance[j] = Math.Sqrt((zombo[j].getRec().Y - player[3].getRec().Y) * (zombo[j].getRec().Y - player[3].getRec().Y) + (zombo[j].getRec().X - player[3].getRec().X) * (zombo[j].getRec().X - player[3].getRec().X));
                            }//for each zombo the distance from each player is recorded if the controller for that player is connected
                            for (int k = 0; k < zombo.Count; k++)
                            {
                                if (zomboMoving[k])
                                {
                                    zombo[k].trace(player, zombo, player1Distance, player2Distance, player3Distance, player4Distance);
                                }
                                if (zombo[k].getfacingDown())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboDown");

                                }
                                if (zombo[k].getfacingLeft())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboLeft");
                                }
                                if (zombo[k].getfacingUp())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboUp");
                                }
                                if (zombo[k].getfacingRight())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboRight");
                                }
                                zombo[k].setPic(zomboPic);

                            }//if the zombie is moving it will follow the nearest player

                            Titanus.bossTrace(player, pad, Titanus);
                            //the boss follows the closest player
                            if (Titanus.getHealth() <= 0)
                            {
                                currentLevel = "Break";
                            }//once the boss is defeated a loading screen appears
                            if (player[1] == null && player[2] == null && player[3] == null)
                            {
                                if (player[0].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }
                            else if (player[2] == null && player[3] == null)
                            {
                                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }
                            else if (player[3] == null)
                            {
                                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0 && player[2].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }
                            else
                            {
                                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0 && player[2].getHealth() <= 0 && player[3].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }//if all the connected player's health has reached 0 the level changes to game over
                        }
                        if (gamestate == "Paused")
                        {
                            MediaPlayer.Pause();
                            if (pad[0].Buttons.A == ButtonState.Pressed)
                            {
                                MediaPlayer.Resume();
                                gamestate = "Playing";
                            }
                            if (pad[0].Buttons.Back == ButtonState.Pressed)
                                this.Exit();
                        }//if the game is paused the music is paused until player 1 presses the A button to resume
                    }

                    if (currentLevel == "Break")
                    {
                        loadingScreenTimer--;
                        if (loadingScreenTimer == 0 && !lvlChange)
                        {

                            currentLevel = "Level 2";
                            loadingScreenTimer = 60;
                        }
                        else if (loadingScreenTimer == 0 && lvlChange)
                        {
                            currentLevel = "Level 3";
                        }
                    }
                    if (currentLevel == "Level 2")
                    {

                        if (reset)
                        {
                            for (int i = 0; i < pad.Length; i++)
                            {
                                playerHealth[i] = 125;
                            }//set the player health
                            loadingScreenTimer = 240;
                            player[0] = new Character(currentPoints[0], playerHealth[0], new Rectangle(0, 20, 50, 50), Content.Load<Texture2D>("player"));
                            player[1] = new Character(currentPoints[1], playerHealth[1], new Rectangle(GraphicsDevice.Viewport.Width - playerRec[1].Width - 50, 20, 50, 50), Content.Load<Texture2D>("player"));
                            player[2] = new Character(currentPoints[2], playerHealth[2], new Rectangle(0, GraphicsDevice.Viewport.Height - playerRec[2].Height - 20, 50, 50), Content.Load<Texture2D>("player"));
                            player[3] = new Character(currentPoints[3], playerHealth[3], new Rectangle(GraphicsDevice.Viewport.Width - playerRec[3].Width, GraphicsDevice.Viewport.Height - playerRec[3].Height - 20, 50, 50), Content.Load<Texture2D>("player"));
                            //instantiates 4 different players that spawn in the corners of the screen
                            backgroundPic = Content.Load<Texture2D>("dungeon");//sets the background rectangle and the picture
                            for (int j = 0; j < 10; j++)
                            {
                                zombo2.Add(new Enemy(25, 200, new Rectangle(zomboPosX[0], zomboPosY[0], 50, 50), zomboPic));
                                zombo2.Add(new Enemy(25, 200, new Rectangle(zomboPosX[1], zomboPosY[1], 50, 50), zomboPic));
                            }//instatiates zombies that spawn at the middle top and bottom of the screen
                            for (int k = 0; k < zombo2.Count; k++)
                            {
                                zomboMoving2 = new bool[zombo2.Count];
                                zomboMoving2[k] = false;
                            }//creates a new bool to determine if the zombos are moving for each zombo, assumes they are not moving to start
                            player1Distance = new double[zombo2.Count];
                            player2Distance = new double[zombo2.Count];
                            player3Distance = new double[zombo2.Count];
                            player4Distance = new double[zombo2.Count];
                            backgroundMusic = Content.Load<Song>("STARFORCE");
                            MediaPlayer.Play(backgroundMusic);
                            reset = false;
                        }
                        if (gamestate == "Playing")
                        {
                            if (pad[0].Buttons.Start == ButtonState.Pressed && oldpad[0].Buttons.Start == ButtonState.Released)
                            {
                                gamestate = "Paused";
                            }//player 1 can pause the game by pressing start
                            if (MediaPlayer.State == MediaState.Stopped)
                            {
                                MediaPlayer.Play(backgroundMusic);
                            }//plays the background music again once it ends
                            noisetimer++;
                            if (noisetimer % 600 == 0)
                            {
                                zomboNoise.Play();
                            }//plays a zombo noise every 10 seconds
                            for (int i = 0; i < player.Length; i++)
                            {
                                if (!pad[i].IsConnected)
                                {
                                    player[i] = null;
                                }
                            }//if a controller is not connected the player for the controller will be set to null
                            for (int i = 0; i < player.Length; i++)
                            {
                                if (pad[i].IsConnected)
                                {
                                    bossTobu.Attack(10, 10, new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 + 50, 20, 20), potatoProjpic, rnd, bossTobu, player[i], 25);
                                    if (player[i].getHealth() > 0)
                                    {
                                        player[i].Move(pad[i], GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                                        if (currentProj[i] == "Potato")
                                        {
                                            maxCount = 10;
                                            player[i].Update(pad[i], oldpad[i], new Rectangle(player[i].getRec().X, player[i].getRec().Y, 20, 20), potatoProjpic, maxCount, 10, 10);
                                            player[i].Damage(zombo2, 15);
                                            player[i].bossDamage(bossTobu, 15);

                                        }
                                        if (currentProj[i] == "The WaterSprayer")
                                        {
                                            maxCount = 30;
                                            player[i].Update(pad[i], oldpad[i], new Rectangle(player[i].getRec().X, player[i].getRec().Y, 10, 10), watersprayerProjpic, maxCount, 25, 250);
                                            player[i].Damage(zombo2, 5);
                                            player[i].bossDamage(bossTobu, 5);
                                        }
                                        if (currentProj[i] == "The Firecracker")
                                        {
                                            maxCount = 5;
                                            player[i].Update(pad[i], oldpad[i], new Rectangle(player[i].getRec().X, player[i].getRec().Y, 35, 35), firecrackerProjpic, maxCount, 5, 5);
                                            player[i].Damage(zombo2, 35);
                                            player[i].bossDamage(bossTobu, 35);
                                        }//fires a certain type of projectile and deals damage based on their stats
                                    }
                                    if (player[i].getHealth() <= 0)
                                    {
                                        player[i] = new Character(0, 0, new Rectangle(5000, 5000, 0, 0), Content.Load<Texture2D>("player"));
                                    }//if the player health reaches 0 or below the player is moved off screen
                                    oldpad[i] = pad[i];//sets the old gamepad state to the new gamepad state
                                    if (player[i].getCount() == maxCount)
                                    {
                                        refresh[i] = true;
                                    }//if the player's current projectile has reached its maximum count then that player's refresh option is set to true
                                    if (pad[i].Buttons.X == ButtonState.Pressed)
                                    {
                                        refresh[i] = false;
                                    }//if the player presses X the refresh option is set to false
                                    if (player[i].getRec().Intersects(magicBox))
                                    {
                                        boxContact = true;
                                    }
                                    else
                                    {
                                        boxContact = false;
                                    }//determines if the player is in contact with the box
                                    if (player[i].getRec().Intersects(magicBox) && pad[i].Buttons.X == ButtonState.Pressed && player[i].getPoints() >= 1000)
                                    {
                                        spinning = true;
                                        countdown = true;
                                        currentPlayer = i;
                                        player[i].losePoints();
                                    }//starts the timer and randomly giving the player a projectile, the player loses 5000 points 
                                    if (spinning)
                                    {
                                        currentProj[currentPlayer] = proj[rnd.Next(0, 3)];
                                    }//gives the player using the box a random projectile
                                    currentPoints[i] = player[i].getPoints();
                                }
                            }
                            boxMsgWidth = font.MeasureString(boxMsg).X;
                            boxMsgHeight = font.MeasureString(boxMsg).Y;
                            //measures the width and height of the message that appears when a player intersects with the magic box
                            if (countdown)
                            {
                                boxTimer--;
                            }//starts the countdown of the box timer
                            if (boxTimer == 0)
                            {

                                spinning = false;
                                countdown = false;
                                boxTimer = 240;
                            }//when the box timer is 0 the player receives their random projectile and the timer is refreshed until its next use

                            tick++;//adds one to the tick every update to keep track fo every update
                            if (tick % 60 == 0 && zomboCounter < 10)
                            {
                                zomboMoving2[zomboCounter] = true;
                                zomboCounter++;
                            }//every 2 seconds a new zombo starts to move
                            for (int j = 0; j < zombo2.Count; j++)
                            {
                                if (pad[0].IsConnected)
                                    player1Distance[j] = Math.Sqrt((zombo2[j].getRec().Y - player[0].getRec().Y) * (zombo2[j].getRec().Y - player[0].getRec().Y) + (zombo2[j].getRec().X - player[0].getRec().X) * (zombo2[j].getRec().X - player[0].getRec().X));
                                if (pad[1].IsConnected)
                                    player2Distance[j] = Math.Sqrt((zombo2[j].getRec().Y - player[1].getRec().Y) * (zombo2[j].getRec().Y - player[1].getRec().Y) + (zombo2[j].getRec().X - player[1].getRec().X) * (zombo2[j].getRec().X - player[1].getRec().X));
                                if (pad[2].IsConnected)
                                    player3Distance[j] = Math.Sqrt((zombo2[j].getRec().Y - player[2].getRec().Y) * (zombo2[j].getRec().Y - player[2].getRec().Y) + (zombo2[j].getRec().X - player[2].getRec().X) * (zombo2[j].getRec().X - player[2].getRec().X));
                                if (pad[3].IsConnected)
                                    player4Distance[j] = Math.Sqrt((zombo2[j].getRec().Y - player[3].getRec().Y) * (zombo2[j].getRec().Y - player[3].getRec().Y) + (zombo2[j].getRec().X - player[3].getRec().X) * (zombo2[j].getRec().X - player[3].getRec().X));
                            }//for each zombo the distance from each player is recorded if the controller for that player is connected

                            for (int k = 0; k < zombo2.Count; k++)
                            {
                                if (zomboMoving2[k])
                                {
                                    zombo2[k].trace(player, zombo, player1Distance, player2Distance, player3Distance, player4Distance);
                                }
                                if (zombo2[k].getfacingDown())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboDown");
                                }
                                if (zombo2[k].getfacingLeft())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboLeft");
                                }
                                if (zombo2[k].getfacingUp())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboUp");
                                }
                                if (zombo2[k].getfacingRight())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboRight");
                                }
                                zombo2[k].setPic(zomboPic);
                            }//if the zombie is moving it will follow the nearest player
                            if (bossTobu.getHealth() <= 0)
                            {
                                reset = true;
                                lvlChange = true;
                                currentLevel = "Break";
                            }
                            if (player[1] == null && player[2] == null && player[3] == null)
                            {
                                if (player[0].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }
                            else if (player[2] == null && player[3] == null)
                            {
                                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }
                            else if (player[3] == null)
                            {
                                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0 && player[2].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }
                            else
                            {
                                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0 && player[2].getHealth() <= 0 && player[3].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }//if all the connected player's health has reached 0 the level changes to game over
                        }
                        if (gamestate == "Paused")
                        {
                            MediaPlayer.Pause();
                            if (pad[0].Buttons.A == ButtonState.Pressed)
                            {
                                MediaPlayer.Resume();
                                gamestate = "Playing";
                            }
                            if (pad[0].Buttons.Back == ButtonState.Pressed)
                                this.Exit();
                        }//if the game is paused the music is paused until player 1 presses the A button to resume


                    }
                    if (currentLevel == "Level 3")
                    {
                        if (reset)
                        {
                            for (int i = 0; i < pad.Length; i++)
                            {
                                playerHealth[i] = 150;
                            }//set the player health
                            player[0] = new Character(currentPoints[0], playerHealth[0], new Rectangle(0, 20, 50, 50), Content.Load<Texture2D>("player"));
                            player[1] = new Character(currentPoints[1], playerHealth[1], new Rectangle(GraphicsDevice.Viewport.Width - playerRec[1].Width - 50, 20, 50, 50), Content.Load<Texture2D>("player"));
                            player[2] = new Character(currentPoints[2], playerHealth[2], new Rectangle(0, GraphicsDevice.Viewport.Height - playerRec[2].Height - 20, 50, 50), Content.Load<Texture2D>("player"));
                            player[3] = new Character(currentPoints[3], playerHealth[3], new Rectangle(GraphicsDevice.Viewport.Width - playerRec[3].Width, GraphicsDevice.Viewport.Height - playerRec[3].Height - 20, 50, 50), Content.Load<Texture2D>("player"));
                            //instantiates 4 different players that spawn in the corners of the screen
                            backgroundPic = Content.Load<Texture2D>("volcano");//sets the background rectangle and the picture
                            for (int j = 0; j < 15; j++)
                            {
                                zombo3.Add(new Enemy(25, 200, new Rectangle(zomboPosX[0], zomboPosY[0], 50, 50), Content.Load<Texture2D>("zomboRight")));
                                zombo3.Add(new Enemy(25, 200, new Rectangle(zomboPosX[1], zomboPosY[1], 50, 50), Content.Load<Texture2D>("zomboRight")));
                            }//instatiates zombies that spawn at the middle top and bottom of the screen

                            for (int k = 0; k < zombo3.Count; k++)
                            {
                                zomboMoving3 = new bool[zombo3.Count];
                                zomboMoving3[k] = false;
                            }//creates a new bool to determine if the zombos are moving for each zombo, assumes they are not moving to start
                            player1Distance = new double[zombo3.Count];
                            player2Distance = new double[zombo3.Count];
                            player3Distance = new double[zombo3.Count];
                            player4Distance = new double[zombo3.Count];
                            backgroundMusic = Content.Load<Song>("8-Bit");
                            MediaPlayer.Play(backgroundMusic);
                            reset = false;
                        }
                        if (gamestate == "Playing")
                        {
                            if (pad[0].Buttons.Start == ButtonState.Pressed && oldpad[0].Buttons.Start == ButtonState.Released)
                            {
                                gamestate = "Paused";
                            }//if player 1 presses start the game is paused
                            if (MediaPlayer.State == MediaState.Stopped)
                            {
                                MediaPlayer.Play(backgroundMusic);
                            }//if the music is stopped it will replay
                            noisetimer++;
                            if (noisetimer % 600 == 0)
                            {
                                zomboNoise.Play();
                            }
                            for (int i = 0; i < player.Length; i++)
                            {
                                if (!pad[i].IsConnected)
                                {
                                    player[i] = null;
                                }
                            }//if a controller is not connected the player for the controller will be set to null
                            for (int i = 0; i < player.Length; i++)
                            {
                                if (pad[i].IsConnected)
                                {
                                    bossHiveMind.spawn(bossHiveMind, Content.Load<Texture2D>("zomboRight"), player[i], player, player1Distance, player2Distance, player3Distance, player4Distance, 25, pad);
                                    if (player[i].getHealth() > 0)
                                    {
                                        player[i].Move(pad[i], GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                                        if (currentProj[i] == "Potato")
                                        {
                                            maxCount = 10;
                                            player[i].Update(pad[i], oldpad[i], new Rectangle(player[i].getRec().X, player[i].getRec().Y, 20, 20), potatoProjpic, maxCount, 10, 10);
                                            player[i].Damage(zombo3, 15);
                                            player[i].bossDamage(bossHiveMind, 15);

                                        }
                                        if (currentProj[i] == "The WaterSprayer")
                                        {
                                            maxCount = 30;
                                            player[i].Update(pad[i], oldpad[i], new Rectangle(player[i].getRec().X, player[i].getRec().Y, 10, 10), watersprayerProjpic, maxCount, 25, 250);
                                            player[i].Damage(zombo3, 5);
                                            player[i].bossDamage(bossHiveMind, 5);
                                        }
                                        if (currentProj[i] == "The Firecracker")
                                        {
                                            maxCount = 5;
                                            player[i].Update(pad[i], oldpad[i], new Rectangle(player[i].getRec().X, player[i].getRec().Y, 35, 35), firecrackerProjpic, maxCount, 5, 5);
                                            player[i].Damage(zombo3, 35);
                                            player[i].bossDamage(bossHiveMind, 35);
                                        }//fires a certain type of projectile and deals damage based on their stats
                                    }
                                    if (player[i].getHealth() <= 0)
                                    {
                                        player[i] = new Character(0, 0, new Rectangle(5000, 5000, 0, 0), Content.Load<Texture2D>("player"));
                                    }//if the player health reaches 0 or below the player is moved off screen
                                    oldpad[i] = pad[i];//sets the old gamepad state to the new gamepad state
                                    if (player[i].getCount() == maxCount)
                                    {
                                        refresh[i] = true;
                                    }//if the player's current projectile has reached its maximum count then that player's refresh option is set to true
                                    if (pad[i].Buttons.X == ButtonState.Pressed)
                                    {
                                        refresh[i] = false;
                                    }//if the player presses X the refresh option is set to false
                                    if (player[i].getRec().Intersects(magicBox))
                                    {
                                        boxContact = true;
                                    }
                                    else
                                    {
                                        boxContact = false;
                                    }//determines if the player is in contact with the box
                                    if (player[i].getRec().Intersects(magicBox) && pad[i].Buttons.X == ButtonState.Pressed && player[i].getPoints() >= 1000)
                                    {
                                        spinning = true;
                                        countdown = true;
                                        currentPlayer = i;
                                        player[i].losePoints();
                                    }//starts the timer and randomly giving the player a projectile, the player loses 5000 points 

                                    if (spinning)
                                    {
                                        currentProj[currentPlayer] = proj[rnd.Next(0, 3)];
                                    }//gives the player using the box a random projectile
                                    currentPoints[i] = player[i].getPoints();
                                }
                            }
                            boxMsgWidth = font.MeasureString(boxMsg).X;
                            boxMsgHeight = font.MeasureString(boxMsg).Y;
                            //measures the width and height of the message that appears when a player intersects with the magic box
                            if (countdown)
                            {
                                boxTimer--;
                            }//starts the countdown of the box timer
                            if (boxTimer == 0)
                            {

                                spinning = false;
                                countdown = false;
                                boxTimer = 240;
                            }//when the box timer is 0 the player receives their random projectile and the timer is refreshed until its next use

                            tick++;//adds one to the tick every update to keep track fo every update
                            if (tick % 60 == 0 && zomboCounter < 15)
                            {
                                zomboMoving3[zomboCounter] = true;
                                zomboCounter++;
                            }//every 2 seconds a new zombo starts to move
                            for (int j = 0; j < zombo3.Count; j++)
                            {
                                if (pad[0].IsConnected)
                                    player1Distance[j] = Math.Sqrt((zombo3[j].getRec().Y - player[0].getRec().Y) * (zombo3[j].getRec().Y - player[0].getRec().Y) + (zombo3[j].getRec().X - player[0].getRec().X) * (zombo3[j].getRec().X - player[0].getRec().X));
                                if (pad[1].IsConnected)
                                    player2Distance[j] = Math.Sqrt((zombo3[j].getRec().Y - player[1].getRec().Y) * (zombo3[j].getRec().Y - player[1].getRec().Y) + (zombo3[j].getRec().X - player[1].getRec().X) * (zombo3[j].getRec().X - player[1].getRec().X));
                                if (pad[2].IsConnected)
                                    player3Distance[j] = Math.Sqrt((zombo3[j].getRec().Y - player[2].getRec().Y) * (zombo3[j].getRec().Y - player[2].getRec().Y) + (zombo3[j].getRec().X - player[2].getRec().X) * (zombo3[j].getRec().X - player[2].getRec().X));
                                if (pad[3].IsConnected)
                                    player4Distance[j] = Math.Sqrt((zombo3[j].getRec().Y - player[3].getRec().Y) * (zombo3[j].getRec().Y - player[3].getRec().Y) + (zombo3[j].getRec().X - player[3].getRec().X) * (zombo3[j].getRec().X - player[3].getRec().X));
                            }//for each zombo the distance from each player is recorded if the controller for that player is connected

                            for (int k = 0; k < zombo3.Count; k++)
                            {
                                if (zomboMoving3[k])
                                {
                                    zombo3[k].trace(player, zombo, player1Distance, player2Distance, player3Distance, player4Distance);
                                }
                                if (zombo3[k].getfacingDown())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboDown");

                                }
                                if (zombo3[k].getfacingLeft())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboLeft");
                                }
                                if (zombo3[k].getfacingUp())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboUp");
                                }
                                if (zombo3[k].getfacingRight())
                                {
                                    zomboPic = Content.Load<Texture2D>("zomboRight");
                                }
                                zombo3[k].setPic(zomboPic);
                            }//if the zombie is moving it will follow the nearest player
                            if (bossHiveMind.getHealth() <= 0)
                            {
                                currentLevel = "End Game";
                            }
                            if (player[1] == null && player[2] == null && player[3] == null)
                            {
                                if (player[0].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }
                            else if (player[2] == null && player[3] == null)
                            {
                                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }
                            else if (player[3] == null)
                            {
                                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0 && player[2].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }
                            else
                            {
                                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0 && player[2].getHealth() <= 0 && player[3].getHealth() <= 0)
                                {
                                    currentLevel = "Game Over";
                                }
                            }//if all the connected player's health has reached 0 the level changes to game over
                        }
                    }
                    if (gamestate == "Paused")
                    {
                        MediaPlayer.Pause();
                        if (pad[0].Buttons.A == ButtonState.Pressed)
                        {
                            MediaPlayer.Resume();
                            gamestate = "Playing";
                        }
                        if (pad[0].Buttons.Back == ButtonState.Pressed)
                            this.Exit();
                    }//if the game is paused the music is paused until player 1 presses the A button to resume
                    if (currentLevel == "Game Over")
                    {
                        if (pad[0].Buttons.Back == ButtonState.Pressed)
                            this.Exit();
                        backgroundMusic = Content.Load<Song>("You Died");
                        MediaPlayer.Play(backgroundMusic);
                    }//plays the defeated sound effect and the player 1 can exit the game
                    if(currentLevel == "End Game")
                    {
                        MediaPlayer.Stop();
                        if(pad[0].Buttons.B == ButtonState.Pressed)
                        {
                            CurrentGameState = Gamestate.MainMenu;
                        }
                    }//returns the player to the start menu 
                    break;

                case Gamestate.Options:
                    if (pad[0].Buttons.B == ButtonState.Pressed && oldpad[0].Buttons.B == ButtonState.Released)
                    {
                        CurrentGameState = Gamestate.MainMenu;
                    }
                    if(CurrentGameState == Gamestate.MainMenu)
                    {
                        
                    }
                    oldpad[0] = pad[0];
                    break;

                case Gamestate.Exit:

                    this.Exit();

                    break;
            }


                    base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case Gamestate.MainMenu:

                    spriteBatch.Draw(Content.Load<Texture2D>("mainbg"), backgroundRec, Color.White);
                    btnply.Draw(spriteBatch);
                    btnOpt.Draw(spriteBatch);
                    btnEsc.Draw(spriteBatch);

                    break;

                case Gamestate.Playing:
                    if (currentLevel == "Level 1")
                    {
                        spriteBatch.Draw(backgroundPic, backgroundRec, Color.White);//draws the background
                        spriteBatch.Draw(magicBoxPic, magicBox, Color.White);//draws the magic box

                        if (pad[0].IsConnected)
                        {
                            if (player[0].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "1: Health: " + Convert.ToString(player[0].getHealth()) + " Equiped: " + currentProj[0], new Vector2(0, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[0].getPoints()), new Vector2(0, 20), Color.White);
                                if (refresh[0])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(0, 40), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "1: Dead " + " Equiped: " + currentProj[0], new Vector2(0, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[0].getPoints()), new Vector2(0, 20), Color.White);
                                if (refresh[0])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(0, 40), Color.White);
                                }
                            }

                        }
                        if (pad[1].IsConnected)
                        {
                            if (player[1].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "2: Health: " + Convert.ToString(player[1].getHealth()) + " Equiped: " + currentProj[1], new Vector2(GraphicsDevice.Viewport.Width - 400, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, 20), Color.White);
                                if (refresh[1])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 400, 40), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "2: Dead " + " Equiped: " + currentProj[1], new Vector2(GraphicsDevice.Viewport.Width - 400, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, 20), Color.White);
                                if (refresh[1])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 400, 40), Color.White);
                                }
                            }
                        }
                        if (pad[2].IsConnected)
                        {
                            if (player[2].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "3: Health: " + Convert.ToString(player[2].getHealth()) + " Equiped: " + currentProj[2] + "  Points: " + Convert.ToString(player[2].getPoints()), new Vector2(0, GraphicsDevice.Viewport.Height - 50), Color.White);

                                if (refresh[2])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(0, GraphicsDevice.Viewport.Height - 30), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "3: Dead " + " Equiped: " + currentProj[2], new Vector2(0, GraphicsDevice.Viewport.Height - 50), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[2].getPoints()), new Vector2(0, -50), Color.White);
                                if (refresh[2])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(470, GraphicsDevice.Viewport.Height - 30), Color.White);
                                }
                            }

                        }
                        if (pad[3].IsConnected)
                        {
                            if (player[3].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "4: Health: " + Convert.ToString(player[3].getHealth()) + " Equiped: " + currentProj[3] + "  Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 500, GraphicsDevice.Viewport.Height - 50), Color.White);

                                if (refresh[3])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 500, GraphicsDevice.Viewport.Height - 30), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "4: Dead " + " Equiped: " + currentProj[3], new Vector2(GraphicsDevice.Viewport.Width - 500, GraphicsDevice.Viewport.Height - 50), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, GraphicsDevice.Viewport.Height - 40), Color.White);
                                if (refresh[3])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 500, GraphicsDevice.Viewport.Height - 30), Color.White);
                                }
                            }

                        }//draws the UI for all the connected players and their current projectile

                        spriteBatch.DrawString(font, "Titanus ", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, GraphicsDevice.Viewport.Height - 100), Color.White);
                        //draws the name of the boss

                        Titanus.Draw(spriteBatch);
                        //draws the boss
                        spriteBatch.Draw(Content.Load<Texture2D>("HealthBar"), new Rectangle(GraphicsDevice.Viewport.Width / 2 - 600, GraphicsDevice.Viewport.Height - 70, Titanus.getHealth(), 10), Color.White);
                        //draws the healthbar of the boss(has alot of health so it takes a while to visually be seen)

                        if (boxContact)
                        {
                            spriteBatch.DrawString(font, boxMsg, new Vector2(GraphicsDevice.Viewport.Width / 2 - boxMsgWidth / 2, GraphicsDevice.Viewport.Height - boxMsgHeight), Color.White);
                        }//if the player is in contact with the box a message will appear instructing the player on its use


                        for (int i = 0; i < pad.Length; i++)
                        {
                            if (pad[i].IsConnected)
                            {
                                player[i].Draw(spriteBatch);
                                player[i].projDraw(spriteBatch);

                            }
                        }//draws the player models of the connected players

                        for (int j = 0; j < zombo.Count; j++)
                        {
                            zombo[j].Draw(spriteBatch);
                        }//draws all the zombos

                    }
                    if (currentLevel == "Break")
                    {
                        spriteBatch.Draw(Content.Load<Texture2D>("Loading Screen"), backgroundRec, Color.White);
                    }
                    if (currentLevel == "Level 2")
                    {
                        spriteBatch.Draw(backgroundPic, backgroundRec, Color.White);//draws the background
                        spriteBatch.Draw(magicBoxPic, magicBox, Color.White);//draws the magic box
                        bossTobu.drawBossProj(spriteBatch);
                        if (pad[0].IsConnected)
                        {
                            if (player[0].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "1: Health: " + Convert.ToString(player[0].getHealth()) + " Equiped: " + currentProj[0], new Vector2(0, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[0].getPoints()), new Vector2(0, 20), Color.White);
                                if (refresh[0])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(0, 40), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "1: Dead " + " Equiped: " + currentProj[0], new Vector2(0, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[0].getPoints()), new Vector2(0, 20), Color.White);
                                if (refresh[0])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(0, 40), Color.White);
                                }
                            }

                        }
                        if (pad[1].IsConnected)
                        {
                            if (player[1].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "2: Health: " + Convert.ToString(player[1].getHealth()) + " Equiped: " + currentProj[1], new Vector2(GraphicsDevice.Viewport.Width - 400, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, 20), Color.White);
                                if (refresh[1])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 400, 40), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "2: Dead " + " Equiped: " + currentProj[1], new Vector2(GraphicsDevice.Viewport.Width - 400, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, 20), Color.White);
                                if (refresh[1])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 400, 40), Color.White);
                                }
                            }
                        }
                        if (pad[2].IsConnected)
                        {
                            if (player[2].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "3: Health: " + Convert.ToString(player[2].getHealth()) + " Equiped: " + currentProj[2] + "  Points: " + Convert.ToString(player[2].getPoints()), new Vector2(0, GraphicsDevice.Viewport.Height - 30), Color.White);

                                if (refresh[2])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(470, GraphicsDevice.Viewport.Height - 30), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "3: Dead " + " Equiped: " + currentProj[2], new Vector2(0, GraphicsDevice.Viewport.Height - 30), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[2].getPoints()), new Vector2(0, -50), Color.White);
                                if (refresh[2])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(470, GraphicsDevice.Viewport.Height - 30), Color.White);
                                }
                            }

                        }
                        if (pad[3].IsConnected)
                        {
                            if (player[3].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "4: Health: " + Convert.ToString(player[3].getHealth()) + " Equiped: " + currentProj[3], new Vector2(GraphicsDevice.Viewport.Width - 400, GraphicsDevice.Viewport.Height - 20), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, GraphicsDevice.Viewport.Height - 40), Color.White);
                                if (refresh[3])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 270, GraphicsDevice.Viewport.Height - 40), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "4: Dead " + " Equiped: " + currentProj[3], new Vector2(GraphicsDevice.Viewport.Width - 400, GraphicsDevice.Viewport.Height - 20), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, GraphicsDevice.Viewport.Height - 40), Color.White);
                                if (refresh[3])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 270, GraphicsDevice.Viewport.Height - 40), Color.White);
                                }
                            }

                        }//draws the UI for all the connected players and their current projectile
                        if (boxContact)
                        {
                            spriteBatch.DrawString(font, boxMsg, new Vector2(GraphicsDevice.Viewport.Width / 2 - boxMsgWidth / 2, GraphicsDevice.Viewport.Height - boxMsgHeight), Color.White);
                        }//if the player is in contact with the box a message will appear instructing the player on its use

                        bossTobu.Draw(spriteBatch);//draws the boss
                        spriteBatch.DrawString(font, "Tobu ", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, GraphicsDevice.Viewport.Height - 100), Color.White);
                        spriteBatch.Draw(Content.Load<Texture2D>("HealthBar"), new Rectangle(GraphicsDevice.Viewport.Width / 2 - 600, GraphicsDevice.Viewport.Height - 50, bossTobu.getHealth(), 10), Color.White);//draws the bosses name above the health bar(has alot of health so it takes a while to visually be seen)
                        for (int i = 0; i < pad.Length; i++)
                        {
                            if (pad[i].IsConnected)
                            {
                                player[i].Draw(spriteBatch);
                                player[i].projDraw(spriteBatch);
                            }
                        }//draws the player models of the connected players

                        for (int j = 0; j < zombo2.Count; j++)
                        {
                            zombo2[j].Draw(spriteBatch);
                        }//draws all the zombos

                    }
                    if (currentLevel == "Level 3")
                    {
                        spriteBatch.Draw(backgroundPic, backgroundRec, Color.White);//draws the background
                        spriteBatch.Draw(magicBoxPic, magicBox, Color.White);//draws the magic box

                        if (pad[0].IsConnected)
                        {
                            if (player[0].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "1: Health: " + Convert.ToString(player[0].getHealth()) + " Equiped: " + currentProj[0], new Vector2(0, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[0].getPoints()), new Vector2(0, 20), Color.White);
                                if (refresh[0])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(0, 40), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "1: Dead " + " Equiped: " + currentProj[0], new Vector2(0, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[0].getPoints()), new Vector2(0, 20), Color.White);
                                if (refresh[0])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(0, 40), Color.White);
                                }
                            }

                        }
                        if (pad[1].IsConnected)
                        {
                            if (player[1].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "2: Health: " + Convert.ToString(player[1].getHealth()) + " Equiped: " + currentProj[1], new Vector2(GraphicsDevice.Viewport.Width - 400, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, 20), Color.White);
                                if (refresh[1])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 400, 40), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "2: Dead " + " Equiped: " + currentProj[1], new Vector2(GraphicsDevice.Viewport.Width - 400, 0), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, 20), Color.White);
                                if (refresh[1])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 400, 40), Color.White);
                                }
                            }
                        }
                        if (pad[2].IsConnected)
                        {
                            if (player[2].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "3: Health: " + Convert.ToString(player[2].getHealth()) + " Equiped: " + currentProj[2] + "  Points: " + Convert.ToString(player[2].getPoints()), new Vector2(0, GraphicsDevice.Viewport.Height - 30), Color.White);

                                if (refresh[2])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(470, GraphicsDevice.Viewport.Height - 30), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "3: Dead " + " Equiped: " + currentProj[2], new Vector2(0, GraphicsDevice.Viewport.Height - 30), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[2].getPoints()), new Vector2(0, -50), Color.White);
                                if (refresh[2])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(470, GraphicsDevice.Viewport.Height - 30), Color.White);
                                }
                            }

                        }
                        if (pad[3].IsConnected)
                        {
                            if (player[3].getHealth() > 0)
                            {
                                spriteBatch.DrawString(font, "4: Health: " + Convert.ToString(player[3].getHealth()) + " Equiped: " + currentProj[3], new Vector2(GraphicsDevice.Viewport.Width - 400, GraphicsDevice.Viewport.Height - 20), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, GraphicsDevice.Viewport.Height - 40), Color.White);
                                if (refresh[3])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 270, GraphicsDevice.Viewport.Height - 40), Color.White);
                                }
                            }
                            else
                            {
                                spriteBatch.DrawString(font, "4: Dead " + " Equiped: " + currentProj[3], new Vector2(GraphicsDevice.Viewport.Width - 400, GraphicsDevice.Viewport.Height - 20), Color.White);
                                spriteBatch.DrawString(font, "Points: " + Convert.ToString(player[1].getPoints()), new Vector2(GraphicsDevice.Viewport.Width - 400, GraphicsDevice.Viewport.Height - 40), Color.White);
                                if (refresh[3])
                                {
                                    spriteBatch.DrawString(font, "Press X to Refresh", new Vector2(GraphicsDevice.Viewport.Width - 270, GraphicsDevice.Viewport.Height - 40), Color.White);
                                }
                            }
                        }//draws the UI for all the connected players and their current projectile
                        if (boxContact)
                        {
                            spriteBatch.DrawString(font, boxMsg, new Vector2(GraphicsDevice.Viewport.Width / 2 - boxMsgWidth / 2, GraphicsDevice.Viewport.Height - boxMsgHeight), Color.White);
                        }//if the player is in contact with the box a message will appear instructing the player on its use
                        bossHiveMind.Draw(spriteBatch);//draws the boss
                        spriteBatch.DrawString(font, "HiveMind ", new Vector2(GraphicsDevice.Viewport.Width / 2 - 600, GraphicsDevice.Viewport.Height - 100), Color.White);
                        spriteBatch.Draw(Content.Load<Texture2D>("HealthBar"), new Rectangle(GraphicsDevice.Viewport.Width / 2 - 600, GraphicsDevice.Viewport.Height - 50, bossHiveMind.getHealth(), 10), Color.White);//draws the bosses name above the health bar(has alot of health so it takes a while to visually be seen)
                        for (int i = 0; i < pad.Length; i++)
                        {
                            if (pad[i].IsConnected)
                            {
                                player[i].Draw(spriteBatch);
                                player[i].projDraw(spriteBatch);
                            }
                        }//draws the player models of the connected players

                        for (int j = 0; j < zombo3.Count; j++)
                        {
                            zombo3[j].Draw(spriteBatch);
                        }//draws all the zombos
                        bossHiveMind.drawHive(spriteBatch);
                    }
                    if (currentLevel == "Game Over")
                    {
                        spriteBatch.Draw(Content.Load<Texture2D>("gameoverscreen"), backgroundRec, Color.White);
                    }//draws the game over screen
                    if (gamestate == "Paused")
                    {
                        spriteBatch.Draw(Content.Load<Texture2D>("Pause Menu"), backgroundRec, Color.White);
                    }//draws the pause screen when the game is paused
                    if(gamestate == "End Game")
                    {

                    }//draws the end game screen
                    break;

                case Gamestate.Options:
                    spriteBatch.Draw(Content.Load<Texture2D>("controls"), backgroundRec, Color.White);//draws the controls for the game
                    break;

                case Gamestate.Exit:

                    break;
            }
            spriteBatch.End();
                    base.Draw(gameTime);
        }
    }
}
