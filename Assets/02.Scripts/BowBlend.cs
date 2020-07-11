using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BowBlend : MonoBehaviour
{
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
}
