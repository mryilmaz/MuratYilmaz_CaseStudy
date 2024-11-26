using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform coinParent; 
    [SerializeField] private int poolSize = 10; 
    private Queue<GameObject> coinPool;

    private void Awake()
    {
        coinPool = new Queue<GameObject>();


        for (int i = 0; i < poolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab,coinParent);
            coin.SetActive(false);  
            coinPool.Enqueue(coin);
        }
    }


    public GameObject GetCoin()
    {
        if (coinPool.Count > 0)
        {
            GameObject coin = coinPool.Dequeue();
            coin.SetActive(true);  
            return coin;
        }
        else
        {
            
            GameObject coin = Instantiate(coinPrefab,coinParent);
            return coin;
        }
    }

    
    public void ReturnCoinToPool(GameObject coin)
    {
        coin.SetActive(false);  
        coinPool.Enqueue(coin);  
    }
}