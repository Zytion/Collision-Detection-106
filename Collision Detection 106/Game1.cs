using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Collision_Detection_106
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		/// <summary> Player sprite </summary>
		Texture2D player;
		/// <summary> Player sprite (fliped horizontally)</summary>
		Texture2D playerLeft;
		/// <summary> Player rectangle (coordenets and size) </summary>
		Rectangle playerRectangle;
		/// <summary> Is the player facing left? </summary>
		bool facingLeft = false;

		/// <summary> Block sprite </summary>
		Texture2D block;
		/// <summary> List of all the blocks' rectangles</summary>
		List<Rectangle> rectList;
		/// <summary> List of all the blocks' directions (used for bounce effect) </summary>
		List<int> directions;
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
			//Create player rectangle
			playerRectangle = new Rectangle(0,0, 40,45);

			//Generates 5-10 blocks (along with their directions) randomlly scattered around the screen
			rectList = new List<Rectangle>();
			directions = new List<int>();
			int blockNumber = rnd.Next(5, 10);

			for(int i = 0; i < blockNumber; i++)
			{
				rectList.Add(new Rectangle(rnd.Next(GraphicsDevice.Viewport.Width), rnd.Next(GraphicsDevice.Viewport.Height - 33), 33, 33));
				directions.Add(1);
			}

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

			player = Content.Load<Texture2D>("terrariaguide");
			playerLeft = Content.Load<Texture2D>("terrariaguideleft");
			block = Content.Load<Texture2D>("Mario_brick");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			//Get keyboard state
			KeyboardState keyboard = Keyboard.GetState();

			//Pressing "A" Key moves the player to the left
			//If the player isn't at the left edge of the screen
			if(keyboard.IsKeyDown(Keys.A) && playerRectangle.X > GraphicsDevice.Viewport.X)
			{
				facingLeft = true;
				playerRectangle.X -= 2;
			}
			//Pressing the "D" key moves the player to the right
			//If the player isn't at the right edge of the screen
			if(keyboard.IsKeyDown(Keys.D) && playerRectangle.X < GraphicsDevice.Viewport.Width - playerRectangle.Width)
			{
				facingLeft = false;
				playerRectangle.X += 2;
			}
			//Pressing the "W" key moves the player up
			//If the player isn't at the top of the screen
			if(keyboard.IsKeyDown(Keys.W) && playerRectangle.Y > GraphicsDevice.Viewport.Y)
				playerRectangle.Y -= 2;
			//Pressing the "S" key moves the player down
			//If the player isn't at the bottom of the screen
			if(keyboard.IsKeyDown(Keys.S) && playerRectangle.Y < GraphicsDevice.Viewport.Height - playerRectangle.Height)
				playerRectangle.Y += 2;

			//Blocks move horizontally and bounce off the edges of the screen
			for(int i = 0; i < rectList.Count; i++)
			{
				Rectangle temp = rectList[i];
				
				//If at the edge of the screen, change direction
				if(temp.X >= GraphicsDevice.Viewport.Width - temp.Width || temp.X <= GraphicsDevice.Viewport.X)
					directions[i] *= -1;

				temp.X += 2 * directions[i];
				
				rectList[i] = temp;
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// TODO: Add your drawing code here

			spriteBatch.Begin();

			//Draws the left or right facing player
			if(facingLeft)
				spriteBatch.Draw(playerLeft, playerRectangle, Color.White);
			else
				spriteBatch.Draw(player, playerRectangle, Color.White);

			//Checks each block for collion with the player and draws the block 
			for(int i = 0; i < rectList.Count; i ++)
			{
				//If the player is in contact with the block, draw it as blue
				if(rectList[i].Intersects(playerRectangle))
					spriteBatch.Draw(block, rectList[i], Color.Blue);
				//Otherwise draw it normally
				else
					spriteBatch.Draw(block, rectList[i], Color.White);

			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
