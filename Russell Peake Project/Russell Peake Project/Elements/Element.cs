using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Henge3D.Physics;
using Henge3D;
using Russell_Peake_Project.Interfaces;
using Microsoft.Xna.Framework;

using IDrawable = Russell_Peake_Project.Interfaces.IDrawable;
using IUpdateable = Russell_Peake_Project.Interfaces.IUpdateable;

namespace Russell_Peake_Project.Elements
{
    public abstract class Element : IComponent
    {
        protected Game1 game;
        private PhysicsManager physics;
        private List<SolidComponent> components;

        public abstract void InitializeComponents();

        public Element(Game1 game, PhysicsManager physics)
        {
            this.components = new List<SolidComponent>();
            this.physics = physics;
            this.game = game;

            InitializeComponents();
        }

        public void Add(SolidComponent component)
        {
            components.Add(component);
            physics.Add(component);
        }

        public void Add(Constraint constraint)
        {
            physics.Add(constraint);
        }

        public void Remove(SolidComponent component)
        {
            components.Remove(component);
            physics.Remove(component);
        }

        public void Draw(Matrix view, string technique)
        {
            foreach (IDrawable i in components)
            {
                i.Draw(view, technique);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (IUpdateable i in components)
            {
                i.Update(gameTime);
            }
        }
    }
}
