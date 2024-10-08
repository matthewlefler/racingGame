using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

namespace EntityClass
{
    public class Entity
    {

    #region parameters
        internal Vector3 _position;
        public Vector3 position { get{ return _position; } set{ _position = value; calculateWorldMatrix(); }}

        internal Vector3 originPoint = Vector3.Zero;

        internal Vector3 _rotation { get{ return _rotation; } set{ _rotation = value; calculateWorldMatrix(); }}
        public  Vector3 rotation;
        internal Matrix worldMatrix;

        internal ModelMesh mesh;
    #endregion

        public Entity(Vector3 position, Vector3 rotation, ModelMesh mesh)
        {
            this.mesh = mesh;
            this.position = position;
            this.rotation = rotation;
        }

        public virtual void move(Vector3 translation, Vector3 rotation)
        {
            this.position += translation;
            this.rotation += rotation;
        }

        public virtual void draw(Effect effect)
        {
            effect.Parameters["World"].SetValue(worldMatrix);

            foreach(EffectTechnique technique in effect.Techniques)
            {
                foreach(EffectPass pass in technique.Passes)
                {
                    pass.Apply();

                    mesh.Draw();
                }
            }
        }

        private void calculateWorldMatrix()
        {
            this.worldMatrix = Matrix.CreateTranslation(-originPoint) * Matrix.CreateFromYawPitchRoll(_rotation.X, _rotation.Y, _rotation.Z) / Matrix.CreateTranslation(-originPoint) * Matrix.CreateTranslation(this._position);
        }
    }

    class CollisionEntity : Entity
    {
    
    #region parameters
        bool doCollision = false;

        BoundingSphere boundingSphere;

    #endregion

        public CollisionEntity(Vector3 position, Vector3 rotation, ModelMesh mesh, BoundingSphere boundingSphere, bool doCollision) : base(position, rotation, mesh)
        {
            this.doCollision = doCollision; 
            this.boundingSphere = boundingSphere;
        }

        /// <summary>
        /// will attempt a move by the given translation
        /// if it collides it will move up to the collision
        /// if it does not collide it will move by the translation given
        /// </summary>
        /// <param name="translation"></param> the amount to move in x,y,z
        /// <param name="otherEntities"></param> the other entities to do collision checks on
        public void move(Vector3 translation, CollisionEntity[] otherEntities)
        {
            Vector3 unitTranslation = Vector3.Normalize(translation - this.position); //essentally the rotation of the translation
            float moveDistance = (translation - this.position).Length(); //the amount of distance the move wants to go

            float shortestDistance = moveDistance;

            List<CollisionEntity> entitiesList = otherEntities.ToList<CollisionEntity>();
            
            foreach(CollisionEntity entity in otherEntities) 
            {
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
    }
}