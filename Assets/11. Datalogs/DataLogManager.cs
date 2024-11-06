using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataLogManager : MonoBehaviour
{
    [SerializeField]
    private DataLog[] dataLogDatabase;

    [SerializeField]
    private float moveDuration, typingSpeed;

    [SerializeField]
    private GameObject dataLogInformationScreen, dataLogPasswordScreen;

    [SerializeField]
    private TextMeshProUGUI dataLogTitle, dataLogText;

    [SerializeField]
    private RectTransform closedPos, openedPos;

    [Header("Password Variables")]
    [SerializeField]
    private TMP_InputField[] passwordInput;
    [SerializeField]
    private string passwordTyped;
    private DataLog currentDataLog;
    private RectTransform pos;
    private bool passwordCorrect = false;

    public static DataLogManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        dataLogInformationScreen.transform.position = closedPos.position;
        dataLogPasswordScreen.SetActive(false);
    }
    public void TriggerDataLog(int dataLogID)
    {
        dataLogPasswordScreen.SetActive(true);
        pos = dataLogPasswordScreen.GetComponent<RectTransform>();
        currentDataLog = dataLogDatabase[dataLogID];
        StartCoroutine(OpenDataLogUI());
        passwordInput[0].Select();
    }
    private IEnumerator OpenDataLogUI()
    {
        yield return new WaitUntil(() => passwordCorrect); 

        dataLogPasswordScreen.SetActive(false);

        pos = dataLogInformationScreen.GetComponent<RectTransform>();

        float timer = 0f;

        dataLogTitle.text = currentDataLog.dataLogTitle;

        while (timer < moveDuration)
        {
            pos.anchoredPosition = new Vector2(pos.anchoredPosition.x,
                Mathf.SmoothStep(closedPos.anchoredPosition.y,
                openedPos.anchoredPosition.y, timer / moveDuration));

            timer += Time.deltaTime;

            yield return null;
        }

        StartCoroutine(TypingEffect(dataLogText, currentDataLog.dataLogText));
    }
    public void CloseDataLog()
    {
        StartCoroutine(CloseDataLogUI());
    }
    private IEnumerator CloseDataLogUI()
    {
        float timer = 0f;

        while (timer < moveDuration)
        {
            pos.anchoredPosition = new Vector2(pos.anchoredPosition.x,
                Mathf.SmoothStep(openedPos.anchoredPosition.y,
                closedPos.anchoredPosition.y, timer / moveDuration));

            timer += Time.deltaTime;

            yield return null;
        }
    }
    private IEnumerator TypingEffect(TextMeshProUGUI text, string textToType)
    {
        text.text = "";

        foreach(char c in textToType)
        {
            text.text += c;
            yield return typingSpeed;
        }
    }
    public void Typed(int id)
    {
        if (passwordInput[id].text == "")
        {
            if (id == 0) return;
            id--;
        }
        else
        {
            id++;
            if (id == passwordInput.Length) return;
        }

        passwordInput[id].Select();
    }
    public void SubmitPassword()
    {
        passwordTyped = "";

        for (int i = 0; i < passwordInput.Length; i++)
        {
            passwordTyped += passwordInput[i].text;
        }
        if(passwordTyped == currentDataLog.dataLogPassword) passwordCorrect = true;
    }
}

[System.Serializable]
public class DataLog
{
    public string dataLogPassword;
    public string dataLogTitle;
    [TextArea]
    public string dataLogText;
}