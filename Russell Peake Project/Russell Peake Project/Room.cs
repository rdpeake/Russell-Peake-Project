using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Henge3D.Physics;
using Henge3D;

namespace Russell_Peake_Project
{
    public class Room : RigidBody, Interfaces.IComponent
    {
        //-------------------------- FROM DEMO
        //model for room
        #region Mesh

        private static VertexPositionNormalTexture[] _vertices = 
		{
			new VertexPositionNormalTexture(new Vector3(-1.0F, -1.0F, 0.0f), Vector3.UnitZ, new Vector2(0,20)), // B-1
			new VertexPositionNormalTexture(new Vector3(-1.0F, 1.0F, 0.0f), Vector3.UnitZ, new Vector2(20,20)), // B-2
			new VertexPositionNormalTexture(new Vector3(1.0F, 1.0F, 0.0f), Vector3.UnitZ, new Vector2(20,0)), // B-3
			new VertexPositionNormalTexture(new Vector3(1.0F, -1.0F, 0.0f), Vector3.UnitZ, new Vector2(0,0)), // B-4

			new VertexPositionNormalTexture(new Vector3(-1.0F, -1.0F, 1.0f), Vector3.UnitZ, new Vector2(0,20)), // T-1
			new VertexPositionNormalTexture(new Vector3(-1.0F, 1.0F, 1.0f), Vector3.UnitZ, new Vector2(20,20)), // T-2
			new VertexPositionNormalTexture(new Vector3(1.0F, 1.0F, 1.0f), Vector3.UnitZ, new Vector2(20,0)), // T-3
			new VertexPositionNormalTexture(new Vector3(1.0F, -1.0F, 1.0f), Vector3.UnitZ, new Vector2(0,0)), // T-4

			new VertexPositionNormalTexture(new Vector3(-1.0F, -1.0F, 1.0f), Vector3.UnitZ, new Vector2(0,0)), // T-1
			new VertexPositionNormalTexture(new Vector3(-1.0F, 1.0F, 1.0f), Vector3.UnitZ, new Vector2(20,0)), // T-2
			new VertexPositionNormalTexture(new Vector3(1.0F, 1.0F, 1.0f), Vector3.UnitZ, new Vector2(40,0)), // T-3
			new VertexPositionNormalTexture(new Vector3(1.0F, -1.0F, 1.0f), Vector3.UnitZ, new Vector2(60,0)), // T-4
			new VertexPositionNormalTexture(new Vector3(-1.0F, -1.0F, 0.0f), Vector3.UnitZ, new Vector2(0,10)), // B-1
			new VertexPositionNormalTexture(new Vector3(-1.0F, 1.0F, 0.0f), Vector3.UnitZ, new Vector2(20,10)), // B-2
			new VertexPositionNormalTexture(new Vector3(1.0F, 1.0F, 0.0f), Vector3.UnitZ, new Vector2(40,10)), // B-3
			new VertexPositionNormalTexture(new Vector3(1.0F, -1.0F, 0.0f), Vector3.UnitZ, new Vector2(60,10)), // B-4

			new VertexPositionNormalTexture(new Vector3(-1.0F, -1.0F, 1.0f), Vector3.UnitZ, new Vector2(80,0)), // T-1
			new VertexPositionNormalTexture(new Vector3(-1.0F, -1.0F, 0.0f), Vector3.UnitZ, new Vector2(80,10)), // B-1
		};

        static short[] _indices =
		{
			2, 1, 0, 0, 3, 2, // floor
			5, 6, 7, 7, 4, 5, // ceiling
			13, 9, 8, 8, 12, 13, // front
			14, 10, 9, 9, 13, 14, // right
			15, 11, 10, 10, 14, 15, // back
			17, 16, 11, 11, 15, 17 // left
		};

        #endregion
        private static float _scale = 20.0f;
        private static Effect effect;
        private static VertexDeclaration _vertexDeclaration;
        Texture2D wallTexture;
        //--------------------------

        //other components
        List<Interfaces.IComponent> GameComponents;

