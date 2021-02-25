using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/*!
Контроллер врага("Астероид")
         \param MaxStage int максимум стадий разлома астероида
         \param _stageSprites List<Sprite> приватное сериализованное поле - лист спрайтов для различных стадий
         \param _stageSprites List<Sprite> приватное поле - стадия объекта(врага), содержащего этот контроллер

         \param _particle ParticleSystem приватное поле - партинклы врага
         \param _particleTime float приватное сериализованное поле - время анимации партиклов при уничтожении врага

         \param _gameManager GameManager менеджер игры
         \param _particleAnim Action событие: анимация при уничтожении врага

         \param isReadyToDestroy bool булиновка, которая становится "true" когда враг проходит через видимые границы экрана
 */
public class AsteroidController : MonoBehaviour
{

    public int MaxStage = 3;
    [SerializeField]
    private List<Sprite> _stageSprites;
    private int _nowStage = 1;

    private ParticleSystem _particle;
    [SerializeField]
    [Range(0f, 2f)]
    private float _particleTime = 1f;

    private GameManager _gameManager;
    private Action _particleAnim;

    private bool isReadyToDestroy = false;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _particle = GetComponentInChildren<ParticleSystem>();
        _particleAnim = (() => StartParticle(_particleTime));
    }

    private void Update()
    {
        if (!transform.position._isOffScreen())
        {
            isReadyToDestroy = true;
        }
        else if (isReadyToDestroy)
            Destroy(gameObject);
    }
    /*!
    При уничтожении врага он разделяется на два меньшего размера(учитывая условия максимальной стадии)
     */
    private void NextStage()
    {

        if(_nowStage < MaxStage)
        {
            _particleAnim.Invoke();

            _nowStage++;
            GetComponent<RectTransform>().sizeDelta /= 2;
            GetComponent<Image>().sprite = _stageSprites[_nowStage - 1];

            //Second Asteroid
            GameObject cloneObject = Instantiate(gameObject, transform.parent);
            AsteroidController cloneObjController = cloneObject.GetComponent<AsteroidController>();
            cloneObjController._nowStage = _nowStage;

            SecondForce();
            cloneObjController.SecondForce();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /*!
    Партикл проигрывается при попадании по врагу. Копируется партикл, переносится в родителя партиклов, ожидается заранее заданное время, уничтожается
     */
    private async void StartParticle(float time)
    {
        ParticleSystem newParticle = Instantiate(_particle.gameObject, _gameManager._canvasGame.ParticleParent.transform).GetComponent<ParticleSystem>();
        newParticle.transform.position = transform.position;

        newParticle.Play();
        await Task.Delay((int)time * 1000);
        if(newParticle != null)
            Destroy(newParticle.gameObject);
    }
    /*!
    Первое ускорение после Spawn'а
     */
    public void Force()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0, UnityEngine.Random.Range(-Screen.height/4, Screen.height / 4)) - (Vector2)transform.localPosition);
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, 20f ,40f);
        rb.AddTorque(UnityEngine.Random.Range(10f,30f));
    }
    /*!
    Ускорение при разделении врага на 2 таких же
     */
    public void SecondForce()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(_gameManager.SpawnController._randomPosOutScreen /6f - (Vector2)transform.localPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            NextStage();
            if (!_gameManager._isGameOver)
                _gameManager.SetScore(GameManager.setScore/_nowStage);
        }
    }
}
