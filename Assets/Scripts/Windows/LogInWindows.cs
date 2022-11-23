using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class LogInWindows : MonoBehaviour, IWindows {
    [SerializeField] private WindowsType _type = WindowsType.login;

    [Header("UI elements")]
    [SerializeField] private Button[] b_toggleButtons = new Button[2];

    [SerializeField] private GameObject mobileBlock;
    [SerializeField] private GameObject emailBlock;

    [SerializeField] private TMP_InputField mobileNumber_inputField;
    [SerializeField] private TMP_Text mobileVisualNumber;
    private string mobileTextFormat = "{0, -1} ({1,-3})-{2,-3}-{3,-2}-{4,-2}";

    [SerializeField] private TMP_InputField email_inputField;
    [SerializeField] private TMP_InputField password_inputField;

    [SerializeField] private Button b_Continue;

    [SerializeField] private TMP_Text t_error;


    private IEnumerator Start() {
        while (!AuthController.Instance)
            yield return new WaitForFixedUpdate();

        AuthController.Instance.InitWindows(this);

        for (int i = 0; i < b_toggleButtons.Length; i++)
            b_toggleButtons[i].onClick.AddListener(() => {
                mobileNumber_inputField.text = "";
                email_inputField.text = "";
            });

        b_Continue.onClick.AddListener(() => {
            b_Continue.interactable = false;
            StartCoroutine(LogIn());
        });


        mobileNumber_inputField.onValueChanged.AddListener(value => {
            if (mobileNumber_inputField.text.Length <= 0) {
                mobileVisualNumber.text = "";
                return;
            }

            if (mobileNumber_inputField.text[0] == '8' || (mobileNumber_inputField.text[0] == '7' ||
            (mobileNumber_inputField.text.Length > 1 && (mobileNumber_inputField.text[0] == '7' && mobileNumber_inputField.text[1] == '9')))) {
                WriteRUNumber();

                if (mobileNumber_inputField.text.Length > 11) // проверка на длину номера, по международному стандарту. максимальное количество цифр в номере телефона не может привышать 18
                    mobileNumber_inputField.text = mobileNumber_inputField.text.Substring(0, 11);
            }
            else
                mobileVisualNumber.text = mobileNumber_inputField.text;
        });

        gameObject.SetActive(false);
    }

    private void WriteRUNumber() {
        string text = (mobileNumber_inputField.text.Length >= 1) ? mobileNumber_inputField.text.Substring(1, mobileNumber_inputField.text.Length - 1) : "";
        string[] number = new string[4];

        for (int i = 0; i < 10; i++) {
            if (i < 3)
                number[0] = number[0] + ((i < text.Length) ? text[i] : '_');
            else
                if (i < 6)
                number[1] = number[1] + ((i < text.Length) ? text[i] : '_');
            else
                if (i < 8)
                number[2] = number[2] + ((i < text.Length) ? text[i] : '_');
            else
                if (i < 10)
                number[3] = number[3] + ((i < text.Length) ? text[i] : '_');
        }

        mobileVisualNumber.text = "+" + System.String.Format(mobileTextFormat, 7, number[0], number[1], number[2], number[3]);
    }

    public void CloseWindows() {
        gameObject.SetActive(false);
    }

    public WindowsType GetWindowType() {
        return _type;
    }

    public void OpenWindow() {
        gameObject.SetActive(true);

        mobileNumber_inputField.text = "";
        email_inputField.text = "";
        t_error.text = "";
        t_error.color = Color.red;

        b_Continue.interactable = true;

        if (PlayerPrefs.HasKey("MobileLogin"))
            mobileNumber_inputField.text = PlayerPrefs.GetString("MobileLogin");

        mobileBlock.SetActive(true);
        emailBlock.SetActive(false);
    }

    private IEnumerator LogIn() {
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            t_error.text = "ќтсутствует подключение к сети";
            yield break;
        }

        if (mobileNumber_inputField.text.Length < 2 && email_inputField.text.Length < 1) {
            t_error.text = "¬ведите ваш номер телефона или email";
            yield return new WaitForFixedUpdate();
            b_Continue.interactable = true;
            yield break;
        }

        string _login = "";
        if (email_inputField.text.Length < 1) {
            _login = mobileNumber_inputField.text;
            if (_login[0] == '8' && _login[1] == '9')
                _login = "7" + _login.Substring(1, _login.Length - 1);
        }
        else if (mobileNumber_inputField.text.Length < 1) {
            if (email_inputField.text.Contains('@')) {
                _login = email_inputField.text;
            }
            else {
                t_error.text = "¬веден некорректный email адрес";
                yield return new WaitForFixedUpdate();
                b_Continue.interactable = true;
                yield break;
            }
        }

        WWWForm form = new WWWForm();
        form.AddField("method", "login");

        form.AddField("login", _login);
        form.AddField("pass", password_inputField.text);

        using (UnityWebRequest www = UnityWebRequest.Post(WebData.UserPath, form))//отправл€ем данные на сервер и получаем ответ с данными пользовател€
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError) {
                Debug.LogError(www.error);
                www.Dispose();
                yield break;
            }

            UserCallResult result = JsonUtility.FromJson<UserCallResult>(www.downloadHandler.text);

            www.Dispose();

            if (result.error.Length < 1) {
                t_error.color = Color.green;
                t_error.text = "—оединение...";
                yield return new WaitForSeconds(.5f);

                if (AuthController.Instance) {
                    AuthController.Instance.SetUserData(result.user);
                    AuthController.Instance.OpenWindows(WindowsType.qr_read);
                }
                yield break;
            }

            yield return new WaitForFixedUpdate();
            t_error.text = result.error[0];
            b_Continue.interactable = true;
        }
    }
}
