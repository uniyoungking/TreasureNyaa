using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    public SkillButton mainSkill;
    public SkillButton[] subSkills;

    private PlatformGenerator platformGenerator;
    private PlayerController player;

    private float mainSkillTime = 30f;
    private float subSkillTime = 30f;


    private void Awake()
    {
        platformGenerator = FindObjectOfType<PlatformGenerator>();
        player = FindObjectOfType<PlayerController>();

        SetMainSkill();
        SetSubSkills();
    }


    private void SetMainSkill()
    {
        mainSkill.button.onClick.AddListener(OnClickMainSkill);
        mainSkill.SetCooldownTime(mainSkillTime);
    }


    private void SetSubSkills()
    {
        subSkills[0].button.onClick.AddListener(OnClickSubSkill1);
        subSkills[1].button.onClick.AddListener(OnClickSubSkill2);
    }



    //private void SetSubSkills()
    //{
    //    for (int i = 0; i < subSkills.Length; i++)
    //    {
    //        int j = i;
    //        subSkills[j].button.onClick.AddListener(() => OnClickSubSkill(subSkills[j]));
    //        subSkills[j].SetCooldownTime(subSkillTime);
    //    }
    //}


    private void OnClickMainSkill()
    {
        mainSkill.SetCooldownTime(mainSkillTime);
        mainSkill.StartCooldown();

        List<GameObject> platforms = platformGenerator.GetActivatedObjects();

        int[] randomIndex = GetRandomInt(platforms.Count, 0, platforms.Count);

        for (int i = 0; i < platforms.Count; i++)
        {
            GameObject obj = platforms[randomIndex[i]];

            if (player.transform.position.y > obj.transform.position.y)
                continue;

            Vector3 newVec = new Vector3(obj.transform.position.x, obj.transform.position.y + 0.5f, 0);
            player.transform.position = newVec;

            break;
        }
    }


    // ----- 임시... 바꾸기 -----

    private void OnClickSubSkill1()
    {
        subSkills[0].SetCooldownTime(subSkillTime);
        subSkills[0].StartCooldown();

        for (int i = 0; i < 12; i++)
        {
            if (!platformGenerator.InsertPlatform())
                break;
        }        
    }


    private void OnClickSubSkill2()
    {
        subSkills[1].SetCooldownTime(subSkillTime);
        subSkills[1].StartCooldown();


    }




    public int[] GetRandomInt(int length, int min, int max)
    {
        int[] randArray = new int[length];
        bool isSame;

        for (int i = 0; i < length; ++i)
        {
            while (true)
            {
                randArray[i] = Random.Range(min, max);
                isSame = false;

                for (int j = 0; j < i; ++j)
                {
                    if (randArray[j] == randArray[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
        }
        return randArray;
    }
}
