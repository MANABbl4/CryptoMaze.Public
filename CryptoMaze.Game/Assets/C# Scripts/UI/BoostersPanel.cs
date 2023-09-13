using UnityEngine;
using UnityEngine.UI;

public class BoostersPanel : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Button _freezeTimeButton;

    [SerializeField]
    private Button _speedRocketButton;

    [SerializeField]
    private Button _cryptoKeysButton;

    [SerializeField]
    private Button _cryptoKeyFragmentsButton;

    [SerializeField]
    private Text _freezeTimeBoosters;

    [SerializeField]
    private Text _speedRocketBoosters;

    [SerializeField]
    private Text _cryptoKeys;

    [SerializeField]
    private Text _cryptoKeyFragments;

    [SerializeField]
    private Dialog _freezeTimeDialog;

    [SerializeField]
    private Dialog _speedRocketDialog;

    [SerializeField]
    private Dialog _cryptoKeyDialog;

    [SerializeField]
    private Dialog _cryptoKeyFragmentDialog;

    [SerializeField]
    private Vector3 _translateLeft;

    [SerializeField]
    private Vector3 _translateRight;

    public void SetFreezeTimeBoosters(int amount)
    {
        _freezeTimeBoosters.text = amount.ToString();
    }

    public void SetSpeedRocketBoosters(int amount)
    {
        _speedRocketBoosters.text = amount.ToString();
    }

    public void SetCryptoKeys(int amount)
    {
        _cryptoKeys.text = amount.ToString();
    }

    public void SetCryptoKeyFragments(int amount)
    {
        _cryptoKeyFragments.text = amount.ToString();
    }

    private void Start()
    {
        _freezeTimeButton.onClick.AddListener(() => { OnShowDialog(_freezeTimeDialog, _freezeTimeButton, _translateLeft); });
        _speedRocketButton.onClick.AddListener(() => { OnShowDialog(_speedRocketDialog, _speedRocketButton, _translateLeft); });
        _cryptoKeysButton.onClick.AddListener(() => { OnShowDialog(_cryptoKeyDialog, _cryptoKeysButton, _translateRight); });
        _cryptoKeyFragmentsButton.onClick.AddListener(() => { OnShowDialog(_cryptoKeyFragmentDialog, _cryptoKeyFragmentsButton, _translateRight); });
    }

    private void OnShowDialog(Dialog dialog, Button translateButton, Vector3 translate)
    {
        _audioSource.Play();

        translateButton.transform.Translate(translate);

        dialog.gameObject.SetActive(true);
        dialog.OnDialogAction += OnDialogAction;
    }

    private void OnDialogAction(Dialog dialog)
    {
        dialog.OnDialogAction -= OnDialogAction;
        dialog.gameObject.SetActive(false);
    }
}
