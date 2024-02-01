using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;       // 플레이어가 가지고 있는 웨폰 오브젝트와 연동해줘야 함
    public Gear gear;

    Image icon;                 // using UnityEngine.UI;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];     // 자식들 모두 끌어오기, 본인포함이기 때문에 [1]
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>(); // GetComponents의 순서는 계층구조의 순서를 따라감
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()                              // 레벨 텍스트 로직(활성화 될 때 자동으로 실행)
    {
        textLevel.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                // damage는 소수점으로 나와있기 때문에 백분율을 위해 100 곱하기
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            case ItemData.ItemType.Heal:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();        // 게임 오브젝트를 스크립트로 만듦
                    // 자기가 직접 만든 컴포넌트를 반환해주는 함수
                    weapon = newWeapon.AddComponent<Weapon>();               // AddComponent<T> : 게임 오브젝트에 T 컴포넌트를 추가하는 함수
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];    // 레벨당 데미지를 백분율로 했기 때문에 레벨당 공격력을 곱해서 넣어줌
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);                  // Weapon의 작성된 레벨업 함수를 그대로 활용
                }

                level++;                // 레벨 값을 올리는 로직은 일회성 케이스를 피해서 적용
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }

                level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
