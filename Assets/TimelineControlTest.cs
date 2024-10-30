using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using GGG.Tool.Singleton;
public class TimelineControlTest : Singleton<TimelineControlTest>
{

    public PlayableDirector playableDirector; // 将包含Timeline的PlayableDirector分配给这个字段


    private void OnEnable()
    {
        EventManager.MainInstance.AddEventListening<Vector3, Quaternion, Animator>("SetTimelineCharacterPosition", SetCharacterPosition);
    }
    private void OnDisable()
    {
        EventManager.MainInstance.RemoveEvent<Vector3, Quaternion, Animator>("SetTimelineCharacterPosition", SetCharacterPosition);
    }
    private void Start()
    {


        
    }
    public void PlayTimeline()
    {
        playableDirector.Play();
    }

    public void SetCharacterPosition(Vector3 position,Quaternion eulerAngles, Animator self)
    {
        var track = FindSelfAnimationTrack(self);
        if (track != null)
        {
            AnimationPlayableAsset playableAsset = GetOneClip(track);
            playableAsset.position = position;
            playableAsset.rotation = eulerAngles ;
        }
    }

    public AnimationTrack FindSelfAnimationTrack(Animator anim)
    {
        var timelineAsset = playableDirector.playableAsset as TimelineAsset;
        foreach (var track in timelineAsset.GetOutputTracks())
        {
            if (track is AnimationTrack animationTrack)
            {
                foreach (var blind in animationTrack.outputs)
                {
                    if(anim == playableDirector.GetGenericBinding(blind.sourceObject) as Animator)
                    {
                        return animationTrack;
                    }
                    break;
                }
            }
            break;
        }
        return null;
    }

    public AnimationPlayableAsset GetOneClip(AnimationTrack animationTrack)
    {
        foreach (var clip in animationTrack.GetClips())
        {
            var animationPlayableAsset = clip.asset as AnimationPlayableAsset;
            return animationPlayableAsset;
        }
        return null;
    }
}
