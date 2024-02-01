using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{

    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int index)       // 버튼을 대신 눌러주는 함수
    {
        items[index].OnClick();
    }

    void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 그 중에서 랜덤 아이템 3개 활성화
        int[] ran = new int[3];                     // 랜덤으로 활성화 할 아이템의 인덱스 3개를 담을 배열 선언
        while (true)                                // while 문이 true조건이면 무한루프
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])   // 서로 비교하여 모두 같지 않으면 반복문을 빠져나가도록 작성
            {
                break;                              // while문을 빠져나오는 break 필요
            }
        }

        for (int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];

            // 3. 만렙 아이템의 경우는 소비아이템으로 대체
            if (ranItem.level == ranItem.data.damages.Length)   // 갖고 온 아이템의 레벨이 자신이 가지고 있는 데이터의 최대 길이(최대 레벨)와 같다면
            {
                items[4].gameObject.SetActive(true);            // 아이템이 최대 레벨이면 소비 아이템이 대신 활성화 되도록 작성
                // if 소비아이템이 다수라면 items 배열 안에 [Random.Range(소비아이템 첫번째 인덱스, 소비아이템 마지막 인덱스)] 넣고 활성화 시키면 됨
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }

        }


    }
}
