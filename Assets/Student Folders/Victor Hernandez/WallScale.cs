using UnityEngine;

public class WallScale : MonoBehaviour
{
    public GameObject Walls;
    public float ScaleSpeed = 5f;
    public Vector3 ScaleTarget = new Vector3(0.7f, 0.7f, 0.7f);

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.MoveTowards(transform.position, ScaleTarget, ScaleSpeed * Time.deltaTime);  
    }
}
