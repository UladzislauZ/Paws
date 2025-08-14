using UnityEngine;

namespace FruityPaw.Scripts.Level.FieldObjects
{
    public class UIGrid : MonoBehaviour
    {
        [SerializeField] private RectTransform[] gameField;
        [SerializeField] private RectTransform[] playerField;
        [SerializeField] private RectTransform[] botField;
    }
}