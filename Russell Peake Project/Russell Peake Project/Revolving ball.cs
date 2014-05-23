using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Henge3D.Physics;

namespace Russell_Peake_Project
{
    public class Revolving_ball : Russell_Peake_Project.Elements.SolidComponent, Interfaces.ITrackable
    {
        public Revolving_ball(Game1 game) :
            base(game, Game1.models["sphere"], Color.Red)
        {
            SetWorld(new Vector3(14f, 19f, 18f), Quaternion.Identity);
            //this.SetVelocity(10*Vector3.UnitY + 5 *Vector3.UnitX, Vector3.Zero);
        }

        public Vector3 Location
        {
            get { return this.Position; }
        }
    }
}
