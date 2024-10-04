using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Collections.Generic;

using ParticleClass;
using System.ComponentModel;
using System.Diagnostics;
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
                particles[i].position += particles[i].acceleration;

                particles[i].acceleration.X *= 0.2f * frameTimeInSeconds;
                particles[i].acceleration.Z *= 0.2f * frameTimeInSeconds;

                particles[i].acceleration.Y += 9.8f * frameTimeInSeconds;

                particles[i].lifeTime += frameTimeInSeconds;

                if(particles[i].lifeTime > 10f)
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

        public void addParticlesFromPositionsRotationsAccelerations(Vector3[] positions, Vector3[] rotations, Vector3[] accelerations, Texture2D texture, float scale)
        {
            if(positions.Length != rotations.Length || rotations.Length != accelerations.Length || positions.Length != accelerations.Length)
            {
                throw new ArgumentException("array position and array rotation and array acceleration are not of same length");
            }
            for(int i = 0; i < positions.Length; i++)
            {
                particles.Add(new Particle(texture, positions[i], rotations[i], scale, accelerations[i]));
            }
        }

        public void addParticlesFromPositions(Vector3[] positions, Vector3 rotation, Vector3 acceleration, Texture2D texture, float scale)
        {
            foreach(Vector3 position in positions)
            {
                particles.Add(new Particle(texture, position, rotation, scale, acceleration));
            }
        }
    }
}