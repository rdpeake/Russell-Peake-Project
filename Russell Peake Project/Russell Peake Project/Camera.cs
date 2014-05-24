using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Russell_Peake_Project
{
    //borrowed code from henge3d hollowdeck code
    class Camera
    {
        private Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private float _yaw = 0.0f;
        public float Yaw
        {
            get { return _yaw; }
            set { _yaw = (value >= MathHelper.TwoPi) ? value % MathHelper.TwoPi : value; }
        }

        private float minPitch = float.NegativeInfinity;

        public float MinPitch
        {
            get { return minPitch; }
            set { minPitch = value; }
        }

        private float maxPitch = float.PositiveInfinity;

        public float MaxPitch
        {
            get { return maxPitch; }
            set { maxPitch = value; }
        }

        private float _pitch = 0.0f;
        public float Pitch
        {
            get { return _pitch; }
            set { _pitch = MathHelper.Clamp(value, minPitch, maxPitch); }
        }

        public Vector3 Direction
        {
            get { 
                return Vector3.TransformNormal(
                    _forwardAxis,
                    Matrix.CreateFromAxisAngle(_sideAxis, _pitch) * Matrix.CreateFromAxisAngle(_upAxis, _yaw));
            }
            set
            {
                this.Pitch = (float)Math.Asin(Vector3.Dot(_upAxis, value));
                this.Yaw = (float)Math.Atan2(
                    Vector3.Dot(-_forwardAxis, value), Vector3.Dot(-_sideAxis, value));
            }
        }
        private Vector3 _upAxis = Vector3.UnitY;
        public Vector3 UpAxis
        {
            get { return _upAxis; }
            set {
                _upAxis = value;
                //create side from up and forward
                Vector3.Cross(ref _forwardAxis, ref _upAxis, out _sideAxis);
            }
        }
        private Vector3 _forwardAxis = -Vector3.UnitZ;
        public Vector3 ForwardAxis
        {
            get { return _forwardAxis; }
            set { 
                _forwardAxis = value;
                //create side from up and forward
                Vector3.Cross(ref _forwardAxis, ref _upAxis, out _sideAxis);
            }
        }
        //dependand on up and side but fixed
        private Vector3 _sideAxis = Vector3.UnitX;

        public Vector3 SideAxis
        {
            get { return _sideAxis; }
        }

        private Matrix _viewMatrix;
        private Game1 Game;
        private Matrix ProjectionMatrix;

        public Camera(Game1 game, Vector3 Position, Matrix Projection,float yaw = 0.0f, float pitch = 0.0f)
        {
            _position = Position;
            Yaw = yaw;
            Pitch = pitch;
            Game = game;
            this.ProjectionMatrix = Projection;
        }

        public void Move(Vector3 delta)
        {
            if (delta != Vector3.Zero)
            {
                Vector3 sideAxis;
                Vector3.Cross(ref _forwardAxis, ref _upAxis, out sideAxis);
                _position += Vector3.Transform(
                    delta,
                    Matrix.CreateFromAxisAngle(sideAxis, _pitch) * Matrix.CreateFromAxisAngle(_upAxis, _yaw));
            }
        }

        public void lookat(Vector3 at, Vector3 from)
        {
            Vector3 direction = at - from;
                if (at == from)
                {
                    direction = Vector3.UnitZ;
                }
                if (direction.Length() > 0)
                {
                    direction.Normalize();
                    this.ForwardAxis = direction;
                    this.Position = at;
                    this.Move(-direction * 4f);
                }
        }

        public void draw(bool lights)
        {
            Vector3 look = this.Direction;
            Vector3.Add(ref _position, ref look, out look);

            Matrix.CreateLookAt(
                ref _position,
                ref look,
                ref _upAxis,
                out _viewMatrix);

            //loop through game.physics.bodies and draw them??
            Game.machine.Draw(_viewMatrix * ProjectionMatrix, lights ? "ShadowedScene" : "Simplest");
            if (lights) {
                Game.light.drawLight(_viewMatrix * ProjectionMatrix);
            }
        }

    }
}
