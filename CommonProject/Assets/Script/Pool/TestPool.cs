using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sdy.Pool
{

    public class TestPool : MonoBehaviour
    {

        public GameObejct Prefabs;

        private ReusePool<GameObject> pool;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        private GameObejct NewPrefabs()
        {
            GameObeject go = instantiate(Prefabs);
            return go;
        }


        private void ResetPrefabs(GameObject go)
        {
            go.setActive(true);
        }


        private void OnDestroy(GameObejct go)
        {
            go.setActive(true);
            pool.Store(go);
        }
    }
}
