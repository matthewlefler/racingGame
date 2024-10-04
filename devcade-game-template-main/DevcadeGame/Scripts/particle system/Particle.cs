using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;

namespace ParticleClass
{
    class Quad
    {
        public VertexPositionNormalTexture[] vertices { get; private set; }

        Vector3 _position;
        public Vector3 position { get { return _position; } set { _position = value; calcVertices(); } }

        Vector3 _rotation;
        public Vector3 rotation { get { return _rotation; } set { _rotation = value; calcVertices(); } }
        
        public float scale;

        public Quad(Vector3 position, Vector3 rotation, float scale)
        {
            this._position = position;
            this._rotation = rotation;
            this.scale = scale;

            calcVertices();
        }

        private void calcVertices()
        {
            vertices = new VertexPositionNormalTexture[6];
            /*      _____
            *   |\  \    |
            *   | \  \ 2 |   
            *   |  \  \  |
            *   | 1 \  \ |
            *   |____\  \|
            */
            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);

            Vector3 right = Vector3.Transform(Vector3.Right, rotationMatrix);
            Vector3 up = Vector3.Transform(Vector3.Up, rotationMatrix);

            //counter clockwise winding 
            //triangle 1 
            vertices[0] = new VertexPositionNormalTexture((position - up - right) * scale, Vector3.Up, new Vector2(0, 0));
            vertices[1] = new VertexPositionNormalTexture((position - up + right) * scale, Vector3.Up, new Vector2(1, 0));
            vertices[2] = new VertexPositionNormalTexture((position + up - right) * scale, Vector3.Up, new Vector2(0, 1));
            //triangle 2
            vertices[3] = new VertexPositionNormalTexture((position - up + right) * scale, Vector3.Up, new Vector2(1, 0));
            vertices[4] = new VertexPositionNormalTexture((position + up - right) * scale, Vector3.Up, new Vector2(0, 1));
            vertices[5] = new VertexPositionNormalTexture((position + up + right) * scale, Vector3.Up, new Vector2(1, 1));
        }
    }

    public class Particle
    {
        #region parameters
        Texture2D texture;
        public Vector3 position {get { return quad.position; } set { quad.position = value; }} 
        public Vector3 rotation {get { return quad.rotation; } set { quad.rotation = value; }}  //in radians: rotation around the x, y, and z axies  
        public Vector3 acceleration;
        float scale {get { return quad.scale; } set { quad.scale = value; }}

        public float lifeTime = 0f;

        Quad quad;

        #endregion
        public Particle(Texture2D texture, Vector3 position, Vector3 rotation, float scale, Vector3 acceleration)
        {
            this.texture = texture;
            this.acceleration = acceleration;

            this.quad = new Quad(position, rotation, scale);
        }

        public void draw(Effect effect, GraphicsDevice graphicsDevice)
        {
            foreach(EffectTechnique technique in effect.Techniques)
            {
                foreach(EffectPass pass in technique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, quad.vertices, 0, 2);
                }
            }
        }
    }
}