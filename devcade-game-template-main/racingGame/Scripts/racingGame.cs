using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Diagnostics;

using CameraClass;

using ParticleManagerClass;
using ParticleClass;

using EntityManagerClass;
using EntityClass;

using solidColorTextures;
using input;
using System.Collections.Generic;
using System;

using FireEffectClass;
using CubeClass;
using CylinderClass;
using WheelClass;
using System.IO;
using System.Linq.Expressions;

// MAKE SURE YOU RENAME ALL PROJECT FILES FROM DevcadeGame TO YOUR YOUR GAME NAME
namespace RacingGame
{
	public class Game1 : Game
	{
		private static SpriteFont DebugFont; 

		private GraphicsDeviceManager _graphics;
		private static SpriteBatch _spriteBatch;
		private static TextureMaker textureMaker = new TextureMaker();

		private static Camera cameraPlayer1;


		private static ParticleManager particleManager;
		private static EntityManager entityManager;
		private static InputHandler inputHandler;
		private enum Actions
		{
			translateForward,
			translateBack,
			translateLeft,
			translateRight,
			translateUp,
			translateDown,
			rotateUp,
			rotateDown,
			rotateLeft,
			rotateRight,

			boost,
		}


		private static Texture2D textureTest;
		private static VertexPositionColorNormalTexture[] debugAxes;
		private static Texture2D axesTexture;

		private static Random random;

		
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
			_graphics.PreferredBackBufferWidth = 1420;
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
			entityManager = new EntityManager();

			cameraPlayer1 = new Camera(new Vector3(0, 0, 40), Vector3.Zero, _graphics);

			random = new Random(Seed: 1);

			debugAxes = new VertexPositionColorNormalTexture[6];
			debugAxes[0] = new VertexPositionColorNormalTexture(Vector3.Zero, Color.White, Vector3.Up, new Vector2(0.5f, 0.5f));
			debugAxes[1] = new VertexPositionColorNormalTexture(Vector3.Right, Color.White, Vector3.Up, new Vector2(0.5f, 0.5f));
			debugAxes[2] = new VertexPositionColorNormalTexture(Vector3.Zero, Color.White, Vector3.Up, new Vector2(0, 0));
			debugAxes[3] = new VertexPositionColorNormalTexture(Vector3.Up, Color.White, Vector3.Up, new Vector2(0, 0));
			debugAxes[4] = new VertexPositionColorNormalTexture(Vector3.Zero, Color.White, Vector3.Up, new Vector2(1, 0));
			debugAxes[5] = new VertexPositionColorNormalTexture(Vector3.Forward, Color.White, Vector3.Up, new Vector2(1, 0));

			//entityManager.add(new FireEffect(particleManager, Vector3.Right, Vector3.Zero, new Vector3(4f, 2f, 4f), new Vector3(5f, 4f, 5f), 320f, 0f, 100f, _graphics.GraphicsDevice, cameraPlayer1));
			



#region userInputKeybindCreation

#if DEBUG

			Dictionary<int, inputKey> actionToKeyInput = new Dictionary<int, inputKey>();
			
			actionToKeyInput.Add((int) Actions.translateForward, new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.W} ));
			actionToKeyInput.Add((int) Actions.translateBack,    new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.S} ));
			actionToKeyInput.Add((int) Actions.translateLeft,    new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.A} ));
			actionToKeyInput.Add((int) Actions.translateRight,   new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.D} ));
			actionToKeyInput.Add((int) Actions.translateUp,      new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.E} ));
			actionToKeyInput.Add((int) Actions.translateDown,    new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.Q} ));

			actionToKeyInput.Add((int) Actions.rotateUp,         new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.Up} ));
			actionToKeyInput.Add((int) Actions.rotateDown,       new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.Down} ));
			actionToKeyInput.Add((int) Actions.rotateLeft,       new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.Left} ));
			actionToKeyInput.Add((int) Actions.rotateRight,      new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.Right} ));

			actionToKeyInput.Add((int) Actions.boost,            new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.LeftShift} ));
			

			Dictionary<int, string> actionToString = new Dictionary<int, string>();
			
			actionToString.Add((int) Actions.translateForward, "translate forward");
			actionToString.Add((int) Actions.translateBack,    "translate back");
			actionToString.Add((int) Actions.translateLeft,    "translate left");
			actionToString.Add((int) Actions.translateRight,   "translate right");
			actionToString.Add((int) Actions.translateUp,      "translate up");
			actionToString.Add((int) Actions.translateDown,    "translate down");

			actionToString.Add((int) Actions.rotateUp,         "rotate up");
			actionToString.Add((int) Actions.rotateDown, 	   "rotate down");
			actionToString.Add((int) Actions.rotateLeft,       "rotate left");
			actionToString.Add((int) Actions.rotateRight,      "rotate right");

			actionToString.Add((int) Actions.boost,          "Boost");
