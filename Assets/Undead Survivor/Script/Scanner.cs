using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    // 스캔 범위, 레이어, 스캔 결과 배열, 가장 가까운 목표
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;


    void FixedUpdate()
    {
        // CircleCastAll : 원형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        // 1. 캐스팅 시작 위치 2. 원의 반지름 3. 캐스팅 방향 4. 캐스팅 길이 5. 대상 레이어
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        // 가장 가까운 타겟과의 거리를 반복문을 통해서 result로 반환
        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;                     // 플레이어 위치
            Vector3 targetPos = target.transform.position;          // 타겟 위치
            float curDiff = Vector3.Distance(myPos, targetPos);     // 거리 함수를 이용

            if (curDiff < diff)
            {
                // 반복문을 돌며 가져온 거리가 저장된 거리보다 작으면 교체
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
