using EnhancedBossesRedone.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace EnhancedBossesRedone.StatusEffects
{
    public class SE_Trip : StatusEffect
    {
        public SE_Trip() {
            name = "eb_trip";
            m_name = "Hallucinations";
            m_ttl = 1f;
            m_icon = Main.Bundle!.LoadAsset<Sprite>("trip");
            m_attributes = StatusAttribute.None;
        }

        public override void UpdateStatusEffect(float dt)
        {
            if (component == null)
            {
                SetupPostProcessing();
            }
            
            rainbowtimer += Time.deltaTime;
            if (rainbowtimer > rainbowchangeevery)
            {
                rainbowcurrent = (rainbowcurrent + 1) % rainbowcolors.Length;
                rainbownext = (rainbowcurrent + 1) % rainbowcolors.Length;
                rainbowtimer = 0f;
            }
            component!.m_Vignette.model.m_Settings.color = Color.Lerp(rainbowcolors[rainbowcurrent], rainbowcolors[rainbownext], rainbowtimer / rainbowchangeevery);
            Helpers.Lerp(ref component.m_Vignette.model.m_Settings.intensity, NewVignetteIntensity, DeltaVignetteIntensity);
            Helpers.Lerp(ref component.m_ChromaticAberration.model.m_Settings.intensity, NewChromaticAberrationIntensity, DeltaChromaticAberrationIntensity);
            Helpers.Lerp(ref component.m_ColorGrading.model.m_Settings.basic.saturation, NewColorGradingSaturation, DeltaColorGradingSaturation);
            base.UpdateStatusEffect(dt);
        }

        private void SetupPostProcessing()
        {
            component = GameCamera.instance.gameObject.GetComponent<PostProcessingBehaviour>();
            component.m_Vignette.model.enabled = true;
            component.m_ColorGrading.model.isDirty = true;
        }

        public override bool CanAdd(Character character)
        {
            return ConfigManager.BonemassTripEffect!.Value;
        }

        public override void Stop()
        {
            if (ConfigManager.BonemassTripEffect!.Value)
            {
                RemoveTripEffect();
            }
            base.Stop();
        }

        public override void OnDestroy()
        {
            if (component != null)
            {
                component!.m_Vignette.model.m_Settings.intensity = OldVignetteIntensity;
                component!.m_ChromaticAberration.model.m_Settings.intensity = OldChromaticAberrationIntensity;
                component!.m_ColorGrading.model.m_Settings.basic.saturation = OldColorGradingSaturation;
            }
            base.OnDestroy();
        }

        public void RemoveTripEffect()
        {
            component!.StartCoroutine(FadeVignette());
            component!.StartCoroutine(FadeChromaticAberration());
            component!.StartCoroutine(FadeSaturation());
        }

        IEnumerator FadeVignette()
        {
            if (component == null)
            {
                yield break;
            }

            while (component!.m_Vignette.model.m_Settings.intensity > OldVignetteIntensity)
            {
                Helpers.Lerp(ref component.m_Vignette.model.m_Settings.intensity, OldVignetteIntensity, DeltaVignetteIntensity);
                yield return null;
            }

            component.StopCoroutine(FadeVignette());
        }

        IEnumerator FadeChromaticAberration()
        {
            if (component == null)
            {
                yield break;
            }

            while (component!.m_ChromaticAberration.model.m_Settings.intensity > OldChromaticAberrationIntensity)
            {
                Helpers.Lerp(ref component.m_ChromaticAberration.model.m_Settings.intensity, OldChromaticAberrationIntensity, DeltaChromaticAberrationIntensity);
                yield return null;
            }

            component.StopCoroutine(FadeChromaticAberration());
        }

        IEnumerator FadeSaturation()
        {
            if (component == null)
            {
                yield break;
            }

            while (component!.m_ColorGrading.model.m_Settings.basic.saturation > OldColorGradingSaturation)
            {
                Helpers.Lerp(ref component.m_ColorGrading.model.m_Settings.basic.saturation, OldColorGradingSaturation, DeltaColorGradingSaturation);
                yield return null;
            }

            component.StopCoroutine(FadeSaturation());
        }

        


        public static int rainbowcurrent = 0;
        public static int rainbownext = 1;
        public static float rainbowtimer = 0f;
        public static float rainbowchangeevery = 0.5f;
        public static Color[] rainbowcolors = new Color[]
        {
            new Color(5f, 0f, 0f, 1f),
            new Color(5f, 2f, 0f, 1f),
            new Color(5f, 5f, 0f, 1f),
            new Color(0f, 5f, 0f, 1f),
            new Color(0f, 3f, 2f, 1f),
            new Color(0f, 0f, 5f, 1f),
            new Color(3f, 0f, 5f, 1f),
            new Color(4f, 0f, 2f, 1f)
        };

        public static PostProcessingBehaviour? component;
        public static float Ticks = 50f;
        public static float OldVignetteIntensity = 0.45f;
        public static float OldChromaticAberrationIntensity = 0.15f;
        public static float OldColorGradingSaturation = 1f;
        public static float NewVignetteIntensity = 0.2f;
        public static float NewChromaticAberrationIntensity = 2f * ConfigManager.BonemassTripIntensity!.Value;
        public static float NewColorGradingSaturation = 2f;
        public static float DeltaVignetteIntensity = Mathf.Abs(OldVignetteIntensity - NewVignetteIntensity) / Ticks;
        public static float DeltaChromaticAberrationIntensity = Mathf.Abs(OldChromaticAberrationIntensity - NewChromaticAberrationIntensity) / Ticks;
        public static float DeltaColorGradingSaturation = Mathf.Abs(OldColorGradingSaturation - NewColorGradingSaturation) / Ticks;
    }
}
