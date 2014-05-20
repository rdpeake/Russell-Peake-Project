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
        Vector3 color;

        public SolidComponent(Game1 game, Model model, Vector3 color)
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
                    effect.Parameters["xViewProjection"].SetValue(view * Game.ProjectionMatrix);

                    //effect.World = mesh.ParentBone.Transform * Transform.Combined;
                    //effect.View = view;
                    //effect.Projection = Game.ProjectionMatrix;
                    //effect.DiffuseColor = this.color;

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
