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

    //웨이 포인트 배열
    public Transform[] points;

    //Transform 컴포넌트 저장 변수
    private Transform tr;
    //다음 이동해야 할 위치 변수
    private int nextIdx = 1;

    void Start()
    {
        tr = GetComponent<Transform>();

        GameObject wayPointGroup = GameObject.Find("WayPointGroup");

        if(wayPointGroup != null)
        {
            points = wayPointGroup.GetComponentsInChildren<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (moveType)
        {
            case MoveType.WAY_POINT:
                MoveWayPoint();
                break;
            case MoveType.LOOK_AT:
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
}
