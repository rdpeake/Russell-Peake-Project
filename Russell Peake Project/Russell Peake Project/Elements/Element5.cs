using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Henge3D.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Russell_Peake_Project.Elements
{
    class Element5 : Element
    {
        public Element5(Game1 game, PhysicsManager physics) : base(game, physics) { }

        public override void InitializeComponents()
        {
            SolidComponent s = new SolidComponent(this.game, Game1.models["small_cube"], Color.Violet);
            s.SetWorld(3f, new Vector3(5, 5.5f, 1), Quaternion.Identity);
            s.Freeze();
            this.Add(s);

            s = new SolidComponent(this.game, Game1.models["small_cube"], Color.Violet);
            s.SetWorld(3f, new Vector3(5, 7.4f, 1), Quaternion.Identity);
            s.Freeze();
            this.Add(s);

            TrackOnCollision t = new TrackOnCollision(this.game, Game1.models["obelisk"], Color.Red);
            t.SetWorld(0.75f, new Vector3(5,5.69f,3.15f),
                Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(-65f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f))));
            this.Add(t);

            t = new TrackOnCollision(this.game, Game1.models["obelisk"], Color.Red);
            t.SetWorld(0.75f, new Vector3(5, 7.12f, 3.15f),
                Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(65f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f))));
            this.Add(t);

            for (int i = 0; i < 3; i++)
            {
                t = new TrackOnCollision(this.game, Game1.models["obelisk"], Color.Red);
                t.SetWorld(0.75f, new Vector3(5,4.1f - i * 2f,1.5f),
                    Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f))));
                this.Add(t);
            }

            for (int i = 0; i < 3; i++)
            {
                t = new TrackOnCollision(this.game, Game1.models["obelisk"], Color.Red);
                t.SetWorld(0.75f, new Vector3(5, 8.5f + i*2f, 1.5f),
                    Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f))));
                this.Add(t);
            }

            //    s = new SolidComponent(this.game, Game1.models["sphere"], Color.Brown);
            //s.SetWorld(1f, new Vector3(5, 6.2f, 20), Quaternion.Identity);
            //this.Add(s);
        }
    }
}
