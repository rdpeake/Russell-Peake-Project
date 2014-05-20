﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Henge3D.Physics;

namespace Russell_Peake_Project
{
    public class Revolving_ball : RigidBody, Interfaces.ITrackable, Interfaces.IComponent
    {
        Model model;

        Game1 Game;
        public Revolving_ball(Game1 game) :
            base((Henge3D.Pipeline.RigidBodyModel)Game1.models["sphere"].Tag)
        {
            this.Game = game;

            model = Game1.models["sphere"];

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

            SetWorld(new Vector3(14f, 19f, 18f), Quaternion.Identity);
            //this.SetVelocity(10*Vector3.UnitY + 5 *Vector3.UnitX, Vector3.Zero);
        }

        public void Update(GameTime gameTime)
        {
            //float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //angle += MathHelper.ToRadians(45f) * delta;
        }

        public Vector3 Location
        {
            get { return this.Position; }
        }

        public void Draw(Matrix view)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques["Simplest"];
                    effect.Parameters["xViewProjection"].SetValue(Transform.Combined * view * Game.ProjectionMatrix);
                    effect.Parameters["color"].SetValue(Color.Red.ToVector4());

                    //effect.World = mesh.ParentBone.Transform * Transform.Combined;
                    //effect.View = view;
                    //effect.Projection = Game.ProjectionMatrix;
                    //effect.DiffuseColor = Color.Red.ToVector3();

                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        mesh.Draw();
                    }
                }
            }
        }
    }
}
