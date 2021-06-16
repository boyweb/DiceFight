using EasyButtons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dice : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Dice Actions

    private int _minValue = 1;
    private int _maxValue = 6;

    public int Value { get; set; }

    public int Roll()
    {
        Value = Random.Range(_minValue, _maxValue + 1);
        diceValue.text = $"{Value}";
        return Value;
    }

    [Button("Roll")]
    public void RollTest()
    {
        Roll();
    }

    public void Over()
    {
        if (_cloneTransform != null) Destroy(_cloneTransform.gameObject);
        Destroy(gameObject);
    }

    #endregion

    #region Animations

    private RectTransform _cloneTransform;

    private void LateUpdate()
    {
        if (_cloneTransform == null)
        {
            var parent = diceTransform.parent;
            diceTransform.SetParent(MainCanvas.Instance.MainCanvasTransform, true);

            var obj = new GameObject("DiceClone");
            _cloneTransform = obj.AddComponent<RectTransform>();
            _cloneTransform.SetParent(parent);
        }


        diceTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            _diceContainer.GridLayoutGroup.cellSize.x);
        diceTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            _diceContainer.GridLayoutGroup.cellSize.y);
        if (_dragging) return;
        diceTransform.position = _cloneTransform.position;
    }

    #endregion

    #region Properties

    [SerializeField] private RectTransform diceTransform;
    [SerializeField] private CanvasGroup diceCanvasGroup;

    [SerializeField] private Text diceValue;

    private DiceContainer _diceContainer;

    #endregion

    #region Drag Actions

    public bool Freeze { get; set; }
    private bool _dragging;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Freeze) return;
        _dragging = true;
        diceCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Freeze) return;
        diceTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Freeze) return;
        _dragging = false;
        diceCanvasGroup.blocksRaycasts = true;
    }

    public void SetDiceContainer(DiceContainer diceContainer)
    {
        if (_diceContainer != null) _diceContainer.RemoveDice(this);
        _diceContainer = diceContainer;

        if (_cloneTransform != null) _cloneTransform.SetParent(diceContainer.transform, false);
    }

    #endregion
}