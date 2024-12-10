using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{

    [SerializeField] private EnemyInfo[] allEnemies;
    [SerializeField] private List<Enemy> currentEnemies;

    private const float LEVEL_MODIFIER = 0.5f;


    private void Awake()
    {
        GenerateEnemyByName("Slime", 10);
    }
    public void GenerateEnemyByName(string enemyName, int level)
    {
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (allEnemies[i].EnemyName == enemyName)
            {
                Enemy newEnemy = new Enemy();
                newEnemy.Level = level;
                float levelModifier = (LEVEL_MODIFIER * newEnemy.Level);
                newEnemy.MaxHealth = Mathf.RoundToInt(allEnemies[i].BaseHealth + allEnemies[i].BaseHealth * levelModifier);
                newEnemy.CurrHealth = newEnemy.MaxHealth;
                newEnemy.Strength = Mathf.RoundToInt(allEnemies[i].BaseStr + allEnemies[i].BaseStr * levelModifier);
                newEnemy.Initiative = Mathf.RoundToInt(allEnemies[i].BaseInitiative + allEnemies[i].BaseInitiative * levelModifier);
                newEnemy.EnemyVisualPrefab = allEnemies[i].EnemyVisualPrefab;

                currentEnemies.Add(newEnemy);
            }
        }
    }
}

[System.Serializable]
public class Enemy
{
    public string EnemyName;
    public int Level;
    public int MaxHealth;
    public int CurrHealth;
    public int Strength;
    public int Initiative;
    public GameObject EnemyVisualPrefab;
}