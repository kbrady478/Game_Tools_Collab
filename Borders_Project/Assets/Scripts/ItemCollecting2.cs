using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class ItemCollecting2 : MonoBehaviour
{
    public UnityEvent enteredTrigger, exitedTrigger, stayInTrigger;
    private bool isInsideTrigger;
    public GameObject ui, Lockerui, LockerObj, player, objectInTrigger;
    public GameObject pantui, shirtui, passui;
    public TextMeshProUGUI foodtext, watertext, mainchallengetext;
    public int bananaCount, waterCount = 0;
    public bool Pant, Shirt, Passport, mainchallenge = false;
    public AudioClip soundClip;



    void Start()
    {
        foodtext.text = "0/9";
        watertext.text = "0/5";
        mainchallengetext.text = "Look for items";
        mainchallenge = false;
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
        if (Pant == true && Shirt == true && Passport == true && waterCount > 4 && bananaCount >4)
        {
            mainchallengetext.text = "Back to hideout";
            mainchallenge = true;
        }
        if (mainchallenge = true && waterCount < 4 && bananaCount < 4)
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
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
            Debug.Log("Obj in trigger");
            Destroy(objectInTrigger);
            bananaCount++;
            foodtext.text = bananaCount + "/9";
            ui.SetActive(false);
        }
        if (objectInTrigger.CompareTag("Water"))
        {
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
            Debug.Log("Water");
            Destroy(objectInTrigger);
            waterCount++;
            watertext.text = waterCount + "/5";
            ui.SetActive(false);
        }
        
        if (objectInTrigger.CompareTag("Shirt"))
        {
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
            Destroy(objectInTrigger);
            Shirt = true;
            shirtui.SetActive(true);
            ui.SetActive(false);
        }
        if (objectInTrigger.CompareTag("Pant"))
        {
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
            Destroy(objectInTrigger);
            pantui.SetActive(true);
            Pant = true;
            ui.SetActive(false);

        }
        if (objectInTrigger.CompareTag("Passport"))
        {
            AudioSource.PlayClipAtPoint(soundClip, Camera.main.transform.position);
            passui.SetActive(true);
            Destroy(objectInTrigger);
            Passport = true;
            ui.SetActive(false);
        }
        if (objectInTrigger.CompareTag("Bed") && mainchallenge == true)
        {
            Debug.Log("Next scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            //:(idk
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Banana") || other.CompareTag("Water") || other.CompareTag("Pant") || other.CompareTag("Shirt") || other.CompareTag("Passport") || other.CompareTag("Bed"))
        {
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
        if (other.CompareTag("Banana") || other.CompareTag("Water") || other.CompareTag("Pant") || other.CompareTag("Shirt") || other.CompareTag("Passport") || other.CompareTag("Bed"))
        {
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

    void OnTriggerExit(Collider other)
    {

            ui.SetActive(false);
            isInsideTrigger = false;


    }
}
