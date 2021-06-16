using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceContainer : MonoBehaviour, IDropHandler
{
    #region Properties

    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    public GridLayoutGroup GridLayoutGroup => gridLayoutGroup;

    private readonly List<Dice> _diceList = new List<Dice>();
    public int DiceCount => _diceList.Count;

    #endregion

    #region Drop Actions

    public bool Freeze { get; set; }

    public void OnDrop(PointerEventData eventData)
    {
        if (Freeze) return;
        if (eventData.pointerDrag == null) return;

        var dice = eventData.pointerDrag.GetComponent<Dice>();
        if (dice == null) return;

        _diceList.Add(dice);
        dice.SetDiceContainer(this);
    }

    public void RemoveDice(Dice dice)
    {
        _diceList.Remove(dice);
    }

    #endregion

    #region Roll Actions

    public int Roll()
    {
        var value = 0;
        foreach (var dice in _diceList)
        {
            value += dice.Roll();
            dice.Freeze = true;
        }

        return value;
    }

    [Header("Dice")] [SerializeField] private Dice dicePrefab;

    public void Restart()
    {
        Freeze = false;

        foreach (var dice in _diceList)
        {
            dice.Over();
        }

        _diceList.Clear();
    }

    public void AddDice()
    {
        var dice = Instantiate(dicePrefab, transform);
        dice.SetDiceContainer(this);
    }

    public void X2()
    {
        var list = _diceList.ToList();
        foreach (var dice in list)
        {
            var result = Random.Range(0, 3);
            switch (result)
            {
                case 0:
                    _diceList.Remove(dice);
                    dice.Over();
                    break;
                case 2:
                    AddDice();
                    break;
            }
        }
    }

    #endregion
}