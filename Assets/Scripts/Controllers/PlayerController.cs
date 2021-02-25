using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*!
Контроллер Игрока
         \param BulletsStartPoint GameObject сериализованное приватное поле - Стартовая позиция для пули
         \param BulletPrefab GameObject сериализованное приватное поле - Префаб пули

         \param _shipSpeed float сериализованное приватное поле - скорость игрока
         \param _bulletSpeed float сериализованное приватное поле - скорость пули

         \param _gameManager GameManager менеджер игры
         \param _rb Rigidbody2D компонент rigidbody2d игрока
         \param _particle ParticleSystem партиклы игрока(огонь, выходящий из нижней части корабля)

         \param _IsCenterChatActive bool булиновка, определяющая состояние центрального чата
         \param _IsSuperMode bool булиновка, определяющая находится ли игрок в состояние неуязвимости(Супер мод)
 */
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject BulletsStartPoint, BulletPrefab;

    [SerializeField]
    [Range(1f, 10f)]
    private float _shipSpeed = 2f, _bulletSpeed = 4f;


    private GameManager _gameManager;
    private Rigidbody2D _rb;
    private ParticleSystem _particle;

    private bool _IsCenterChatActive = false;
    private bool _IsSuperMode = false;


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
        _particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        CheckOffScreen();
        RotateToMousePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        CheckSpaceKey();
        CheckMouseButtonDown();
    }
    /*!
    проверяет находится ли игрок за пределами экрана
     */
    private void CheckOffScreen()
    {
        //If player out off screen
        if (transform.position._isOffScreen() != _IsCenterChatActive)
        {
            _IsCenterChatActive = !_IsCenterChatActive;
            _gameManager._canvasUI.CenterChat.enabled = _IsCenterChatActive;
        }
    }
    /*!
    поворачивает игрока в направлении курсора мыши
     */
    private void RotateToMousePos(Vector3 _mousePos)
    {
        float AngleRad = Mathf.Atan2(_mousePos.y - this.transform.position.y, _mousePos.x - this.transform.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }
    /*!
    проверяет нажатие клавиши "Space" на клавиатуре
     */
    private void CheckSpaceKey()
    {
        // Start/stop particle
        if (Input.GetKeyDown(KeyCode.Space))
            _particle.Play();
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _particle.Stop();
            _particle.Clear();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(transform.right/10f * _shipSpeed, ForceMode2D.Impulse);
        }
        
    }
    /*!
    проверяет нажатие левой кнопки мыши
     */
    private void CheckMouseButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnBullet();
        }
    }
    /*!
    Spawn пули и задание движения, учитывая направление игрока
     */
    private void SpawnBullet()
    {
        GameObject bullet = Instantiate(BulletPrefab, _gameManager._canvasGame.BulletsParent.transform);
        bullet.transform.position = BulletsStartPoint.transform.position;
        Rigidbody2D _rbBullet = bullet.GetComponent<Rigidbody2D>();
        _rbBullet.AddForce(transform.right * _bulletSpeed);
    }
    /*!
    включение неуязвимости(супер мод) на время
     */
    private IEnumerator OnSuperMode()
    {
        _IsSuperMode = true;
        GetComponent<Image>().color = Color.red;
        yield return new WaitForSecondsRealtime(3f);
        _IsSuperMode = false;
        GetComponent<Image>().color = Color.white;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid" && !_gameManager._isGameOver && !_IsSuperMode)
        {
            StartCoroutine(OnSuperMode());
            GameManager.lives--;
            _gameManager.LivesController.MinusLive();
            Destroy(collision.gameObject);
            //GameOver
            if (GameManager.lives <= 0)
            {
                _gameManager.GameOver();
                return;
            }
        }
    }
}
