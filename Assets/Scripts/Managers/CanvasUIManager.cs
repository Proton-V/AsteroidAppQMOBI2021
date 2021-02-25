using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*!
Менеджер UI канваса (при загрузке отключает CenterChat (Text компонент на объекте))
         \param TextScore Text текстовое поле для игровых очков
         \param CenterChat Text текстовое поле в центре экрана для чата с игроком
         \param LivesParent GameObject родитель для отображения колличества жизней(UI)
 */
public class CanvasUIManager : MonoBehaviour
{
    public Text TextScore, CenterChat;
    public GameObject LivesParent;

    private void Awake()
    {
        CenterChat.enabled = false;
    }
}
