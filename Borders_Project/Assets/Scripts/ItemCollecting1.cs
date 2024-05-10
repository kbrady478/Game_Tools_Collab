using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class ItemCollecting1 : MonoBehaviour
{
    public UnityEvent enteredTrigger, exitedTrigger, stayInTrigger;
    private bool isInsideTrigger;
    public GameObject ui, Lockerui, LockerObj, player, objectInTrigger;
    public TextMeshProUGUI foodtext, watertext, mainchallengetext;
    public int bananaCount, waterCount = 0;
    public bool Key, Locker, Passport, mainchallenge = false;



    void Start()
    {
        foodtext.text = "0/7";
        watertext.text = "0/4";
        mainchallengetext.text = "Find Key"; 
    }

    
    void TriggerAction()
    {

    }
    void Update()

    {
        if (isInsideTrigger == true && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("mouse press");
            CollectItem();
        }
        if (Locker == true && waterCount > 2 && bananaCount >3)
        {
            mainchallengetext.text = "Back to hideout";
            mainchallenge = true;
        }
        if (Locker == true && waterCount < 2 && bananaCount < 3)
        {
            mainchallengetext.text = "Collect more supplies";
        }

    }

    void FreezeObjects()
    {
       player.gameObject.GetComponent<ActualMovement>().enabled = false;
    }

    void UnfreezeObjects()
    {
       player.gameObject.GetComponent<ActualMovement>().enabled = true;
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
            foodtext.text = bananaCount + "/7";
            ui.SetActive(false);
        }
        if (objectInTrigger.CompareTag("Water"))
        {
            Debug.Log("Water");
            Destroy(objectInTrigger);
            waterCount++;
            watertext.text = waterCount + "/4";
            ui.SetActive(false);
        }
        
        if (objectInTrigger.CompareTag("Key"))
        {
            Debug.Log("Collect key");
            Destroy(objectInTrigger);
            mainchallengetext.text = "Find Chest";
            Key = true;
            ui.SetActive(false);
        }
        if (objectInTrigger.CompareTag("Locker") && Key == true)
        {
            Destroy(objectInTrigger);
            Locker = true;
            //OpenLocker();
            Debug.Log("LockerTrue");
        }
        if (objectInTrigger.CompareTag("Bed") && mainchallenge == true)
        {
            Debug.Log("Next scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Banana") || other.CompareTag("Water") || other.CompareTag("Key") || other.CompareTag("Locker") || other.CompareTag("Bed"))
        {
            ui.SetActive(true);
            isInsideTrigger = true;
            objectInTrigger = other.gameObject;
            Debug.Log(objectInTrigger);

        }
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Banana") || other.CompareTag("Water") || other.CompareTag("Key") || other.CompareTag("Locker") || other.CompareTag("Bed"))
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
