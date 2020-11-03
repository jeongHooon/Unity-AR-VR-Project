using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtrl : MonoBehaviour
{
    public enum MoveType{
        WAY_POINT,
        LOOK_AT,
        DAYDREAM
        }
    //이동 방식
    public MoveType moveType = MoveType.WAY_POINT;
    //이동 속도
    public float speed = 1.0f;
    //회전 속도
    public float damping = 3.0f;
    public static bool isStopped = false;

    //웨이 포인트 배열
    public Transform[] points;

    //Transform 컴포넌트 저장 변수
    private Transform tr;

    //CharacterController 컴포넌트 저장 변수
    private CharacterController cc;

    //MainCamera Transform 컴포넌트 저장 변수
    private Transform camTr;

    //다음 이동해야 할 위치 변수
    private int nextIdx = 1;

    private Transform model;
    void Start()
    {
        tr = GetComponent<Transform>();
        cc = GetComponent<CharacterController>();
        camTr = Camera.main.GetComponent<Transform>();
        GameObject wayPointGroup = GameObject.Find("WayPointGroup");
        model = tr.GetChild(0);
        if(wayPointGroup != null)
        {
            points = wayPointGroup.GetComponentsInChildren<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        model.transform.localRotation = new Quaternion(0, camTr.localRotation.y * 2, 0, camTr.localRotation.w);

        if (isStopped) return;
        switch (moveType)
        {
            case MoveType.WAY_POINT:
                MoveWayPoint();
                break;
            case MoveType.LOOK_AT:
                MoveLookAt(1);
                break;
            case MoveType.DAYDREAM:
                break;
        }

    }
    void MoveWayPoint()
    {
        Vector3 direction = points[nextIdx].position - tr.position;
        Quaternion rot = Quaternion.LookRotation(direction);
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
        tr.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("WAY_POINT"))
        {
            nextIdx = (++nextIdx >= points.Length) ? 1 : nextIdx;
        }
    }
    void MoveLookAt(int facing) 
    {
        Vector3 heading = camTr.forward;
        
        heading.y = 0.0f;
        
        
        Debug.DrawRay(tr.position, heading.normalized * 1.0f, Color.red);
        cc.SimpleMove(heading * speed * facing);
    }

}
