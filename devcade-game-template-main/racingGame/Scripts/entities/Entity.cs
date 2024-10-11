using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

namespace EntityClass
{
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

        public abstract void draw(Effect effect);

        public abstract void tick(float frameTimeInSeconds);

        private void calculateWorldMatrix()
        {
            this.worldMatrix = Matrix.CreateTranslation(-originPoint) * Matrix.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z) * Matrix.CreateTranslation(originPoint) * Matrix.CreateTranslation(this._position);
        }
    }

    public abstract class CollisionEntity
    {
    
    #region parameters
        bool doCollision = false;

        BoundingSphere boundingSphere;

        internal Vector3 _position;
        public Vector3 position { get{ return _position; } set{ _position = value; calculateWorldMatrix(); }}

        internal Vector3 originPoint = Vector3.Zero;

        internal Vector3 _rotation { get{ return _rotation; } set{ _rotation = value; calculateWorldMatrix(); }}
        public  Vector3 rotation;
        internal Matrix worldMatrix;
    #endregion        

        private void calculateWorldMatrix()
        {
            this.worldMatrix = Matrix.CreateTranslation(-originPoint) * Matrix.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z) * Matrix.CreateTranslation(originPoint) * Matrix.CreateTranslation(this._position);
        }

        public CollisionEntity(Vector3 position, Vector3 rotation, BoundingSphere boundingSphere, bool doCollision)
        {
            this.doCollision = doCollision; 
            this.boundingSphere = boundingSphere;
            this.position = position;
            this.rotation = rotation;
        }

        /// <summary>
        /// what to draw
        /// </summary>
        /// <param name="effect"></param>
        public abstract void draw(Effect effect);

        /// <summary>
        /// the things that the entity does
        /// </summary>
        public abstract void tick(float frameTimeInSeconds);

        /// <summary>
        /// will attempt a move by the given translation
        /// if it collides it will move up to the collision
        /// if it does not collide it will move by the translation given
        /// </summary>
        /// <param name="translation"></param> the amount to move in x,y,z
        /// <param name="otherEntities"></param> the other entities to do collision checks on
        public void move(Vector3 translation, CollisionEntity[] otherEntities)
        {
            if(doCollision)
            {
                Vector3 unitTranslation = Vector3.Normalize(translation - this.position); //essentally the rotation of the translation
                float moveDistance = (translation - this.position).Length(); //the amount of distance the move wants to go

                float shortestDistance = moveDistance;

                List<CollisionEntity> entitiesList = otherEntities.ToList<CollisionEntity>();

                foreach(CollisionEntity entity in otherEntities) 
                {
                    if(entity.doCollision == false)
                    {
                        continue;
                    }

                    if((entity.position - this.position).Length() > moveDistance)
                    {
                        continue;
                    }

                    Ray ray = new Ray(this.position, unitTranslation);

                    float? distance = null;
                    ray.Intersects(ref entity.boundingSphere, out distance);

                    if(distance == null)
                    {
                        continue;
                    }

                    shortestDistance = distance ?? 0;

                }

                this.position += unitTranslation * shortestDistance;
            }
            else
            {
                this.position += translation;
            }
            
        }
    }
}