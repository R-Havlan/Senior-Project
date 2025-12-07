using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public GameObject controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Reset pressed");
        if (controller != null)
        {
            controller.SendMessage("OnResetPressed");
        }
    }
}
