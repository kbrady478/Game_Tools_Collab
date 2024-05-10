using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class ItemCollecting : MonoBehaviour
{
    public UnityEvent enteredTrigger, exitedTrigger, stayInTrigger;
    private bool isInsideTrigger;
    public GameObject ui, Lockerui, LockerObj, player, objectInTrigger;
    public TextMeshProUGUI foodtext, watertext, Medtext, mainchallengetext;
    public int bananaCount, waterCount, MedCount = 0;
    public bool Key, Locker, Passport, mainchallenge = false;



    void Start()
    {
        foodtext.text = "0/5";
        watertext.text = "0/3";
        Medtext.text = "0/1";
        mainchallengetext.text = "Collect Medicine";
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
        if (MedCount == 1 && waterCount > 1 && bananaCount >2)
        {
            mainchallengetext.text = "Back to hideout";
            mainchallenge = true;
        }
        if (MedCount == 1 && waterCount < 1 && bananaCount < 2)
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
            foodtext.text = bananaCount + "/5";
            ui.SetActive(false);
        }
        if (objectInTrigger.CompareTag("Water"))
        {
            Debug.Log("Water");
            Destroy(objectInTrigger);
            waterCount++;
            watertext.text = waterCount + "/3";
            ui.SetActive(false);
        }
        if (objectInTrigger.CompareTag("Medicine"))
        {
            Destroy(objectInTrigger);
            MedCount++;
            Medtext.text = MedCount + "/1";
            ui.SetActive(false);
        }

        if (objectInTrigger.CompareTag("Bed") && mainchallenge == true)
        {
            Debug.Log("Next scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Banana") || other.CompareTag("Water") || other.CompareTag("Keys") || other.CompareTag("Locker") || other.CompareTag("Bed") || other.CompareTag("Medicine"))
        {
            ui.SetActive(true);
            isInsideTrigger = true;
            objectInTrigger = other.gameObject;
            Debug.Log(objectInTrigger);

        }
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Banana") || other.CompareTag("Water") || other.CompareTag("Keys") || other.CompareTag("Locker") || other.CompareTag("Bed") || other.CompareTag("Medicine"))
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
