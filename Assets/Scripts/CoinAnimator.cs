using System;
using UnityEngine;
using DG.Tweening;  
using UnityEngine.UI;

public class CoinAnimator : MonoBehaviour
{
    private Camera mainCamera;  
    private ObjectPooler objectPooler; 
    private GameManager gameManager;
    [SerializeField] private Image mainCoinImage;  
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private Transform playerTransform;

    private void Start()
    {
        gameManager=GameManager.instance;
        mainCamera = Camera.main;
        objectPooler=FindObjectOfType<ObjectPooler>();
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SpawnCoins(5,playerTransform.position);
        }
    }*/

    public void SpawnCoins(int coinCount, Vector3 coinWorldPosition)
    {
        StartCoroutine(SpawnAndAnimateCoins(coinCount, coinWorldPosition));
    }

    private System.Collections.IEnumerator SpawnAndAnimateCoins(int coinCount, Vector3 coinWorldPosition)
    {

        Vector2 screenPosition = mainCamera.WorldToScreenPoint(coinWorldPosition);
        Vector2 targetPosition = mainCoinImage.transform.position;  

        for (int i = 0; i < coinCount; i++)
        {

            GameObject coin = objectPooler.GetCoin();
            RectTransform coinRect = coin.GetComponent<RectTransform>();


            coinRect.position = screenPosition;  


            coinRect.DOMove(targetPosition, moveDuration)  
                .OnComplete(() =>
                {
                    gameManager.CoinAmount++;
                    objectPooler.ReturnCoinToPool(coin);
                });
            
            yield return new WaitForSeconds(0.1f);  
        }
    }
}
