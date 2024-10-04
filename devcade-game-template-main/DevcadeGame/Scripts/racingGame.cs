using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Diagnostics;

using CameraClass;
using ParticleManagerClass;
using ParticleClass;
using input;
using System.Collections.Generic;

// MAKE SURE YOU RENAME ALL PROJECT FILES FROM DevcadeGame TO YOUR YOUR GAME NAME
namespace RacingGame
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private static Camera cameraPlayer1;


		private static ParticleManager particleManager;
		private static InputHandler inputHandler;
		private enum Actions
		{
			translateForward,
			translateBack,
			translateLeft,
			translateRight,
			rotateUp,
			rotateDown,
			rotateLeft,
			rotateRight,
		}


		private static Texture2D textureTest;

		
		/// <summary>
		/// Stores the window dimensions in a rectangle object for easy use
		/// </summary>
		private Rectangle windowSize;
		
		/// <summary>
		/// Game constructor
		/// </summary>
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = false;
		}

		/// <summary>
		/// Performs any setup that doesn't require loaded content before the first frame.
		/// </summary>
		protected override void Initialize()
		{
			// Sets up the input library
			Input.Initialize();
			//Persistence.Init(); Uncomment if using the persistence section for save and load

			// Set window size if running debug (in release it will be fullscreen)
			#region
#if DEBUG
			_graphics.PreferredBackBufferWidth = 420;
			_graphics.PreferredBackBufferHeight = 980;
			_graphics.ApplyChanges();
#else
			_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			_graphics.ApplyChanges();
#endif
			#endregion
			
			// TODO: Add your initialization logic here

			windowSize = GraphicsDevice.Viewport.Bounds;

			particleManager = new ParticleManager();

			cameraPlayer1 = new Camera(new Vector3(0, 0, -10), Vector3.Zero, _graphics);



			#region userInputKeybindCreation

			Dictionary<int, inputKey> actionToKeyInput = new Dictionary<int, inputKey>();
			
			actionToKeyInput.Add()
			actionToKeyInput.Add()
			actionToKeyInput.Add()
			actionToKeyInput.Add()
			actionToKeyInput.Add()
			actionToKeyInput.Add()
			actionToKeyInput.Add()
			actionToKeyInput.Add()



			Dictionary<int, string> actionToString = new Dictionary<int, string>();
			



			inputHandler = new InputHandler()







			#endregion


			
			base.Initialize();
		}

		/// <summary>
		/// Performs any setup that requires loaded content before the first frame.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			textureTest = Content.Load<Texture2D>("testparticleTexture");

			// TODO: use this.Content to load your game content here
			// ex:
			// texture = Content.Load<Texture2D>("fileNameWithoutExtension");
		}

		static float deltaTimeInSeconds;
		/// <summary>
		/// Your main update loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Update(GameTime gameTime)
		{
			deltaTimeInSeconds = gameTime.ElapsedGameTime.Seconds;

			Input.Update(); // Updates the state of the input library

			// Exit when both menu buttons are pressed (or escape for keyboard debugging)
			// You can change this but it is suggested to keep the keybind of both menu
			// buttons at once for a graceful exit.
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) ||
				(Input.GetButton(1, Input.ArcadeButtons.Menu) &&
				Input.GetButton(2, Input.ArcadeButtons.Menu)))
			{
				Exit();
			}

			// TODO: Add your update logic here


			particleManager.addParticle(new Particle(textureTest, Vector3.Up * 10f, Vector3.Zero, 1f, Vector3.Up * 2f));

			particleManager.physicsTick(deltaTimeInSeconds);

			


			base.Update(gameTime);
		}

		/// <summary>
		/// Your main draw loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Draw(GameTime gameTime)
		{
			RasterizerState rasterizer = new RasterizerState();
			rasterizer.CullMode = CullMode.None;
			GraphicsDevice.RasterizerState = rasterizer;

			GraphicsDevice.Clear(Color.LightCyan);
			BasicEffect basicEffect = new BasicEffect(GraphicsDevice);

			basicEffect.EmissiveColor = Vector3.One;
			basicEffect.TextureEnabled = true;
			basicEffect.AmbientLightColor = Vector3.One;
			basicEffect.LightingEnabled = true;

			basicEffect.View = cameraPlayer1.viewMatrix;
			basicEffect.Projection = cameraPlayer1.projectionMatrix;
			
			// Batches all the draw calls for this frame, and then performs them all at once
			_spriteBatch.Begin();
			// TODO: Add your drawing code here

			particleManager.draw(GraphicsDevice, basicEffect);
			
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}