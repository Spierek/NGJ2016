using UnityEngine;

public class Crosshair : MonoBehaviour {
	#region Monobehaviour
	private void Start()
	{
		Cursor.visible = false;
	}

	private void Update() {
		SetPosition();
	}
	#endregion

	#region Methods
	private void SetPosition()
	{
		Vector3 tempV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		tempV3.z = 0;

		transform.position = tempV3;
	}
	#endregion
}