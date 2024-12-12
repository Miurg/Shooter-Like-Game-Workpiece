using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using System;
using System.Linq;

namespace player
{
    public partial class PlayerCamera : Camera3D
    {
        RayCast3D GeneralRay;
        PlayerMain PlayerMain;
        Node3D Weapons;


        private float _RotationSpeed = 0.1f;
        private int _LerpWeight = 40;
        public Vector2 _CameraInput;
        private Vector2 _RotationVelocity = new();
        private PhysicsDirectSpaceState3D SpaceState;
        public override void _Ready()
        {
            GeneralRay = GetNode<RayCast3D>("GeneralRay");
            PlayerMain = GetNode<PlayerMain>("..");
            Weapons = GetNode<Node3D>("../Weapons");
            SpaceState = GetWorld3D().DirectSpaceState;
        }

        public override void _Process(double delta)
        {
            _RotationVelocity = _CameraInput * _RotationSpeed;
            this.RotateX(-Mathf.DegToRad(_RotationVelocity.Y));
            RotationDegrees = RotationDegrees with { X = Mathf.Clamp(RotationDegrees.X, -90, 90) };

            PlayerMain.RotateY(-Mathf.DegToRad(_RotationVelocity.X));
            Weapons.Rotation = Rotation; 
            _CameraInput = Vector2.Zero;
            
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion)
            {
                _CameraInput = ((InputEventMouseMotion)@event).Relative;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            Select();
        }


        public Node3D InstanceWeapon = null;
        private void Select()
        {
            GeneralRay.ForceRaycastUpdate();

            if (GeneralRay.IsColliding())
            {
                InstanceWeapon = (Node3D)GeneralRay.GetCollider();
                MeshInstance3D meshInstance = InstanceWeapon.GetNode<MeshInstance3D>("MeshInstance3D");
                Vector3[] faces = meshInstance.Mesh.GetFaces();
                Vector2[] unproject = new Vector2[faces.Length];
                for (int i = 0; i<faces.Length;i++)
                {
                    unproject[i] = GetViewport().GetCamera3D().UnprojectPosition(meshInstance.GlobalTransform * faces[i]);
                }

                Vector2 p1 = unproject[0];
                Vector2 p2 = unproject[0];
                foreach (Vector2 p in unproject)
                {
                    p1.X = Math.Min(p1.X, p.X);
                    p1.Y = Math.Min(p1.Y, p.Y);
                    p2.X = Math.Max(p2.X, p.X);
                    p2.Y = Math.Max(p2.Y, p.Y);
                }
                PlayerMain.HUDUpdateSelected((int)(p2.X - p1.X), (int)(p2.Y - p1.Y), (int)p1.X, (int)p1.Y);
            }
            else
            {
                PlayerMain.HUDNormalSelected();
                InstanceWeapon = null;
            }
        }

        public Dictionary RayFromCamera(uint CollisionMask,Vector3 NewRayTarget)
        {
            Vector3 rayStart = ProjectRayOrigin(GetViewport().GetVisibleRect().Size / 2);
            Vector3 rayEnd = rayStart + NewRayTarget.Rotated(new Vector3(1,0,0),Rotation.X).Rotated(new Vector3(0,1,0), PlayerMain.Rotation.Y);
            Dictionary newGeneralRay = SpaceState.IntersectRay(PhysicsRayQueryParameters3D.Create(rayStart, rayEnd, CollisionMask));
            if (newGeneralRay.ContainsKey("collider"))
            {
                return newGeneralRay;
            }
            else
            {
                return null;
            }
        }
    }
}

