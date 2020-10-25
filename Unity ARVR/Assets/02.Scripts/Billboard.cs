using UnityEngine;
public class Billboard : MonoBehaviour
{
    private Transform camTr;
    private Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        camTr = Camera.main.GetComponent<Transform>();
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.LookAt(camTr.position);
    }
}
