using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*!
Статичный класс для проверки нахождения за пределами экрана
 */
public static class CheckOutOffScreen
{
    /*!
    Статичный вектор2, возвращает положение на экране
    */
    public static Vector2 _posOnScreen(Vector3 pos)
    {
        return Camera.main.WorldToScreenPoint(pos);
    }
    /*!
    Метод расширения для Vector3, возвращает булиновку, говорит находится ли объект за пределами экрана
    */
    public static bool _isOffScreen(this Vector3 pos)
    {
        Vector2 posOnScreen = _posOnScreen(pos);
        return (posOnScreen.x < 0 || posOnScreen.x > Screen.width || posOnScreen.y < 0 || posOnScreen.y > Screen.height);
    }
}
/*!
Контроллер пули
 */
public class BulletController : MonoBehaviour
{
    private void Update()
    {
        if(transform.position._isOffScreen())
        {
            Destroy(gameObject);
        }
    }
}
