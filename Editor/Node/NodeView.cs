using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace UnityGraphEditor
{
    public abstract class NodeView : Node
    {
        protected abstract void OnInit();
        public Dictionary<string, Port> Ports = new();

        public void Init()
        {
            OnInit();
            RefreshExpandedState();
        }

        internal virtual void CreateEdges() 
        { 

        }

        public virtual void SavePosition()
        {

        }

        protected void AddLabel(string text, string ussClassName = "NodeViewLabel")
        {
            var label = new Label();
            label.text = text;
            label.AddToClassList(ussClassName);
            extensionContainer.Add(label);
        }

        protected Port AddInPort<T>(
            string userData,
            string name = "In",
            Port.Capacity capacity = Port.Capacity.Multi)
        {
            var port = InstantiatePort(Orientation.Horizontal, Direction.Input, capacity, typeof(T));
            port.portName = name;
            port.userData = userData;

            inputContainer.Add(port);
            Ports.Add(userData, port);

            return port;
        }

        protected Port AddOutPort<T>(
            string userData,
            string name = "Out",
            Port.Capacity capacity = Port.Capacity.Multi)
        {
            var port = InstantiatePort(Orientation.Horizontal, Direction.Output, capacity, typeof(T));
            port.portName = name;
            port.userData = userData;

            outputContainer.Add(port);
            Ports.Add(userData, port);

            return port;
        }
    }
}
