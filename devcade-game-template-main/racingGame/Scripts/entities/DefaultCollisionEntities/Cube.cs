using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EntityClass;
using QuadClass;


namespace CubeClass
{
    class CollisionCube : CollisionEntity
    {
        Quad[] quads = new Quad[6];

        GraphicsDevice graphicsDevice;
        Texture2D texture;

        public CollisionCube(float width, float height, float depth, Vector3 position, Vector3 rotation, Texture2D texture, GraphicsDevice graphicsDevice) : base(position, rotation, new CollisionMesh(new BoundingBox(new Vector3(-width/2f, -height/2f, -depth/2f), new Vector3(width/2f, height/2f, depth/2f)), position), true)
        {
            this.graphicsDevice = graphicsDevice;
            this.texture = texture;

            quads[0] = new Quad(position + Vector3.Down * height,   rotation + new Vector3(0, MathHelper.PiOver2, 0), width, depth);
            quads[1] = new Quad(position - Vector3.Down * height,   rotation + new Vector3(0, -MathHelper.PiOver2, 0), width, depth);
            quads[2] = new Quad(position + Vector3.Forward * depth, rotation + new Vector3(0, MathHelper.Pi, MathHelper.Pi), width, height);
            quads[3] = new Quad(position - Vector3.Forward * depth, rotation + new Vector3(0, 0, 0), width, height);
            quads[4] = new Quad(position + Vector3.Right * width,   rotation + new Vector3(MathHelper.PiOver2, 0, 0), depth, height);
            quads[5] = new Quad(position - Vector3.Right * width,   rotation + new Vector3(-MathHelper.PiOver2, 0, 0), depth, height);
        }

        public override void draw(BasicEffect effect)
        {
            effect.World = worldMatrix;
            effect.Texture = texture;

            foreach(EffectTechnique technique in effect.Techniques)
            {
                foreach(EffectPass pass in technique.Passes)
                {
                    pass.Apply();

                    foreach(Quad quad in quads)
                    {
                        graphicsDevice.DrawUserPrimitives<VertexPositionColorNormalTexture>(PrimitiveType.TriangleList, quad.vertices, 0, 2);
                    }
                }
            }
        }

        public override void tick(float frameTimeInSeconds, CollisionEntity[] collisionEntities)
        {
            //it will sit there and be a good cube
            return;
        }
    }
}