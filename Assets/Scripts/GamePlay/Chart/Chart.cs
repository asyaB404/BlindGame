using System.Collections.Generic;
using GamePlay.Notes;


[System.Serializable]
public class Chart
{
    public int keys;
    public float offset;
    public float musicLength;
    public float totalBeat;
    public float curBpm;
    public List<List<Note>> notes = new();


    public static Chart SampleChart()
    {
        Chart chart = new Chart
        {
            keys = 2,
            curBpm = 90,
            musicLength = 30f,
            totalBeat = 60f,
            notes = new List<List<Note>>() { new(), new() }
        };
        List<Note> notes0 = new List<Note>() { };
        AddTap7_th(notes0, 0, 6);
        AddTap7_th(notes0, 0, 6 + 7);
        AddTap7_th(notes0, 0, 6 + 7 * 2);
        AddTap7_th(notes0, 0, 6 + 7 * 3);
        AddTap7_th(notes0, 0, 6 + 7 * 4, 0.5f);
        AddTap7_th(notes0, 0, 6 + 7 * 4.5f, 0.5f);
        chart.notes[0] = notes0;
        return chart;
    }

    private static void AddTap7_th(IList<Note> notes, int pos = 0, float start = 0, float duration = 1)
    {
        for (int i = 0; i < 6; i++)
        {
            notes.Add(new SoundNote(start + duration * i, pos));
        }

        notes.Add(new TapNote(start + duration * 6, pos));
    }
}