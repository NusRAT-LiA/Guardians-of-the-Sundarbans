using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionManager : MonoBehaviour
{
    public Text QuestionText; // UI Text for the question
    public Button[] AnswerButtons; // Buttons for answers
    public Question[] Questions; // Array of questions
    public AudioSource CorrectSound; // Sound for correct answers
    public AudioSource WrongSound; // Sound for wrong answers
    
    
    private int currentQuestionIndex = 0;

    void Start()
    {
        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (currentQuestionIndex < Questions.Length)
        {
            Question currentQuestion = Questions[currentQuestionIndex];

            QuestionText.text = currentQuestion.QuestionText;

            for (int i = 0; i < AnswerButtons.Length; i++)
            {
                AnswerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.AnswerOptions[i];

                // Add click listener
                int index = i; // Local copy of the loop variable
                AnswerButtons[i].onClick.RemoveAllListeners();
                AnswerButtons[i].onClick.AddListener(() => CheckAnswer(index));
            }
        }
        else
        {
            Debug.Log("All questions answered correctly! Level up!");
            LevelUp();

        }
    }

    void CheckAnswer(int selectedIndex)
    {
        if (selectedIndex == Questions[currentQuestionIndex].CorrectAnswerIndex)
        {
            Debug.Log("Correct!");
            CorrectSound.Play(); // Play correct answer sound
            currentQuestionIndex++;
            LoadQuestion();
        }
        else
        {
            Debug.Log("Incorrect! Try again.");
            WrongSound.Play();
        }
    }

    void LevelUp()
    {
        SceneManager.LoadScene("Level 3"); // Load Level 3 scene
    }
}
