using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuadClass;

namespace ParticleClass
{
    public class Particle
    {
        #region parameters
        Texture2D texture;
        public Vector3 position {get { return quad.position; } set { quad.position = value; calculateWorldMatrix(); }} 
        public Vector3 rotation {get { return quad.rotation; } set { quad.rotation = value; calculateWorldMatrix(); }}  //in radians: rotation around the x, y, and z axies  
        private Matrix worldMatrix;

        public Vector3 acceleration;
        public float gravity { get; private set; }
        public Vector3 velocity;
        float scale {get { return quad.scale; } set { quad.scale = value; }}

        public float lifeTime = 0f;

        Quad quad;

        #endregion

        public Particle(Texture2D texture, Vector3 position, Vector3 rotation, float scale, Vector3 velocity, Vector3 acceleration, float gravity)
        {
            this.texture = texture;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.gravity = gravity;

            this.quad = new Quad(position, rotation, scale);
        }

        public void draw(Effect effect, GraphicsDevice graphicsDevice)
        {
            effect.Parameters["World"].SetValue(this.worldMatrix);
            effect.Parameters["Texture"].SetValue(this.texture);

            foreach(EffectTechnique technique in effect.Techniques)
            {
                foreach(EffectPass pass in technique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawUserPrimitives<VertexPositionColorNormalTexture>(PrimitiveType.TriangleList, quad.vertices, 0, 2);
                }
            }
        }

        private void calculateWorldMatrix()
        {
            this.worldMatrix = Matrix.CreateFromYawPitchRoll(quad.rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(quad.position);
        }
    }
}