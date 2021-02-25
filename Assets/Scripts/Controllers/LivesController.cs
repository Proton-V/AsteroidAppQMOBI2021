using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!
Контроллер жизней
         \param _livesPrefab GameObject сериализованное приватное поле - префаб жизни(UI)
         \param _livesUIObject List<GameObject> приватное поле - лист жизней(UI)
 */
public class LivesController : MonoBehaviour
{
    [SerializeField]
    private GameObject _livesPrefab;

    private List<GameObject> _livesUIObject = new List<GameObject>();

    /*!
    Задаем жизни в UI канвас
     */
    public void SetLives(int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject _newObj = GameObject.Instantiate(_livesPrefab, transform);
            _livesUIObject.Add(_newObj);
        }
    }
    /*!
    Убираем крайний объект "жизни" с канваса / вызываем Destroy
     */
    public void MinusLive()
    {
        GameObject liveObj = _livesUIObject[_livesUIObject.Count - 1];
        _livesUIObject.Remove(liveObj);
        Destroy(liveObj);
    }
}
