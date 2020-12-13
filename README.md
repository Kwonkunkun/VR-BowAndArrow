# 온고지신 - 국궁 (VR-BowAndArrow)


## **프로젝트 개요 : VR을 이용한 교육적 목적의 국궁 체험 시뮬레이션 콘텐츠**
### 특징
1. 활의 모델링, 리깅, 활쏘기 까지 직접구현
2. 활쏘기를 체험하는 씬과 활 관련 내용을 학습하는 씬을 따로 만들어 교육적인 내용 추가
3. 손의 자연스러운 애니메이션 구현
4. steamVR의 playerPrefab을 사용하지 않고 직접 잡기, 놓기 등의 인터렉션 구현


### 씬 소개
<img width="1019" alt="image" src="https://user-images.githubusercontent.com/59603575/102003822-a9325400-3cbf-11eb-8367-af01291f9b33.png">


### 기획 의도
![image](https://user-images.githubusercontent.com/59603575/102003835-d252e480-3cbf-11eb-8475-440a4bb72e96.png)

- 잊혀져가는 전통문화 ‘국궁’을 가장 현대적인 기술인 VR을 통해서 더 이상 잊혀가는 전통이 아닌 재미있는 놀이로 다시 우리의 곁으로 돌아오길 바라는 마음으로 기획


### UML
<img width="907" alt="image" src="https://user-images.githubusercontent.com/59603575/102003859-00d0bf80-3cc0-11eb-8549-0574d85cf9ce.png">


## **개발 내용**

### 자연스러운 손 움직임 구현 

<img width="408" alt="image" src="https://user-images.githubusercontent.com/59603575/102004148-febc3000-3cc2-11eb-9f05-f8b8e6e22a16.png">

```c#
//ArrowBlend, BowBlend script 중..
protected float blendToPoseTime = 0.1f;
protected float releasePoseBlendTime = 0.2f;
SteamVR_Skeleton_Poser skeletonPoser;
void Start()
{
    skeletonPoser = GetComponent<SteamVR_Skeleton_Poser>();
}
public void OnGripPose(SteamVR_Behaviour_Skeleton skeleton)
{
    if (skeletonPoser != null && skeleton != null)
    {
        Debug.Log("On Pose");
        skeleton.BlendToPoser(skeletonPoser, blendToPoseTime);
    }
}
public void OffGripPose(SteamVR_Behaviour_Skeleton skeleton)
{
    if (skeletonPoser != null)
    {
        if (skeleton != null)
        {
            Debug.Log("Off Pose");
            skeleton.BlendToSkeleton(releasePoseBlendTime);
        }
    }
}
```
**기능**
- 활, 화살 등의 오브젝트를 잡을 때 미리 설정해 뒀던 포즈를 호출하는 스크립트

**특징**
- Steam VR에서 Skeleton poser의 일부를 수정하여 사용
- 특정 오브젝트를 잡았을 때 미리 저장해놓은 pose를  호출

### 활쏘기

<img width="873" alt="image" src="https://user-images.githubusercontent.com/59603575/102004165-172c4a80-3cc3-11eb-99ac-d2a22849db48.png">

#### 1. 화살 생성

```c#
//Bow script 중...
public void CreateArrow(string whatHand)
{
    //create child
    GameObject arrowObject = Instantiate(m_ArrowPrefab, m_Socket);

    //orient
    arrowObject.transform.localScale /= 6;
    arrowObject.transform.localPosition = new Vector3(0, 0, 0);
    arrowObject.transform.localEulerAngles = Vector3.zero;

    if (whatHand == "Left")
        arrowObject.transform.LookAt(rightArrowSetPos);
    else if (whatHand == "Right")
        arrowObject.transform.LookAt(leftArrowSetPos);

    whatIsHand = whatHand;

    //set
    m_CurrentArrow = arrowObject.GetComponent<Arrow>();

    ObjStatus objStatus = arrowObject.GetComponent<ObjStatus>();
    objStatus.isGrip = true;
}
```

- 활을 쏘는 과정 중 첫번째 단계
- 활시위의 Collider를 인식해 화살을 거는 기능
- 지정된 위치에 화살이 생겨나는 방식으로 사용자가 활을 거는 과정을 묘사

#### 2. 활시위 당기기, 활시위 당긴 정도 계산
```c#
//Bow script 중...
private float CalculaterPull(Transform pullHand)
{
    Vector3 direction = m_End.position - m_Start.position;
    float magnitude = direction.magnitude;

    direction.Normalize();
    Vector3 diffrence = pullHand.position - m_Start.position;

    return Vector3.Dot(diffrence, direction) / magnitude;
}
 public void Pull(Transform hand)
{
    Debug.Log("Pull");
    float distance = Vector3.Distance(hand.position, m_Start.position);

    if (distance >= m_GrabThreshold)
        return;
    PullBackSound.Post(gameObject);
    m_PullingHand = hand;
}
```
- 활 시위를 당기는 함수.
- 활을 들고 있는 손과 화살을 들고 있는 손의 거리값을 계산.
- 당겨진 값의 거리를 return하여 실수 값으로 저장하는 함수.
- 시위를 당기는 생생한 사운드까지 함께 실행

#### 3. 활시위 놓기
```c#
//Bow script 중...
public void Release()
{
    Debug.Log("Release");

    if (m_PulValue > 0.25f)
    {
        if(m_CurrentArrow != null)
            FireArrow();
        RealeseSound.Post(gameObject);
    }

    m_PullingHand = null;
    m_PulValue = 0.0f;
    m_Animator.SetFloat("Blend", 0);
    //pose
    steamVR_Skeleton_Poser.SetBlendingBehaviourValue("ShotPose", m_PulValue);
}
private void FireArrow()
{
    InAirSound.Post(gameObject);
    m_CurrentArrow.Fire(m_PulValue);
    m_CurrentArrow = null;
}
```
- 활시위를 놓았을 때, 화살이 잘 걸려 있다면 활이 쏘아지는 함수.
- 특정 거리 이상으로 활 시위를 당기지 않는 경우 화살이 날아가지 않는다.

### 잡기, 놓기 인터렉션

<img width="498" alt="image" src="https://user-images.githubusercontent.com/59603575/102004127-ca487400-3cc2-11eb-99de-6ec2c51e7385.png">

**기능**
- VR컨트롤러로 게임상 활잡기, 화살잡기, 여러 물체 잡기 등등 기능을 전체적으로 담당하는 스크립트

**특징**
- Approach와 Grip확인 변수를 따로둬서 Approach된 상태의 오브젝트만 잡을수 있도록 함
- 자세한 내용 설명 URL : https://kunkunwoo.tistory.com/167

## 개발 기간, 맡은 역할, 사용 프로그램

개발 기간 : 약 3주 ~ 4주

맡은 역할 : Player 구현, 활쏘기, ox퀴즈, 잡기, 놓기 등의 VR관련 기능 구현 

<img width="771" alt="image" src="https://user-images.githubusercontent.com/59603575/102004221-9b7ecd80-3cc3-11eb-9aca-93c65286cbc9.png">
