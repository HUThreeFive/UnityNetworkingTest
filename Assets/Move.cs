using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    public float speed;

    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame

    void Update()
    {

        MovePlayer();
//        float MoveVertical = Input.GetAxis("Vertical") * 10 * Time.deltaTime;
//        float MoveHorizontal = Input.GetAxis("Horizontal") * 10 * Time.deltaTime;
//
//        transform.Translate(MoveHorizontal, 0, MoveVertical);
        //transform.Rotate(Vector3.up * MoveRotate);
    }
	
    private void MovePlayer()
    {
        float MoveVertical = Input.GetAxis("Vertical") * 10 * Time.deltaTime;
        float MoveHorizontal = Input.GetAxis("Horizontal") * 10 * Time.deltaTime;

        transform.Translate(MoveHorizontal, 0, MoveVertical);
    }

    public void MovePlayer(float x, float z)
    {
        float MoveVertical = z * 10 * Time.deltaTime;
        float MoveHorizontal = x * 10 * Time.deltaTime;

        //transform.Translate(MoveHorizontal, 0, MoveVertical);
        Vector3 temp = new Vector3(x, 0, z);
        transform.position = temp;
    }
}
