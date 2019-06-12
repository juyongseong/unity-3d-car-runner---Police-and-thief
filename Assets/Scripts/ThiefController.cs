using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThiefController : MonoBehaviour
{

    public float speed = 5f;
    public float go_speed = 5f;
    public Text speedText;
    bool isboost = false;
    Material[] m;
    public Text defenceText;
    int defencecnt=0;

    public Camera camera;

    Rigidbody rigidbody;
    Vector3 movement;

    bool isLightsOn;
    bool go = true;
    int slowstart = 0;

    public GameObject defence_prefab;
    GameObject defence;

    public GameObject cloud_prefab;
  
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float s = go_speed * 4;

        if (slowstart < go_speed)
        {
            slowstart += 1;
            speedText.text = slowstart.ToString();
        }

        else speedText.text = s.ToString();

        defenceText.text = defencecnt.ToString();

        if (Input.GetKeyDown(KeyCode.K))
        {
            if(defencecnt>0)
            {
                defencecnt--;
                defence = Instantiate(defence_prefab, transform.position+ new Vector3(0,-1,0), defence_prefab.transform.rotation) as GameObject;
            }
        }

        /*float v = Input.GetAxisRaw("Vertical");

        //Debug.Log(h + " " + v);

        if (v >= 0)
        {
            Accelerate(v);
        }
        else
        {
            SlowDown(v);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleLights();
        }*/
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Vertical");

        Move(h);
    }

    void Move(float h)
    {
        //Debug.Log(movement.normalized.x + " " + movement.normalized.y + " " + movement.normalized.z);
        movement.Set(h*5, 0, go_speed);
        movement *= speed * Time.deltaTime;//movement.normalized 
        //Debug.Log(speed);
        //Debug.Log(Time.deltaTime);
        //Debug.Log(movement.x + " " + movement.y + " " + movement.z);
        rigidbody.MovePosition(transform.position + movement);

        camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position, 0.2f);
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y + 1, camera.transform.position.z);
    }

    void Accelerate(float power)
    {
        transform.Find("Lights Break").gameObject.SetActive(false);
    }

    void SlowDown(float power)
    {
        transform.Find("Lights Break").gameObject.SetActive(true);

    }

    void ToggleLights()
    {
        isLightsOn = !isLightsOn;
        transform.Find("Lights").gameObject.SetActive(isLightsOn);
    }

    private void OnTriggerEnter(Collider other)
    { 

        if (other.tag == "Ostacle")
        {
            slowstart = 0;
            go_speed = 0;
        }

        else if (other.tag == "star")
        {
            Destroy(other.gameObject);

            if(isboost==false)
            {
                isboost = true;
                go_speed = 32;
                m = transform.Find("Group19995").gameObject.GetComponent<MeshRenderer>().materials;
                m[1].color = Color.yellow;
                Invoke("resetSpeed", 3);
               
            }
        }

        else if (other.tag == "defenceitem")
        {
            Destroy(other.gameObject);
            defencecnt++;
        }

        else if (other.tag == "cloud")
        {
            Destroy(other.gameObject);
            defence = Instantiate(cloud_prefab, transform.position + new Vector3(0, -1, 0), cloud_prefab.transform.rotation) as GameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ostacle")
        {
            go_speed = 28;
        }

       
    }

    void resetSpeed()
    {
        go_speed = 28;
        isboost = false;
        m[1].color = Color.red;
    }
}
