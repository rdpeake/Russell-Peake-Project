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
            SolidComponent sc = new SolidComponent(game, game.Content.Load<Model>("slab"), Color.Black.ToVector3());
            sc.SetWorld(1, new Vector3(10, 10, 5), Quaternion.CreateFromAxisAngle(Vector3.UnitY, -MathHelper.ToRadians(25f)));
            Add(sc);

        }
    }
}
