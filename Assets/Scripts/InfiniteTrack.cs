using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTrack : MonoBehaviour
{

    public float speed = 3f;

    public GameObject player;
    public GameObject track;
    public Vector3 spawnPosition;
    public GameObject ostacle_prefab;
    public GameObject star_prefab;
    public GameObject defence_prefab;
    GameObject ostacle;
    GameObject star;
    GameObject defence;

    public GameObject defence_prefab2;
    GameObject defence2;

    public GameObject cloud_prefab;
    GameObject cloud;

    GameObject trackPrevious;
    GameObject trackNext;
    GameObject trackNext2;//
    float spawnOffset;

    // Use this for initialization
    void Start()
    {
        //Debug.Log(spawnPosition);
        trackPrevious = Instantiate(track, spawnPosition, transform.rotation) as GameObject;
        spawnOffset = trackPrevious.transform.Find("Plane").GetComponent<Collider>().bounds.size.z;
        trackNext = Instantiate(track, spawnPosition + new Vector3(0f, 0f, spawnOffset), transform.rotation) as GameObject;
        spawnOffset = trackNext.transform.Find("Plane").GetComponent<Collider>().bounds.size.z;//
        //Debug.Log(spawnOffset);
        trackNext2 = Instantiate(track, spawnPosition + new Vector3(0f, 0f, 2*spawnOffset), transform.rotation) as GameObject;

        /*ostaclePrevious = Instantiate(ostacle, spawnPosition, transform.rotation) as GameObject;
        spawnOffset = trackPrevious.transform.Find("Plane").GetComponent<Collider>().bounds.size.z;
        ostacleNext = Instantiate(ostacle, spawnPosition + new Vector3(0f, 0f, spawnOffset), transform.rotation) as GameObject;
        spawnOffset = trackNext.transform.Find("Plane").GetComponent<Collider>().bounds.size.z;//
        ostacleNext2 = Instantiate(ostacle, spawnPosition + new Vector3(0f, 0f, spawnOffset), transform.rotation) as GameObject;*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        Move();
    }

    void Move()
    {
        /*trackPrevious.transform.Translate(Vector3.back * speed * Time.deltaTime);
        trackNext.transform.Translate(Vector3.back * speed * Time.deltaTime);
        trackNext2.transform.Translate(Vector3.back * speed * Time.deltaTime);*/

        /*if (trackPrevious.transform.position.z + (spawnOffset / 2f) <= player.transform.position.z)
        {
            Destroy(trackPrevious);
            Destroy(ostacle);
            float y = trackNext2.transform.position.y;

            trackPrevious = trackNext;
            trackNext = trackNext2;
            trackNext2 = Instantiate(track, spawnPosition + new Vector3(0f, 0f, spawnOffset*2f), transform.rotation) as GameObject;

            ostaclePrevious = ostacleNext;
            ostacleNext = ostacleNext2;
            ostacleNext2 = Instantiate(ostacle, spawnPosition + new Vector3(100f, 10f, spawnOffset * 3.5f), transform.rotation) as GameObject;
        }*/

        if (trackPrevious.transform.position.z + (spawnOffset / 2f) +100f <= player.transform.position.z)
        {
            Destroy(trackPrevious);
            Vector3 pre_position = trackPrevious.transform.position;

            trackPrevious = trackNext;
            trackNext = trackNext2;
            trackNext2 = Instantiate(track, pre_position + new Vector3(0f, 0f, spawnOffset * 3f), transform.rotation) as GameObject;

            int random = Random.Range(-19, 19);
            ostacle = Instantiate(ostacle_prefab, trackNext2.transform.position + new Vector3(random, 1f, 0f), transform.rotation) as GameObject;
            ostacle.transform.parent = trackNext2.transform;

            random = Random.Range(-19, 19);
            int random2  = Random.Range(1, 4);
            if (random2 == 1)
            {
                star = Instantiate(star_prefab, trackNext2.transform.position + new Vector3(random, 3f, 100f), transform.rotation) as GameObject;
                star.transform.parent = trackNext2.transform;
            }

            random = Random.Range(-19, 19);
            random2 = Random.Range(1, 4);
            if (random2 == 1)
            {
                defence = Instantiate(defence_prefab, trackNext2.transform.position + new Vector3(random, 5f, 200f), defence_prefab.transform.rotation) as GameObject;
                defence.transform.parent = trackNext2.transform;
            }

            random = Random.Range(-19, 19);
            random2 = Random.Range(1, 3);
            if (random2 == 1)
            {
                defence2 = Instantiate(defence_prefab2, trackNext2.transform.position + new Vector3(random, 5f, 300f), defence_prefab2.transform.rotation) as GameObject;
                defence2.transform.parent = trackNext2.transform;
            }

            random = Random.Range(-19, 19);
            random2 = Random.Range(1, 4);
            if (random2 == 1)
            {
                cloud = Instantiate(cloud_prefab, trackNext2.transform.position + new Vector3(random, 5f, 400f), cloud_prefab.transform.rotation) as GameObject;
                cloud.transform.parent = trackNext2.transform;
            }

        }
    }
}
