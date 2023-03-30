using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [TabGroup("GamePlay")]
    public float speed = 15f; // Vertical movement sensitivity for the player
    [Title("Controlling")]
    [TabGroup("GamePlay")]
    public float horizontalSens = 0.4f; // Horizontal movement sensitivity for the player
    [TabGroup("GamePlay")]
    public float xMin = -5.0f; // Minimum target x position value for the player
    [TabGroup("GamePlay")]
    public float xMax = 5.0f; // Maksimum target x position value for the player

    [TabGroup("GameData")]
    public float camSpeed = 1f; // Maksimum target x position value for the player
    [TabGroup("GameData")]
    public float collectedCupDistance = 4f; // Maksimum target x position value for the player

    [TabGroup("GameObjects")]
    public GameObject cam; // The main camera in the scene
    [TabGroup("GameObjects")]
    public GameObject player; // The player object in the scene
    [TabGroup("GameObjects")]
    public GameObject collectedCups; // The cups object in the scene

    Vector3 camOffset; // The offset value between the player and camera at the start of the game
    Vector3 targetPosZ; // The target position value used for updating the player's Z position

    int cupCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        // MUST BE UPDATED AS TO FALLOW HAND!
        camOffset = player.transform.position - cam.transform.position; // Calculate the offset value between the player and camera
        cupCount = collectedCups.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the player's X position when the left mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            UpdatePlayerPositionX();
        }

        // Update the player's Z position
        UpdatePlayerPositionZ();

        // Update the camera's position
        UpdateCamPosition();

        // Restart the game when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    // Update the camera's position to follow the player's position according to camOffset
    void UpdateCamPosition()
    {
        //cam.transform.position = player.transform.position - camOffset;

        cam.transform.position = Vector3.Lerp(cam.transform.position, player.transform.position - camOffset, camSpeed * Time.deltaTime) ;
    }

    // Update the players's horizontal/X position while mouse/finger is moving
    void UpdatePlayerPositionX()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float moveAmount = mouseX * horizontalSens;
        Vector3 newPosX = player.transform.position + new Vector3(moveAmount, 0, 0);
        newPosX.x = Mathf.Clamp(newPosX.x, xMin, xMax);
        player.transform.position = newPosX;
    }

    // Update players's vertical/Z position using Lerp
    void UpdatePlayerPositionZ()
    {
        targetPosZ = player.transform.position + new Vector3(0,0,1);
        player.transform.position = Vector3.Lerp(player.transform.position, targetPosZ, speed * Time.deltaTime);
    }

    public void CollectCup(GameObject collectedCup)
    {
        collectedCup.GetComponent<CupScript>().collected = true;
        collectedCup.tag = "CollectedCup";
        collectedCup.GetComponent<Collider>().isTrigger = false;
        //collectedCup.GetComponent<Rigidbody>().isKinematic = false;
        GameObject refCup = collectedCups.transform.GetChild(cupCount-1).gameObject;
        collectedCup.transform.position = refCup.transform.position + new Vector3(0,0,collectedCupDistance);
        collectedCup.transform.parent = collectedCups.transform;
        cupCount = collectedCups.transform.childCount;
        //collectedCup.transform.position = refCup.transform.position + new Vector3(0, 0, 3);
    }

    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
