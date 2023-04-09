using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public float camFinishYOffs = 1f;
    
    [TabGroup("GameData")]
    public float collectedCupDistance = 4f; // Maksimum target x position value for the player
    [TabGroup("GameData")]
    public float cupWavingSens = 1f; // Maksimum target x position value for the player
    [TabGroup("GameData")]
    public float cupScattingSens = 5f; // Maksimum target x position value for the player
    [TabGroup("GameData")]
    public float collectedCupYOffset = 1f; // Y position offset for the collected cups 

    [Title("Prices")]
    [TabGroup("GameData")]
    public int lidPrice = 1;
    [TabGroup("GameData")]
    public int fillPrice = 1;
    [TabGroup("GameData")]
    public int sleevePrice = 1;

    [Title("Hit Obstacle")]
    [TabGroup("GameData")]
    public float minScattingX = 1f; // Minimum scatting distance on the X plane
    [TabGroup("GameData")]
    public float maxScattingX = 5f; // Maximum scatting distance on the X plane
    [TabGroup("GameData")]
    public float minScattingZ = 3f; // Minimum scatting distance on the Z
    [TabGroup("GameData")]
    public float maxScattingZ = 3f; // Maximum scatting distance on the Z
    [TabGroup("GameData")]
    public float maxScattingHeight = 5f; // Maximum scatting height that the object will jump to
    [TabGroup("GameData")]
    public float hitBackSens = 1f; // Hit back sensivity    
    [TabGroup("GameData")]
    public float hitBackZPoint = 5f; // Hit back force    
    [TabGroup("GameData")]
    public float hitBackBreakZ = 1f; // Hit back animation stop limit  
    

    [Title("Collect Cup")]
    [TabGroup("GameData")]
    public float collectAnimSens = 1f; // Collect cup animation lerp value
    [TabGroup("GameData")]
    public float collectAnimScaleMultiplier = 1f; // Collect cup animation max scale up value
    [TabGroup("GameData")]
    public float collectAnimTimeDiff = 1f; // Collect cup animation time difference between 2 cup in seconds


    [Title("Destroy Cup")]
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
    public GameObject collectedCups; // The collected cups object in the scene
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject droppedCups; // The dropped cups object in the scene

    [Title("UI")]
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject handledPriceText; // Handled cups price text
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject startPanel;
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject playingPanel;
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject pausePanel;

    [Title("Assets only")]
    [TabGroup("GameObjects")]
    [AssetsOnly]
    public GameObject destroyCupEffect; // The destroy particle effect
    [TabGroup("GameObjects")]
    [AssetsOnly]
    public GameObject flowCoffeParticle;
    [TabGroup("GameObjects")]
    [AssetsOnly]
    public GameObject sleeveMachineParticle;

    Vector3 camOffset; // The offset value between the player and camera at the start of the game
    Vector3 camTarget;
    Vector3 targetPosZ; // The target position value used for updating the player's Z position

    public int cupCount = 1;

    int handledPrice = 0;

    //[NonSerialized]
    public bool stopMoving = false;

    bool isGameStarted = false;

    AudioManager audioManager;

    bool isFinished = false;

    void Awake()
    {
        // Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        // MUST BE UPDATED AS TO FALLOW HAND!
        camOffset = player.transform.position - cam.transform.position; // Calculate the offset value between the player and camera
        cupCount = collectedCups.transform.childCount;
        audioManager = FindObjectOfType<AudioManager>();

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
        if (Input.GetMouseButtonDown(0) && !isGameStarted)
        {            
            startPanel.SetActive(false);
            stopMoving = false;
            isGameStarted = true;
        }
        // Update the player's X position when the left mouse button is pressed
        if (Input.GetMouseButton(0) && !stopMoving && !isFinished)
        {
            UpdatePlayerPositionX();
        }

        // Restart the game when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (isFinished && (player.transform.position.x < -0.01 || player.transform.position.x > 0.01))
        {
            player.transform.position = Vector3.Lerp(player.transform.position,
                                                     player.transform.position - new Vector3(player.transform.position.x, 0,0),
                                                     speed/4 * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // Update the camera's position
        UpdateCamPosition();

        // Update the collected cups's positions if there is at least one collected cup
        if(collectedCups.transform.childCount > 0)
        {
            SetCollectedCupsPositions();
        }

        // Update the player's position
        if (!stopMoving)
        {
            UpdatePlayerPositionZ();
        }
    }

    public void PlayerGetCup()
    {

    }

    public int GetCupCount()
    {
        return collectedCups.transform.childCount;
    }

    // Update the camera's position to follow the player's position according to camOffset
    void UpdateCamPosition()
    {
        camTarget = isFinished ? player.transform.position - camOffset + Vector3.up * camFinishYOffs + Vector3.forward * 12f : player.transform.position - camOffset;
        cam.transform.position = Vector3.Lerp(cam.transform.position, camTarget, camSpeed * Time.deltaTime);
        // cam.transform.position = player.transform.position - camOffset;
        if (isFinished && cam.GetComponent<Camera>().fieldOfView < 31f)
        {
            camSpeed *= 2;
            cam.GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(cam.GetComponent<Camera>().fieldOfView, 31, camSpeed * Time.deltaTime);
            camSpeed /= 2;
        }
        else if(!isFinished && cam.GetComponent<Camera>().fieldOfView > 20f)
        {
            cam.GetComponent<Camera>().fieldOfView = 20f;
        }
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
        targetPosZ = player.transform.position + Vector3.forward; 
        player.transform.position = Vector3.Lerp(player.transform.position, targetPosZ, speed * Time.deltaTime);
    }

    // Set the taken collectedCup as a child of collectedCups
    public void CollectCup(GameObject collectedCup)
    {
        audioManager.Play("TakeCup");
        collectedCup.GetComponent<CupScript>().collected = true;
        collectedCup.tag = "CollectedCup";
        collectedCup.GetComponent<Collider>().isTrigger = false;
        collectedCup.transform.parent = collectedCups.transform;
        cupCount = collectedCups.transform.childCount;
        SetHandledPrice(collectedCup.GetComponent<CupScript>().price, true);
        collectedCup.GetComponent<CupScript>().SetSizeDefault();
        if (isGameStarted)
        {
            //StartCoroutine(CollectCupAnim());
            CollectCupAnim();
        }
    }

    // Adds or remove cup price from the handledPrice
    public void SetHandledPrice(int opPrice, bool operation)
    {
        // Adds opPrice to the handled price
        if(operation)
        {
            handledPrice += opPrice;
            handledPriceText.GetComponent<Text>().text = handledPrice.ToString() + " $";
        }
        // Removes opPrice from the handledPrice
        else
        {
            handledPrice -= opPrice;
            handledPriceText.GetComponent<Text>().text = handledPrice.ToString() + " $";
        }
    }

    public void PutLidToCup(GameObject liddedCup)
    {
        audioManager.Play("PutLid");
        liddedCup.GetComponent<CupScript>().PutLid(lidPrice);
        SetHandledPrice(lidPrice, true);
    }
    public void PutSleeveToCup(GameObject sleevedCup)
    {
        audioManager.Play("PutSleeve");
        sleevedCup.GetComponent<CupScript>().PutSleeve(sleevePrice);
        SetHandledPrice(sleevePrice, true);
    }

    public void FillTheCup(GameObject filledCup)
    {
        audioManager.Play("Fill");
        filledCup.GetComponent<CupScript>().FillTheCup(fillPrice);
        SetHandledPrice(fillPrice, true);
    }

    public void HitCup(GameObject hitCup, ObsScript obsSc)
    {
        audioManager.Play("Hit");

        if (hitCup.transform.GetSiblingIndex() == cupCount - 1)
        {
            print("Destroy");
            DestroyCup(hitCup);
        }
        else if(obsSc.hitBack)
        {
            stopMoving = true;
            obsSc.isHit = true;
            int index = hitCup.transform.GetSiblingIndex();
            DestroyCup(hitCup);
            for (int i = collectedCups.transform.childCount - 1; i > index; i--)
            {
                collectedCups.transform.GetChild(i).transform.position = obsSc.hitPoint;
                DropTheCup(collectedCups.transform.GetChild(i).gameObject);
            }

            Vector3 targetPosition = player.transform.position + (Vector3.back * hitBackZPoint);
            StartCoroutine(HitBack(targetPosition));
        }
        else
        {
            int index = hitCup.transform.GetSiblingIndex();
            DestroyCup(hitCup);
            for (int i = collectedCups.transform.childCount - 1; i > index; i--)
            {
                collectedCups.transform.GetChild(i).transform.position = obsSc.hitPoint;
                DropTheCup(collectedCups.transform.GetChild(i).gameObject);
            }
        }
    }
    
    IEnumerator HitBack(Vector3 hitTo)
    {
        if (player.transform.position.z > hitTo.z + hitBackBreakZ)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, hitTo, hitBackSens * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
            StartCoroutine(HitBack(hitTo));
        }
        else
        {
            stopMoving = false;
        }
    }


    public void DropTheCup(GameObject droppedCup)
    {
        droppedCup.tag = "DroppedCup";
        droppedCup.transform.parent = droppedCups.transform;
        droppedCup.GetComponent<Collider>().isTrigger = true;
        droppedCup.GetComponent<CupScript>().collected = false;
        SetHandledPrice(droppedCup.GetComponent<CupScript>().price, false);
        cupCount = collectedCups.transform.childCount;
        ScatterObject(droppedCup);
    }

    public void DestroyCup(GameObject destroyCup)
    {
        Vector3 effectPos = destroyCup.transform.position + Vector3.up * 0.5f;
        GameObject destroyEffect = Instantiate(destroyCupEffect, effectPos, Quaternion.identity);
        SetHandledPrice(destroyCup.GetComponent<CupScript>().price, false);
        Destroy(destroyCup);
        Destroy(destroyEffect, particleKillTime);
        cupCount = collectedCups.transform.childCount;
    }

    public void EnterToFinish()
    {
        isFinished = true;
    }

    /*IEnumerator CollectCupAnim()
    {
        for(int i = collectedCups.transform.childCount-1; i >= 0; i--)
        {
            if (stopMoving)
            {
                SetCupsSizesDefault();
                break;
            }
            StartCoroutine(collectedCups.transform.GetChild(i).GetComponent<CupScript>().ScaleObject(collectAnimSens, collectAnimScaleMultiplier));
            yield return new WaitForSeconds(collectAnimTimeDiff);
        }
    }*/


    public void CollectCupAnim()
    {
        print("CollectCupAnim triggered");
        if (stopMoving)
        {
            SetCupsSizesDefault();
        }
        else
        {
            print("CollectCupAnim first object triggered triggered");
            StartCoroutine( collectedCups.transform.GetChild(collectedCups.transform.childCount - 1).GetComponent<CupScript>().ScaleObject(collectAnimSens, collectAnimScaleMultiplier) );
        }
    }

    public void SetCupsSizesDefault()
    {
        for (int i = collectedCups.transform.childCount - 1; i >= 0; i--)
        {
            collectedCups.transform.GetChild(i).GetComponent<CupScript>().SetSizeDefault();
        }
    }

    void ScatterObject(GameObject scatteredObject)
    {
        Rigidbody rb = scatteredObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        float forceX = UnityEngine.Random.Range(minScattingX, maxScattingX) * (UnityEngine.Random.Range(0, 2) * 2 - 1);
        float forceY = maxScattingHeight;
        float forceZ = UnityEngine.Random.Range(minScattingZ, maxScattingZ);
        rb.AddForce(forceX, forceY, forceZ, ForceMode.Impulse);
    }


    // Update cups's X positions using Lerp and
    // cups's Z positions according to distance of index
    public void SetCollectedCupsPositions()
    {
        for(int i=0; i< collectedCups.transform.childCount; i++)
        {
            // Set positions of the cups except 0th cup
            if(i != 0)
            {
                Transform preCup = collectedCups.transform.GetChild(i - 1).transform;
                float targetX = Mathf.Lerp(collectedCups.transform.GetChild(i).position.x, preCup.transform.position.x, cupWavingSens * Time.deltaTime);
                float targetZ = preCup.transform.position.z + collectedCupDistance;
                Vector3 targetPos = new Vector3(targetX, preCup.transform.position.y, targetZ);

                collectedCups.transform.GetChild(i).position = targetPos;
            }
            // Set position of the 0th cup to player's position
            else
            {
                collectedCups.transform.GetChild(0).position = player.transform.position + Vector3.up * collectedCupYOffset;
            }
        }
    }

    // Reload the current scene to restart the game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseButton()
    {
        // Time.timeScale = 0f;
        stopMoving = true;
        playingPanel.SetActive(false);
        pausePanel.SetActive(value: true);
    }
    public void ContinueButton()
    {
        stopMoving = false;
        // Time.timeScale = 1f;
        playingPanel.SetActive(true);
        pausePanel.SetActive(value: false);
    }

    public void RestartButton()
    {
        playingPanel.SetActive(true);
        pausePanel.SetActive(value: false);
        Restart();
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


    /*
    public void WaitHitBack()
    {
        stopMoving = true;
        Vector3 targetPosition = player.transform.position + Vector3.back * hitBackZPoint;
        while (player.transform.position.z > player.transform.position.z - hitBackZPoint + 0.1f)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, hitBackSens * Time.deltaTime);
        }
        stopMoving = false;
    }
    */
}
