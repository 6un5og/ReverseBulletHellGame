using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    // GameManager.instance.player 정적변수는 즉시 클래스에서 부를 수 있다는 편리함이 있음 (굳이 위에 변수만들고 인스펙터에 넣으면서 할 필요 없음)

    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // OnTriggerExit2D의 매개변수 상대방 콜라이더의 태그를 조건으로
        if (!collision.CompareTag("Area"))      // 콜라이더의 주인(CompareTag"") 반대로 쓸거면 부울조건 ! 써서 반대로쓰기
            return;

        // 거리를 구하기 위해 플레이어 위치와 타일맵 위치를 미리 저장
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                // 두 오브젝트의 차이를 활용한 로직으로 변경
                float diffX = playerPos.x - myPos.x;     // 플레이어 위치 - 타일맵 위치 계산으로 거리 구하기
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;              // 3항 연산자 : (조건) ? (true일 때 값) : (false일 때 값)
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)      // 두 오브젝트의 거리 차이에서 X축이 Y축보다 크면 수평 이동
                    transform.Translate(Vector3.right * dirX * 40);     // 땅 두칸 이동

                else if (diffX < diffY)
                    transform.Translate(Vector3.up * dirY * 40);

                break;
            case "Enemy":
                if (coll.enabled)                           // 적 콜라이더가 활성화 되어있는지 (적 시체는 콜라이더 false)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);              // 두 오브젝트의 거리를 그대로 활용하는 것이 포인트
                    // 플레이어의 이동 방향에 따라 맞은 편에서 등장하도록 이동 (맵 하나 크기)
                    // 랜덤한 위치에서 등장하도록 벡터 더하기
                }

                break;
        }
    }
}

