using UnityEngine;

public class TestMonoBehaviourScript : MonoBehaviour
{

    private UIGridSelector _uIGridSelector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _uIGridSelector = new UIGridSelector(3, 2);
    }

    // Update is called once per frame
    void Update()
    {
        _uIGridSelector.Update();
    }
}
