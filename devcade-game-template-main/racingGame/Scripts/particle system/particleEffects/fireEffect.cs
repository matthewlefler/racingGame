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

        Vector3 velocitySpread;
        Vector3 positionSpread;
        Random random;
        
        float time = 0f;
        float particlesPerSecond;
        float velocity;
        float acceleration;

        
        public FireEffect(ParticleManager particleManager, Vector3 rotation, Vector3 position, Vector3 velocitySpread, Vector3 positionSpread, float velocity, float acceleration, float particlesPerSecond, GraphicsDevice graphicsDevice) : base(position, rotation)
        {
            this.particleManager = particleManager;
            
            this.velocitySpread = velocitySpread;
            this.positionSpread = positionSpread;
            this.random = new Random();
            
            this.velocity = velocity;
            this.acceleration = acceleration;

            this.particlesPerSecond = particlesPerSecond;

            TextureMaker textureMaker = new TextureMaker();

            float startR = 0.1f;

            for(int i = 0; i < textures.Length/2; i++)
            {
                textures[i] = textureMaker.makeTexture(new Color(((float)i + startR)/(textures.Length/2 + startR), 0f, 0f), 50, 50, graphicsDevice);
            }

            for(int i = textures.Length/2; i < textures.Length; i++)
            {
                textures[i] = textureMaker.makeTexture(new Color(1f, ((float)i + startR)/(textures.Length + startR), 0f), 50, 50, graphicsDevice);
            }
        }

        public override void tick(float frameTimeInSeconds)
        {
            time += frameTimeInSeconds;

            if(time > 1f/particlesPerSecond)
            {
                Vector3 velocity =     this.rotation * this.velocity +     new Vector3(this.velocitySpread.X * ((float)random.NextDouble() - 0.5f), this.velocitySpread.Y * ((float)random.NextDouble() - 0.5f), this.velocitySpread.Z * ((float)random.NextDouble() - 0.5f));
                Vector3 acceleration = this.rotation * this.acceleration; // + new Vector3(this.velocitySpread.X * (float)random.NextDouble() - 0.5f), this.velocitySpread.Y * ((float)random.NextDouble() - 0.5f), this.velocitySpread.Z * ((float)random.NextDouble() - 0.5f));
                Vector3 position =     this.position +                     new Vector3(this.positionSpread.X * ((float)random.NextDouble() - 0.5f), this.positionSpread.Y * ((float)random.NextDouble() - 0.5f), this.positionSpread.Z * ((float)random.NextDouble() - 0.5f));

                Particle particle = new Particle(textures[random.Next(0, textures.Length - 1)], position, rotation, 0.1f, velocity, acceleration, 0f);
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