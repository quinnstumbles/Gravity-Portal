using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{

    [Header("Parameters")]
    [Tooltip("Duration of the fade-to-black at the end of the game")]
    public float endSceneLoadDelay = 3f;
    [Tooltip("The canvas group of the fade-to-black screen")]
    public CanvasGroup endGameFadeCanvasGroup;

    [Header("Win")]
    [Tooltip("This string has to be the name of the scene you want to load when winning")]
    public string winSceneName = "WinScene";
    [Tooltip("Duration of delay before the fade-to-black, if winning")]
    public float delayBeforeFadeToBlack = 4f;
    [Tooltip("Duration of delay before the win message")]
    public float delayBeforeWinMessage = 2f;
    [Tooltip("Sound played on win")]
    public AudioClip victorySound;
    [Tooltip("Prefab for the win game message")]
    public GameObject WinGameMessagePrefab;

    [Header("Lose")]
    [Tooltip("This string has to be the name of the scene you want to load when losing")]
    public string loseSceneName = "LoseScene";

    [Header("Set in Inspector")]
    public int moreStages;

    public bool gameIsEnding { get; private set; }


    PlayerCharacterController m_Player;
    NotificationHUDManager m_NotificationHUDManager;
    ObjectiveManager m_ObjectiveManager;
    float m_TimeLoadEndGameScene;
    string m_SceneToLoad;

    void Start()
    {
        m_Player = FindObjectOfType<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, GameFlowManager>(m_Player, this);

        m_ObjectiveManager = FindObjectOfType<ObjectiveManager>();
		DebugUtility.HandleErrorIfNullFindObject<ObjectiveManager, GameFlowManager>(m_ObjectiveManager, this);

        AudioUtility.SetMasterVolume(1);

    }

    void Update()
    {
        // Test if player died
        if (m_Player.isDead)
        {
            EndGame(false);
        }


        if (m_ObjectiveManager.AreAllObjectivesCompleted() && moreStages != 0)
        { SceneManager.LoadScene("SecondaryScene"); }

        else if (m_ObjectiveManager.AreAllObjectivesCompleted() && moreStages == 0)
        {
            float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / endSceneLoadDelay;
            endGameFadeCanvasGroup.alpha = timeRatio;

            AudioUtility.SetMasterVolume(1 - timeRatio);
            EndGame(true);
        }



    }

    void EndGame(bool win)
    {
        // unlocks the cursor before leaving the scene, to be able to click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Remember that we need to load the appropriate end scene after a delay
        gameIsEnding = true;
        endGameFadeCanvasGroup.gameObject.SetActive(true);
        if (win)
        {
            m_TimeLoadEndGameScene = Time.time + endSceneLoadDelay + delayBeforeFadeToBlack;

            // play a sound on win
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = victorySound;
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
            audioSource.PlayScheduled(AudioSettings.dspTime + delayBeforeWinMessage);

            SceneManager.LoadScene(winSceneName);

            // create a game message
            var message = Instantiate(WinGameMessagePrefab).GetComponent<DisplayMessage>();
            if (message)
            {
                message.delayBeforeShowing = delayBeforeWinMessage;
                message.GetComponent<Transform>().SetAsLastSibling();
            }
        }
        else
        {
            SceneManager.LoadScene(loseSceneName);
            m_TimeLoadEndGameScene = Time.time + endSceneLoadDelay;
        }
    }
}
