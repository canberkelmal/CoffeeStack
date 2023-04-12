using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

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
    public GameObject untakenCoffees, unservedCoffees, untakenCoffeeTable, unservedCoffeeTable;

    [Title("Asset objects")]
    [TabGroup("GameObjects")]
    [AssetsOnly]
    public GameObject pickParticle, coffeeBox, coffee;


    Vector3 boxSpawnPoint;
    Quaternion boxSpawnRotation;


    float coffeeRow = 0;
    
    float unsCoffeeRow = 0;
    

    




    void Awake()
    {
        
        InvokeRepeating("SpawnCoffeeBox", spawnTime2, spawnDelay2);

        boxSpawnPoint = GameObject.Find("SpawnBox").transform.position;
        boxSpawnRotation = GameObject.Find("SpawnBox").transform.rotation;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        obj.transform.parent = stand.transform;
        worker.GetComponent<Workers>().SetHandled(false);

        if (worker.CompareTag("coffeeWorker"))
        {
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
            /*
            int untakens3 = unservedCoffees.transform.childCount;
            Vector3 targetPoint3 = unservedCoffeeTable.transform.GetChild(0).position + Vector3.up * 0.15f;
            //Vector3 targetPoint3 = unservedCoffeeTable.transform..position + Vector3.up * 0.904f;
            print(untakens3 % 4);
            if (untakens3 % 4 == 1) // First one -1 , 1
            {
                targetPoint3 += new Vector3(-0.3f, unsCoffeeRow, 0.3f);
            }
            else if (untakens3 % 4 == 2) // Second one 1 , 1
            {
                targetPoint3 += new Vector3(0.3f, unsCoffeeRow, 0.3f);
            }
            else if (untakens3 % 4 == 3) // Third one -1 , -1
            {
                targetPoint3 += new Vector3(-0.3f, unsCoffeeRow, -0.3f);
            }
            else if (untakens3 % 4 == 0) // Last one 1 , -1
            {
                targetPoint3 += new Vector3(0.3f, unsCoffeeRow, -0.3f);
                unsCoffeeRow += coffeeYOffs;
            }
            */
            StartCoroutine(SlideObj(obj, targetPoint3));
        }
        
    }

   

    public void SpawnCoffeeBox()
    {
        Instantiate(coffeeBox, boxSpawnPoint, boxSpawnRotation);
        if (stopSpawning)
        {
            CancelInvoke("SpawnCoffeeBox");
        }
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

 

    IEnumerator SlideObj(GameObject slidingObj, Vector3 slidesTo)
    {
        while(slidingObj.transform.position.x < slidesTo.x - 0.005f || slidingObj.transform.position.x > slidesTo.x + 0.005f)
        {
            if(slidingObj.transform.parent == coffeePersonel.transform.GetChild(0))
            {
                print("while breaked");
                break;
            }
            slidingObj.transform.position = Vector3.Lerp(slidingObj.transform.position, slidesTo, slidingSens * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }

   

    
}
