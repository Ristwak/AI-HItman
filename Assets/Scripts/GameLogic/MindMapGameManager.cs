using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

[Serializable]
public class ReverseBotQuestion
{
    public string output;
    public List<string> options;
    public int correctIndex;
}

[Serializable]
public class ReverseBotQuestionList
{
    public List<ReverseBotQuestion> questions;
}

public class MindMapGameManager : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("JSON file placed under StreamingAssets")]
    public string jsonFileName = "MindMapAI.json";
    [Tooltip("Seconds per question")]
    public float questionTime = 10f;
    [Tooltip("Maximum questions per session before returning to MainScene")]
    public int maxQuestions = 5;

    [Header("UI References")]
    public TMP_Text outputText;
    public TMP_Text[] optionTexts;    // e.g. size = 3
    public Button[]  optionButtons;  // e.g. size = 3
    public TMP_Text  timerText;

    [Header("Button Colors")]
    public Color defaultColor = Color.white;
    public Color correctColor = Color.green;
    public Color wrongColor   = Color.red;

    private List<ReverseBotQuestion> allQuestions;
    private int currentQuestionIndex = 0;
    private int selectedOption       = -1;
    private float timer;
    private bool inputAllowed        = true;
    private Coroutine countdownCoroutine;

    private void Start()
    {
        StartCoroutine(LoadQuestions());
    }

    private IEnumerator LoadQuestions()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        string json = string.Empty;

    #if UNITY_ANDROID && !UNITY_EDITOR
        // On Android the StreamingAssets folder is inside the .apk
        using (UnityWebRequest www = UnityWebRequest.Get(filePath))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Failed to load JSON on Android: " + www.error);
                yield break;
            }
            json = www.downloadHandler.text;
        }
    #else
        if (!File.Exists(filePath))
        {
            Debug.LogError("JSON not found: " + filePath);
            yield break;
        }
        json = File.ReadAllText(filePath);
    #endif

        // Parse JSON
        try
        {
            var data = JsonUtility.FromJson<ReverseBotQuestionList>(json);
            allQuestions = data.questions;
            Debug.Log($"✅ Loaded {allQuestions.Count} questions.");
        }
        catch (Exception ex)
        {
            Debug.LogError("JSON parse error: " + ex.Message);
            yield break;
        }

        // If no questions, go back
        if (allQuestions == null || allQuestions.Count == 0)
        {
            SceneManager.LoadScene("MainScene");
            yield break;
        }

        ShuffleQuestions();
        SetupNextRound();
    }

    // Fisher–Yates shuffle
    private void ShuffleQuestions()
    {
        for (int i = 0; i < allQuestions.Count - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, allQuestions.Count);
            var tmp = allQuestions[i];
            allQuestions[i] = allQuestions[rnd];
            allQuestions[rnd] = tmp;
        }
    }

    private void SetupNextRound()
    {
        // If we've shown all or hit the maxQuestions cap, return to MainScene
        if (currentQuestionIndex >= allQuestions.Count ||
            currentQuestionIndex >= maxQuestions)
        {
            SceneManager.LoadScene("MainScene");
            return;
        }

        inputAllowed     = true;
        selectedOption   = -1;
        timer            = questionTime;
        var q            = allQuestions[currentQuestionIndex];
        outputText.text  = q.output;

        // Reset buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            // Reset color
            var img = optionButtons[i].GetComponent<Image>();
            if (img != null) img.color = defaultColor;

            // Set text
            optionTexts[i].text = q.options[i];

            // Wire click
            int index = i;
            optionButtons[i].interactable = true;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }

        // Start timer coroutine
        countdownCoroutine = StartCoroutine(CountdownAndSubmit());
    }

    private void OnOptionSelected(int index)
    {
        if (!inputAllowed) return;

        inputAllowed    = false;
        selectedOption  = index;

        // Disable all buttons
        foreach (var btn in optionButtons)
            btn.interactable = false;

        // Stop timer
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);

        StartCoroutine(DelayedSubmit());
    }

    private IEnumerator CountdownAndSubmit()
    {
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timer).ToString("0");
            yield return null;
        }
        StartCoroutine(DelayedSubmit());
    }

    private IEnumerator DelayedSubmit()
    {
        // Small pause so user sees button color change
        yield return new WaitForSeconds(0.5f);
        SubmitAnswer();
    }

    private void SubmitAnswer()
    {
        var q = allQuestions[currentQuestionIndex];

        // Color the correct one green and the selected wrong one red
        for (int i = 0; i < optionButtons.Length; i++)
        {
            var img = optionButtons[i].GetComponent<Image>();
            if (img == null) continue;

            if (i == q.correctIndex)
                img.color = correctColor;
            else if (i == selectedOption)
                img.color = wrongColor;
        }

        currentQuestionIndex++;
        Invoke(nameof(SetupNextRound), 1.5f);
    }
}
