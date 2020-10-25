
using UnityEngine;

public class EyeCast : MonoBehaviour
{
    private Transform tr;
    private Ray ray;
    private RaycastHit hit;
    private Animator anim;
    private int hashIsLook;
    public float dist = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        anim = tr.Find("Canvas/Image").GetComponent<Animator>();
        hashIsLook = Animator.StringToHash("IsLook");
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(tr.position, tr.forward);

        Debug.DrawRay(ray.origin, ray.direction * dist, Color.green);

        if(Physics.Raycast(ray, out hit, dist, 1<<8 | 1 << 9))
        {
            MoveCtrl.isStopped = true;
            anim.SetBool(hashIsLook, true);
        }
        else
        {
            MoveCtrl.isStopped = false;
            anim.SetBool(hashIsLook, false);
        }
    }
}
