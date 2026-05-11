using UnityEngine;

namespace Clouds
{
	public class CloudFollow : MonoBehaviour
	{
		public Transform target;

		public float cloudHeight;

		private void Update()
		{
			base.transform.position = new Vector3(target.position.x, cloudHeight, target.position.z);
		}
	}
}
