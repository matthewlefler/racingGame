using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Collections.Generic;
using EntityClass;

namespace EntityManagerClass
{
    public class EntityManager
    {
        private List<Entity> entities = new List<Entity>();
        private List<CollisionEntity> physicalEntities = new List<CollisionEntity>();

        public EntityManager()
        {

        }

        public void draw(Effect effect)
        {
            foreach(Entity entity in entities)
            {
                entity.draw(effect);
            }

            foreach(CollisionEntity entity in physicalEntities)
            {
                entity.draw(effect);
            }
        }

        public void physicsTick(float frameTimeInSeconds)
        {
            foreach(Entity entity in entities)
            {
                entity.tick(frameTimeInSeconds);
            }

            foreach(CollisionEntity entity in physicalEntities)
            {
                entity.tick(frameTimeInSeconds);
            }
        }

        public void add(Entity entity)
        {
            this.entities.Add(entity);
        }

        public void add(CollisionEntity entity)
        {
            this.physicalEntities.Add(entity);
        }
    }
}