using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleText : MonoBehaviour
{
    [SerializeField] private RectTransform battleRectTransform;
    [SerializeField] private Text battleText;

    [Header("Color")] [SerializeField] private Color hurtColor;
    [SerializeField] private Color regenerationColor;

    private void Animation()
    {
        var position = battleRectTransform.anchoredPosition.y;
        battleRectTransform.DOAnchorPosY(position + 100, 0.5f);
        battleText.DOFade(0, 0.1f).SetDelay(0.4f).OnComplete(() => { Destroy(gameObject); });
    }

    public void Hurt(int value)
    {
        battleText.color = hurtColor;
        battleText.text = $"-{value}";
        Animation();
    }

    public void Regeneration(int value)
    {
        battleText.color = regenerationColor;
        battleText.text = $"+{value}";
        Animation();
    }
}