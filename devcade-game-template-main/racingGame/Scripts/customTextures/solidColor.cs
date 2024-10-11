using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace solidColorTextures
{
    public class TextureMaker
    {
        public TextureMaker()
        {

        }

        public Texture2D makeTexture(Color color, int width, int height, GraphicsDevice graphicsDevice)
        {
            Color[] colors = new Color[width * height];
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    colors[i * width + j] = color;
                }    
            }

            Texture2D newTexture = new Texture2D(graphicsDevice, width, height);

            newTexture.SetData<Color>(colors);

            return newTexture;
        }

    }
}