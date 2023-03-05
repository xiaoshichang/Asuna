using UnityEngine;
using UnityEngine.EventSystems;

namespace Asuna.UI
{
    public delegate void ListviewItemOnSelectCallback(ListviewItemView itemView);
    public delegate void ListviewItemOnUnselectCallback(ListviewItemView itemView);
    
    public abstract class ListviewItemView : MonoBehaviour, IPointerClickHandler
    {
        public ListviewItemOnSelectCallback OnSelectCallback;
        public ListviewItemOnUnselectCallback OnUnselectCallback;
        private Listview _Owner;
        protected object _Model;
        
        public void __SetOwner(Listview owner)
        {
            _Owner = owner;
        }

        protected T _Seek<T>(string path) where T : class
        {
            var node = gameObject.transform.Find(path);
            if (node == null)
            {
                return null;
            }

            var cmpt = node.GetComponent<T>();
            if (cmpt == null)
            {
                return null;
            }

            return cmpt;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _Owner.OnItemSelected(this);
        }

        public void Select()
        {
            _OnSelect();
            OnSelectCallback?.Invoke(this);
        }

        public void Unselect()
        {
            _OnUnselect();
            OnUnselectCallback?.Invoke(this);
        }

        public object GetModel()
        {
            return _Model;
        }

        protected abstract void _OnSelect();
        protected abstract void _OnUnselect();
        
        public abstract void SetupView();
        public abstract void SetupModel(object model);
        

    }
}