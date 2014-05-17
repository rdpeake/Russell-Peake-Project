using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Russell_Peake_Project.Interfaces
{
    public interface IDrawable
    {
        void Draw(Matrix view);
    }

    public interface IUpdateable
    {
        void Update(GameTime gameTime);
    }

    public interface IComponent : IDrawable, IUpdateable
    {

    }

    public interface ITrackable
    {
        Vector3 Location { get; }
    }
}