#else

			Dictionary<int, inputKey> actionToKeyInput = new Dictionary<int, inputKey>();
			
			actionToKeyInput.Add((int) Actions.translateForward, new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.W} ));
			actionToKeyInput.Add((int) Actions.translateBack,    new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.S} ));
			actionToKeyInput.Add((int) Actions.translateLeft,    new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.A} ));
			actionToKeyInput.Add((int) Actions.translateRight,   new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.D} ));
			actionToKeyInput.Add((int) Actions.rotateUp,         new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.Up} ));
			actionToKeyInput.Add((int) Actions.rotateDown,       new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.Down} ));
			actionToKeyInput.Add((int) Actions.rotateLeft,       new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.Left} ));
			actionToKeyInput.Add((int) Actions.rotateRight,      new inputKey( new (int?, Input.ArcadeButtons?)[] {  }, new Keys?[] {Keys.Right} ));

			Dictionary<int, string> actionToString = new Dictionary<int, string>();
			
			actionToString.Add((int) Actions.translateForward, "translate forward");
			actionToString.Add((int) Actions.translateBack,    "translate back");
			actionToString.Add((int) Actions.translateLeft,    "translate left");
			actionToString.Add((int) Actions.translateRight,   "translate right");
			actionToString.Add((int) Actions.rotateUp,         "rotate up");
			actionToString.Add((int) Actions.rotateDown, 	   "rotate down");
			actionToString.Add((int) Actions.rotateLeft,       "rotate left");
			actionToString.Add((int) Actions.rotateRight,      "rotate right");

#endif

			inputHandler = new InputHandler(actionToKeyInput, actionToString);

