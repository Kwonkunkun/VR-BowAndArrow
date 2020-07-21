using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Score")]
    public int score = 0;

    [Header("Target Move")]
    public Transform target;
    public Transform easy;
    public Transform middle;
    public Transform hard;
    public int currentLevel = 0;
    public int maxGameLevel = 3;
    public Transform targetTo;
    public bool isMove = false;

    [Header("Level UI")]
    public Image[] levelChecker = new Image[3];

    private void FixedUpdate()
    {
        if (targetTo == null)
            return;

        if(isMove == true)
        {
            //UI채우기
            if(levelChecker[currentLevel].fillAmount != 1.0f)
                levelChecker[currentLevel].fillAmount += 0.2f;

            //타겟 움직이기
            target.position = Vector3.Lerp(target.position, targetTo.position, 0.05f);
            if (Vector3.Distance(target.position, targetTo.position) < 0.1f)
                isMove = false;
        }
    }

    public void GameInit()
    {
        Debug.Log("GameInit");
    }

    public void MoveTarget()
    {
        levelChecker[currentLevel].fillAmount = 0;
        currentLevel++;
        currentLevel /= maxGameLevel;

        if (currentLevel == 0)
            targetTo = easy;
        else if (currentLevel == 1)
            targetTo = middle;
        else if (currentLevel == 2)
            targetTo = hard;

        isMove = true;
    }
}
