using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Henge3D.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Russell_Peake_Project.Elements
{
    class Element1 : Element
    {
        public Element1(Game1 game, PhysicsManager physics)
            : base(game, physics)
        {
        }

        public override void InitializeComponents()
        {
            SolidComponent sc = new SolidComponent(game, Game1.models["slab"], Color.Green);
            sc.SetWorld(0.5f, new Vector3(14, 19, 17), Quaternion.CreateFromAxisAngle(new Vector3(0,1,0), -MathHelper.ToRadians(25f)));
            Add(sc);

            sc = new SolidComponent(game, Game1.models["slab"], Color.Green);
            sc.SetWorld(0.5f, new Vector3(12, 19, 15), Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.ToRadians(35f)));
            Add(sc);

            sc = new SolidComponent(game, Game1.models["slab"], Color.Green);
            sc.SetWorld(0.5f, new Vector3(14, 19, 14), Quaternion.CreateFromAxisAngle(Vector3.UnitY, -MathHelper.ToRadians(35f)));
            Add(sc);

            //sc = new SolidComponent(game, game.Content.Load<Model>("slab"), Color.Black.ToVector3());
            //sc.SetWorld(1, new Vector3(10, 10, 7), Quaternion.CreateFromAxisAngle(Vector3.UnitY, -MathHelper.ToRadians(25f)));
            //Add(sc);

        }
    }
}
