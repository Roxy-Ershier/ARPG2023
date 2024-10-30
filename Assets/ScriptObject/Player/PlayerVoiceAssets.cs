using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum PlayerVoiceType
    {
        ATK,
        HIT,
        DIE,
        FinalityVoice,
    }


    [CreateAssetMenu(fileName = "PlayerVoiceAssets", menuName = "SO/PlayerVoiceAssets")]
    public class PlayerVoiceAssets : ScriptableObject
    {
        [System.Serializable]
        private class SoundConfig
        {
            public PlayerVoiceType SoundType;
            public AudioClip[] _audioClips;
        }

        [SerializeField] private List<SoundConfig> _soundAssets = new List<SoundConfig>();


        public AudioClip TryGetOneClip(PlayerVoiceType type)
        {
            if (_soundAssets.Count <= 0) return null;
            switch (type)
            {
                case PlayerVoiceType.ATK: return _soundAssets[0]._audioClips[Random.Range(0, _soundAssets[0]._audioClips.Length)];
                case PlayerVoiceType.HIT: return _soundAssets[1]._audioClips[Random.Range(0, _soundAssets[1]._audioClips.Length)];
                case PlayerVoiceType.DIE: return _soundAssets[2]._audioClips[Random.Range(0, _soundAssets[2]._audioClips.Length)];

                case PlayerVoiceType.FinalityVoice: return _soundAssets[3]._audioClips[Random.Range(0, _soundAssets[3]._audioClips.Length)];
            }


            return null;
        }
}
