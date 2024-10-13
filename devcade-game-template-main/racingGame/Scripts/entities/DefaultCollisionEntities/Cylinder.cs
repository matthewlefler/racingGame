using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EntityClass;
using QuadClass;
using CircleClass;
using System;
using System.Diagnostics;


namespace CylinderClass
{
    class CollisionCylinder : CollisionEntity
    {
        Quad[] quads;
        Circle[] circles = new Circle[2];

        GraphicsDevice graphicsDevice;
        Texture2D texture;

        public CollisionCylinder(float height, float radius, int resolution, Vector3 position, Vector3 rotation, Texture2D texture, GraphicsDevice graphicsDevice) : base(position, rotation, new CollisionMesh(new BoundingSphere(position, radius), position), true)
        {
            this.graphicsDevice = graphicsDevice;
            this.texture = texture;

            quads = new Quad[resolution];

            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);

            float anglePer = MathHelper.TwoPi/quads.Length;

            for(int i = 0; i < quads.Length; i++)
            {
                float angle = (MathHelper.TwoPi / quads.Length) * (float)i;
                float angle2 = (MathHelper.TwoPi / quads.Length) * ((float)i + 1);

                Vector3 pos1 = new Vector3(MathF.Cos(angle) * radius, position.Y, MathF.Sin(angle) * radius);
                Vector3 pos2 = new Vector3(MathF.Cos(angle2) * radius, position.Y, MathF.Sin(angle2) * radius);
                Vector3 center = (pos1 + pos2)/2f;

                float zRotation = 0f;
                if(i > quads.Length/2 - 1)
                {
                    zRotation = MathHelper.Pi;
                }

                quads[i] = new Quad(center, new Vector3(MathF.Atan(center.X/center.Z) + zRotation, 0, 0), Vector3.Distance(pos1, pos2)/2f , height/2f);
            }
            
            circles[0] = new Circle(radius, resolution, position + Vector3.Transform(new Vector3(0, height/2f, 0), rotationMatrix), rotation + new Vector3(0, MathHelper.Pi, 0));
            circles[1] = new Circle(radius, resolution, position - Vector3.Transform(new Vector3(0, height/2f, 0), rotationMatrix), rotation);
        }

        public override void draw(BasicEffect effect)
        {
            effect.Parameters["World"].SetValue(worldMatrix);
            effect.Parameters["Texture"].SetValue(texture);

            foreach(EffectTechnique technique in effect.Techniques)
            {
                foreach(EffectPass pass in technique.Passes)
                {
                    pass.Apply();

                    foreach(Quad quad in quads)
                    {
                        graphicsDevice.DrawUserPrimitives<VertexPositionColorNormalTexture>(PrimitiveType.TriangleList, quad.vertices, 0, 2);
                    }

                    foreach(Circle circle in circles)
                    {
                        graphicsDevice.DrawUserPrimitives<VertexPositionColorNormalTexture>(PrimitiveType.TriangleList, circle.vertices, 0, circle.resolution);
                    }
                }
            }
        }

        public override void tick(float frameTimeInSeconds, CollisionEntity[] collisionEntities)
        {
            //it will sit there and be a good cylinder
            return;
        }
    }

    class Cylinder : BasicEntity
    {
        Texture2D texture;

        Quad[] quads;
        Circle[] circles = new Circle[2];

        public Cylinder(float height, float radius, int resolution, Vector3 position, Vector3 rotation, Texture2D texture, GraphicsDevice graphicsDevice) : base(position, rotation, graphicsDevice)
        {
            this.texture = texture;

            quads = new Quad[resolution];

            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);

            for(int i = 0; i < quads.Length; i++)
            {
                float angle = (MathHelper.TwoPi / quads.Length) * (float)i;
                float angle2 = (MathHelper.TwoPi / quads.Length) * ((float)i + 1);

                Vector3 pos1 = new Vector3(MathF.Cos(angle) * radius, position.Y, MathF.Sin(angle) * radius);
                Vector3 pos2 = new Vector3(MathF.Cos(angle2) * radius, position.Y, MathF.Sin(angle2) * radius);
                Vector3 center = (pos1 + pos2)/2f;

                float zRotation = 0f;
                if(i > quads.Length/2 - 1)
                {
                    zRotation = MathHelper.Pi;
                }

                quads[i] = new Quad(center, new Vector3(MathF.Atan(center.X/center.Z) + zRotation, 0, 0), Vector3.Distance(pos1, pos2)/2f , height/2f);
            }
            
            circles[0] = new Circle(radius, resolution, position + Vector3.Transform(new Vector3(0, height/2f, 0), rotationMatrix), rotation + new Vector3(0, MathHelper.Pi, 0));
            circles[1] = new Circle(radius, resolution, position - Vector3.Transform(new Vector3(0, height/2f, 0), rotationMatrix), rotation);
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

                    foreach(Circle circle in circles)
                    {
                        graphicsDevice.DrawUserPrimitives<VertexPositionColorNormalTexture>(PrimitiveType.TriangleList, circle.vertices, 0, circle.resolution);
                    }
                }
            }
        }
    }
}