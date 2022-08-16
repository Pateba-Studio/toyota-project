using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavascriptWindowGraph : MonoBehaviour {

    [SerializeField] private Window_Graph windowGraph;

    public void SetGraphData(string json) {
        GraphData graphData = JsonUtility.FromJson<GraphData>(json);
        windowGraph.ShowGraph(new List<int>(graphData.valueArray), windowGraph.lineGraphVisual);
    }

    public class GraphData {

        public int[] valueArray;

    }
}
