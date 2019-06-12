using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PoliceController : MonoBehaviour
{

    public float speed = 5f;
    public int go_speed = 0;
    double t = 0;
    bool isgo = true;
    int count = 5;
    int slowstart = 0;
   
    public Text speedText;
    public Text countText;

    public Camera camera ;

	Rigidbody rigidbody;
	Vector3 movement;
    public Text defenceText;
    int defencecnt = 0;

    public GameObject defence_prefab;
    GameObject defence;

    bool isLightsOn;

	// Use this for initialization
	void Start ()
	{
		rigidbody = GetComponent<Rigidbody> ();

        InvokeRepeating("CountDown", 0, 1);
    }

 
	// Update is called once per frame
	void Update ()
	{
        float s = go_speed * 4;

        if (t > 5)
        {
            if (slowstart<go_speed)
            {
                slowstart += 1;
                speedText.text = slowstart.ToString();
            }

            else speedText.text = s.ToString();
        }

        defenceText.text = defencecnt.ToString();

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (defencecnt > 0)
            {
                defencecnt--;
                defence = Instantiate(defence_prefab, transform.position + new Vector3(0, -1, 300), defence_prefab.transform.rotation) as GameObject;
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

    void CountDown()
    {
        countText.text = count.ToString();
        count--;
    }

	void FixedUpdate ()
	{
        if(t<=5)
        {
            t += Time.deltaTime;
            if (t > 4.8)
            {
                CancelInvoke();
                Destroy(countText);
            }
        }

        else if (t>5)
        {
            float h = Input.GetAxisRaw("Horizontal");

            Move(h);
        }
       // transform.Translate(new Vector3(-go_speed * Time.deltaTime, 0 , 0));
    }

	void Move (float h)
	{
        //Debug.Log(movement.normalized.x + " " + movement.normalized.y + " " + movement.normalized.z);
        movement.Set (h*5, 0, go_speed);
		movement *= speed * Time.deltaTime;//movement.normalized 
        //Debug.Log(speed);
        //Debug.Log(Time.deltaTime);
        //Debug.Log(movement.x + " " + movement.y + " " + movement.z);
        rigidbody.MovePosition (transform.position + movement);

        if (isgo)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position, 0.2f);
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y + 1, camera.transform.position.z - 1);
        }
    }

	void Accelerate (float power)
	{
		transform.Find ("Lights Break").gameObject.SetActive (false);
	}

	void SlowDown (float power)
	{
		transform.Find ("Lights Break").gameObject.SetActive (true);

	}

	void ToggleLights ()
	{
		isLightsOn = !isLightsOn;
		transform.Find ("Lights").gameObject.SetActive (isLightsOn);
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Theif")
        {
            SceneManager.LoadScene("Rank");
            Debug.Log("충돌");
        }

        else if(other.tag == "Ostacle")
        {
            slowstart = 0;
            go_speed = 0;
            isgo = false;
        }

        else if (other.tag == "StoneWallitem")
        {
            Destroy(other.gameObject);
            defencecnt++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ostacle")
        {
            go_speed = 32;
            isgo = true ;
        }
    }
}
