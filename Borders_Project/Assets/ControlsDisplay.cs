using UnityEngine;

public class DisableGameObject : MonoBehaviour
{
    public GameObject objectToDisable;

    void Update()
    {
        // Check if the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Check if the object to disable is not null
            if (objectToDisable != null)
            {
                // Disable the object
                objectToDisable.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Object to disable is not assigned!");
            }
        }
    }
}