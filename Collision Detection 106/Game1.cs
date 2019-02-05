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
		
		Texture2D player;
		Texture2D playerLeft;
		Rectangle playerRectangle;
		bool facingLeft = false;

		Texture2D block;
		List<Rectangle> rectList;
		Random rnd = new Random();
		List<int> directions;

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
			// TODO: Add your initialization logic here

			playerRectangle = new Rectangle(0,0, 40,45);
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

			// TODO: use this.Content to load your game content here
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

			KeyboardState keyboard = Keyboard.GetState();

			if(keyboard.IsKeyDown(Keys.A) && playerRectangle.X > GraphicsDevice.Viewport.X)
			{
				facingLeft = true;
				playerRectangle.X -= 2;
			}
			if(keyboard.IsKeyDown(Keys.D) && playerRectangle.X < GraphicsDevice.Viewport.Width - playerRectangle.Width)
			{
				facingLeft = false;
				playerRectangle.X += 2;
			}

			if(keyboard.IsKeyDown(Keys.W) && playerRectangle.Y > GraphicsDevice.Viewport.Y)
				playerRectangle.Y -= 2;
			if(keyboard.IsKeyDown(Keys.S) && playerRectangle.Y < GraphicsDevice.Viewport.Height - playerRectangle.Height)
				playerRectangle.Y += 2;

			for(int i = 0; i < rectList.Count; i++)
			{
				Rectangle temp = rectList[i];
				
				if(temp.X >= GraphicsDevice.Viewport.Width - temp.Width)
					directions[i] = -1;
				if(temp.X <= GraphicsDevice.Viewport.X)
					directions[i] = 1;

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

			if(facingLeft)
				spriteBatch.Draw(playerLeft, playerRectangle, Color.White);
			else
				spriteBatch.Draw(player, playerRectangle, Color.White);



			for(int i = 0; i < rectList.Count; i ++)
			{
				
				if(rectList[i].Intersects(playerRectangle))
					spriteBatch.Draw(block, rectList[i], Color.Blue);
				else
					spriteBatch.Draw(block, rectList[i], Color.White);

			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
