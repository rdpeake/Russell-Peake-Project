using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Russell_Peake_Project
{
    public class Light
    {

        private static VertexPositionNormalTexture[] _vertices = 
		{
			new VertexPositionNormalTexture(new Vector3(0.0F, 0.0F, 0.0f), Vector3.UnitZ, new Vector2(0,0)), // B-1
			new VertexPositionNormalTexture(new Vector3(0.0F, 1.0F, 0.0f), Vector3.UnitZ, new Vector2(0,0)), // B-2
			new VertexPositionNormalTexture(new Vector3(1.0F, 1.0F, 0.0f), Vector3.UnitZ, new Vector2(0,0)), // B-3
			new VertexPositionNormalTexture(new Vector3(1.0F, 0.0F, 0.0f), Vector3.UnitZ, new Vector2(0,0)), // B-4

            new VertexPositionNormalTexture(new Vector3(0.0F, 0.0F, 1.0f), Vector3.UnitZ, new Vector2(1,1)), // T-1
            new VertexPositionNormalTexture(new Vector3(0.0F, 1.0F, 1.0f), Vector3.UnitZ, new Vector2(1,1)), // T-2
            new VertexPositionNormalTexture(new Vector3(1.0F, 1.0F, 1.0f), Vector3.UnitZ, new Vector2(1,1)), // T-3
            new VertexPositionNormalTexture(new Vector3(1.0F, 0.0F, 1.0f), Vector3.UnitZ, new Vector2(1,1)), // T-4

            new VertexPositionNormalTexture(new Vector3(0.0F, 0.0F, 0.0f), Vector3.UnitZ, new Vector2(-1,-1)), // point
		};

        private static Vector2[] bounding = {
                                                new Vector2(-1,-1),
                                                new Vector2(-1,1),
                                                new Vector2(1,1),
                                                new Vector2(1,-1)
                                            };

        static short[] _indices =
		{
			//2, 1, 0, 0, 3, 2, // floor
            2,3,0,0,1,2,
            5, 6, 7, 7, 4, 5, // ceiling
            1, 5, 4, 4, 0, 1, // front
            2, 6, 5, 5, 1, 2, // right
            3, 7, 6, 6, 2, 3, // back
            0, 4, 7, 7, 3, 0 // left
		};


        Game1 Game;
        RenderTarget2D renderTarget;
        public Vector3 lightPos;
        public float lightPower;

        public Matrix lightsViewProjectionMatrix;
        private Effect effect;

        

        public Light(Game1 game, Vector3 Position, float power, Effect _effect)
        {
            this.Game = game;
            this.effect = _effect;
            renderTarget = new RenderTarget2D(game.GraphicsDevice, 
                game.GraphicsDevice.PresentationParameters.BackBufferWidth, 
                game.GraphicsDevice.PresentationParameters.BackBufferHeight, 
                false, SurfaceFormat.Color, DepthFormat.Depth24);
            lightPos = Position;
            _vertices[8].Position = lightPos;
            lightPower = power;
        }

        public void update(Vector3 ballPos)
        {
            Matrix lightsView = Matrix.CreateLookAt(lightPos, ballPos, new Vector3(0, 0, 1));
            Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 5f, 40f);

            lightsViewProjectionMatrix = lightsView * lightsProjection;
            Matrix iviewprojection = Matrix.Invert(lightsViewProjectionMatrix);
            int i = 4;
            foreach (Vector2 v in bounding)
            {
                Vector4 transformed = Vector4.Transform(v, iviewprojection);
                Vector3 position = new Vector3(transformed.X / transformed.W, transformed.Y / transformed.W, transformed.Z / transformed.W);
                Vector3 direction = (position - lightPos);
                direction = direction / 5;
                _vertices[i - 4].Position = lightPos + 0 * direction;
                _vertices[i++].Position = lightPos + 0.5f * direction; 
            }            
        }

        public static explicit operator Texture(Light l)
        {
            return l.renderTarget;
        }

        public void drawShadowMap()
        {
            Game.GraphicsDevice.SetRenderTarget(this.renderTarget);
            Game.GraphicsDevice.Clear(Color.Black);
            Game.machine.Draw(lightsViewProjectionMatrix, "ShadowMap");
        }

        public void drawLight(Matrix viewProjection)
        {
            //TODO decide to keep or not...
            //return;
            RasterizerState previous = Game.GraphicsDevice.RasterizerState;
            Game.GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.CullClockwiseFace
            };
            

            effect.CurrentTechnique = effect.Techniques["LightSource"];

            effect.Parameters["xCamerasViewProjection"].SetValue(viewProjection);
            effect.Parameters["xLightsViewProjection"].SetValue(Game.light.lightsViewProjectionMatrix); ;
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            effect.Parameters["xLightPos"].SetValue(Game.light.lightPos);
            effect.Parameters["xLightPower"].SetValue(Game.light.lightPower);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                    _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);
            }
            Game.GraphicsDevice.RasterizerState = previous;

        }
    }
}
