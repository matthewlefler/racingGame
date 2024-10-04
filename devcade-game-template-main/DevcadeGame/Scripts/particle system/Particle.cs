using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;

namespace ParticleClass
{
    class Quad
    {
        VertexPositionTexture[] vertices;

        Vector3 _position;
        Vector3 position { get; set; }

        Vector3 _rotation;
        Vector3 rotation { get; set; }
        
        float scale;

        Quad(Vector3 position, Vector3 rotation, float scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;

            calcVertices();
        }

        private void calcVertices()
        {
            vertices = new VertexPositionTexture[6];
            /*   _____
            *   |\    |
            *   | \ 2 |   
            *   |  \  |
            *   | 1 \ |
            *   |____\|
            */
            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);

            Vector3 right = Vector3.Transform(Vector3.Right, rotationMatrix);
            Vector3 up = Vector3.Transform(Vector3.Up, rotationMatrix);

            //counter clockwise winding 
            //triangle 1 
            vertices[0] = new VertexPositionTexture(position - up - right, new Vector2(0, 0));
            vertices[1] = new VertexPositionTexture(position - up + right, new Vector2(1, 0));
            vertices[2] = new VertexPositionTexture(position + up - right, new Vector2(0, 1));

            //triangle 2
            vertices[3] = new VertexPositionTexture(position - up + right, new Vector2(1, 0));
            vertices[4] = new VertexPositionTexture(position + up - right, new Vector2(0, 1));
            vertices[5] = new VertexPositionTexture(position + up + right, new Vector2(1, 1));
        }
    }

    class Particle
    {
        #region parameters
        Texture2D texture;
        Vector3 position;
        Vector3 rotation;

        #endregion
        public Particle()
        {

        }
    }
}