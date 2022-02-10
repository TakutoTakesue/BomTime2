using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
	[System.Serializable]
	struct Sound_Data
    {
		public AudioClip clip;
		public string name;
    }

	[SerializeField] AudioSource bgmSource;
	[SerializeField] AudioSource seSource;

	[Header("SoundのFade速度")]
	[SerializeField, Tooltip("FadeIn開始から終了までの時間")]  float fadeInSeconds;
	[SerializeField, Tooltip("FadeOut開始から終了までの時間")] float fadeOutSeconds;
	float fadeDeltaTime;

	[Header("各種音量")]
	[SerializeField, Tooltip("全体音量"), Range(0.0f, 1.0f)] float masterVolume;
	[SerializeField, Tooltip("BGM音量"), Range(0.0f, 1.0f)] float bgmVolume;
	[SerializeField, Tooltip("SE音量"), Range(0.0f, 1.0f)] float seVolume;

	[Header("Slider")]
	//[SerializeField] Slider masterSlider;
	//[SerializeField] Slider bgmrSlider;
	//[SerializeField] Slider seSlider;

	[Header("Sound_Data")]
	[SerializeField, Tooltip("BGM")] Sound_Data[] bgm_Clip;
	[SerializeField, Tooltip("SE")] Sound_Data[] se_Clip;


    public float MasterVolume
    {
		set
        {
			masterVolume = Mathf.Clamp01(value);
			bgmSource.volume = masterVolume * bgmVolume;
			seSource.volume = masterVolume * seVolume;
        }
        get
        {
			return masterVolume;
        }
    }

	public float BgmVolume
	{
		set
		{
			bgmVolume = Mathf.Clamp01(value);
			bgmSource.volume = masterVolume * bgmVolume;
		}
		get
		{
			return bgmVolume;
		}
	}

	public float SeVolume
	{
		set
		{
			seVolume = Mathf.Clamp01(value);
			seSource.volume = masterVolume * seVolume;
		}
		get
		{
			return seVolume;
		}
	}

	private void Awake()
	{
		if (this != Instance)
		{
			Destroy(this);
			return;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	//指定したBGMを鳴らし始める
	public void StartBgm(string name,float fadeTime = 0.0f)
	{
		if((int)bgmVolume != (int)bgmSource.volume)
        {
			bgmSource.volume = bgmVolume;
        }
		if(bgmSource.isPlaying || bgmSource.clip != null)
        {
			bgmSource.clip = null;
			bgmSource.Stop();
        }
		for (int i = 0; i < bgm_Clip.Length; i++)
		{
			if (name == bgm_Clip[i].name || name == se_Clip[i].clip.name)
			{
				if(fadeTime > 0.0f)
                {
					bgmSource.volume = 0.0f;
					StartCoroutine("FadeIn", fadeTime);
                }
				bgmSource.clip = bgm_Clip[i].clip;
				bgmSource.Play();
				break;
			}
		}
	}

	//セットされているBGMを鳴らす
	public void PlayBgm(float setFadeTime = 0)
    {
		if(setFadeTime > 0)
        {
			bgmSource.volume = 0.0f;
			StartCoroutine("FadeIn", setFadeTime);
        }
		bgmSource.UnPause();
    }


	//指定したSEを鳴らす
	public void StartSe(string name)
	{
		for (int i = 0; i < se_Clip.Length; i++)
		{
			if (name == se_Clip[i].name || name == se_Clip[i].clip.name)
			{
				seSource.PlayOneShot(se_Clip[i].clip);
				break;
			}
		}
	}

	//セットされているSEを鳴らす
	public void PlaySe()
    {
		seSource.UnPause();
    }

	//全てのSoundを一時停止
	public void SoundPause(float setFadeTime = 0)
    {
		if(setFadeTime > 0)
        {
			StartCoroutine("FadeOut", setFadeTime);
        }
        else
        {
			bgmSource.Pause();
		}
		seSource.Pause();
	}

	//BGMを終了
	public void StopBgm(float setFadeTime = 0)
    {
		if(setFadeTime > 0)
        {
			StartCoroutine("FadeOut", setFadeTime);
        }
        else
        {
			bgmSource.Stop();
			bgmSource.clip = null;
		}
		
    }

	//SEを終了
	public void StopSe()
    {
		seSource.Stop();
		seSource.clip = null;
    }

	public void StartFadeIn(float setFadeTime)
    {
		FadeIn(setFadeTime);
    }

	public void StartFadeOut(float setFadeTime)
    {
		FadeOut(setFadeTime);
    }

    IEnumerator FadeIn(float setFadeTime)
    {
		while(true)
        {
			fadeDeltaTime += Time.deltaTime;
			bgmSource.volume = (fadeDeltaTime / setFadeTime) * bgmVolume;
			if (fadeDeltaTime >= setFadeTime)
			{
				yield break;
			}

			yield return null;
		}
	}

	IEnumerator FadeOut(float setFadeTime)
    {
		while(true)
        {
			fadeDeltaTime += Time.deltaTime;
			bgmSource.volume = (1 - (fadeDeltaTime / setFadeTime)) * BgmVolume;
			if(fadeDeltaTime >= setFadeTime)
            {
				bgmSource.Stop();
				bgmSource.clip = null;
				yield break;
            }
			yield return null;
        }
    }
}