using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField]
    private ItemGenerator itemGenerator;

    private LSY.ObjectPooler pooler;
    private PlayerController player;

    private Vector2 rangeX = new Vector2(-8f, 8f);

    private int platformDicKey = 0;
    private int minimumKey = 0;

    private int minBlockCount = 3;
    private int maxBlockCount = 6;


    // Key값으로 발판이 몇번째 줄인지 계산
    public Dictionary<int, List<PlatformBase>> PlatformListDic { get; private set; } = new Dictionary<int, List<PlatformBase>>();


    private void Awake()
    {
        pooler = GetComponent<LSY.ObjectPooler>();
        player = FindObjectOfType<PlayerController>();
    }

    // 활성화된 발판들을 리스트로 반환
    public List<GameObject> GetActivatedObjects()
    {
        return pooler.activatedObjects;
    }

    // 한줄에 기본이되는 발판들 생성
    public void GenerateBasicePlatformLine(float yPos, int level)
    {
        if (level == 1)
        {
            minBlockCount = 3;
            maxBlockCount = 6;
        }
        else if (level == 2)
        {
            minBlockCount = 3;
            maxBlockCount = 5;
        }
        else
        {
            minBlockCount = 3;
            maxBlockCount = 4;
        }

        List<PlatformBase> platformList = new List<PlatformBase>();
        PlatformListDic.Add(platformDicKey, platformList);

        int count = Random.Range(minBlockCount, maxBlockCount);

        // x축 나누기
        float divideRangeValue = (rangeX.y * 2) / count;
        float leftStartPoint = rangeX.x;

        // Platform 생성
        for (int j = 0; j < count; j++)
        {
            float rightPoint = leftStartPoint + divideRangeValue;

            //GeneratePlatformInRange(leftPoint, rightPoint, yPos);
            string name = pooler.GetRandomName(level);

            PlatformBase platform = CreatePlatform(name);

            // Platform에 필요한 정보들 넘김
            PlatformInfo info = new PlatformInfo();
            info.generator = this;
            info.pooler = pooler;
            info.objName = name;
            info.listKey = platformDicKey;
            platform.Initialize(info);

            platformList.Add(platform);

            Vector3 createPos = CalculateCreatePos(platform, leftStartPoint, rightPoint, yPos);
            platform.transform.position = createPos;

            platform.gameObject.SetActive(true);

            leftStartPoint = rightPoint;

            GenerateItem(createPos, name, level);
        }

        //Debug.Log("Now key" + platformDicKey.ToString());
        platformDicKey++;
    }


    public void RemovePlatformInList(int key, PlatformBase platform)
    {
        PlatformListDic[key].Remove(platform);

        if (PlatformListDic[key].Count == 0)
        {
            PlatformListDic.Remove(key);
            minimumKey = key + 1;
        }
    }



    // 발판 생성 위치 결정
    private Vector3 CalculateCreatePos(PlatformBase platform, float leftPoint, float rightPoint, float yPos)
    {
        float halfSizeX = platform.GetSpriteSizeX() * 0.5f;
        leftPoint += halfSizeX;
        rightPoint -= halfSizeX;

        float newPos = Random.Range(leftPoint, rightPoint);
        //Vector3 createPos = new Vector3(newPos, yPos, 0);

        return new Vector3(newPos, yPos, 0);
    }


    // 빈 발판 1개 생성
    private PlatformBase CreatePlatform(string name)
    {
        GameObject platformObj = pooler.DequeueObject(name);
        PlatformBase platform = platformObj.GetComponent<PlatformBase>();

        return platform;
    }



    // 발판 추가로 끼워넣기
    public bool InsertPlatform()
    {
        PlatformBase platform = CreatePlatform(pooler.GetRandomName(1));

        int insertStartKey = 0;

        // 플레이어보다 높은 발판 찾기
        for (int key = minimumKey; key < platformDicKey; key++)
        {
            if (PlatformListDic[key][0].transform.position.y > player.transform.position.y)
            {
                insertStartKey = key;
                break;
            }
        }


        // 한줄에서 생성 못하면 다음줄로 넘김
        // 모든 줄에서 생성 못하면....???
        for (int i = insertStartKey; i < platformDicKey; i++)
        {
            // 한 줄 랜덤
            List<PlatformBase> list = PlatformListDic[i];

            // 해당 줄에 최소 2개 이상의 블록이 있을 때... 2개도 없는 상황이 있을까??
            if (list.Count < 2)
                continue;

            // 이거 랜덤값이 중복되서 나올것 같다.
            System.Random ran = new System.Random();

            for (int j = 0; j < list.Count; j++)
            {
                int ranIndex = ran.Next(0, list.Count - 1);

                // 랜덤 인덱스의 플랫폼과 해당 플랫폼의 오른쪽을 비교
                float leftPosX = list[ranIndex].GetRightPosX();
                float rightPosX = list[ranIndex + 1].GetLeftPosX();

                // 사이에 들어갈 수 있는지 판단
                float platformSize = platform.GetSpriteSizeX();

                platform.gameObject.SetActive(true);

                // 생성가능
                if (platformSize < rightPosX - leftPosX)
                {
                    Transform tmpTr = list[0].transform;

                    float ranX = Random.Range(leftPosX + platformSize * 0.5f, rightPosX - platformSize * 0.5f);

                    // 플랫폼을 두 위치 사이에 넣기
                    platform.transform.position = new Vector3(ranX, tmpTr.position.y, tmpTr.position.z);

                    list.Insert(ranIndex + 1, platform);

                    platform.gameObject.SetActive(true);

                    return true;
                }
                // 불가능
                else
                {
                    continue;
                }
            }
        }

        return false;
    }


    // 아이템, 몬스터 생성 (지금은 여기서 처리하도록)
    // 나중에는 매니저가?? => 상황에 따라서...
    private void GenerateItem(Vector3 createPos, string platformName, int level)
    {
        // 생성확률은 1/5 => 20%
        int ran = Random.Range(0, 5);

        if (ran >= 2)
        {
            itemGenerator.GenerateItem(createPos, platformName, level);
        }
    }

}
