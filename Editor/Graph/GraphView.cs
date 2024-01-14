using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace UnityGraphEditor
{
    public abstract class GraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        protected abstract void OnEdgeCreate(Edge edge);
        protected abstract void OnEdgeRemove(Edge edge);
        protected abstract void OnGraphDestroy();

        public GraphView()
        {
            AddStyle();
            AddBackground();
            AddManipulators();

            graphViewChanged = OnGraphViewChanged;
        }

        ~GraphView()
        {
            OnGraphDestroy();
        }

        protected virtual void AddStyle()
        {
            this.StretchToParentSize();

            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.farlenkov.unity-graph-editor/Editor/Style/GraphStyles.uss");
            styleSheets.Add(style);

            AddToClassList("UnityGraphView");
        }

        void AddBackground()
        {
            var bg = new GridBackground();
            bg.StretchToParentSize();
            Insert(0, bg);
        }

        void AddManipulators()
        {
            this.SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var result = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                    return;

                if (startPort.node == port.node)
                    return;

                if (startPort.direction == port.direction)
                    return;

                if (startPort.portType != port.portType)
                    return;

                result.Add(port);
            });

            return result;
        }

        GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.edgesToCreate != null)
            {
                foreach (var edge in change.edgesToCreate)
                    OnEdgeCreate(edge);
            }

            if (change.elementsToRemove != null)
            {
                for (var i = 0; i < change.elementsToRemove.Count; i++)
                {
                    if (change.elementsToRemove[i] is NodeView)
                    {
                        change.elementsToRemove.RemoveAt(i);
                        i--;
                    }
                }

                foreach (var el in change.elementsToRemove)
                {
                    if (el is Edge edge)
                        OnEdgeRemove(edge);
                }
            }

            if (change.movedElements != null)
            {
                foreach (var el in change.movedElements)
                    if (el is NodeView view)
                        view.SavePosition();
            }

            return change;
        }

        public bool TryGetNode(string guid, out NodeView result)
        {
            var node = GetNodeByGuid(guid);

            if (node == null)
            {
                result = null;
                return false;
            }
            else if (node is NodeView view)
            {
                result = view;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        protected void ClearNodes()
        {
            foreach (var node in nodes)
                RemoveElement(node);

            foreach (var edge in edges)
                RemoveElement(edge);
        }
    }
}
