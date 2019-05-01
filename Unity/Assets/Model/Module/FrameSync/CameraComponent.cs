using UnityEngine;

namespace ETModel
{
	[ObjectSystem]
	public class CameraComponentAwakeSystem : AwakeSystem<CameraComponent>
	{
		public override void Awake(CameraComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class CameraComponentLateUpdateSystem : LateUpdateSystem<CameraComponent>
	{
		public override void LateUpdate(CameraComponent self)
		{
			self.LateUpdate();
		}
	}

	public class CameraComponent : Component
	{
		// 战斗摄像机
		public Camera mainCamera;

		public GameObject Unit;

		public Camera MainCamera
		{
			get
			{
				return this.mainCamera;
			}
		}

		public void Awake()
		{
			this.mainCamera = Camera.main;
            this.mainCamera.transform.eulerAngles = new Vector3(90, 0, 0);
        }

		public void LateUpdate()
		{
			// 摄像机每帧更新位置
			UpdatePosition();
		}

		private void UpdatePosition()
		{
			Vector3 cameraPos = this.mainCamera.transform.position;
			this.mainCamera.transform.position = new Vector3(this.Unit.transform.position.x, this.Unit.transform.position.y + 10, this.Unit.transform.position.z);
        }
	}
}
