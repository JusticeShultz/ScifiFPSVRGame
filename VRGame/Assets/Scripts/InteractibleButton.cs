using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Valve.VR.InteractionSystem.Sample
{
    public class InteractibleButton : MonoBehaviour
    {
        public Color ButtonDownColor;
        public Color ButtonUpColor;
        public Color ButtonPressedColor;

        private void Start()
        {
            ColorSelf(ButtonUpColor);
        }

        public void OnButtonDown(Hand fromHand)
        {
            ColorSelf(ButtonDownColor);
            fromHand.TriggerHapticPulse(1000);
        }

        public void OnButtonUp(Hand fromHand)
        {
            ColorSelf(ButtonUpColor);
        }

        public void OnButtonPressed(Hand fromHand)
        {
            ColorSelf(ButtonPressedColor);
            fromHand.TriggerHapticPulse(1000);
        }

        private void ColorSelf(Color newColor)
        {
            Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
            for (int rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++)
            {
                renderers[rendererIndex].material.color = newColor;
            }
        }
    }
}
