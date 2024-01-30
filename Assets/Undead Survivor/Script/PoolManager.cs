using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 1. 프리팹들을 보관할 변수            .. 프리팹도 게임 오브젝트
    // 2. 풀 담당을 하는 리스트
    // 3. 프리팹과 리스트의 관계는 1:1 (즉 프리팹의 개수만큼 리스트의 개수가 필요)

    public GameObject[] prefabs;        // 1번 변수

    List<GameObject>[] pools;           // 2번 리스트

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];       // 리스트 배열 변수 초기화 할 때 크기는 프리펩 배열 길이 사용

        for (int index = 0; index < pools.Length; index++)  // 리스트 초기화 반복문 사용
        {
            pools[index] = new List<GameObject>();
        }
    }

    // 어디서든 사용을 위해 공개        .. 게임 오브젝트를 반환하는 함수 선언
    public GameObject Get(int index)    // 가져올 오브젝트 종류를 결정하는 매개변수 추가 (index)
    {
        GameObject select = null;       // pool 하나 안에서 놀고있는 오브젝트 하나를 선택해서 반환한다

        // 선택한 풀의 놀고 있는 (비활성화 된) 게임 오브젝트 접근
        // (만약에) 발견하면 select 번수에 할당

        // foreach : 배열, 리스트 들의 데이터를 순차적으로 접근하는 반복문
        foreach (GameObject item in pools[index])   // foreach에서 나오는 값은 따로 지역변수로 담아줘야 함
        {
            if (!item.activeSelf)    // 내용물 오브젝트가 비활성화 (대기 상태)인지 확인
            {
                select = item;              // 발견하면 select 변수에 할당
                select.SetActive(true);     // 활성화
                break;                      // 1개 발견했기 때문에 반복문 종료
            }
        }

        // (모두 쓰고있다면 or 못찾았다면) 새롭게 생성하고 select 변수에 할당
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);    // Instantiate : 원본 오브젝트를 복제하여 장면에 생성하는 함수 (게임 오브젝트도 반환해줌)
            pools[index].Add(select);                           // 생성된 오브젝트는 해당 오브젝트 풀 리스트에 Add 함수로 추가
        }

        return select;
    }
}
