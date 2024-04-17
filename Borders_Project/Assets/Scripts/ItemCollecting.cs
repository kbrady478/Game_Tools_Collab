using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ItemCollecting : MonoBehaviour
{
    public UnityEvent enteredTrigger, exitedTrigger, stayInTrigger;
    private bool isInsideTrigger;
    public GameObject ui, Lockerui, player;
    public TextMeshProUGUI uitext, foodtext, watertext, mainchallengetext;
    public int bananaCount, waterCount = 0;
    public bool Key, Locker, Passport = false;



    void Start()
    {
        foodtext.text = "0/6";
        watertext.text = "0/3";
        mainchallengetext.text = "Find Key";
        
    }

    
    void TriggerAction()
    {

    }
    void Update()
    {
        // Check for input to collect bananas
        if (Input.GetMouseButtonDown(0))
        {
            CollectItem();
            
        }
        if (Locker ==true)
        {
            UnfreezeObjects();
            Lockerui.SetActive(false);
            Debug.Log("LockerTrue");
        }

    }

    void FreezeObjects()
    {
        player.gameObject.GetComponent<PlayerMovement>().enabled = false;
    }

    void UnfreezeObjects()
    {
        player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        Lockerui.SetActive(false);

    }
    void OpenLocker()
    {
        FreezeObjects();
        Lockerui.SetActive(true);
    }

    void CollectItem()
    {
        // Cast a ray from the center of the screen
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Check if the ray hits a banana
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;


            // Check if the hit object is a banana
            if (hitObject.CompareTag("Banana"))
            {
                
                Destroy(hitObject);
                bananaCount++;
                foodtext.text = bananaCount + "/6";
                ui.SetActive(false);
            }
            if (hitObject.CompareTag("Water"))
            {
                // Destroy the banana
                Destroy(hitObject);
                waterCount++;
                watertext.text = waterCount + "/3";
                ui.SetActive(false);
            }
            if (hitObject.CompareTag("Key"))
            {
                Destroy(hitObject);
                mainchallengetext.text = "Find Locker";
                Key = true;
            }
            if (hitObject.CompareTag("Locker") && Key == true)
            {
                OpenLocker();
                mainchallengetext.text = "Get Passport";
            }
            if (hitObject.CompareTag("Passport") && Locker == true)
            {
                Destroy(hitObject);
                mainchallengetext.text = "Find Place to rest";
                Passport = true;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag("Banana"))
         {
            ui.SetActive(true);
            isInsideTrigger = true;
            uitext.text = "Pick up Bananas";
         }
         if (other.CompareTag("Water"))
         {
            ui.SetActive(true);
            isInsideTrigger = true;
            uitext.text = "Pick up Water";
         }
    }

    void OnTriggerStay(Collider other)
    {
          if (other.CompareTag("Banana"))
          {
           stayInTrigger.Invoke();
          }
          if (other.CompareTag("Water"))
          {
            ui.SetActive(true);
            isInsideTrigger = true;
            uitext.text = "Pick up Water";
          }
    }

    void OnTriggerExit(Collider other)
    {
         ui.SetActive(false);

    }
}
