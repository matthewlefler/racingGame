namespace PhysicsMaterialClass
{
    public class PhysicsMaterial
    {
        float bouncyness;
        float friction;

        public PhysicsMaterial(float bouncyness, float friction, float gravity)
        {
            this.bouncyness = bouncyness;
            this.friction = friction;
        }
    }
}