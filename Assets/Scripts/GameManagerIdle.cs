using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerIdle : MonoBehaviour
{
    [Title("Player parameters")]
    [TabGroup("GamePlay")]
    public float speed;


    [Title("Spawn Object Parameters")]
    [TabGroup("GameData")]
    public bool stopSpawning = false;
    [TabGroup("GameData")]
    public float spawnTime2;
    [TabGroup("GameData")]
    public float spawnDelay2;

    [Title("Slide Parameters")]
    [TabGroup("GameData")]
    public float slidingSens = 1f;

    [Title("Stack Parameters")]
    [TabGroup("GameData")]
    public float coffeeYOffs = 0.3f;

    [Title("Scene objects")]
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject coffeePersonel;
    [TabGroup("GameObjects")]
    [SceneObjectsOnly]
    public GameObject playingPanel,untakenCoffees, unservedCoffees, untakenCoffeeTable, unservedCoffeeTable;
    

    [Title("Asset objects")]
    [TabGroup("GameObjects")]
    [AssetsOnly]
    public GameObject pickParticle, coffeeBox, coffee;


    Vector3 boxSpawnPoint;
    Quaternion boxSpawnRotation;


    float coffeeRow = 0;
    
    float unsCoffeeRow = 0;

    int boxRemaining = 0;
    void Awake()
    {        
        InvokeRepeating("SpawnCoffeeBox", spawnTime2, spawnDelay2);

        boxSpawnPoint = GameObject.Find("SpawnBox").transform.position;
        boxSpawnRotation = GameObject.Find("SpawnBox").transform.rotation;
        boxRemaining = PlayerPrefs.GetInt("playerBoxCount", 0);
    }

    private void Start()
    {
        SetTotalBoxCount(true, 0);
        SetTotalMoney(true, 0);
    }

    public void WorkerTakeObject(GameObject worker, GameObject obj)
    {
        if (worker.CompareTag("coffeeWorker"))
        {
            int checkCofTakens = untakenCoffees.transform.childCount;
            if (checkCofTakens % 4 == 0)
            {
                coffeeRow -= coffeeYOffs;
            }
        }
        

        obj.transform.parent = worker.transform.GetChild(0);
        worker.GetComponent<Workers>().SetHandled(true);
        obj.transform.localPosition = Vector3.zero;
    }

    public void WorkerLeftObject(GameObject stand ,GameObject worker, GameObject obj)
    {

        if (worker.CompareTag("coffeeWorker"))
        {
            obj.transform.parent = stand.transform;
            worker.GetComponent<Workers>().SetHandled(false);
            int untakens3 = unservedCoffees.transform.childCount;
            Vector3 targetPoint3 = unservedCoffeeTable.transform.GetChild(0).position + Vector3.up * 0.15f;
            //Vector3 targetPoint3 = unservedCoffeeTable.transform..position + Vector3.up * 0.904f;
            print(untakens3 % 4);
            if (untakens3 % 4 == 1) // First one -1 , -1
            {
                targetPoint3 += new Vector3(-0.3f, unsCoffeeRow, -0.3f);
            }
            else if (untakens3 % 4 == 2) // Second one 1 , -1
            {
                targetPoint3 += new Vector3(0.3f, unsCoffeeRow, -0.3f);
            }
            else if (untakens3 % 4 == 3) // Third one -1 , 1
            {
                targetPoint3 += new Vector3(-0.3f, unsCoffeeRow, 0.3f);
            }
            else if (untakens3 % 4 == 0) // Last one 1 , 1
            {
                targetPoint3 += new Vector3(0.3f, unsCoffeeRow, 0.3f);
                unsCoffeeRow += coffeeYOffs;
            }
            StartCoroutine(SlideObj(obj, targetPoint3));
        }
        else if (worker.CompareTag("PlayerHand"))
        {
            print("GM coffe to player");
            obj.transform.parent = worker.transform;
            int untakens4 = worker.transform.childCount;
            Vector3 targetPoint4 = Vector3.up * 0.15f;
            //Vector3 targetPoint3 = unservedCoffeeTable.transform..position + Vector3.up * 0.904f;
            print(untakens4 % 4);
            if (untakens4 % 4 == 1) // First one -1 , -1
            {
                targetPoint4 += new Vector3(-0.3f, unsCoffeeRow, -0.3f);
            }
            else if (untakens4 % 4 == 2) // Second one 1 , -1
            {
                targetPoint4 += new Vector3(0.3f, unsCoffeeRow, -0.3f);
            }
            else if (untakens4 % 4 == 3) // Third one -1 , 1
            {
                targetPoint4 += new Vector3(-0.3f, unsCoffeeRow, 0.3f);
            }
            else if (untakens4 % 4 == 0) // Last one 1 , 1
            {
                targetPoint4 += new Vector3(0.3f, unsCoffeeRow, 0.3f);
                unsCoffeeRow += coffeeYOffs;
            }

            StartCoroutine(SlideObj2(obj, targetPoint4));
        }



    }

   

    public void SpawnCoffeeBox()
    {
        Instantiate(coffeeBox, boxSpawnPoint, boxSpawnRotation);

        boxRemaining--;
        SetTotalBoxCount(true, -1);

        if (stopSpawning || boxRemaining <= 0)
        {
            CancelInvoke("SpawnCoffeeBox");
        }
    }
    public void SetTotalMoney(bool op, int price)
    {
        PlayerPrefs.SetInt("playerTotalMoney", PlayerPrefs.GetInt("playerTotalMoney") + price);
        playingPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("playerTotalMoney").ToString();
    }
    public void SetTotalBoxCount(bool op, int boxCount)
    {
        PlayerPrefs.SetInt("playerBoxCount", PlayerPrefs.GetInt("playerBoxCount") + boxCount);
        playingPanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("playerBoxCount", 0).ToString();
    }
    public void LoadRunner()
    {
        SceneManager.LoadScene("RunnerScene");
    }

    public void CoffeeToUntaken(GameObject movedCoffee)
    {
        GameObject slidingCoffee = Instantiate(coffee, movedCoffee.transform.position , untakenCoffees.transform.rotation * Quaternion.AngleAxis(270 , Vector3.right) , untakenCoffees.transform);
        Destroy(movedCoffee); // WILL BE DELETED after box transformation!!
        int untakens1 = untakenCoffees.transform.childCount;
        Vector3 targetPoint = untakenCoffeeTable.transform.position + Vector3.up * 1.93f + Vector3.right * 2.0f;
        print(untakens1 % 4);
        if (untakens1 % 4 == 1) // First one -1 , 1
        {
            targetPoint +=  new Vector3(-0.3f, coffeeRow, 0.3f);
        }
        else if (untakens1 % 4 == 2) // Second one 1 , 1
        {
            targetPoint += new Vector3(0.3f, coffeeRow, 0.3f);
        }
        else if (untakens1 % 4 == 3) // Third one -1 , -1
        {
            targetPoint += new Vector3(-0.3f, coffeeRow, -0.3f);
        }
        else if (untakens1 % 4 == 0) // Last one 1 , -1
        {
            targetPoint += new Vector3(0.3f, coffeeRow, -0.3f);
            coffeeRow += coffeeYOffs;
        }

        StartCoroutine(SlideObj(slidingCoffee, targetPoint));
    }

 

    public IEnumerator SlideObj(GameObject slidingObj, Vector3 slidesTo)
    {
        while(slidingObj.transform.position.x < slidesTo.x - 0.005f || slidingObj.transform.position.x > slidesTo.x + 0.005f)
        {
            if(slidingObj.transform.parent == coffeePersonel.transform.GetChild(0))
            {
                print("while breaked");
                break;
            }
            slidingObj.transform.position = Vector3.Lerp(slidingObj.transform.position, slidesTo, slidingSens * Time.deltaTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    public IEnumerator SlideObj2(GameObject slidingObj, Vector3 slidesTo)
    {
        while (slidingObj.transform.localPosition.x < slidesTo.x - 0.005f || slidingObj.transform.localPosition.x > slidesTo.x + 0.005f)
        {
            slidingObj.transform.localPosition = Vector3.Lerp(slidingObj.transform.localPosition, slidesTo, slidingSens * Time.deltaTime);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }




}
