using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceObject : MonoBehaviour
{
    public AudioSource _audioSource { get; set; } = null;
    // ��A�N�e�B�u�����邽�߂̃R�[���o�b�N
    private Action _onDisable;  

    // ����������
    public void Initialize(Action onDisable)
    {
        _audioSource = GetComponent<AudioSource>();
        _onDisable = onDisable;
    }

    public void SetAudio(AudioClip clip , AudioMixerGroup group)
    {
        _audioSource.clip = clip;
        _audioSource.outputAudioMixerGroup = group;
    }

    private void Update()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        // it's possible that a playOnAwake sound will not play if too many other sounds are playing
        if (_audioSource.timeSamples == _audioSource.clip.samples || _audioSource.isPlaying == false)
        {
            _onDisable?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
