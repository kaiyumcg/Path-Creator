using System.Collections.Generic;
using UnityEngine;

namespace PathCreation {

    [System.Serializable]
    public class PathData
    {
        public PathCreatorData editorData;
        public List<Vector3> roadNormals, pointsForRoadNormals;
    }

    public class PathCreator : MonoBehaviour {

        /// This class stores data for the path editor, and provides accessors to get the current vertex and bezier path.
        /// Attach to a GameObject to create a new path editor.

        public event System.Action pathUpdated;

        [SerializeField, HideInInspector]
        PathCreatorData editorData;
        [SerializeField, HideInInspector]
        bool initialized;
        [SerializeField]
        List<Vector3> roadNormals = new List<Vector3>();
//#if UNITY_EDITOR
        public List<Vector3> RoadNormals 
        { 
            get 
            { 
                return roadNormals; 
            }
            set
            {
                roadNormals = value;
            }
        }
//#endif
        [SerializeField]
        List<Vector3> pointsForRoadNormals = new List<Vector3>();
//#if UNITY_EDITOR
        public List<Vector3> PointsForRoadNormals
        {
            get
            {
                return pointsForRoadNormals;
            }
            set
            {
                pointsForRoadNormals = value;
            }
        }
//#endif

        public Vector3 GetNormalAtDistance(float dist)
        {
            Vector3 result = Vector3.up;
            if (roadNormals != null && roadNormals.Count > 0)
            {
                var len = path.length;
                var prog = dist / len;
                int ptNo = (int)(prog * (float)roadNormals.Count);
                ptNo = Mathf.Clamp(ptNo, 1, roadNormals.Count);
                result = roadNormals[ptNo - 1];
            }
            return result;
        }


        GlobalDisplaySettings globalEditorDisplaySettings;

        // Vertex path created from the current bezier path
        public VertexPath path {
            get {
                if (!initialized) {
                    InitializeEditorData (false);
                }
                return editorData.GetVertexPath(transform);
            }
        }

        // The bezier path created in the editor
        public BezierPath bezierPath {
            get {
                if (!initialized) {
                    InitializeEditorData (false);
                }
                return editorData.bezierPath;
            }
            set {
                if (!initialized) {
                    InitializeEditorData (false);
                }
                editorData.bezierPath = value;
            }
        }

        #region Internal methods

        /// Used by the path editor to initialise some data
        public void InitializeEditorData (bool in2DMode) {
            if (editorData == null) {
                editorData = new PathCreatorData ();
            }
            editorData.bezierOrVertexPathModified -= TriggerPathUpdate;
            editorData.bezierOrVertexPathModified += TriggerPathUpdate;

            editorData.Initialize (in2DMode);
            initialized = true;
        }

        public PathCreatorData EditorData {
            get {
                return editorData;
            }
            set
            {
                editorData = value;
            }
        }

        public void TriggerPathUpdate () {
            if (pathUpdated != null) {
                pathUpdated ();
            }
        }

#if UNITY_EDITOR

        // Draw the path when path objected is not selected (if enabled in settings)
        void OnDrawGizmos () {

            // Only draw path gizmo if the path object is not selected
            // (editor script is resposible for drawing when selected)
            GameObject selectedObj = UnityEditor.Selection.activeGameObject;
            if (selectedObj != gameObject) {

                if (path != null) {
                    path.UpdateTransform (transform);

                    if (globalEditorDisplaySettings == null) {
                        globalEditorDisplaySettings = GlobalDisplaySettings.Load ();
                    }

                    if (globalEditorDisplaySettings.visibleWhenNotSelected) {

                        Gizmos.color = globalEditorDisplaySettings.bezierPath;

                        for (int i = 0; i < path.NumPoints; i++) {
                            int nextI = i + 1;
                            if (nextI >= path.NumPoints) {
                                if (path.isClosedLoop) {
                                    nextI %= path.NumPoints;
                                } else {
                                    break;
                                }
                            }
                            Gizmos.DrawLine (path.GetPoint (i), path.GetPoint (nextI));
                        }
                    }
                }
            }
        }
#endif

        #endregion
    }
}