using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


/*!
Контроллер Spawn'а врагов
         \param _gameManager GameManager менеджер игры

 */
public class SpawnController : MonoBehaviour
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    /*!
    Вектор2 с рандомной позицией за пределами экрана(стандартные расчеты, не учитывая статичность пикселей канваса)

    */
    public Vector2 _randomPosOutScreen
    {
        get
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    return new Vector2(Random.Range(-Screen.width / 2, Screen.width / 2), Screen.height / 2);
                case 1:
                    return new Vector2(Random.Range(-Screen.width / 2, Screen.width / 2), -Screen.height / 2);
                case 2:
                    return new Vector2(-Screen.width / 2, Random.Range(-Screen.height / 2, Screen.height / 2));
                case 3:
                    return new Vector2(Screen.width / 2, Random.Range(-Screen.height / 2, Screen.height / 2));
            }
            return Vector2.zero;
        }
    }
    /*!
    Старт корутины Spawn'а

    */
    public void StartSpawn()
    {
        StartCoroutine(Spawn());
    }
    /*!
    IEnumerator Spawn'а - ожидает, создает врага, устанавливает его позицию и вызывает у него метод Force(), если игра не окончена вызывает сама себя

    */
    public IEnumerator Spawn()
    {
        yield return new WaitForSecondsRealtime(GameManager.enemySpawnTime);
        GameObject newEnemy = GameObject.Instantiate(_gameManager.AsteroidPrebab, _gameManager._canvasGame.EnemyParent.transform);
        newEnemy.transform.localPosition = _randomPosOutScreen;
        newEnemy.GetComponent<AsteroidController>().Force();
        if(!_gameManager._isGameOver)
            StartCoroutine(Spawn());
    }
}
