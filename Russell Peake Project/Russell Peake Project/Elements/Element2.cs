using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Henge3D.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Russell_Peake_Project.Elements
{
    class Element2 : Element
    {
        public Element2(Game1 game, PhysicsManager physics) : base(game, physics) { }

        public override void InitializeComponents()
        {
            float angle = 0.6f;
            SolidComponent sc = new SolidComponent(game, Game1.models["rail"], Color.Silver);
            sc.SetWorld(.5f, new Vector3(6, 19.5f, 12.5f), Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(87f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-angle))));            
            Add(sc);

            sc = new SolidComponent(game, Game1.models["rail"], Color.Silver);
            sc.SetWorld(.5f, new Vector3(6, 18.7f, 12.5f), Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(87f))*Matrix.CreateRotationZ(MathHelper.ToRadians(angle))));
            Add(sc);

            angle = 0.6f;
            sc = new SolidComponent(game, Game1.models["rail"], Color.Silver);
            sc.SetWorld(.5f, new Vector3(0, 19.5f, 10.5f), Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(87f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-angle))));
            Add(sc);

            sc = new SolidComponent(game, Game1.models["rail"], Color.Silver);
            sc.SetWorld(.5f, new Vector3(0, 18.7f, 10.5f), Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(87f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(angle))));
            Add(sc);

            angle = 0.0f;
            sc = new SolidComponent(game, Game1.models["rail"], Color.Silver);
            sc.SetWorld(.5f, new Vector3(-6, 19.4f, 8.5f), Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(80f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(-angle))));
            Add(sc);

            sc = new SolidComponent(game, Game1.models["rail"], Color.Silver);
            sc.SetWorld(.5f, new Vector3(-6, 18.8f, 8.5f), Quaternion.CreateFromRotationMatrix(Matrix.CreateRotationY(MathHelper.ToRadians(80f)) * Matrix.CreateRotationZ(MathHelper.ToRadians(angle))));
            Add(sc);

        }
    }
}
