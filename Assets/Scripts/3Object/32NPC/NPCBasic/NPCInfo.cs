using Pathfinding;
using System;

[Serializable]
public struct NPCSaveData
{
    public SNPC NPC;
    public DialogueInfo[] DialInfo;
    public bool[] ProductSold;
    public readonly bool IsNull { get { return NPC == SNPC.Null; } }
    public static NPCSaveData Null { get { return new(SNPC.Null, new DialogueInfo[0], new bool[0]); } }
    public NPCSaveData(SNPC _npc, DialogueInfo[] _info)
    {
        NPC = _npc; DialInfo = new DialogueInfo[_info.Length];
        for (int i = 0; i<_info.Length; i++)
        {
            DialInfo[i] = new(_info[i]);
        }
        ProductSold = new bool[0];
    }
    public NPCSaveData(SNPC _npc, DialogueInfo[] _info, bool[] _sold)
    {
        NPC = _npc; DialInfo = new DialogueInfo[_info.Length];
        for (int i = 0; i<_info.Length; i++)
        {
            DialInfo[i] = new(_info[i]);
        }
        ProductSold = new bool[_sold.Length];
        for (int i = 0; i<_sold.Length; i++)
        {
            ProductSold[i] = _sold[i];
        }
    }
    public NPCSaveData(NPCSaveData _other) : this(_other.NPC, _other.DialInfo, _other.ProductSold) { }
}