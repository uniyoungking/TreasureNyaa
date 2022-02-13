using System;
using System.Collections.Generic;
using UnityEngine;

namespace LSY
{
    public class ObjectPooler : MonoBehaviour
    {
        [Serializable]
        public struct Pool
        {
            [HideInInspector]
            public Transform parentTr;
            public GameObject poolPrefab;

            public string createName;
            public int poolSize;
        }


        public List<Pool> poolList;
        public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        public List<GameObject> activatedObjects = new List<GameObject>();


        private void Awake()
        {
            foreach (Pool pool in poolList)
            {
                Pool tmpPool = pool;

                if (tmpPool.createName == string.Empty)
                    throw new Exception("Name is Empty");

                // 프리팹 이름 중복 확인
                if (poolDictionary.ContainsKey(tmpPool.createName))
                    throw new Exception("Name is Exist");

                Queue<GameObject> poolDictionaryQueue = new Queue<GameObject>();

                // 부모 오브젝트 생성
                GameObject parentObj = new GameObject($"{tmpPool.createName}Parent");
                parentObj.transform.parent = transform.parent;

                tmpPool.parentTr = parentObj.transform;

                for (int i = 0; i < tmpPool.poolSize; i++)
                {
                    GameObject obj = CreateObject(tmpPool.poolPrefab, parentObj.transform);
                    poolDictionaryQueue.Enqueue(obj);
                }

                poolDictionary.Add(tmpPool.createName, poolDictionaryQueue);
            }
        }

        // String을 가져와야함
        public void EnqueueObject(string name, GameObject obj)
        {
            if (!poolDictionary.ContainsKey(name))
                return;

            if (obj.activeSelf)
                obj.SetActive(false);

            poolDictionary[name].Enqueue(obj);

            if (activatedObjects.Contains(obj))
                activatedObjects.Remove(obj);
        }


        public GameObject DequeueObject(string name)
        {
            if (!poolDictionary.ContainsKey(name))
            {
                Debug.Log("return null");
                return null;
            }

            GameObject obj = null;

            // 큐에 오브젝트가 없을때는 새로 생성
            if (poolDictionary[name].Count == 0)
            {
                foreach (var pool in poolList)
                {
                    if (pool.createName.Equals(name))
                    {
                        obj = CreateObject(pool.poolPrefab, pool.parentTr);
                        break;
                    }
                }                
            }
            else
            {
                obj = poolDictionary[name].Dequeue();
            }

            if (!activatedObjects.Contains(obj))
            {
                activatedObjects.Add(obj);
            }

            //Debug.Log(obj.name + "a");
            return obj;
        }


        public string GetRandomName(int level)
        {
            int ranNum;
            string tagName;

            while (true)
            {
                ranNum = UnityEngine.Random.Range(0, poolList.Count);

                tagName = poolList[ranNum].createName;

                if (level == 1)
                {
                    if (tagName.Equals("NormalBlockB") || tagName.Equals("NormalBlockC") || tagName.Equals("MucusBlockB") || tagName.Equals("MucusBlockC") ||
                        tagName.Equals("Honey") || tagName.Equals("FlowerB"))
                    {
                        int ran_1 = UnityEngine.Random.Range(0, 100);
                        if (ran_1 >= 80)
                            tagName = "NormalBlockB";
                        else if (ran_1 >= 60)
                            tagName = "NormalBlockC";

                        break;
                    }
                }
                else if (level == 2)
                {
                    if (tagName.Equals("NormalBlockA") || tagName.Equals("NormalBlockB") || tagName.Equals("MucusBlockA") || tagName.Equals("MucusBlockB") ||
                        tagName.Equals("Honey") || tagName.Equals("FlowerB"))
                    {
                        int ran_2 = UnityEngine.Random.Range(0, 100);
                        if (ran_2 >= 85)
                            tagName = "NormalBlockA";
                        else if (ran_2 >= 70)
                            tagName = "NormalBlockB";
                        else if (ran_2 >= 55)
                            tagName = "MucusBlockA";
                        else if (ran_2 >= 40)
                            tagName = "MucusBlockB";

                        break;
                    }
                }
                else
                {
                    if (tagName.Equals("NormalBlockA") || tagName.Equals("NormalBlockB") || tagName.Equals("MucusBlockA") || tagName.Equals("MucusBlockB") ||
                        tagName.Equals("Honey") || tagName.Equals("FlowerA"))
                    {
                        int ran_3 = UnityEngine.Random.Range(0, 100);
                        if (ran_3 >= 80)
                            tagName = "NormalBlockA";
                        else if (ran_3 >= 60)
                            tagName = "MucusBlockA";
                        else if (ran_3 >= 45)
                            tagName = "NormalBlockB";
                        else if (ran_3 >= 30)
                            tagName = "MucusBlockB";

                        break;
                    }
                }
            }

            if (!tagName.Equals(string.Empty))
                return tagName;
            else
                return string.Empty;
        }


        // 오브젝트 생성
        private GameObject CreateObject(GameObject prefab, Transform parent)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            obj.transform.parent = parent;

            return obj;
        }

    }

}
