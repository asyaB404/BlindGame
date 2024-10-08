using GamePlay;
using UnityEngine;

public class ChartManager : MonoManager<ChartManager>
{
    /// <summary>
    /// 表示轨道数量
    /// </summary>
    public static int KeysCount { get; private set; } = 2;

    public static Chart Chart => Instance.chart;

    [SerializeField] private Chart chart;


    #region Debug

    [ContextMenu("sampleStart")]
    private void TestStart()
    {
        GameManager.Instance.Reset();
        chart = Chart.SampleChart();
        GameManager.Instance.StartGame();
    }

    #endregion
}