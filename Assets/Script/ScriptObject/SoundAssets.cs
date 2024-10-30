using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SoundType
{
    ATK,
    HIT,
    BLOCK,
    FOOT,
    Break,
    Finality,
}


[CreateAssetMenu(fileName = "SoundAssets", menuName = "SO/SoundAssets")]
public class SoundAssets : ScriptableObject
{
    [System.Serializable]
    private class SoundConfig
    {
        public SoundType SoundType;
        public AudioClip[] _audioClips;
    }

    [SerializeField] private List<SoundConfig> _soundAssets = new List<SoundConfig>();


    public AudioClip TryGetOneClip(SoundType type)
    {
        if (_soundAssets.Count <= 0) return null;
        switch (type)
        {
            case SoundType.ATK:return _soundAssets[0]._audioClips[Random.Range(0, _soundAssets[0]._audioClips.Length)];
            case SoundType.HIT:return _soundAssets[1]._audioClips[Random.Range(0, _soundAssets[1]._audioClips.Length)];
            case SoundType.BLOCK:return _soundAssets[2]._audioClips[Random.Range(0, _soundAssets[2]._audioClips.Length)];
            case SoundType.FOOT:return _soundAssets[3]._audioClips[Random.Range(0, _soundAssets[3]._audioClips.Length)];
            case SoundType.Break:return _soundAssets[4]._audioClips[Random.Range(0, _soundAssets[4]._audioClips.Length)];
            case SoundType.Finality:return _soundAssets[5]._audioClips[Random.Range(0, _soundAssets[5]._audioClips.Length)];
        }


        return null;
    }


}
