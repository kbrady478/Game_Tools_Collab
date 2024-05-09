using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ItemCollecting : MonoBehaviour
{
    public UnityEvent enteredTrigger, exitedTrigger, stayInTrigger;
    private bool isInsideTrigger;
    public GameObject ui, Lockerui, player, objectInTrigger;
    public TextMeshProUGUI foodtext, watertext, mainchallengetext;
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
        

        if (Locker ==true)
        {
            UnfreezeObjects();
            Lockerui.SetActive(false);
            Debug.Log("LockerTrue");
        }

    }

    void FreezeObjects()
    {
       // player.gameObject.GetComponent<PlayerMovement>().enabled = false;
    }

    void UnfreezeObjects()
    {
       // player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        Lockerui.SetActive(false);

    }
    void OpenLocker()
    {
        FreezeObjects();
        Lockerui.SetActive(true);
    }

    void CollectItem()
    {
        Debug.Log("collect item");

        // Check if the hit object is a banana
        if (objectInTrigger.CompareTag("Banana"))
        {
            Debug.Log("Obj in trigger");
            Destroy(objectInTrigger);
            bananaCount++;
            foodtext.text = bananaCount + "/6";
            ui.SetActive(false);
        }
        else if (objectInTrigger.CompareTag("Water"))
        {
            Debug.Log("Water");
            Destroy(objectInTrigger);
            waterCount++;
            watertext.text = waterCount + "/3";
            ui.SetActive(false);
        }
        else if (objectInTrigger.CompareTag("Key"))
        {
            Destroy(objectInTrigger);
            mainchallengetext.text = "Find Locker";
            Key = true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Banana") || other.CompareTag("Water"))
        {
            Debug.Log("See Banana");
            ui.SetActive(true);
            isInsideTrigger = true;
            objectInTrigger = other.gameObject;
            Debug.Log(objectInTrigger);
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse Shity");
                CollectItem();



            }

        }
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Banana") || other.CompareTag("Water"))
        {
            ui.SetActive(true);
            isInsideTrigger = true;
            objectInTrigger = other.gameObject;
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse Shity");
                CollectItem();



            }


        }
    }

    void OnTriggerExit(Collider other)
    {

            ui.SetActive(false);
            isInsideTrigger = false;


    }
}
