using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*!
Класс содержит основные методы логики игры и хранит ссылки на другие контроллеры и менеджеры.
         \param LivesController LivesController контроллер жизней

         \param _canvasUI CanvasUIManager менеджер UI канваса
         \param _canvasGame CanvasGameManager менеджер Game канваса
         \param SpawnController SpawnController контроллер Spawn'а врагов
          
         \param score int статичное поле - количество очков
         \param lives int статичное поле - количество жизней
         \param setScore int статичное поле - получаемое количество очков(за действие: убийство врага)

         \param enemySpawnTime int статичное поле - время Spawn'а врагов

         \param _lives int сериализованное приватное поле - количество жизней
         \param _setScore int сериализованное приватное поле - получаемое количество очков(за действие: убийство врага)
         \param _enemySpawnTime int сериализованное приватное поле - время Spawn'а врагов

         \param AsteroidPrebab GameObject префаб врага("Астероид")

         \param _centerText string сериализованное приватное поле - текст в центре при вылете игрока за пределы экрана

         \param _isGameOver bool булиновка, определяющая закончился ли гейм

*/
public class GameManager : MonoBehaviour
{
    public LivesController LivesController;

    public CanvasUIManager _canvasUI;
    public CanvasGameManager _canvasGame;
    public SpawnController SpawnController;

    public static int score = 0, lives = 3, setScore = 60;
    public static int enemySpawnTime = 2;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    [Range(60, 240)]
    private int _setScore = 60;
    [SerializeField]
    [Range(0, 10)]
    private int _enemySpawnTime = 2;

    public GameObject AsteroidPrebab;

    [SerializeField]
    private string _centerText;

    public bool _isGameOver = false;

    private void OnValidate()
    {
        if(_canvasUI.CenterChat.text != _centerText)
        {
            _canvasUI.CenterChat.text = _centerText;
        }
    }
    /*!
    Для кнопки "Play" основного меню 
    */
    public void ClickPlay()
    {
        _canvasUI.gameObject.SetActive(true);
        _canvasGame.gameObject.SetActive(true);
        StartGame();
    }
    /*!
    Метод устанавливает статичные поля из сериализованных и вызывает методы SetLives и StartSpawn контроллеров LivesController и SpawnController
     */
    public void StartGame()
    {
        score = 0;
        lives = _lives;
        setScore = _setScore;
        enemySpawnTime = _enemySpawnTime;

        SetScore(0);

        LivesController.SetLives(lives);

        SpawnController.StartSpawn();
    }

    /*!
    Отвечает за добавление игровых очков
         \param setScore отрицательное или положительное колличество очков
     */
    public void SetScore(int setScore)
    {
        score += setScore;
        _canvasUI.TextScore.text = score.ToString();
    }
    /*!
    Вызывается при проигрыше
    Дествие: 
    - окончание игры
    - ожидание
    - перезагрузка игры
     */
    public async void GameOver()
    {
        _canvasUI.CenterChat.enabled = true;
        _canvasUI.CenterChat.text = "END";
        await Task.Run(async () => await Task.Delay(3500));
        SceneManager.LoadSceneAsync(0);
    }
}
