using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Henge3D.Physics;
using ITrackable = Russell_Peake_Project.Interfaces.ITrackable;

namespace Russell_Peake_Project.Elements
{
    class TrackOnCollision : SolidComponent, ITrackable
    {
        protected Room room;
        protected bool hasMoved = false;

        public TrackOnCollision(Game1 Game, Model model, Color color) : base(Game, model, color )
        {
            this.room = Game.machine;
            this.OnCollision = new CollisionEventHandler(this.onCollide);
        }

        protected bool onCollide(RigidBody r1, RigidBody r2)
        {
            if (r2 == room.TrackedObject)
            {
                room.TrackedObject = r1 as ITrackable;
                this.OnCollision = null;
            }
            return false;
        }

        public Vector3 Location
        {
            get { return this.Position; }
        }
    }
}
