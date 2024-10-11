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

        private Vector3 _rotation;
        public Vector3 rotation { get{ return _rotation; } set{ _rotation = value; if(_rotation.Y > MathHelper.PiOver2) { _rotation.Y = MathHelper.PiOver2; } if(_rotation.Y < -1 * MathHelper.PiOver2) { _rotation.Y = -1 * MathHelper.PiOver2; } } }

        public float speed = 2f;

        public Matrix viewMatrix { get { return Matrix.CreateLookAt(position, position + Matrix.CreateFromYawPitchRoll(-rotation.X, rotation.Y, rotation.Z).Forward * 10, Vector3.Up); } private set { } }    
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
            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);
            this.position += Vector3.Transform(translation * speed, rotationMatrix);
        }

        public void absoluteMove(Vector3 position)
        {
            this.position = position;
        }
    }


}
