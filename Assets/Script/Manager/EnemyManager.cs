using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using GGG.Tool.Singleton;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField]private Transform CurPlayer;

    [SerializeField] private List<GameObject> _allEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> _activeEnemies = new List<GameObject>();


    [SerializeField] private Vector2 _attackInterval = new Vector2(10, 12);

    private WaitForSeconds _waitTime;


    public Transform GetCurTarget() => CurPlayer;
    protected override void Awake()
    {
        //CurPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        _waitTime = new WaitForSeconds(4);
    }
    private void Start()
    {
        foreach(var enemy in _allEnemies)
        {
            if (enemy.activeSelf == true)
            {
                _activeEnemies.Add(enemy);
            }
        }
        StartCoroutine(SelectOneEnemyToAttack());
    }


    IEnumerator SelectOneEnemyToAttack()
    {

        while (true)
        {
            int ran = Random.Range((int)_attackInterval.x, (int)_attackInterval.y);
            //DevelopmentToos.WTF($"¹¥»÷¼ä¸ô+{ran}");
            yield return new WaitForSeconds(ran);
            int index = Random.Range(0, _activeEnemies.Count);
            _activeEnemies[index].GetComponent<EnemyCombatControl>().SetAttackCommand(true);
            
        }


    }


    public void AddEnemyToList(GameObject enemy)
    {
        _allEnemies.Add(enemy);
        Debug.Log("a + " + _allEnemies.Count);
    }


    public void RemoveEnemyInList(GameObject enemy)
    {
        Debug.Log("r + " + _allEnemies.Count);
        _allEnemies.Remove(enemy);
    }

}
