using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using ParticleClass;
using System;

namespace ParticleManagerClass
{
    public class ParticleManager
    {
        #region parameters

        List<Particle> particles;
        public int particlesLength { get {return particles.Count; } private set {} }

        #endregion
        

        public ParticleManager()
        {
            this.particles = new List<Particle>();
        }

        public void draw(GraphicsDevice graphicsDevice, Effect effect)
        {
            foreach(Particle particle in particles)
            {
                particle.draw(effect, graphicsDevice);
            }
        }

        public void physicsTick(float frameTimeInSeconds)
        {
            for(int i = 0; i < particles.Count; i++)
            {
                particles[i].position += particles[i].velocity * frameTimeInSeconds;
                particles[i].velocity += particles[i].acceleration * frameTimeInSeconds;

                particles[i].acceleration.X -= frameTimeInSeconds * MathF.Sign(particles[i].acceleration.X);
                particles[i].acceleration.Z -= frameTimeInSeconds * MathF.Sign(particles[i].acceleration.Y);

                particles[i].velocity.X -= 0.1f * frameTimeInSeconds * MathF.Sign(particles[i].velocity.X);
                particles[i].velocity.Z -= 0.1f * frameTimeInSeconds * MathF.Sign(particles[i].velocity.Y);

                particles[i].acceleration.Y -= particles[i].gravity * frameTimeInSeconds;

                particles[i].lifeTime += frameTimeInSeconds;

                if(particles[i].lifeTime > 5f)
                {
                    particles.Remove(particles[i]);
                    i--;
                }
            }
        }

        public void addParticle(Particle particle)
        {
            this.particles.Add(particle);
        }

        public void addParticlesFromPositionsRotationsAccelerations(Vector3[] positions, Vector3[] rotations, Vector3[] velocities, Vector3[] accelerations, float gravity, Texture2D texture, float scale)
        {
            if(positions.Length != rotations.Length || positions.Length != accelerations.Length || positions.Length != velocities.Length)
            {
                throw new ArgumentException("array position and array rotation and array acceleration and array acceleration are not of same length");
            }
            for(int i = 0; i < positions.Length; i++)
            {
                particles.Add(new Particle(texture, positions[i], rotations[i], scale, velocities[i], accelerations[i], gravity));
            }
        }

        public void addParticlesFromPositions(Vector3[] positions, Vector3 rotation, Vector3 velocity, Vector3 acceleration, float gravity, Texture2D texture, float scale)
        {
            foreach(Vector3 position in positions)
            {
                particles.Add(new Particle(texture, position, rotation, scale, velocity, acceleration, gravity));
            }
        }
    }
}