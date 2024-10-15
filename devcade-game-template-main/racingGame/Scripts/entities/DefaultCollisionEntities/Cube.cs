using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EntityClass;
using QuadClass;

using BoundingMeshesClass;
using System.Diagnostics;

namespace CubeClass
{
    class CollisionCube : CollisionEntity
    {
        Quad[] quads = new Quad[6];

        GraphicsDevice graphicsDevice;
        Texture2D texture;

        public CollisionCube(float width, float height, float depth, Vector3 position, Vector3 rotation, Texture2D texture, GraphicsDevice graphicsDevice) : base(position, rotation, BasicBoundingMeshes.BoundingRectangluarPrism(width, height, depth, 1f, position), true)
        {
            this.graphicsDevice = graphicsDevice;
            this.texture = texture;

            quads[0] = new Quad( 0.5f * Vector3.Down * height,   rotation + new Vector3(0, MathHelper.PiOver2, 0), width * 0.5f, depth * 0.5f);            // bottom
            quads[1] = new Quad(-0.5f * Vector3.Down * height,   rotation + new Vector3(0, -MathHelper.PiOver2, 0), width * 0.5f, depth * 0.5f);           // top
            quads[2] = new Quad( 0.5f * Vector3.Forward * depth, rotation + new Vector3(0, MathHelper.Pi, MathHelper.Pi), width * 0.5f, height * 0.5f);    // back
            quads[3] = new Quad(-0.5f * Vector3.Forward * depth, rotation + new Vector3(0, 0, 0), width * 0.5f, height * 0.5f);                            // front
            quads[4] = new Quad( 0.5f * Vector3.Right * width,   rotation + new Vector3(MathHelper.PiOver2, 0, 0), depth * 0.5f, height * 0.5f);           // right
            quads[5] = new Quad(-0.5f * Vector3.Right * width,   rotation + new Vector3(-MathHelper.PiOver2, 0, 0), depth * 0.5f, height * 0.5f);          // left
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
            
#if DEBUG
            drawBoundingMesh(effect, graphicsDevice);
#endif
        }

        public override void tick(float frameTimeInSeconds, CollisionEntity[] collisionEntities)
        {
            //it will sit there and be a good cube
            return;
        }
    }
}