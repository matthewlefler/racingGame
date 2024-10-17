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
using PhysicsMaterialClass;
using BoundingMeshesClass;
using System;

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
            this.worldMatrix = Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) * Matrix.CreateTranslation(this._position);
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
            this.worldMatrix = Matrix.CreateTranslation(-1f * this._position) * Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) * Matrix.CreateTranslation(2f * this._position);
        }
    }

    public abstract class CollisionEntity
    {
    
        #region parameters

        public bool doCollision = false;
        public bool unMoveable = false;

        internal BoundingMesh boundingMesh;
        internal PhysicsMaterial physicsMaterial;

        internal Vector3 _position;
        public virtual Vector3 position { get{ return _position; } set{ calculateWorldMatrix(value); _position = value; }}

        internal Vector3 originPoint = Vector3.Zero;

        internal Vector3 _rotation;
        public  virtual Vector3 rotation { get{ return _rotation; } set{ _rotation = value; calculateWorldMatrix(_position); }}

        public Vector3 velocity = Vector3.Zero;
        public Vector3 acceleration = Vector3.Zero;

        internal Matrix worldMatrix;

        #endregion        


        public CollisionEntity(Vector3 position, Vector3 rotation, BoundingMesh boundingMesh, bool doCollision)
        {
            this.doCollision = doCollision;
            this.boundingMesh = boundingMesh;
            this._position = position;
            this._rotation = rotation;

            calculateWorldMatrix(position);
        }

        internal void calculateWorldMatrix(Vector3 position)
        {
            this.worldMatrix = Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) * Matrix.CreateTranslation(position);
            this.boundingMesh.position = position;
            this.boundingMesh.rotation = rotation;
        }

        /// <summary>
        /// what to draw
        /// </summary>
        /// <param name="effect"></param>
        public abstract void draw(BasicEffect effect);

        public void drawBoundingMesh(BasicEffect effect, GraphicsDevice graphicsDevice)
        {
            effect.World = Matrix.Identity;

            List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>();

            foreach(Face face in this.boundingMesh.faces)
            {
                Vector3 worldUp = this.boundingMesh.localUp;

                if(worldUp == Vector3.Normalize(face.normal) || -1f * worldUp == Vector3.Normalize(face.normal))
                {
                    worldUp = this.boundingMesh.localForward;
                }

                Vector3 rightVector = Vector3.Normalize(Vector3.Cross(worldUp, face.normal));  
                Vector3 upVector = Vector3.Normalize(Vector3.Cross(rightVector, face.normal)); 
                
                vertices.Add(new VertexPositionColorTexture(face.position + rightVector * face.vertices[0].X + upVector * face.vertices[0].Y + (Vector3.Up * 2f), Color.Red, Vector2.Zero));
                
                foreach(Vector2 vertex in face.vertices)
                {
                    Vector3 offset = rightVector * vertex.X + upVector * vertex.Y;
                    Vector3 vertexPosition = face.position + offset;

                    vertices.Add(new VertexPositionColorTexture(vertexPosition + (Vector3.Up * 2f), Color.Red, Vector2.Zero));
                    vertices.Add(new VertexPositionColorTexture(vertexPosition + (Vector3.Up * 2f), Color.Red, Vector2.Zero));
                }
                
                vertices.Add(new VertexPositionColorTexture(face.position + rightVector * face.vertices[0].X + upVector * face.vertices[0].Y + (Vector3.Up * 2f), Color.Red, Vector2.Zero));


                vertices.Add(new VertexPositionColorTexture(face.position + (Vector3.Up * 2f), Color.Red, Vector2.Zero));
                vertices.Add(new VertexPositionColorTexture(face.position + face.normal + (Vector3.Up * 2f), Color.Red, Vector2.Zero));

            }
            
            vertices.Add(new VertexPositionColorTexture(this.boundingMesh.position + (Vector3.Up * 2f), Color.Red, Vector2.Zero));
            vertices.Add(new VertexPositionColorTexture(this.boundingMesh.position + this.boundingMesh.localUp + (Vector3.Up * 2f), Color.Red, Vector2.Zero));

            vertices.Add(new VertexPositionColorTexture(this.boundingMesh.position + (Vector3.Up * 2f), Color.Red, Vector2.Zero));
            vertices.Add(new VertexPositionColorTexture(this.boundingMesh.position + this.boundingMesh.localForward + (Vector3.Up * 2f), Color.Red, Vector2.Zero));


            foreach(EffectTechnique technique in effect.Techniques)
            {
                foreach(EffectPass pass in technique.Passes)
                {
                    pass.Apply();

                    graphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.LineList, vertices.ToArray(), 0, vertices.Count/2);
                }
            }
        }

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
                float shortestDistanceFromCenter;

                Vector3 rayOrigin;

                float? distanceFromCenter = this.boundingMesh.rayIntersects(new Ray(this.position, unitTranslation));

                if(distanceFromCenter.HasValue)
                {
                    rayOrigin = this.position + (unitTranslation * distanceFromCenter.Value);
                    shortestDistanceFromCenter = distanceFromCenter.Value;
                }
                else
                {
                    rayOrigin = this.position;
                    shortestDistanceFromCenter = 0f;
                }

                List<CollisionEntity> otherEntitiesList = otherEntities.ToList<CollisionEntity>();

                CollisionEntity entityHit;

                otherEntitiesList.Remove(this);

                foreach(CollisionEntity entity in otherEntitiesList)
                {
                    if(entity.doCollision == false)
                    {
                        continue;
                    }

                    Ray ray = new Ray(rayOrigin, unitTranslation);
                    Ray centerRay = new Ray(this.position, unitTranslation);

                    float? distance = entity.boundingMesh.rayIntersects(ray);
                    float? centerDistance = entity.boundingMesh.rayIntersects(centerRay);

                    if(distance.HasValue && distance < shortestDistance)
                    {
                        shortestDistance = distance.Value;
                        entityHit = entity;
                    }

                    if(centerDistance.HasValue && centerDistance < shortestDistanceFromCenter)
                    {
                        shortestDistanceFromCenter = centerDistance.Value;
                    }
                }

                if(distanceFromCenter.HasValue && shortestDistanceFromCenter < distanceFromCenter)
                {
                    this.position -= unitTranslation * (distanceFromCenter.Value - shortestDistanceFromCenter);

                    this.acceleration = Vector3.Zero;
                    this.velocity = Vector3.Zero;

                    return;
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
}