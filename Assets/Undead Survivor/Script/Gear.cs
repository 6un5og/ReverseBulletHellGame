using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;      // 장비 타입
    public float rate;                  // 레벨 별 데이터

    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear" + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();            // 처음 기어가 생성 되자마자 기어 기능 적용
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;       // 지금 데이터는 새로 들어 온 데이터로 갱신한다
        ApplyGear();            // 레벨업 시 기어 업그레이드
    }

    void ApplyGear()            // weapon이 새로 생성될 때, 업그레이드 됐을 때, 기어 자체가 새로 생겼을 때, 기어 자체에서 레벨업 될 때
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp()       // 장갑의 기능인 연사력을 올리는 함수
    {
        // 모든 무기를 다 올려야 하기 때문에 플레이어로 올라가서 모든 Weapon을 가져옴
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)          // 근접과 원거리
            {
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);      // 근거리 : 값이 길수록 빠름
                    break;
                default:
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);      // 원거리 : 값이 짧을수록 많이 발사
                    break;
            }
        }
    }

    void SpeedUp()       // 이속 올리는 함수
    {
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
