using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Button _homeButton;

    [SerializeField]
    private Button _shopButton;

    [SerializeField]
    private Button _swapButton;

    [SerializeField]
    private Button _leadersButton;

    [SerializeField]
    private Image _homeMax;

    [SerializeField]
    private Image _shopMax;

    [SerializeField]
    private Image _swapMax;

    [SerializeField]
    private Image _leadersMax;

    [SerializeField]
    private string _homeButtonSceneName;

    [SerializeField]
    private string _shopButtonSceneName;

    [SerializeField]
    private string _swapButtonSceneName;

    [SerializeField]
    private string _leadersButtonSceneName;

    private void Start()
    {
        _homeButton.onClick.AddListener(() => { GoToScene(_homeButtonSceneName); });
        _shopButton.onClick.AddListener(() => { GoToScene(_shopButtonSceneName); });
        _swapButton.onClick.AddListener(() => { GoToScene(_swapButtonSceneName); });
        _leadersButton.onClick.AddListener(() => { GoToScene(_leadersButtonSceneName); });

        _homeMax.gameObject.SetActive(false);
        _shopMax.gameObject.SetActive(false);
        _swapMax.gameObject.SetActive(false);
        _leadersMax.gameObject.SetActive(false);

        if (_homeButtonSceneName == SceneManager.GetActiveScene().name)
        {
            _homeButton.gameObject.SetActive(false);
            _homeMax.gameObject.SetActive(true);
        }
        else if (_shopButtonSceneName == SceneManager.GetActiveScene().name)
        {
            _shopButton.gameObject.SetActive(false);
            _shopMax.gameObject.SetActive(true);
        }
        else if (_swapButtonSceneName == SceneManager.GetActiveScene().name)
        {
            _swapButton.gameObject.SetActive(false);
            _swapMax.gameObject.SetActive(true);
        }
        else if (_leadersButtonSceneName == SceneManager.GetActiveScene().name)
        {
            _leadersButton.gameObject.SetActive(false);
            _leadersMax.gameObject.SetActive(true);
        }
    }

    private void GoToScene(string sceneName)
    {
        _audioSource.Play();

        SceneManager.LoadScene(sceneName);
    }
}
