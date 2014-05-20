using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Henge3D;
using Henge3D.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Russell_Peake_Project.Interfaces;
using IDrawable = Russell_Peake_Project.Interfaces.IDrawable;

namespace Russell_Peake_Project.Elements
{
    public class SolidComponent : RigidBody, IComponent
    {
        Game1 Game;
        Model model;
        Color color;

        public SolidComponent(Game1 game, Model model, Color color)
            : base ((Henge3D.Pipeline.RigidBodyModel)model.Tag)
        {
            this.Game = game;
            this.model = model;
            this.color = color;
            //foreach (var mesh in model.Meshes)
            //{
            //    foreach (Effect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();
            //        effect.AmbientLightColor = Vector3.One * 0.75f;
            //        effect.SpecularColor = Vector3.One;
            //        effect.PreferPerPixelLighting = true;
            //    }
            //}
        }

        public void Draw(Matrix view)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["Simplest"];

                    effect.Parameters["xCamerasViewProjection"].SetValue(view * Game.ProjectionMatrix);
                    //effect.Parameters["xLightsViewProjection"].SetValue(lightsViewProjectionMatrix);;
                    effect.Parameters["xWorld"].SetValue(mesh.ParentBone.Transform * Transform.Combined);
                    effect.Parameters["xLightPos"].SetValue(Game1.lightPos);
                    effect.Parameters["xLightPower"].SetValue(Game1.lightPower);
                    effect.Parameters["xAmbient"].SetValue(Game1.ambientPower);
                    effect.Parameters["color"].SetValue(color.ToVector4());

                    //effect.Parameters["xShadowMap"].SetValue(shadowMap);

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        mesh.Draw();
                    }

                }
            }
        }

        public void Update(GameTime gameTime)
        {
            //do nothing
        }
    }
}