#endregion

			base.Initialize();
		}

		static CollisionCube testCube;
		static Wheel wheel;

		/// <summary>
		/// Performs any setup that requires loaded content before the first frame.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			textureTest = Content.Load<Texture2D>("uvTexture");

			DebugFont = Content.Load<SpriteFont>("DebigFont");
			Texture2D tireTexure = Content.Load<Texture2D>("tireTexture");
			axesTexture = Content.Load<Texture2D>("redGreenBlueTex");

			testCube = new CollisionCube(10,2,10, Vector3.Up * 10, Vector3.Zero, textureTest, GraphicsDevice);
			entityManager.add(testCube);
			//entityManager.add(new Cylinder(2f, 1f, 30, new Vector3(0,10,0), Vector3.Zero, textureTest, GraphicsDevice));
			wheel = new Wheel(new Vector3(0, 20, 0), new Vector3(0, MathHelper.PiOver2, 0), textureTest, GraphicsDevice);
			entityManager.add(wheel);
			
			// TODO: use this.Content to load your game content here
			// ex:
			// texture = Content.Load<Texture2D>("fileNameWithoutExtension");
		}

		float deltaTime;
		/// <summary>
		/// Your main update loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Update(GameTime gameTime)
		{
			deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

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

			particleManager.physicsTick(deltaTime);
			entityManager.physicsTick(deltaTime);

#if DEBUG
			//camera translation
			if(inputHandler.isKeyDown((int)Actions.translateForward)) { cameraPlayer1.deltaMove(Vector3.Forward  * deltaTime * 2f ); }
			if(inputHandler.isKeyDown((int)Actions.translateBack))    { cameraPlayer1.deltaMove(Vector3.Backward * deltaTime * 2f ); }
			if(inputHandler.isKeyDown((int)Actions.translateLeft))    { cameraPlayer1.deltaMove(Vector3.Left     * deltaTime * 2f ); }
			if(inputHandler.isKeyDown((int)Actions.translateRight))   { cameraPlayer1.deltaMove(Vector3.Right    * deltaTime * 2f ); }
			if(inputHandler.isKeyDown((int)Actions.translateUp))      { cameraPlayer1.deltaMove(Vector3.Up       * deltaTime * 2f ); }
			if(inputHandler.isKeyDown((int)Actions.translateDown))    { cameraPlayer1.deltaMove(Vector3.Down     * deltaTime * 2f ); }

			// camera rotation
			if(inputHandler.isKeyDown((int)Actions.rotateUp))         { cameraPlayer1.rotation += new Vector3(deltaTime, 0, 0)  * 2f; }
			if(inputHandler.isKeyDown((int)Actions.rotateDown))       { cameraPlayer1.rotation += new Vector3(-deltaTime, 0, 0) * 2f; }
			if(inputHandler.isKeyDown((int)Actions.rotateLeft))       { cameraPlayer1.rotation += new Vector3(0, deltaTime, 0) * 2f; }
			if(inputHandler.isKeyDown((int)Actions.rotateRight))      { cameraPlayer1.rotation += new Vector3(0, -deltaTime, 0)  * 2f; }

			//other camera movement modifiers
			if(inputHandler.isKeyDown((int)Actions.boost))            { cameraPlayer1.speed = 8f; } else { cameraPlayer1.speed = 2f; }
			//if(inputHandler.isKeyDown((int)Actions.boost))            { testCube.position += Vector3.Down * deltaTime; }

#else

#endif

			base.Update(gameTime);
		}

		/// <summary>
		/// Your main draw loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Draw(GameTime gameTime)
		{
			RasterizerState rasterizer = new RasterizerState();
			rasterizer.CullMode = CullMode.CullClockwiseFace;
			GraphicsDevice.RasterizerState = rasterizer;
			GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			GraphicsDevice.Clear(Color.LightCyan);
			BasicEffect basicEffect = new BasicEffect(GraphicsDevice);

			basicEffect.TextureEnabled = true;
			basicEffect.LightingEnabled = true;

			basicEffect.EmissiveColor = Vector3.One;

			basicEffect.View = cameraPlayer1.viewMatrix;
			basicEffect.Projection = cameraPlayer1.projectionMatrix;
			
			// Batches all the draw calls for this frame, and then performs them all at once
			_spriteBatch.Begin();
			// TODO: Add your drawing code here

			particleManager.draw(GraphicsDevice, basicEffect);
			entityManager.draw(basicEffect);

			_spriteBatch.DrawString(DebugFont, cameraPlayer1.rotation.ToString(), new Vector2(10, 10), Color.DarkOrange);
			_spriteBatch.DrawString(DebugFont, cameraPlayer1.position.ToString(), new Vector2(10, 40), Color.DarkGreen);

			for(int i = 0; i < entityManager.physicalEntities.Count; i++)
			{
				_spriteBatch.DrawString(DebugFont, entityManager.physicalEntities[i].position.ToString(), new Vector2(10, 60 + 20 * i), Color.DeepPink);
				_spriteBatch.DrawString(DebugFont, entityManager.physicalEntities[i].rotation.ToString(), new Vector2(310, 60 + 20 * i), Color.DarkOrange);
				_spriteBatch.DrawString(DebugFont, entityManager.physicalEntities[i].boundingMesh.rotation.ToString(), new Vector2(610, 60 + 20 * i), Color.Black);
			}

			_spriteBatch.DrawString(DebugFont, (1f/(float)gameTime.ElapsedGameTime.TotalSeconds).ToString(), new Vector2(1000, 40), Color.DarkGreen);

			_spriteBatch.Draw(textureTest, new Rectangle(10, 500, 100, 100), Color.White);
#if DEBUG
			BasicEffect axesEffect = new BasicEffect(GraphicsDevice);

			axesEffect.EnableDefaultLighting();
			axesEffect.VertexColorEnabled = true;
			axesEffect.LightingEnabled = true;
			axesEffect.TextureEnabled = true;
			axesEffect.Texture = axesTexture;

			axesEffect.World = Matrix.CreateTranslation(Vector3.Up * 17);
			axesEffect.View = cameraPlayer1.viewMatrix;
			axesEffect.Projection = cameraPlayer1.projectionMatrix;

			foreach(EffectTechnique technique in axesEffect.Techniques)
			{
				foreach(EffectPass pass in technique.Passes)
				{
					pass.Apply();
					GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, debugAxes, 0, 3, VertexPositionColorNormalTexture.VertexDeclaration);
				}
			}
#endif
			
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}