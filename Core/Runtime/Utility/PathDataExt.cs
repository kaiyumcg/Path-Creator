using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExt;

namespace PathCreation
{
    public static class PathDataExt
    {
        public static PathData ConvertToData(this PathCreator path)
        {
            var result = new PathData
            {
                editorData = path.EditorData.Clone() as PathCreatorData,
                pointsForRoadNormals = path.PointsForRoadNormals.ExShallowCopy(),
                roadNormals = path.RoadNormals.ExShallowCopy()
            };
            return result;
        }
        public static List<PathData> ConvertToData(this List<PathCreator> paths)
        {
            var result = new List<PathData>();
            paths.ExForEach((i) =>
            {
                result.Add(i.ConvertToData());
            });
            return result;
        }
        public static void UpdateTransforms(this List<PathCreator> paths)
        {
            paths.ExForEach((i) =>
            {
                if (i != null && i.EditorData != null && i.EditorData.VertPath != null)
                {
                    i.EditorData.VertPath.UpdateTransform(i.transform);
                }
            });
        }
        public static void SetDataToPath(this PathData data, PathCreator path)
        {
            path.EditorData = data.editorData;
            path.PointsForRoadNormals = data.pointsForRoadNormals.ExShallowCopy();
            path.RoadNormals = data.roadNormals.ExShallowCopy();
        }
        public static void SetDataToPath(this List<PathData> data, List<PathCreator> paths)
        {
            data.ExForEach((i, index) =>
            {
                i.SetDataToPath(paths[index]);
            });
        }
    }
}