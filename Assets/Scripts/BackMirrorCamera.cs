using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackMirrorCamera : MonoBehaviour {

    public GameObject player;

    public float offsetX = 0f;
    public float offsetY = 3f;
    public float offsetZ = -5f;
    public float followSpeed = 1.75f;

    Camera camera;

    Vector3 position;

    // Use this for initialization
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        position.x = player.transform.position.x + offsetX;
        position.y = player.transform.position.y + offsetY;
        position.z = player.transform.position.z + offsetZ;

        transform.position = Vector3.Lerp(transform.position, position, followSpeed * Time.deltaTime);
    }

    void OnPreCull()
    {
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
    }

    void OnPreRender()
    {
        GL.SetRevertBackfacing(true);
    }

    void OnPostRender()
    {
        GL.SetRevertBackfacing(false);
    }

}
