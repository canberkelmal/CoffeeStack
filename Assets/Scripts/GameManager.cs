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
    [TabGroup("GameData")]
    public float cupWavingSens = 1f; // Maksimum target x position value for the player
    [TabGroup("GameData")]
    public float cupScattingSens = 5f; // Maksimum target x position value for the player

    [Title("Drop Cup")]
    [TabGroup("GameData")]
    public float maxScattingRadius = 5f; // Maximum scatting radius on the X-Z plane
    [TabGroup("GameData")]
    public float maxScattingHeight = 2f; // Maximum scatting height that the object will jump to
    [TabGroup("GameData")]
    public float scattingDuration = 1f; // Total duration of the scatting
    [Title("Collect Cup")]
    [TabGroup("GameData")]
    public float collectAnimDur = 1f; // Collect cup animation lerp value
    [TabGroup("GameData")]
    public float collectAnimScaleMultiplier = 1f; // Collect cup animation max scale up value
    [TabGroup("GameData")]
    public float collectAnimTimeDiff = 1f; // Collect cup animation time difference between 2 cup in seconds


    [Title("Collect Cup")]
    [TabGroup("GameData")]
    public float particleKillTime = 1f; // Destroy cup particle effect duration
    

    [Title("Scene Objects only")]
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject cam; // The main camera in the scene
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject player; // The player object in the scene
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject collectedCups; // The cups object in the scene
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject droppedCups; // The cups object in the scene

    [Title("Assets only")]
    [TabGroup("Assets")]
    [AssetsOnly]
    public GameObject destroyCupEffect; // The main camera in the scene


    Vector3 camOffset; // The offset value between the player and camera at the start of the game
    Vector3 targetPosZ; // The target position value used for updating the player's Z position

    int cupCount = 1;

    bool stopMoving = false;

    bool isGameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        // MUST BE UPDATED AS TO FALLOW HAND!
        camOffset = player.transform.position - cam.transform.position; // Calculate the offset value between the player and camera
        cupCount = collectedCups.transform.childCount;

        // Set the cups which are placed as collected at beginning as collected
        for (int i=0; i<cupCount; i++)
        {
            CollectCup(collectedCups.transform.GetChild(i).gameObject);
        }
        stopMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the player's X position when the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            stopMoving = false;
            isGameStarted = true;
        }

        // Update the player's X position when the left mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            UpdatePlayerPositionX();
        }

        // Update the player's Z position
        if (!stopMoving)
        {
            UpdatePlayerPositionZ();
        }

        // Update the camera's position
        UpdateCamPosition();

        // Update the collected cups's positions if there is at least one collected cup
        if(collectedCups.transform.childCount > 0)
        {
            SetCollectedCupsPositions();
        }

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

    // Set the taken collectedCup as a child of collectedCups
    public void CollectCup(GameObject collectedCup)
    {
        collectedCup.GetComponent<CupScript>().collected = true;
        collectedCup.tag = "CollectedCup";
        collectedCup.GetComponent<Collider>().isTrigger = false;
        collectedCup.transform.parent = collectedCups.transform;
        cupCount = collectedCups.transform.childCount;

        if (isGameStarted)
        {
            StartCoroutine(CollectCupAnim());
        }
    }

    public void HitCup(GameObject hitCup)
    {
        print(hitCup.transform.GetSiblingIndex() + ". child in" + cupCount);
        if (hitCup.transform.GetSiblingIndex() == cupCount - 1)
        {
            print("Destroy");
            DestroyCup(hitCup);
        }
        else
        {
            DropTheCup(hitCup);
        }
    }

    public void DropTheCup(GameObject droppedCup)
    {
        stopMoving = true;
        droppedCup.transform.parent = null;
        cupCount = collectedCups.transform.childCount;
        ScatterObject(droppedCup);
        droppedCup.GetComponent<Collider>().isTrigger = true;
        droppedCup.GetComponent<CupScript>().collected = false;
        droppedCup.tag = "DroppedCup";
        droppedCup.transform.parent = droppedCups.transform;
    }

    public void DestroyCup(GameObject destroyCup)
    {
        Vector3 effectPos = destroyCup.transform.position + Vector3.up * 0.5f;
        GameObject destroyEffect = Instantiate(destroyCupEffect, effectPos, Quaternion.identity);
        Destroy(destroyCup);
        Destroy(destroyEffect, particleKillTime);
        cupCount--;
    }

    IEnumerator CollectCupAnim()
    {
        for(int i = cupCount-1; i >= 0; i--)
        {
            StartCoroutine(collectedCups.transform.GetChild(i).GetComponent<CupScript>().ScaleObject(collectAnimDur, collectAnimScaleMultiplier));
            yield return new WaitForSeconds(collectAnimTimeDiff);
        }
    }

    void ScatterObject(GameObject scatteredObject)
    {
        Rigidbody rb = scatteredObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(UnityEngine.Random.Range(-maxScattingRadius, maxScattingRadius), maxScattingHeight, UnityEngine.Random.Range(1, maxScattingRadius), ForceMode.Impulse);
    }


    // Update cups's X positions using Lerp and cups's Z positions according to distance of index
    public void SetCollectedCupsPositions()
    {
        for(int i=0; i<cupCount; i++)
        {
            // Set positions of the cups except 0th cup
            if(i != 0)
            {
                Transform preCup = collectedCups.transform.GetChild(i - 1).transform;
                float targetX = Mathf.Lerp(collectedCups.transform.GetChild(i).position.x, preCup.transform.position.x, cupWavingSens * Time.deltaTime);
                float targetZ = preCup.transform.position.z + collectedCupDistance;
                Vector3 targetPos = new Vector3(targetX, 0, targetZ);

                collectedCups.transform.GetChild(i).position = targetPos;
            }
            // Set position of the 0th cup to player's position
            else
            {
                collectedCups.transform.GetChild(0).position = player.transform.position;
            }
        }
    }

    /*// ScattedObject objesi bu methodda kullanýlacak.
    public void ScatterObject(GameObject scattedObject)
    {
        // x-z düzleminde 5 birim yarýçapta rastgele bir pozisyon belirleyin
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle.normalized * 5f;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y);
        randomPosition += scattedObject.transform.position; // verilen nesnenin pozisyonuna göre ayarla

        // y ekseninde max 2 birim yüksekliðe çýkacak þekilde pozisyon ayarlayýn
        float maxHeight = 2f;
        float randomHeight = UnityEngine.Random.Range(0f, maxHeight);
        randomPosition.y += randomHeight;

        // nesneyi yeni pozisyonuna doðru hareket ettirin
        StartCoroutine(MoveObject(scattedObject, randomPosition));
    }

    // verilen nesneyi verilen pozisyona doðru hareket ettirir
    private IEnumerator MoveObject(GameObject obj, Vector3 targetPosition)
    {
        float distance = Vector3.Distance(obj.transform.position, targetPosition);
        float duration = distance / cupScattingSens;

        float startTime = Time.time;
        Vector3 startPosition = obj.transform.position;

        while (Time.time < startTime + duration)
        {
            float timePassed = Time.time - startTime;
            float percentageComplete = timePassed / duration;
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, percentageComplete);
            yield return null;
        }

        obj.transform.position = targetPosition;
    }*/




    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
