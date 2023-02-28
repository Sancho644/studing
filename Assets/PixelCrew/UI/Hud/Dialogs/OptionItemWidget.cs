﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;

namespace PixelCrew.UI.Hud.Dialogs
{
    public class OptionItemWidget : MonoBehaviour, IItemRenderer<OptionData>
    {
        [SerializeField] private Text _label;
        [SerializeField] private SelectOption _onSelect;

        private OptionData _data;

        public void SetData(OptionData data, int index)
        {
            _data = data;
            _label.text = data.Text.Localize();
        }

        public void OnSelect()
        {
            _onSelect.Invoke(_data);
        }
        
        [Serializable]
        public class SelectOption : UnityEvent<OptionData>
        {
        }
    }
}