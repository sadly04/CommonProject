using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sdy.Pool
{

    public class TestPool : MonoBehaviour
    {

        public GameObject Prefabs;

        private ReusePool<GameObject> pool;

        public List<GameObject> testPool = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            pool = new ReusePool<GameObject>(NewPrefabs, ResetPrefabs);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                testPool.Add(pool.New());
            }
            if (Input.GetMouseButton(1))
            {
                if (testPool.Count > 0)
                {
                    GameObject go = testPool[0];
                    testPool.RemoveAt(0);
                    DestroyPrefabs(go);
                }
            }
        }


        private GameObject NewPrefabs()
        {
            GameObject go = Instantiate(Prefabs);
            return go;
        }


        private void ResetPrefabs(GameObject go)
        {
            go.SetActive(true);
        }


        private void DestroyPrefabs(GameObject go)
        {
            go.SetActive(false);
            pool.Store(go);
        }
    }
}
