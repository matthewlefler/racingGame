using System.Net.NetworkInformation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LineClass
{
    class Line
    {
        public VertexPositionColorNormalTexture[] vertices { get; private set; }

        Vector3 _position1;
        public Vector3 position1 { get { return _position1; } set { _position1 = value; calcVertices(); } }
        Vector3 _position2;
        public Vector3 position2 { get { return _position2; } set { _position2 = value; calcVertices(); } }

        Vector3 _rotation;
        public Vector3 rotation { get { return _rotation; } set { _rotation = value; calcVertices(); } }

        public Line(Vector3 pos1, Vector3 pos2, Vector3 rotation)
        {
            this._position1 = pos1;
            this._position2 = pos2;

            this._rotation = rotation;

            calcVertices();
        }

        private void calcVertices()
        {
            vertices = new VertexPositionColorNormalTexture[2];
            /*
            *   0
            *    \
            *     \
            *      \
            *       \
            *        1
            */          
            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);

            Vector3 normal = Vector3.Transform(Vector3.Backward, rotationMatrix);

            //the line
            vertices[0] = new VertexPositionColorNormalTexture(position1, Color.White, normal, new Vector2(0, 0));
            vertices[1] = new VertexPositionColorNormalTexture(position2, Color.White, normal, new Vector2(0, 1));

        }
    }
}