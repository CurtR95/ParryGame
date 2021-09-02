using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyData;

[System.Serializable]

public class Enemy : EnemyData
{
    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }
}
