using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsMenu : MonoBehaviour {

    [SerializeField]
    private Slider speedSlider;

	public void Open () {
        gameObject.SetActive(true);
	}

    public void Close () {
        gameObject.SetActive(false);
	}

    public void OnSubmitName(string name)
    {
        Debug.Log(name);
    }

    public void OnSpeedValue(float speed)
    {

    }
}
