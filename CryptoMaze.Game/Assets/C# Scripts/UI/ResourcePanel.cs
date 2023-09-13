using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour
{
    [SerializeField]
    private Text _tonBalance;

    [SerializeField]
    private Text _blcBalance;

    [SerializeField]
    private Text _btcBlocks;

    [SerializeField]
    private Text _ethBlocks;

    [SerializeField]
    private Text _tonBlocks;

    [SerializeField]
    private Text _energy;

    public void SetTonBalance(int amount)
    {
        _tonBalance.text = amount.ToString();
    }

    public void SetBlcBalance(int amount)
    {
        _blcBalance.text = amount.ToString();
    }

    public void SetBtcBlocks(int amount)
    {
        _btcBlocks.text = amount.ToString();
    }

    public void SetEthBlocks(int amount)
    {
        _ethBlocks.text = amount.ToString();
    }

    public void SetTonBlocks(int amount)
    {
        _tonBlocks.text = amount.ToString();
    }

    public void SetEnergy(int amount)
    {
        _energy.text = amount.ToString();
    }
}
