using System.Collections;
using UnityEngine;

namespace EnhancedBossesRedone.Data
{
    public class ExtendedTimedDestruction : MonoBehaviour
    {
		private void Awake()
		{
			this.m_nview = base.GetComponent<ZNetView>();
			if (this.m_triggerOnAwake)
			{
				this.Trigger();
			}
		}

		public void Trigger()
		{
			base.InvokeRepeating("StopAllAudioSources", m_audiotimeout, 1f);
			base.InvokeRepeating("StopAllParticleSystems", m_pstimeout, 1f);
			base.InvokeRepeating("DestroyNow", m_timeout, 1f);
		}

		public void Trigger(float timeout, float pstimeout, float audiotimeout)
		{
			base.InvokeRepeating("StopAllAudioSources", audiotimeout, 1f);
			base.InvokeRepeating("StopAllParticleSystems", pstimeout, 1f);
			base.InvokeRepeating("DestroyNow", timeout, 1f);
		}

		private void DestroyNow()
		{
			if (!this.m_nview)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (!this.m_nview!.IsValid() || !this.m_nview.IsOwner())
			{
				return;
			}
			ZNetScene.instance.Destroy(base.gameObject);
		}

		public void StopAllParticleSystems()
        {
			ParticleSystem[] particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
			if (particleSystems == null || particleSystems.Length == 0)
            {
				return;
            }

			foreach (ParticleSystem ps in particleSystems)
            {
				if (ps.isPlaying)
				{
					ps.Stop();
				}
            }
			return;
		}

		public void StopAllAudioSources()
        {
			AudioSource[] audioSources = gameObject.GetComponentsInChildren<AudioSource>(); 
			if (audioSources == null || audioSources.Length == 0)
            {
				return;
            }

			if (audioCoroutines == null || audioCoroutines.Length == 0)
            {
				return;
            }

			audioCoroutines = new IEnumerator[audioSources.Length];
			for (int i = 0; i < audioCoroutines.Length; i += 1)
            {
				audioCoroutines[i] = FadeOutAudioThenStop(audioSources[i]);
				StartCoroutine(audioCoroutines[i]);
            }
        }

		private IEnumerator FadeOutAudioThenStop(AudioSource audio)
        {
			if (!audio.isPlaying || !audio.isActiveAndEnabled)
            {
				yield break;
            }

			while (audio.volume > 0.1f)
            {
				audio.volume -= 0.05f;
				yield return new WaitForSeconds(0.1f);
            }

			audio.Stop();
        }



		private IEnumerator[]? audioCoroutines;
		public float m_timeout = 1f;
		public float m_pstimeout = 1f;
		public float m_audiotimeout = 1f;
		public bool m_triggerOnAwake;
		private ZNetView? m_nview;
	}


}
