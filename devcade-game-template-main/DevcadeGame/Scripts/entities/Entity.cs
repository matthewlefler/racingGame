using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Runtime.CompilerServices;

namespace EntityClass
{
    class Entity
    {

    #region parameters
        Vector3 position;
        Vector3 rotation;

        ModelMesh mesh;

        BoundingSphere boundingSphere;

        bool doCollision;

        private BasicEffect effect;
    #endregion

        public Entity(Vector3 position, Vector3 rotation, ModelMesh mesh, BoundingSphere? boundingSphere)
        {
            this.doCollision = true;

            this.mesh = mesh;
            this.position = position;
            this.rotation = rotation;

            this.boundingSphere = boundingSphere ?? mesh.BoundingSphere;
        }

        public void move(Vector3 translation, Vector3 rotation)
        {
            this.position += translation;
            this.rotation += rotation;
        }

        public void draw()
        {
            
        }
    }
}