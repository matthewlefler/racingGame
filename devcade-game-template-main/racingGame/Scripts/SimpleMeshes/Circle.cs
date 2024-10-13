using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CircleClass
{
    public class Circle
    {
        public VertexPositionColorNormalTexture[] vertices { get; private set; }

        Vector3 _position;
        public Vector3 position { get { return _position; } set { _position = value; calcVertices(); } }

        Vector3 _rotation;
        public Vector3 rotation { get { return _rotation; } set { _rotation = value; calcVertices(); } }
    
        public float radius = 1f;
        public readonly int resolution = 10; //aka the number of sides

        public Circle(float radius, int resolution, Vector3 position, Vector3 rotation)
        {
            this.radius = radius;
            this.resolution = resolution;

            this._position = position;
            this._rotation = rotation;

            calcVertices();
        }

        private void calcVertices()
        {
             vertices = new VertexPositionColorNormalTexture[resolution * 3];

            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);

            Vector3 right = Vector3.Transform(Vector3.Right, rotationMatrix);
            Vector3 up = Vector3.Transform(Vector3.Up, rotationMatrix);
            Vector3 normal = Vector3.Transform(Vector3.Backward, rotationMatrix);

            //counter clockwise winding 
            for(int i = 0, t = 0; i < resolution; i++, t+=3)
            {
                float angle = (MathHelper.TwoPi / resolution) * (float)i;
                float angle2 = (MathHelper.TwoPi / resolution) * ((float)i + 1f);
                Vector3 point1 = new Vector3(MathF.Cos(angle), 0f, MathF.Sin(angle)) * radius;
                Vector3 point2 = new Vector3(MathF.Cos(angle2), 0f, MathF.Sin(angle2)) * radius;

                vertices[t]     = new VertexPositionColorNormalTexture(position, Color.White, normal, new Vector2(0.5f,0.5f));
                vertices[t + 1] = new VertexPositionColorNormalTexture(position + Vector3.Transform(point1, rotationMatrix), Color.White, normal, new Vector2(0,0));
                vertices[t + 2] = new VertexPositionColorNormalTexture(position + Vector3.Transform(point2, rotationMatrix), Color.White, normal, new Vector2(0,0));
            }
        }
    }
}

