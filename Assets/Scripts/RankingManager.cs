using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    public List<Transform> competitors = new List<Transform>(); 
    [SerializeField] private Transform finishLine;
    [SerializeField] private TextMeshProUGUI rankText;

    private bool isRaceFinished = false;

    private void Update()
    {
        if (!isRaceFinished)
        {
            float playerDistance = Vector3.Distance(player.position, finishLine.position);
            int playerRank = 1;  
            
            foreach (Transform competitor in competitors)
            {
                float competitorDistance = Vector3.Distance(competitor.position, finishLine.position);
                
                if (competitorDistance < playerDistance)
                {
                    playerRank++;
                }
            }

            rankText.text = GetRankSuffix(playerRank);

            if (playerDistance < 1f)
            {
                isRaceFinished = true;
            }
        }
    }

    private string GetRankSuffix(int rank)
    {
        if (rank % 10 == 1 && rank != 11)
        {
            return rank + "st";
        }
        else if (rank % 10 == 2 && rank != 12)
        {
            return rank + "nd";
        }
        else if (rank % 10 == 3 && rank != 13)
        {
            return rank + "rd";
        }
        else
        {
            return rank + "th";
        }
    }
}