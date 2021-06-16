using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class X2Action : MonoBehaviour
{
    #region Properties

    [SerializeField] private DiceContainer actionDiceContainer;
    [SerializeField] private Button actionRoll;

    #endregion

    #region Roll Action

    public void Roll()
    {
        if (actionDiceContainer.DiceCount <= 0) return;
        actionRoll.gameObject.SetActive(false);
        actionDiceContainer.X2();
    }

    public void Restart()
    {
        actionDiceContainer.Restart();
        actionRoll.gameObject.SetActive(true);
    }

    public void Over()
    {
        actionRoll.gameObject.SetActive(false);
    }

    #endregion
}