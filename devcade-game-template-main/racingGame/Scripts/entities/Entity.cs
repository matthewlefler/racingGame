using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Reflection.PortableExecutable;
using System.Diagnostics;

namespace EntityClass
{
    public abstract class BasicEntity
    {
        internal Vector3 _position;
        public Vector3 position { get{ return _position; } set{ _position = value; calculateWorldMatrix(); }}

        internal Vector3 originPoint = Vector3.Zero;

        internal Vector3 _rotation;
        public  Vector3 rotation { get{ return _rotation; } set{ _rotation = value; calculateWorldMatrix(); }}
        internal Matrix worldMatrix;
        internal GraphicsDevice graphicsDevice;

        public BasicEntity(Vector3 position, Vector3 rotation, GraphicsDevice graphicsDevice)
        {
            this._position = position;
            this._rotation = rotation;
            calculateWorldMatrix();
            this.graphicsDevice = graphicsDevice;
        }

        internal void calculateWorldMatrix()
        {
            this.worldMatrix = Matrix.CreateTranslation(-originPoint) * Matrix.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z) * Matrix.CreateTranslation(originPoint) * Matrix.CreateTranslation(this._position);
        }

        public abstract void draw(BasicEffect effect);
    }

    public abstract class Entity
    {

    #region parameters
        internal Vector3 _position;
        public Vector3 position { get{ return _position; } set{ _position = value; calculateWorldMatrix(); }}

        internal Vector3 originPoint = Vector3.Zero;

        internal Vector3 _rotation;
        public  Vector3 rotation { get{ return _rotation; } set{ _rotation = value; calculateWorldMatrix(); }}
        internal Matrix worldMatrix;
    #endregion

        public Entity(Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public virtual void move(Vector3 translation, Vector3 rotation)
        {
            this.position += translation;
            this.rotation += rotation;
        }

        public abstract void draw(BasicEffect effect);

        public abstract void tick(float frameTimeInSeconds);

        internal void calculateWorldMatrix()
        {
            this.worldMatrix = Matrix.CreateTranslation(-1f * this._position) * Matrix.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z) * Matrix.CreateTranslation(2f * this._position);
        }
    }

    public abstract class CollisionEntity
    {
    
        #region parameters

        public bool doCollision = false;

        CollisionMesh boundingMesh;

        internal Vector3 _position;
        public virtual Vector3 position { get{ return _position; } set{ _position = value; calculateWorldMatrix(); }}

        internal Vector3 originPoint = Vector3.Zero;

        internal Vector3 _rotation;
        public  virtual Vector3 rotation { get{ return _rotation; } set{ _rotation = value; calculateWorldMatrix(); }}

        public Vector3 velocity = Vector3.Zero;
        public Vector3 acceleration = Vector3.Zero;

        internal Matrix worldMatrix;

        #endregion        

        internal void calculateWorldMatrix()
        {
            this.worldMatrix = Matrix.CreateTranslation(-1f * this._position) * Matrix.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z) * Matrix.CreateTranslation(2f * this._position);
            this.boundingMesh.position = this.position;
        }

        public CollisionEntity(Vector3 position, Vector3 rotation, CollisionMesh boundingMesh, bool doCollision)
        {
            this.doCollision = doCollision; 
            this.boundingMesh = boundingMesh;
            this._position = position;
            this._rotation = rotation;

            calculateWorldMatrix();
        }

        /// <summary>
        /// what to draw
        /// </summary>
        /// <param name="effect"></param>
        public abstract void draw(BasicEffect effect);

        /// <summary>
        /// the things that the entity does
        /// </summary>
        public abstract void tick(float frameTimeInSeconds, CollisionEntity[] collisionEntities);

        /// <summary>
        /// will attempt a move by the given translation
        /// if it collides it will move up to the collision
        /// if it does not collide it will move by the translation given
        /// </summary>
        /// <param name="translation"></param> the amount to move in x,y,z
        /// <param name="otherEntities"></param> the other entities to do collision checks on
        public void move(Vector3 translation, CollisionEntity[] otherEntities)
        {
            if(translation.Length() == 0.0f)
            {
                return;
            }

            if(doCollision)
            {
                Vector3 unitTranslation = Vector3.Normalize(translation); //essentally the rotation of the translation
                float moveDistance = translation.Length(); //the amount of distance the move wants to go

                float shortestDistance = moveDistance;

                List<CollisionEntity> entitiesList = otherEntities.ToList<CollisionEntity>();

                entitiesList.Remove(this);

                foreach(CollisionEntity entity in entitiesList) 
                {
                    if(entity.doCollision == false)
                    {
                        continue;
                    }

                    if((entity.position - this.position).Length() > moveDistance)
                    {
                        continue;
                    }

                    Ray ray = new Ray(this.position, translation);

                    float? distance = entity.boundingMesh.rayIntersects(ray);

                    if(distance == null)
                    {
                        continue;
                    }

                    shortestDistance = distance ?? 0f;

                }

                this.position += unitTranslation * shortestDistance;

                if(shortestDistance < moveDistance)
                {
                    this.acceleration = Vector3.Zero;
                    this.velocity = Vector3.Zero;
                }
            }
            else
            {
                this.position += translation;
            }
            
        }
    }

    public class CollisionMesh
    {
        internal BoundingBox box;
        internal BoundingSphere sphere;

        private bool boxOrSphere;

        public Vector3 position { get { return getPosition(); } set { setPosition(value); } }

        public CollisionMesh(BoundingBox box, Vector3 position)
        {
            this.box = box;
            this.boxOrSphere = true;

            setPosition(position);
        }

        public CollisionMesh(BoundingSphere sphere, Vector3 position)
        {
            this.sphere = sphere;
            this.boxOrSphere = false;

            setPosition(position);
        }

        private Vector3 getPosition()
        {
            if(boxOrSphere)
            {
                return (box.Min + box.Max) / 2f;
            }
            else
            {
                return sphere.Center;
            }
        }

        private void setPosition(Vector3 value)
        {
            if(boxOrSphere)
            {
                Vector3 offset = value - ((box.Min + box.Max) / 2f);
                box.Max += offset;
                box.Min += offset;

            }
            else
            {
                sphere.Center = position;
            }
        }

        public float? rayIntersects(Ray ray)
        {
            float? distance;
            
            if(boxOrSphere)
            {
                distance = ray.Intersects(box);
            }
            else
            {
                distance = ray.Intersects(sphere);
            }

            return distance;
        }
    }
}