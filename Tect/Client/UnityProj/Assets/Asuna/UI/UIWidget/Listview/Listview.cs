using System;
using System.Collections.Generic;
using Asuna.Foundation.Debug;
using UnityEngine;

namespace Asuna.UI
{
    public class Listview : MonoBehaviour
    {
        private void Awake()
        {
            _Root = transform.Find("Viewport/Content").gameObject;
        }

        public void SetViewTemplate(ListviewItemView view)
        {
            _Template = view;
            _Template.gameObject.SetActive(false);
        }

        public ListviewItemView AddItem(object model)
        {
            var item = Instantiate(_Template, _Root.transform);
            item.gameObject.SetActive(true);
            item.__SetOwner(this);
            item.SetupView();
            item.SetupModel(model);
            _Items.Add(item);
            return item;
        }

        public void RemoveItem(int index)
        {
            _Items.RemoveAt(index);
        }

        public bool RemoveItem(ListviewItemView item)
        {
            return _Items.Remove(item);
        }

        public void OnItemSelected(ListviewItemView item)
        {
            ADebug.Assert(item != null);
            if (item == CurrentSelectedItem)
            {
                return;
            }
            if (CurrentSelectedItem != null)
            {
                CurrentSelectedItem.Unselect();
            }
            item.Select();
            CurrentSelectedItem = item;
        }
        
        private ListviewItemView _Template;
        private GameObject _Root;
        private readonly List<ListviewItemView> _Items = new();
        public ListviewItemView CurrentSelectedItem;
    }
}