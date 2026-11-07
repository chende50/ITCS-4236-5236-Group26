using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[SerializeField] private Player player;
	[SerializeField] private float cameraMoveSpeed;

	private Vector3 targetLocation;
	private void Start()
	{
		transform.position = player.transform.position;
		transform.position += new Vector3(0, 0, -10);
	}

	private void LateUpdate()
	{
		targetLocation = player.transform.position + new Vector3(0, 0, -10);

		transform.position = Vector3.Lerp(transform.position, targetLocation, Time.deltaTime * cameraMoveSpeed);
	}
}
