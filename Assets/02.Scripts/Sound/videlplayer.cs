using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class videlplayer : MonoBehaviour
{
    public RawImage mScreen1= null;
    public VideoPlayer mVideoPlayer1 = null;

    public RawImage mScreen2 = null;
    public VideoPlayer mVideoPlayer2 = null;

    public RawImage mScreen3 = null;
    public VideoPlayer mVideoPlayer3 = null;

    void Start()
    {
        if (mScreen1 != null && mVideoPlayer1 != null)
        {
            // 비디오 준비 코루틴 호출
            StartCoroutine(PrepareVideo1());
        }

        if (mScreen2 != null && mVideoPlayer2 != null)
        {
            // 비디오 준비 코루틴 호출
            StartCoroutine(PrepareVideo2());
        }


        if (mScreen3 != null && mVideoPlayer3 != null)
        {
            // 비디오 준비 코루틴 호출
            StartCoroutine(PrepareVideo3());
        }
    }

    protected IEnumerator PrepareVideo1()
    {
        // 비디오 준비
        mVideoPlayer1.Prepare();

        // 비디오가 준비되는 것을 기다림
        while (!mVideoPlayer1.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // VideoPlayer의 출력 texture를 RawImage의 texture로 설정한다
        mScreen1.texture = mVideoPlayer1.texture;
    }
    protected IEnumerator PrepareVideo2()
    {
        // 비디오 준비
        mVideoPlayer1.Prepare();

        // 비디오가 준비되는 것을 기다림
        while (!mVideoPlayer2.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // VideoPlayer의 출력 texture를 RawImage의 texture로 설정한다
        mScreen2.texture = mVideoPlayer2.texture;
    }
    protected IEnumerator PrepareVideo3()
    {
        // 비디오 준비
        mVideoPlayer3.Prepare();

        // 비디오가 준비되는 것을 기다림
        while (!mVideoPlayer3.isPrepared)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // VideoPlayer의 출력 texture를 RawImage의 texture로 설정한다
        mScreen3.texture = mVideoPlayer3.texture;
    }

    public void PlayVideo1()
    {
        if (mVideoPlayer1 != null && mVideoPlayer1.isPrepared)
        {
            // 비디오 재생
            mVideoPlayer1.Play();
        }
    }

    public void StopVideo1()
    {
        if (mVideoPlayer1 != null && mVideoPlayer1.isPrepared)
        {
            // 비디오 멈춤
            mVideoPlayer1.Stop();
        }
    }

    public void PlayVideo2()
    {
        if (mVideoPlayer2 != null && mVideoPlayer2.isPrepared)
        {
            // 비디오 재생
            mVideoPlayer2.Play();
        }
    }

    public void StopVideo2()
    {
        if (mVideoPlayer2 != null && mVideoPlayer2.isPrepared)
        {
            // 비디오 멈춤
            mVideoPlayer2.Stop();
        }
    }

    public void PlayVideo3()
    {
        if (mVideoPlayer3 != null && mVideoPlayer3.isPrepared)
        {
            // 비디오 재생
            mVideoPlayer3.Play();
        }
    }

    public void StopVideo3()
    {
        if (mVideoPlayer3 != null && mVideoPlayer3.isPrepared)
        {
            // 비디오 멈춤
            mVideoPlayer3.Stop();
        }
    }
}
