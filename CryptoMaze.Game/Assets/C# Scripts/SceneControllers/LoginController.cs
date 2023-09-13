using CryptoMaze.Client;
using UnityEngine;
using UnityEngine.UI;
using CryptoMaze.ClientServer.Authentication.Requests;
using System;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LoginController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _emailBackground;

        [SerializeField]
        private InputField _email;

        [SerializeField]
        private GameObject _codeBackground;

        [SerializeField]
        private InputField _code;

        [SerializeField]
        private Text _timer;

        [SerializeField]
        private Text _emailSent;

        [SerializeField]
        private Text _invalidEmail;

        [SerializeField]
        private Text _enterCode;

        [SerializeField]
        private Button _sendCode;

        [SerializeField]
        private Button _pasteCode;

        [SerializeField]
        private Button _login;

        [SerializeField]
        private Button _cancel;

        [SerializeField]
        private Text _invalidCode;

        private CryptoMazeClient _cryptoMazeClient;
        private float _timeLeftFloat = 0f;
        private int _timeLeft = 0;

        // Start is called before the first frame update
        void Start()
        {
            var accessTokenExpirationDate = PlayerPrefsExt.GetDateTime("accessTokenExpirationDate");
            var refreshTokenExpirationDate = PlayerPrefsExt.GetDateTime("refreshTokenExpirationDate");
            var accessToken = PlayerPrefs.GetString("accessToken");
            var refreshToken = PlayerPrefs.GetString("refreshToken");

            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken) &&
                ((accessTokenExpirationDate.HasValue && accessTokenExpirationDate > DateTime.UtcNow) ||
                (refreshTokenExpirationDate.HasValue && refreshTokenExpirationDate > DateTime.UtcNow)))
            {
                SceneManager.LoadScene(1);
            }

            // can be singleton
            _cryptoMazeClient = new CryptoMazeClient(
                ServerUrlUtil.IdentityServerUrl,
                ServerUrlUtil.GameServerUrl,
                new CryptoMazeClientOptions(string.Empty, string.Empty, null, null));

            _email.gameObject.SetActive(true);
            _emailBackground.SetActive(true);
            _sendCode.gameObject.SetActive(true);

            _emailSent.gameObject.SetActive(false);
            _invalidEmail.gameObject.SetActive(false);
            _codeBackground.SetActive(false);
            _code.gameObject.SetActive(false);
            _timer.gameObject.SetActive(false);
            _enterCode.gameObject.SetActive(false);
            _pasteCode.gameObject.SetActive(false);
            _login.gameObject.SetActive(false);
            _cancel.gameObject.SetActive(false);
            _invalidCode.gameObject.SetActive(false);

            _sendCode.onClick.AddListener(OnSendCode);
            _pasteCode.onClick.AddListener(OnPasteCode);
            _login.onClick.AddListener(OnLogin);
            _cancel.onClick.AddListener(OnCancel);
        }

        // Update is called once per frame
        void Update()
        {
            if (_timeLeftFloat > 0f)
            {
                var prevTimeLift = _timeLeft;

                _timeLeftFloat -= Time.deltaTime;
                _timeLeft = (int)_timeLeftFloat;

                if (_timeLeft < prevTimeLift)
                {
                    _timer.text = $"{_timeLeft} s";
                }

                if (_timeLeft <= 0)
                {
                    _timer.gameObject.SetActive(false);
                    _sendCode.gameObject.SetActive(true);
                }
            }
        }

        private void OnSendCode()
        {
            _cryptoMazeClient.Identity
                .SendCode(new SendCodeRequest() { email = _email.text })
                .Then(result =>
                {
                    _invalidEmail.gameObject.SetActive(!result.success);
                    _invalidEmail.text = result.message;

                    if (result.success)
                    {
                        _sendCode.gameObject.SetActive(false);

                        _emailSent.gameObject.SetActive(true);
                        _timer.gameObject.SetActive(true);
                        _pasteCode.gameObject.SetActive(true);
                        _login.gameObject.SetActive(true);
                        _codeBackground.SetActive(true);
                        _code.gameObject.SetActive(true);
                        _enterCode.gameObject.SetActive(true);
                        _cancel.gameObject.SetActive(true);

                        _timeLeftFloat = result.nextRequestDelaySeconds;
                        _timeLeft = result.nextRequestDelaySeconds;

                        _timer.text = $"{_timeLeft} s";
                    }
                    else
                    {
                        _timeLeftFloat = result.nextRequestDelaySeconds;
                        _timeLeft = result.nextRequestDelaySeconds;

                        _timer.gameObject.SetActive(result.nextRequestDelaySeconds > 0);
                        _sendCode.gameObject.SetActive(result.nextRequestDelaySeconds < 1);
                    }
                })
                .Catch(ex =>
                {
                    Debug.LogError(ex.ToString());
                });
        }

#if UNITY_WEBGL && !UNITY_EDITOR
public void PasteFromClipboard()
{
    Application.ExternalEval(
        @"navigator.clipboard.readText()
        .then((text) => {
            SendMessage('LoginController', 'OnPaste', text);
        })
        .catch((error) => {
            console.error('Paste error:', error);
        });"
    );
}

public void OnPaste(string text)
{
    _code.text = text;
}
#endif

        private void OnPasteCode()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
    PasteFromClipboard();
#else
            TextEditor editor = new TextEditor();
            editor.Paste();
            _code.text = editor.text;
#endif
        }

        private void OnLogin()
        {
            _cryptoMazeClient.Identity
                .Login(new LoginRequest() { email = _email.text, code = _code.text })
                .Then(result =>
                {
                    _invalidCode.gameObject.SetActive(!result.authorized);

                    if (result.authorized)
                    {
                        PlayerPrefs.SetString("accessToken", result.accessToken);
                        PlayerPrefs.SetString("refreshToken", result.refreshToken);
                        PlayerPrefsExt.SetDateTime("accessTokenExpirationDate", _cryptoMazeClient.Identity.Options.AccessTokenExpirationDate.Value);
                        PlayerPrefsExt.SetDateTime("refreshTokenExpirationDate", _cryptoMazeClient.Identity.Options.RefreshTokenExpirationDate.Value);

                        SceneManager.LoadScene(1);
                    }
                })
                .Catch(ex =>
                {
                    Debug.LogError(ex.ToString());
                });
        }

        private void OnCancel()
        {
            _email.gameObject.SetActive(true);
            _emailBackground.SetActive(true);
            _sendCode.gameObject.SetActive(true);

            _emailSent.gameObject.SetActive(false);
            _invalidEmail.gameObject.SetActive(false);
            _codeBackground.SetActive(false);
            _code.gameObject.SetActive(false);
            _timer.gameObject.SetActive(false);
            _enterCode.gameObject.SetActive(false);
            _pasteCode.gameObject.SetActive(false);
            _login.gameObject.SetActive(false);
            _cancel.gameObject.SetActive(false);
            _invalidCode.gameObject.SetActive(false);
        }
    }
}
