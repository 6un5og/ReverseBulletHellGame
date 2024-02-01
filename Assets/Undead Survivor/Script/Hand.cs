using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);                   // 회전 Quaternion 이용
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];            // 부모 쪽으로 올라감, 본인이 찾고있는 속성을 가지고 있으면 본인이 [0], 다음이 [1]
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;              // 플레이어의 반전 상태를 지역 변수로 저장

        if (isLeft) // 근접 무기
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;     // 왼손 회전은 localRotation (플레이어 기준으로 회전이기 때문)
            spriter.flipY = isReverse;                                          // 왼손 스프라이트는 Y축 반전
            spriter.sortingOrder = isReverse ? 4 : 6;                           // 스크립트 상에서는 OrderInLayer() 가 sortingOrder
        }
        else        // 원거리 무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;   // 오른손 이동에는 localReverse
            spriter.flipX = isReverse;                                          // 왼손 스프라이트는 X축 반전
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
