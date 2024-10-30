using GGG.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolItemSound : PoolItemBase
{

    AudioSource _audioSource;
    public SoundType _type;
    public SoundAssets _soundAssets;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public override void Spawn()
    {
        PlayerSound();
    }

    private void PlayerSound()
    {
        _audioSource.clip = _soundAssets.TryGetOneClip(_type);
        _audioSource.Play();
        TimerManager.MainInstance.TryGetOneTimer(4f,StopPlay);
    }


    private void StopPlay()
    {
        _audioSource.Stop();
        _audioSource.gameObject.SetActive(false);
    }


}
