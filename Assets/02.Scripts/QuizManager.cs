using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    #region 싱글톤
    private static QuizManager m_instance; // 싱글톤이 할당될 static 변수
    public static QuizManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<QuizManager>();
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
    public int currentCount = 0;
    public int maxCount = 5;

    [Header("Score UI")]
    public Image[] sucessUI = new Image[5];
    public Image[] failUI_1 = new Image[5];
    public Image[] failUI_2 = new Image[5];

    [Header("Answer")]
    public bool[] answer = new bool[5]; 

    private void Start()
    {
        GameInit();
    }

    public void GameInit()
    {
        Debug.Log("GameInit");
        currentCount = 0;
        for (int i = 0; i < maxCount; i++)
        {
            sucessUI[i].fillAmount = 0;
            failUI_1[i].fillAmount = 0;
            failUI_2[i].fillAmount = 0;
        }
    }

    public void UpdateScore(bool rightAndFalse)
    {
        Debug.Log("UpdateScore");
        bool isRight = false;

        //정답
        if (answer[currentCount] == rightAndFalse)
            isRight = true;

        //정답인지 아닌지 확인
        if (isRight == true)
        {
            //정답 소리넣기
            
            StartCoroutine(DrawCircle(currentCount));
        }
        else
        {
            //실패 소리넣기

            StartCoroutine(DrawX_1(currentCount));
        }
        currentCount++;
    }

    IEnumerator DrawCircle(int num)
    {
        sucessUI[num].fillAmount += 0.05f;
        yield return new WaitForSeconds(0.05f);
        if (sucessUI[num].fillAmount != 1)
            StartCoroutine(DrawCircle(num));
    }

    IEnumerator DrawX_1(int num)
    {
        failUI_1[num].fillAmount += 0.05f;
        yield return new WaitForSeconds(0.025f);
        if (failUI_1[num].fillAmount != 1)
            StartCoroutine(DrawX_1(num));
        else if (failUI_1[num].fillAmount == 1)
            StartCoroutine(DrawX_2(num));
    }
    IEnumerator DrawX_2(int num)
    {
        failUI_2[num].fillAmount += 0.05f;
        yield return new WaitForSeconds(0.025f);
        if (failUI_2[num].fillAmount != 1)
            StartCoroutine(DrawX_1(num));
    }
}
