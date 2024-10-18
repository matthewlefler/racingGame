using ParticleClass;
using ParticleManagerClass;
using solidColorTextures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Input;
using System;
using CameraClass;
using static ConversionHelper.MathConverter;

namespace FireEffectClass
{
    public class FireEffect
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

        BEPUphysicsDemos.Camera camera;

        Vector3 position;
        Vector3 rotation;
        
        public FireEffect(ParticleManager particleManager, Vector3 rotation, Vector3 position, Vector3 velocitySpread, Vector3 positionSpread, float velocity, float acceleration, float particlesPerSecond, GraphicsDevice graphicsDevice, BEPUphysicsDemos.Camera camera)
        {
            this.particleManager = particleManager;
            this.camera = camera;
            
            this.velocitySpread = velocitySpread;
            this.positionSpread = positionSpread;
            this.random = new Random();
            
            this.velocity = velocity;
            this.acceleration = acceleration;

            this.particlesPerSecond = particlesPerSecond;

            TextureMaker textureMaker = new TextureMaker();

            float startR = 7f;

            for(int i = 0; i < textures.Length/2; i++)
            {
                textures[i] = textureMaker.makeTexture(new Color(((float)i + startR)/(textures.Length/2 + startR), 0f, 0f), 50, 50, graphicsDevice);
            }

            for(int i = textures.Length/2; i < textures.Length; i++)
            {
                textures[i] = textureMaker.makeTexture(new Color(1f, ((float)i + startR)/(textures.Length + startR), 0f), 50, 50, graphicsDevice);
            }
        }

        public void tick(float frameTimeInSeconds)
        {
            time += frameTimeInSeconds;

            while(time > 1f/particlesPerSecond)
            {
                Vector3 velocity =     this.rotation * this.velocity +     new Vector3(this.velocitySpread.X * ((float)random.NextDouble() - 0.5f), this.velocitySpread.Y * ((float)random.NextDouble() - 0.5f), this.velocitySpread.Z * ((float)random.NextDouble() - 0.5f));
                Vector3 acceleration = this.rotation * this.acceleration; // + new Vector3(this.velocitySpread.X * (float)random.NextDouble() - 0.5f), this.velocitySpread.Y * ((float)random.NextDouble() - 0.5f), this.velocitySpread.Z * ((float)random.NextDouble() - 0.5f));
                Vector3 position =     this.position +                     new Vector3(this.positionSpread.X * ((float)random.NextDouble() - 0.5f), this.positionSpread.Y * ((float)random.NextDouble() - 0.5f), this.positionSpread.Z * ((float)random.NextDouble() - 0.5f));

                Particle particle = new Particle(textures[random.Next(0, textures.Length - 1)], position, Convert(camera.ViewDirection), 0.1f, velocity, acceleration, 0f);
                particleManager.addParticle(particle);
                time -= 1f/particlesPerSecond;
            }
        }
    }
}