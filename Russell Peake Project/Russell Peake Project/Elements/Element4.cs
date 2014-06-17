using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Henge3D.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Russell_Peake_Project.Elements
{
    class Element4 : Element
    {
        public Element4(Game1 game, PhysicsManager physics) : base(game, physics) { }

        public override void InitializeComponents()
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    t = new TrackOnCollision(this.game, Game1.models["obelisk"], Color.Blue);
            //    t.SetWorld(.75f, new Vector3(-14.2f, -12.55f, 4.5f + i * 3f),
            //        Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(180f))));
            //    this.Add(t);
            //}
        }
    }
}
