﻿using System;
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

        public void Draw(Matrix viewProjection, string technique)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques[technique];

                    effect.Parameters["xCamerasViewProjection"].SetValue(viewProjection);
                    effect.Parameters["xLightsViewProjection"].SetValue(Game.light.lightsViewProjectionMatrix);;
                    effect.Parameters["xWorld"].SetValue(mesh.ParentBone.Transform * Transform.Combined);
                    effect.Parameters["xLightPos"].SetValue(Game.light.lightPos);
                    effect.Parameters["xLightPower"].SetValue(Game.light.lightPower);
                    effect.Parameters["xAmbient"].SetValue(Game1.ambientPower);
                    effect.Parameters["color"].SetValue(color.ToVector4());

                    if (technique != "ShadowMap")
                    {
                        effect.Parameters["xShadowMap"].SetValue((Texture)Game.light);
                    }

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
