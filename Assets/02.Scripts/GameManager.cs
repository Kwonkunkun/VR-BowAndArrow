using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

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

    public Transform target;
    public Transform easy;
    public Transform middle;
    public Transform hard;
    public string currentGameLevel = "Easy";
    public Transform targetTo;
    public bool isMove = false;

    private void FixedUpdate()
    {
        if (targetTo == null)
            return;

        if(isMove == true)
        {
            target.position = Vector3.Lerp(target.position, targetTo.position, 0.5f);
            if (Vector3.Distance(target.position, targetTo.position) < 2.0f)
                isMove = false;
        }
    }

    public void MoveTarget(string where)
    {
        //움직일 필요가 없을때
        if (currentGameLevel == where)
            return;

        if (where == "Easy")
            targetTo = easy;
        else if (where == "Middle")
            targetTo = middle;
        else if (where == "Hard")
            targetTo = hard;

        currentGameLevel = where;
        isMove = true;
    }
}
