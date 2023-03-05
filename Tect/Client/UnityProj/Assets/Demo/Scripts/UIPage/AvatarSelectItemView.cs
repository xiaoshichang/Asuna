using System;
using Asuna.UI;
using TMPro;
using UnityEngine;

namespace Demo
{
    public class AvatarSelectItemView : ListviewItemView
    {
        protected override void _OnSelect()
        {
        }

        protected override void _OnUnselect()
        {
        }

        public override void SetupView()
        {
            GuidText = _Seek<TMP_Text>("");
        }

        public override void SetupModel(object model)
        {
            _Model = model;
            var guid = _Model is Guid ? (Guid)_Model : default;
            GuidText.text = guid.ToString();
        }
        
        private TMP_Text GuidText;
    }
}