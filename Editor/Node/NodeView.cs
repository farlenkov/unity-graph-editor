using System;
using UnityEditor.Experimental.GraphView;

namespace UnityGraphEditor
{
    public abstract class NodeView : Node
    {
        public Port InPort;

        protected abstract void OnInit();

        public void Init()
        {
            OnInit();
        }

        internal virtual void CreateEdges() 
        { 

        }

        public virtual void SavePosition()
        {

        }
    }
}
