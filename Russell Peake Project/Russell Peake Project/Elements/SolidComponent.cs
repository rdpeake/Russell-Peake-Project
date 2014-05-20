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
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = Vector3.One * 0.75f;
                    effect.SpecularColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                    effect.LightingEnabled = true; // turn on the lighting subsystem.
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0); // a red light
                    effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
                    effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights
                }
            }
        }

        public void Draw(Matrix view)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.CurrentTechnique = effect.Techniques["Simplest"];
                    //effect.Parameters["xViewProjection"].SetValue(mesh.ParentBone.Transform * Transform.Combined * view * Game.ProjectionMatrix);
                    //effect.Parameters["color"].SetValue(color.ToVector4());

                    //effect.Parameters["xWorld"].SetValue(mesh.ParentBone.Transform * Transform.Combined);
                    //effect.Parameters["xLightPos"].SetValue(Game1.lightPos);
                    //effect.Parameters["xLightPower"].SetValue(Game1.lightPower);
                    //effect.Parameters["xAmbient"].SetValue(Game1.ambientPower);

                    effect.World = mesh.ParentBone.Transform * Transform.Combined;
                    effect.View = view;
                    effect.Projection = Game.ProjectionMatrix;
                    effect.DiffuseColor = this.color.ToVector3();

                    //foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    //{
                    //    pass.Apply();
                    //    mesh.Draw();
                    //}

                }
                mesh.Draw();
            }
        }

        public void Update(GameTime gameTime)
        {
            //do nothing
        }
    }
}
