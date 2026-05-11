using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager2D : MonoBehaviour
{
	public List<AudioSource> mySources = new List<AudioSource>();

	public AudioClip[] myClips;

	public void PlayClip(int clipId, float volume = 1f, float pitch = 1f)
	{
		for (int i = 0; i < mySources.Count; i++)
		{
			if (!mySources[i].isPlaying || mySources[i].pitch == pitch)
			{
				PlayClip(i, clipId, volume, pitch);
				return;
			}
		}
		mySources.Add(base.gameObject.AddComponent<AudioSource>());
		PlayClip(mySources.Count - 1, clipId, volume, pitch);
	}

	private void PlayClip(int sourceId, int clipId, float volume = 1f, float pitch = 1f)
	{
		mySources[sourceId].pitch = pitch;
		mySources[sourceId].PlayOneShot(myClips[clipId], volume);
	}

	public void PlayClipDelayed(int clipId, float delay, float volume = 1f, float pitch = 1f)
	{
		StartCoroutine(PlayClipDelayedRoutine(delay, clipId, volume, pitch));
	}

	private IEnumerator PlayClipDelayedRoutine(float delay, int clipId, float volume = 1f, float pitch = 1f)
	{
		yield return new WaitForSeconds(delay);
		PlayClip(clipId);
	}

	public void PlayClipDelayedRealTime(int clipId, float delay, float volume = 1f, float pitch = 1f)
	{
		StartCoroutine(PlayClipDelayedRealTimeRoutine(delay, clipId, volume, pitch));
	}

	private IEnumerator PlayClipDelayedRealTimeRoutine(float delay, int clipId, float volume = 1f, float pitch = 1f)
	{
		yield return new WaitForSecondsRealtime(delay);
		PlayClip(clipId);
	}
}
