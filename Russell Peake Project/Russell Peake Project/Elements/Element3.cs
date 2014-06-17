using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Henge3D.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Russell_Peake_Project.Elements
{
    class Element3 : Element
    {
        public Element3(Game1 game, PhysicsManager physics) : base(game, physics) { }

        public override void InitializeComponents()
        {
            TrackOnCollision t;

            for (int i = 0; i < 15; i++)
            {
                t = new TrackOnCollision(this.game, Game1.models["obelisk"], Color.Blue);
                t.SetWorld(.75f, new Vector3(-18, 18.85f - i*2f, 1.5f),
                    Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90f))));
                this.Add(t);
            }

            t = new TrackOnCollision(this.game, Game1.models["obelisk"], Color.Blue);
            t.SetWorld(.75f, new Vector3(-17.3f, -11.15f, 1.5f),
                Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(120f))));
            this.Add(t);

            t = new TrackOnCollision(this.game, Game1.models["obelisk"], Color.Blue);
            t.SetWorld(.75f, new Vector3(-16, -12.15f, 1.5f),
                Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(150f))));
            this.Add(t);
            t = new TrackOnCollision(this.game, Game1.models["obelisk"], Color.Blue);
            t.SetWorld(.75f, new Vector3(-14.2f, -12.55f, 1.5f),
                Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(180f))));
            this.Add(t);

            TrackOnCollision tlast = t;
            //create a stack of dominos on top of last, which the 4th element rests on.
            for (int i = 0; i < 5; i++)
            {
                t = new TrackOnCollision(this.game, Game1.models["obelisk"], Color.Blue);
                t.SetWorld(.75f, new Vector3(-14.2f, -12.55f, 4.5f + i*3f),
                    Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(180f))));
                this.Add(t);
                GenericConstraint c = new GenericConstraint(tlast, t, new Henge3D.Frame(tlast.Position), Henge3D.Axes.All, Vector3.Zero, Vector3.Zero,
                    Henge3D.Axes.All, Vector3.Zero, Vector3.Zero);
                c.IsCollisionEnabled = false;
                this.Add(c);
                tlast = t;
            }
            t.Freeze();

        }
    }
}
