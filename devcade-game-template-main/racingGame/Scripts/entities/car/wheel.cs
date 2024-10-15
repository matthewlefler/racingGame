using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EntityClass;
using CylinderClass;
using System.Diagnostics;
using EntityManagerClass;

using BoundingMeshesClass;

namespace WheelClass
{
    public class Wheel : CollisionEntity
    {
        public Cylinder wheelMesh;

        public override Vector3 position { get { return this._position; } set { this.wheelMesh.position = value; this._position = value; calculateWorldMatrix(value); } }
        public override Vector3 rotation { get { return this._rotation; } set { this.wheelMesh.rotation = value; this._rotation = value; calculateWorldMatrix(this.position); } }


        public Wheel(Vector3 position, Vector3 rotation, Texture2D texture, GraphicsDevice graphicsDevice) : base(position, rotation, BasicBoundingMeshes.BoundingCylinder(10, 0.5f, 0.5f, 1f, position), true)
        {
            float width = 0.5f;
            float height = 0.5f;
            
            wheelMesh = new Cylinder(height, width, 10, position, rotation, texture, graphicsDevice);
        }

        public override void draw(BasicEffect effect)
        {
            wheelMesh.draw(effect);

            foreach(EffectTechnique technique in effect.Techniques)
            {
                foreach(EffectPass pass in technique.Passes)
                {
                    pass.Apply();
                }
            }

#if DEBUG
            drawBoundingMesh(effect, wheelMesh.graphicsDevice);
#endif

        }

        public override void tick(float frameTimeInSeconds, CollisionEntity[] collisionEntities)
        {
            this.velocity += this.acceleration * frameTimeInSeconds;
            this.acceleration += new Vector3(0, -0f, 0) * frameTimeInSeconds;

            this.rotation = new Vector3(0.0f, 0.0f, 0.0f) * frameTimeInSeconds;

            this.move(this.velocity * frameTimeInSeconds, collisionEntities);
        }
    }
}