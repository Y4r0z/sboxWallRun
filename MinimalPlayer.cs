using Sandbox;
using System;
using System.Linq;

namespace MinimalExample
{
	partial class MinimalPlayer : CustomBasePlayer
	{
		
		private Rotation currentRotation;
		private Rotation wishRotation;
		public override void Respawn()
		{
			SetModel( "models/citizenCustom/citizen.vmdl" );

			//
			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			//
			//Controller = new CustomController();
			customController = new CustomController();

			//
			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			//
			Animator = new StandardPlayerAnimator();

			//
			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			//
			Camera = new FirstPersonCamera();


			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			//
			// If you have active children (like a weapon etc) you should call this to 
			// simulate those too.
			//
			SimulateActiveChild( cl, ActiveChild );

			//
			// If we're running serverside and Attack1 was just pressed, spawn a ragdoll
			//

			if (IsServer && Input.Pressed(InputButton.Drop ) )
			{


			}
			
		}

		public override void OnKilled()
		{
			base.OnKilled();

			EnableDrawing = false;
		}
		public override void PostCameraSetup( ref CameraSetup setup )
		{
			base.PostCameraSetup( ref setup );

			if (customController.isWallRunning)
			{
				tiltCamera(ref setup);
				
			}
			else
			{
				wishRotation = Rotation.Identity;
			}

			currentRotation = Rotation.Lerp(currentRotation, wishRotation, 0.2f);

			setup.Rotation *= currentRotation;


		}
		private void tiltCamera(ref CameraSetup setup)
		{
			

			var deltaWallPos = customController.deltaWallPos.WithZ(0);
			var eyeVector = new Vector3( EyeRot.Forward.x, EyeRot.Forward.y, EyeRot.Forward.z ).WithZ( 0 );

			var diffAngle = Vector3.GetAngle( deltaWallPos, eyeVector );


			float tiltAngle = 16f;

			if(Vector3.DistanceBetween(deltaWallPos,EyeRot.Right) < Vector3.DistanceBetween( deltaWallPos, EyeRot.Left ))
			{
				tiltAngle *= -1;
			}

			if (diffAngle > 89f)
			{
				wishRotation = Rotation.FromRoll( tiltAngle * (180 - diffAngle) / 90);
			}
			else
			{
				wishRotation = Rotation.FromRoll( tiltAngle * (diffAngle) / 90);
			}

			//DebugOverlay.ScreenText( (currentCamRoll).ToString() );



		}
	}
}
