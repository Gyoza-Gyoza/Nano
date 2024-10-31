using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataLogManager : MonoBehaviour
{
    [SerializeField]
    private DataLog[] dataLogDatabase;

    [SerializeField]
    private float moveDuration;

    [SerializeField]
    private GameObject dataLogScreen;
    [SerializeField]
    private TextMeshProUGUI dataLogTitle, dataLogText;

    [SerializeField]
    private RectTransform closedPos, openedPos;

    private RectTransform pos;

    public static DataLogManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        pos = dataLogScreen.GetComponent<RectTransform>();
    }
    private void Start()
    {
        dataLogScreen.transform.position = closedPos.position;
    }

    public void TriggerDataLog(int dataLogID)
    {
        StartCoroutine(OpenDataLogUI(dataLogID));
    }
    private IEnumerator OpenDataLogUI(int dataLogID)
    {
        float timer = 0f;

        dataLogTitle.text = dataLogDatabase[dataLogID].dataLogTitle;
        dataLogText.text = dataLogDatabase[dataLogID].dataLogText;

        while (timer < moveDuration)
        {
            pos.anchoredPosition = new Vector2(pos.anchoredPosition.x,
                Mathf.SmoothStep(closedPos.anchoredPosition.y,
                openedPos.anchoredPosition.y, timer / moveDuration));

            timer += Time.deltaTime;
            Debug.Log(timer);
            yield return null;
        }
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
                Mathf.SmoothStep(closedPos.anchoredPosition.y,
                openedPos.anchoredPosition.y, timer / moveDuration));

            timer += Time.deltaTime;

            yield return null;
        }
    }
}

[System.Serializable]
public class DataLog
{
    public string dataLogTitle;
    public string dataLogText;
    public string dataLogCode;
}