        //trackable object
        public Interfaces.ITrackable TrackedObject { get; private set; }

        //the Physics scene
        PhysicsManager Physics;

        //Game
        Game1 Game;

        public Room(Game1 game, PhysicsManager physics)
        {
            this.Game = game;
            this.Physics = physics;
            this.GameComponents = new List<Interfaces.IComponent>();
        }

        public void initRoom(Effect _effect)
        {
            //set up room -- from demo
            this.Skin.DefaultMaterial = new Material(1f, 0.5f);
            this.Skin.Add(
                new PlanePart(new Vector3(0, 0, 1), -Vector3.UnitZ),
                new PlanePart(new Vector3(0, 0, 0), Vector3.UnitZ),
                new PlanePart(new Vector3(0, 1, 0), -Vector3.UnitY),
                new PlanePart(new Vector3(0, -1, 0), Vector3.UnitY),
                new PlanePart(new Vector3(1, 0, 0), -Vector3.UnitX),
                new PlanePart(new Vector3(-1, 0, 0), Vector3.UnitX)
                );
            this.SetWorld(_scale, Vector3.Zero, Quaternion.Identity);

            int ts = 32;
            wallTexture = new Texture2D(Game.GraphicsDevice, ts, ts);
            Color[] pixels = new Color[ts * ts];
            for (int i = 0; i < ts; i++)
            {
                pixels[i] = Color.Yellow;
                pixels[i * ts] = Color.Yellow;
                pixels[i * ts + (ts - 1)] = Color.Yellow;
                pixels[(ts - 1) * ts + i] = Color.Yellow;
            }
            wallTexture.SetData(pixels);

            effect = _effect;
            _vertexDeclaration = VertexPositionNormalTexture.VertexDeclaration;

        }

        public void init()
        {
            Physics.Clear();
            GameComponents.Clear();

            Physics.Add(this);

            //TODO create all the things
            Revolving_ball rb = new Revolving_ball(Game);
            GameComponents.Add(rb);
            Physics.Add(rb);
            this.TrackedObject = rb;

            Elements.Element e = new Elements.Element1(Game, Physics);
            GameComponents.Add(e);

        }

        public void Draw(Matrix viewProjection, string technique)
        {
            RasterizerState previous = Game.GraphicsDevice.RasterizerState;
            Game.GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = CullMode.CullClockwiseFace
            };  
            Game.GraphicsDevice.SamplerStates[0] = technique == "ShadowMap" ? SamplerState.LinearClamp : SamplerState.LinearWrap;
            //draw model - from demo
            //effect.CurrentTechnique = effect.Techniques[technique + (technique == "ShadowMap" ? "" : "Textured")];
            effect.CurrentTechnique = effect.Techniques["SimplestTextured"];

            effect.Parameters["xCamerasViewProjection"].SetValue(viewProjection);
            effect.Parameters["xLightsViewProjection"].SetValue(Game.light.lightsViewProjectionMatrix);;
            effect.Parameters["xWorld"].SetValue(Transform.Combined);
            effect.Parameters["xLightPos"].SetValue(Game.light.lightPos);
            effect.Parameters["xLightPower"].SetValue(Game.light.lightPower);
            effect.Parameters["xAmbient"].SetValue(Game1.ambientPower);
            effect.Parameters["texture0"].SetValue(this.wallTexture);

            if (technique != "ShadowMap")
            {
                effect.Parameters["xShadowMap"].SetValue((Texture)Game.light);
            }

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //if (technique == "ShadowMap") continue;
                Game.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                    _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);
            }
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            //-------------
            //Game.GraphicsDevice.RasterizerState = new RasterizerState() {FillMode = FillMode.WireFrame};
            Game.GraphicsDevice.RasterizerState = previous;
            foreach (Interfaces.IDrawable i in GameComponents)
            {
                i.Draw(viewProjection, technique);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Interfaces.IUpdateable i in GameComponents)
            {
                i.Update(gameTime);
            }
        }
    }
}
