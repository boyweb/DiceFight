using System;
using UnityEngine;
using UnityEngine.UI;

public class FightAction : MonoBehaviour
{
    private void Start()
    {
        SetValue();
    }

    #region Properties

    [SerializeField] private Text actionValue;
    [SerializeField] private DiceContainer actionDiceContainer;
    [SerializeField] private Button actionRoll;

    private void SetValue()
    {
        actionValue.text = $"1<color=#009900>+{Value}</color>";
    }

    #endregion

    #region Roll Action

    public int Value { get; set; }
    public bool Rolled { get; set; }

    public void Roll()
    {
        if (actionDiceContainer.DiceCount <= 0) return;

        Rolled = true;

        Value = actionDiceContainer.Roll();
        SetValue();
        actionDiceContainer.Freeze = true;
        actionRoll.gameObject.SetActive(false);
    }

    public void Restart()
    {
        Rolled = false;

        Value = 0;
        SetValue();
        actionDiceContainer.Restart();
        actionRoll.gameObject.SetActive(true);
    }

    public void Over()
    {
        actionRoll.gameObject.SetActive(false);
    }

    #endregion
}