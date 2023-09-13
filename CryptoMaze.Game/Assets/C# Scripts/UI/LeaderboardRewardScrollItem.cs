using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRewardScrollItem : MonoBehaviour
{
    [SerializeField]
    private Image _itemImage;

    [SerializeField]
    private Text _amountText;

    public void SetData(int amount, Sprite icon)
    {
        _itemImage.sprite = icon;
        _amountText.text = amount.ToString();
    }
}
