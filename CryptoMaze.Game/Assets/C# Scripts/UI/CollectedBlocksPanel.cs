using UnityEngine;
using UnityEngine.UI;

public class CollectedBlocksPanel : MonoBehaviour
{
    [SerializeField]
    private Text _btcBlocks;

    [SerializeField]
    private Text _ethBlocks;

    [SerializeField]
    private Text _tonBlocks;

    public int BtcBlocksAmount { get; private set; } = 0;
    public int EthBlocksAmount { get; private set; } = 0;
    public int TonBlocksAmount { get; private set; } = 0;

    public void AddBtcBlocks(int amount)
    {
        BtcBlocksAmount += amount;
        _btcBlocks.text= BtcBlocksAmount.ToString();
    }

    public void AddEthBlocks(int amount)
    {
        EthBlocksAmount += amount;
        _ethBlocks.text = EthBlocksAmount.ToString();
    }

    public void AddTonBlocks(int amount)
    {
        TonBlocksAmount += amount;
        _tonBlocks.text = TonBlocksAmount.ToString();
    }

    public void SetBtcBlocks(int amount)
    {
        BtcBlocksAmount = amount;
        _btcBlocks.text = BtcBlocksAmount.ToString();
    }

    public void SetEthBlocks(int amount)
    {
        EthBlocksAmount = amount;
        _ethBlocks.text = EthBlocksAmount.ToString();
    }

    public void SetTonBlocks(int amount)
    {
        TonBlocksAmount = amount;
        _tonBlocks.text = TonBlocksAmount.ToString();
    }
}
