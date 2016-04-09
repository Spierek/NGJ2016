using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	#region Variables
	public static UIManager Instance;

	public Slider transitionSlider;
	#endregion

	#region Monobehaviour
	private void Awake() {
		Instance = this;
	}
	
	private void Update() {

	}
	#endregion

	#region Methods

	#endregion
}