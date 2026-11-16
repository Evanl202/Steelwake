using System.Collections.Generic;
using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
    public static readonly List<EnemyShip> ActiveEnemies =
        new List<EnemyShip>();

    public static void Register(EnemyShip enemy)
    {
        if (enemy != null && !ActiveEnemies.Contains(enemy))
        {
            ActiveEnemies.Add(enemy);
        }
    }

    public static void Unregister(EnemyShip enemy)
    {
        ActiveEnemies.Remove(enemy);
    }
}