using CryptoMaze.ClientServer.Game.DataContainers;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopScrollItem : MonoBehaviour
{
    public Action<int> OnBuy;

    [SerializeField]
    private Image _buyImage;

    [SerializeField]
    private Text _buyAmount;

    [SerializeField]
    private Text _buyName;

    [SerializeField]
    private Image _sellImage;

    [SerializeField]
    private Text _sellAmount;

    [SerializeField]
    private Text _sellName;

    [SerializeField]
    private Button _buyButton;

    public ShopProposalModel Proposal { get; private set; }

    public void SetProposal(ShopProposalModel proposal, Sprite buySprite, Sprite sellSprite)
    {
        Proposal = proposal;

        _buyImage.sprite = buySprite;
        _buyAmount.text = $"+{proposal.buyAmount}";
        _buyName.text = proposal.BuyType.GetUIName();

        _sellImage.sprite = sellSprite;
        _sellAmount.text = $"-{proposal.sellAmount}";
        _sellName.text = proposal.SellType.GetUIName();
    }

    private void Start()
    {
        _buyButton.onClick.AddListener(() => { OnBuy?.Invoke(Proposal.proposalId); });
    }
}
