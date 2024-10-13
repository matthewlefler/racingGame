using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EntityClass;
using CylinderClass;
using System.Diagnostics;
using EntityManagerClass;

namespace WheelClass
{
    public class Wheel : CollisionEntity
    {
        Cylinder wheelMesh;
        float time = 0f;

        public override Vector3 position { get { return this._position; } set { this.wheelMesh.position = value; this._position = value; } }
        public override Vector3 rotation { get { return this._rotation; } set { this.wheelMesh.rotation = value; this._rotation = value; } }


        public Wheel(Vector3 position, Vector3 rotation, Texture2D texture, GraphicsDevice graphicsDevice) : base(position, rotation, new CollisionMesh(new BoundingBox(new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(0.5f,0.5f,0.5f)), position), true)
        {
            float width = 0.5f;
            float height = 0.5f;
            
            wheelMesh = new Cylinder(height, width, 12, position, rotation, texture, graphicsDevice);
        }

        public override void draw(BasicEffect effect)
        {
            wheelMesh.draw(effect);
        }

        public override void tick(float frameTimeInSeconds, CollisionEntity[] collisionEntities)
        {
            time += frameTimeInSeconds;
            if(time > 10f)
            {
                time = 0f;
                this.position = new Vector3(0f, 10f, 0f);
            }
            this.velocity += this.acceleration * frameTimeInSeconds;
            this.acceleration += new Vector3(0, -2f, 0) * frameTimeInSeconds;

            this.rotation += new Vector3(0.4f, 0f, 0f) * frameTimeInSeconds;

            this.move(this.velocity * frameTimeInSeconds, collisionEntities);

            if(this.position.Y < -10f) {
                this.position = new Vector3(0f, 10f, 0f);
            }
        }
    }
}