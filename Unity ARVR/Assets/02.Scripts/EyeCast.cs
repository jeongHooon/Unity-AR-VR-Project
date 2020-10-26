
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class EyeCast : MonoBehaviour
{
    private Transform tr;
    private Ray ray;
    private RaycastHit hit;
    private Animator anim;
    private int hashIsLook;
    public float dist = 10.0f;

    private GameObject prevButton;
    private GameObject currButton;

    private Image circleBar;

    public float selectedTime = 1.0f;
    private float passedTime = 0.0f;
    private bool isClicked = false;

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
            GazeButton();
        }
        else
        {
            MoveCtrl.isStopped = false;
            anim.SetBool(hashIsLook, false); 
            ReleaseButton();
        }
    }
    void GazeButton()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        if(hit.collider.gameObject.layer == 9)
        {
            currButton = hit.collider.gameObject;
            circleBar = currButton.GetComponentsInChildren<Image>()[1];
            if(currButton != prevButton)
            {
                ButtonInit();
                ExecuteEvents.Execute(currButton, data, ExecuteEvents.pointerEnterHandler);
                ExecuteEvents.Execute(prevButton, data, ExecuteEvents.pointerExitHandler);
            }
            else
            {
                if (isClicked == true) return;
                passedTime += Time.deltaTime;
                circleBar.fillAmount = passedTime / selectedTime;
                if(passedTime >= selectedTime)
                {
                    Debug.Log(currButton.name + " is clicked!");
                    ExecuteEvents.Execute(currButton, data, ExecuteEvents.pointerClickHandler);
                    isClicked = true;
                }
            }
            prevButton = currButton;
        }
        else
        {
            ReleaseButton();
        }
    }
    void ReleaseButton()
    {
        ButtonInit();
        PointerEventData data = new PointerEventData(EventSystem.current);

        if(prevButton != null)
        {
            ExecuteEvents.Execute(prevButton, data, ExecuteEvents.pointerExitHandler);
            prevButton = null;
        }
    }
    void ButtonInit()
    {
        passedTime = 0.0f;
        isClicked = false;

        if (circleBar != null)
        {
            circleBar.fillAmount = 0.0f;
        }
        if (prevButton != null)
        {
            prevButton.GetComponentsInChildren<Image>()[1].fillAmount = 0.0f;
        }
    }
}
