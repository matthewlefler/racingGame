using ParticleClass;
using ParticleManagerClass;
using solidColorTextures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;
using EntityClass;
using Microsoft.Xna.Framework.Input;
using System;

namespace FireEffectClass
{
    public class FireEffect : Entity
    {
        ParticleManager particleManager;
        Texture2D[] textures = new Texture2D[10];

        Vector3 spread;
        Random random;
        
        float time = 0f;
        float particlesPerSecond;
        float velocity;
        float acceleration;

        
        public FireEffect(ParticleManager particleManager, Vector3 rotation, Vector3 position, Vector3 spread, float velocity, float acceleration, float particlesPerSecond, GraphicsDevice graphicsDevice) : base(position, rotation)
        {
            this.particleManager = particleManager;
            
            this.spread = spread;
            this.random = new Random();
            
            this.velocity = velocity;
            this.acceleration = acceleration;

            this.particlesPerSecond = particlesPerSecond;

            TextureMaker textureMaker = new TextureMaker();

            float startR = 0.1f;

            for(int i = 0; i < textures.Length; i++)
            {
                textures[i] = textureMaker.makeTexture(new Color((i + startR)/(textures.Length + startR), 0f, 0f), 10, 10, graphicsDevice);
            }
        }

        public override void tick(float frameTimeInSeconds)
        {
            time += frameTimeInSeconds;

            if(time > 1f/particlesPerSecond)
            {
                Vector3 velocity = rotation * this.velocity + spread * ((float)random.NextDouble() - 0.5f);
                Vector3 acceleration = rotation * this.acceleration + spread * ((float)random.NextDouble() - 0.5f);

                Particle particle = new Particle(textures[0], position, rotation, 0.1f, velocity, acceleration, 0f);
                particleManager.addParticle(particle);
                time -= 1f/particlesPerSecond;
            }
        }

        public override void draw(Effect effect)
        {
            //skip this as the particle manager will handle drawing the particles 
            return;
        }
    }
}