using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using nv;
using GameEvents;
using ComponentBuff;

public class CollectableSpawnController : MonoBehaviour
{
    public List<Transform> spawnPositions = new List<Transform>();
    public Collectable collectableInstance = null;
    public GameObject collectableRoot = null;
    public float timeBetweenSpawns = 5.0f;
    public float velocity = 0.2f;
    public int poolNumber = 100;

    List<Collectable> objectPoolGrid = new List<Collectable>();
    List<Collectable> objectPool = new List<Collectable>(); //for explosions
    float timer = 0.0f;

    CommunicationNode node = new CommunicationNode();
    private void Start()
    {
        CreateObjectPool();
        timer = timeBetweenSpawns;
    }

    private void OnEnable()
    {
        node.EnableNode(this);
    }

    private void OnDisable()
    {
        node.DisableNode();
    }
    // Update is called once per frame
    void Update()
    {
        //spawn for normal items
        if (timer <= 0.0f)
        {
            timer = timeBetweenSpawns;

            int index = objectPoolGrid.FindIndex(x => x.gameObject.activeSelf == false);
            if (index >= 0 && index < spawnPositions.Count)
            {
                objectPoolGrid[index].gameObject.SetActive(true);
            }
        }
        timer -= Time.deltaTime;
    }

    void CreateObjectPool()
    {
        for (int i = 0; i < spawnPositions.Count; ++i)
        {
            Collectable temp = Instantiate(collectableInstance, gameObject.transform);
            temp.gameObject.transform.position = spawnPositions[i].position;
            temp.gameObject.SetActive(false);
            objectPoolGrid.Add(temp);
        }

        for (int i = 0; i < poolNumber; ++i)
        {
            Collectable temp = Instantiate(collectableInstance, gameObject.transform);
            temp.gameObject.SetActive(false);
            objectPool.Add(temp);
        }
    }

    [CommunicationCallback]
    void HandleBuffsRemoved(SpawnGemsEvent spawnGemsEvent)
    {
        Debug.Log("Spawn gems");
        SpawnExplosion(spawnGemsEvent.position, spawnGemsEvent.numberGems);
    }

    public void SpawnExplosion(Vector3 spawnPosition,int number)
    {
        //find available objects in the pool.
        List<Collectable> availableObjects = objectPool.FindAll(x => x.gameObject.activeSelf == false);

        //easy check for now
        if(availableObjects.Count>=number)
        {
            for(int i=0;i<number;++i)
            {
                availableObjects[i].gameObject.transform.position = spawnPosition;
                availableObjects[i].gameObject.SetActive(true);
                float angle = UnityEngine.Random.Range(30f,150f);
                availableObjects[i].ApplyForce(velocity,Mathf.Deg2Rad*angle);
            }
        }
    }

    
}
