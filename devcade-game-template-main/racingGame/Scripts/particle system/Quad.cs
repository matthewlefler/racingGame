using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuadClass
{
    class Quad
    {
        public VertexPositionColorNormalTexture[] vertices { get; private set; }

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
            vertices = new VertexPositionColorNormalTexture[6];
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
            Vector3 normal = Vector3.Transform(Vector3.Backward, rotationMatrix);

            //counter clockwise winding 
            //triangle 1 
            vertices[0] = new VertexPositionColorNormalTexture((position - up - right) * scale, Color.White, normal, new Vector2(0, 1));
            vertices[1] = new VertexPositionColorNormalTexture((position - up + right) * scale, Color.White, normal, new Vector2(1, 1));
            vertices[2] = new VertexPositionColorNormalTexture((position + up - right) * scale, Color.White, normal, new Vector2(0, 0));
            //triangle 2
            vertices[3] = new VertexPositionColorNormalTexture((position - up + right) * scale, Color.White, normal, new Vector2(1, 1));
            vertices[4] = new VertexPositionColorNormalTexture((position + up + right) * scale, Color.White, normal, new Vector2(1, 0));
            vertices[5] = new VertexPositionColorNormalTexture((position + up - right) * scale, Color.White, normal, new Vector2(0, 0));
        }
    }
}