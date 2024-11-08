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
    private GameObject dataLogScreen;

    [SerializeField]
    private TextMeshProUGUI dataLogTitle, dataLogText;

    [SerializeField]
    private RectTransform closedPos, openedPos;

    private Animator animator;
    private DataLogTrigger currentDataLogTrigger;

    [Header("Password Variables")]
    [SerializeField]
    private TMP_InputField passwordInput;
    [SerializeField]
    private string passwordTyped;
    private DataLog currentDataLog;
    private RectTransform pos;
    private bool passwordCorrect = false, moving = false;

    public static DataLogManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        pos = dataLogScreen.GetComponent<RectTransform>();
        animator = dataLogScreen.GetComponent<Animator>();
    }
    private void Start()
    {
        dataLogScreen.transform.position = closedPos.transform.position;
    }
    public void TriggerDataLog(int dataLogID, DataLogTrigger trigger)
    {
        currentDataLogTrigger = trigger;
        currentDataLog = dataLogDatabase[dataLogID];
        StopAllCoroutines();
        StartCoroutine(OpenDataLogUI());
        passwordInput.ActivateInputField();
    }
    private IEnumerator OpenDataLogUI()
    {
        Move.instance.MovementDisabled = true;

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

        yield return new WaitUntil(() => passwordCorrect);

        animator.SetBool("Open", true);

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(TypingEffect(dataLogTitle, currentDataLog.dataLogTitle));

        StartCoroutine(TypingEffect(dataLogText, currentDataLog.dataLogText));
    }
    public void CloseDataLog()
    {
        if (moving) return;
        moving = true;
        StartCoroutine(CloseDataLogUI());
    }
    private IEnumerator CloseDataLogUI()
    {
        animator.SetBool("Open", false);
        passwordInput.DeactivateInputField();

        yield return new WaitForSeconds(1);

        float timer = 0f;

        while (timer < moveDuration)
        {
            pos.anchoredPosition = new Vector2(pos.anchoredPosition.x,
                Mathf.SmoothStep(openedPos.anchoredPosition.y,
                closedPos.anchoredPosition.y, timer / moveDuration));

            timer += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        if (passwordCorrect)
        {
            UnlockDoor();

            yield return new WaitForSeconds(4f);

            //Resets variables
            passwordCorrect = false;
            currentDataLogTrigger.unlocked = true;
            DialogueManager.dialogueManager.StartDialogue(12 + FinalDoor.instance.DoorsUnlocked);
            passwordInput.text = "";
            dataLogTitle.text = "";
            dataLogText.text = "";
        }
        Move.instance.MovementDisabled = false;
        moving = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) UnlockDoor();
    }
    private void UnlockDoor()
    {
        FinalDoor.instance.DoorsUnlocked++;
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
    public void Typed()
    {
        if (passwordInput.text.Length != 3) return;

        SubmitPassword(passwordInput.text);
    }
    private void SubmitPassword(string password)
    {
        if (password == currentDataLog.dataLogPassword) passwordCorrect = true;
        else
        {
            animator.SetTrigger("Wrong");
            passwordInput.text = "";
        }
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