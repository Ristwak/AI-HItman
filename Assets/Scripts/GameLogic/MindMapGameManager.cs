using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
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
    public string jsonFileName = "MindMapAI.json";
    public float questionTime = 10f;

    [Header("UI References")]
    public TMP_Text outputText;
    public TMP_Text[] optionTexts;    // Size = 3
    public Button[] optionButtons;    // Size = 3
    public TMP_Text timerText;

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
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        if (!File.Exists(path))
        {
            Debug.LogError("JSON not found: " + path);
            yield break;
        }

        string json = File.ReadAllText(path);
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

        if (allQuestions == null || allQuestions.Count == 0)
        {
            SceneManager.LoadScene("MainScene");
            yield break;
        }

        ShuffleQuestions();      // ←— randomize order each run
        SetupNextRound();
    }

    // Fisher–Yates shuffle
    private void ShuffleQuestions()
    {
        for (int i = 0; i < allQuestions.Count - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, allQuestions.Count);
            var temp = allQuestions[i];
            allQuestions[i] = allQuestions[rnd];
            allQuestions[rnd] = temp;
        }
    }

    private void SetupNextRound()
    {
        if (currentQuestionIndex >= allQuestions.Count)
        {
            EndGame();
            return;
        }

        inputAllowed     = true;
        selectedOption   = -1;
        timer            = questionTime;
        var q            = allQuestions[currentQuestionIndex];
        outputText.text  = q.output;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            var img = optionButtons[i].GetComponent<Image>();
            if (img != null) img.color = defaultColor;

            optionTexts[i].text = q.options[i];

            int index = i;
            optionButtons[i].interactable = true;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }

        countdownCoroutine = StartCoroutine(CountdownAndSubmit());
    }

    private void OnOptionSelected(int index)
    {
        if (!inputAllowed) return;
        inputAllowed    = false;
        selectedOption  = index;
        foreach (var btn in optionButtons) btn.interactable = false;
        if (countdownCoroutine != null) StopCoroutine(countdownCoroutine);
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
        yield return new WaitForSeconds(0.5f);
        SubmitAnswer();
    }

    private void SubmitAnswer()
    {
        var q = allQuestions[currentQuestionIndex];
        for (int i = 0; i < optionButtons.Length; i++)
        {
            var img = optionButtons[i].GetComponent<Image>();
            if (img == null) continue;
            if (i == q.correctIndex)       img.color = correctColor;
            else if (i == selectedOption)  img.color = wrongColor;
        }

        currentQuestionIndex++;
        Invoke(nameof(SetupNextRound), 1.5f);
    }

    private void EndGame()
    {
        outputText.text = "खेल समाप्त!";
        timerText.text  = "";
        foreach (var btn in optionButtons)
            btn.gameObject.SetActive(false);
    }
}
