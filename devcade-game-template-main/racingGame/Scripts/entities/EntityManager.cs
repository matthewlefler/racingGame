using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using EntityClass;
using System.Dynamic;

namespace EntityManagerClass
{
    public class EntityManager
    {
        public List<Entity> entities { get; private set; }
        public List<CollisionEntity> physicalEntities { get; private set; }

        public EntityManager()
        {
            entities = new List<Entity>();
            physicalEntities = new List<CollisionEntity>();
        }

        public void draw(BasicEffect effect)
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
                entity.tick(frameTimeInSeconds, this.physicalEntities.ToArray());
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