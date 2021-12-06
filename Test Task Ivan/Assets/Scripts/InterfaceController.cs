using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/* THIS SCRIPT CONTROLS INTERFACE
 */
public class InterfaceController : MonoBehaviour
{
    GameController GC;
    PlayerController PC;
    Camera cam;
    [SerializeField] RectTransform ClosestEnemyRect, ClosestGemRect, PointersAllowedArea; //MOVE AND MODIFY OBJECT PointersAllowedArea TO MAKE CONSTRAINTS FOR 
                                                                                          //CLOSEST GEM AND CLOSEST ENEMY POINTERS
    [SerializeField] GameObject StartCanvas, GameCanvas, EndCanvas;
    [SerializeField] GameObject[] HeartsImages;                                                 
    [SerializeField] Text PointsText, HighScoreText, EndScreenScoreText, EndScreenHighscoreText;
    [SerializeField] Text EnemyDistText, GemDistText;
    [SerializeField] Text EnemiesCountText, GemCountText;
    private void Awake()
    {
        GC = FindObjectOfType<GameController>();
        PC = FindObjectOfType<PlayerController>();
        cam = Camera.main;
        HighScoreText.text = GC.HighScore.ToString();
    }
    void Update()
    {
        if (GC.GameStarted)
        {
            DisplayClosest(GC.ClosestEnemyObject, ClosestEnemyRect, EnemyDistText);
            DisplayClosest(GC.ClosestGemObject, ClosestGemRect, GemDistText);
            PointsText.text = GC.CurrentScore.ToString();
            EnemiesCountText.text = GC.AllEnemies.Count + " / " + GC.MaxEnemies;
            GemCountText.text = GC.AllGems.Count + " / " + GC.MaxGems;
        }
    }

    void DisplayClosest(GameObject evaluatedObject, RectTransform UIElementRect, Text distanceText)
    {
        if (evaluatedObject == null)
        {
            return;
        }
        var pointerPos = cam.WorldToScreenPoint(evaluatedObject.transform.position);
        if (pointerPos.z < 0)
        {
            pointerPos *= -1;
        }
        float halfWidth = UIElementRect.rect.width / 2;
        float halfHeight = UIElementRect.rect.height / 2;
        //IF THERE IS NEED TO CONSTRAINT POINTERS, RESIZE "PointersAllowedArea" OBJECT
        float upperBoundary = PointersAllowedArea.position.y + PointersAllowedArea.rect.height / 2;
        float lowerBoundary = PointersAllowedArea.position.y - PointersAllowedArea.rect.height / 2;
        pointerPos.x = Mathf.Clamp(pointerPos.x, 0 + halfWidth, Screen.width - halfWidth);
        pointerPos.y = Mathf.Clamp(pointerPos.y, lowerBoundary + halfHeight, upperBoundary - halfHeight);
        UIElementRect.position = Vector3.Lerp(UIElementRect.position, pointerPos, 0.3f);

        var dist = Vector3.Distance(evaluatedObject.transform.position, PC.transform.position);
        distanceText.text = System.Math.Round(dist, 1).ToString();
    }
    
    public void UpdateHearts(int HP)
    {
        foreach(GameObject g in HeartsImages)
        {
            g.SetActive(false);
        }
        for(int i = 0; i < HP; i++)
        {
            if (i < HeartsImages.Length)
            {
                HeartsImages[i].SetActive(true);
            }
        }
    }
    public void StartGameButtonMethod()
    {
        StartCanvas.SetActive(false);
        GameCanvas.SetActive(true);
        GC.StartGameMethod();
    }
    public void EndGameInterfaceMethod()
    {
        GameCanvas.SetActive(false);
        EndCanvas.SetActive(true);
        EndScreenHighscoreText.text = GC.HighScore.ToString();
        EndScreenScoreText.text = GC.CurrentScore.ToString();
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
}
