using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // RectTransform 은 따로 변수를 만들어야 함
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    // 캐릭터가 물리적(FixedUpdate)으로 움직이고 있기 때문에 따라다니는 것도 맞춰줘야 함
    void FixedUpdate()
    {
        // UI 스크린 좌표와 월드 좌표는 안맞기 때문에 맞춰줘야 함
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);    // 월드 상의 오브젝트 위치를 스크린 좌표로 변환
    }
}
