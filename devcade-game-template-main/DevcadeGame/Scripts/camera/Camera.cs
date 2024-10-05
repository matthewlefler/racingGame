using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.VisualBasic.FileIO;

namespace CameraClass
{
    /// <summary>
    /// returns an orbit camera object
    /// </summary>
    public class Camera
    {
        public Vector3 position { get; set; }
        public Vector3 rotation { get; set; }

        public float speed = 2f;

        public Matrix viewMatrix { get { return Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(position); } private set { } }
        public Matrix projectionMatrix;

        public Camera(Vector3 position, Vector3 rotation, GraphicsDeviceManager graphics)
        {
            this.position = position;
            this.rotation = rotation;

            //view space to screen space matrix
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 4f, (float)graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight, 0.01f, 1000f);
            //projectionMatrix = Matrix.CreateOrthographic(30 * graphicsDevice.Viewport.AspectRatio, 30, 0.01f, 100f);            
        }

        public void deltaMove(Vector3 translation)
        {
            this.position += translation * speed;
        }

        public void absoluteMove(Vector3 position)
        {
            this.position = position;
        }
    }


}
