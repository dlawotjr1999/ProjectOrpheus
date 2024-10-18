#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class DataImporter
{
    private readonly static string GameManagerPath = PrefabPath + "System/GameManager.prefab";
    private readonly static string HellMapPath = PrefabPath + "Environment/HellMap.prefab";

    private static string CSVPath { get { return Application.dataPath + "/Data/CSVs/"; } }
    private const string ItemCSVName = "ItemSheet.csv";
    private const string MonsterCSVName = "MonsterSheet.csv";
    private const string DropCSVName = "DropSheet.csv";
    private const string PowerCSVName = "PowerSheet.csv";
    private const string NPCCSVName = "NPCSheet.csv";
    private const string DialogueCSVName = "DialogueSheet.csv";
    private const string QuestCSVName = "QuestSheet.csv";

    // 스크립터블 경로
    private const string ScriptablePath = "Assets/Scriptables/";

    private const string MonsterScriptablePath = ScriptablePath + "MonsterScriptable/";

    private const string ItemScriptablePath = ScriptablePath + "ItemScriptable/";
    private readonly static string[] ItemScriptablePaths = new string[(int)EItemType.LAST] {
        ItemScriptablePath + "Weapon/",
        ItemScriptablePath + "Pattern/",
        ItemScriptablePath + "ThrowItem/",
        ItemScriptablePath + "Others/"          };

    private const string PowerScriptablePath = ScriptablePath + "PowerScriptable/";
    private const string NPCScriptablePath = ScriptablePath + "NPCScriptable/";
    private const string QuestScriptablePath = ScriptablePath + "QuestScriptable/";
    private const string DialogueScriptablePath = ScriptablePath + "DialogueScriptable/";

    // 프리펍 경로
    private const string PrefabPath = "Assets/Prefabs/";
    private const string MonsterPrefabPath = PrefabPath + "Monster/";
    private const string ItemPrefabPath = PrefabPath + "Item/";
    private readonly static string[] ItemPrefabPaths = new string[(int)EItemType.LAST] {
        ItemPrefabPath + "Weapon/",
        ItemPrefabPath + "Pattern/",
        ItemPrefabPath + "ThrowItem/",
        ItemPrefabPath + "Others/"
    };
    private const string PowerPrefabPath = PrefabPath + "PlayerPower/";
    private const string NPCPrefabPath = PrefabPath + "NPC/";


    // 이미지 경로
    private const string ImagePath = "Image/";
    private readonly static string[] ItemImgPath = new string[(int)EItemType.LAST]
    {
        ImagePath + "WeaponImage/",
        ImagePath + "PatternImage/",
        ImagePath + "ThrowItemImage/",
        ImagePath + "OtherItemImage/"
    };

    private const string MonsterBodyImgPath = ImagePath + "MonsterBody/";
    private const string MonsterProfilePath = ImagePath + "MonsterProfile/";

    private const string PowerIconPath = ImagePath + "PowerIcon/";

    private static string[] ModifyLines(string _line)
    {
        string[] lines = _line.Split(',');
        for(int i=0;i<lines.Length;i++)
        {
            lines[i] = lines[i].Replace("\\n", "\n");
            lines[i] = lines[i].Replace("\\c", ",");
        }

        return lines;
    }


    [MenuItem("Utilities/GenerateMonsters")]
    private static void GenerateMonsterData()
    {
        // 드랍 정보
        string[] allDropLines = File.ReadAllLines(CSVPath + DropCSVName);

        Dictionary<string, DropInfo> dropInfos = new();

        for (uint i = 1; i < allDropLines.Length; i++)
        {
            string si = allDropLines[i];
            string[] splitDropData = ModifyLines(si);

            string id = splitDropData[(int)EDropAttribute.MONSTER];
            int.TryParse(splitDropData[(int)EDropAttribute.STAT], out int stat);
            int.TryParse(splitDropData[(int)EDropAttribute.SOUL], out int soul);
            int.TryParse(splitDropData[(int)EDropAttribute.PURIFIED], out int purified);
            List<SDropItem> items = new();
            for (int j = 0; j<4; j++)
            {
                string item = splitDropData[(int)EDropAttribute.ITEM1 + j * 2];
                if (item == "") { continue; }
                float.TryParse(splitDropData[(int)EDropAttribute.RATE1 + j * 2], out float rate);
                SDropItem drop = new(item, rate);
                items.Add(drop);
            }

            DropInfo dropInfo = new(items, stat, soul, purified);
            dropInfos[id] = dropInfo;
        }


        // 몬스터 정보
        string[] allMonsterLines = File.ReadAllLines(CSVPath + MonsterCSVName);

        List<MonsterScriptable> datas = new();

        for (uint i = 1; i < allMonsterLines.Length; i++)
        {
            uint idx = i - 1;
            string si = allMonsterLines[i];
            string[] splitMonsterData = ModifyLines(si);

            if (splitMonsterData.Length != (int)EMonsterAttribue.LAST)
            {
                Debug.Log(si + $"does not have {(int)EMonsterAttribue.LAST} values.");
                return;
            }

            string id = splitMonsterData[(int)EMonsterAttribue.ID];

            MonsterScriptable scriptable = AssetDatabase.LoadMainAssetAtPath($"{MonsterScriptablePath + id}.asset")as MonsterScriptable;

            if (scriptable == null)
            {
                scriptable = ScriptableObject.CreateInstance<MonsterScriptable>();
                AssetDatabase.CreateAsset(scriptable, $"{PowerScriptablePath + id}.asset");
            }
            datas.Add(scriptable);

            GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{MonsterPrefabPath + id}.prefab") as GameObject;
            if (prefab != null)
            {
                MonsterScript script = prefab.GetComponentInChildren<MonsterScript>();
                if (script == null) { Debug.LogError("몬스터에 스크립트 없음"); continue; }
                script.SetScriptable(scriptable);
            }

            Sprite profile = Resources.Load<Sprite>($"{MonsterProfilePath + id}_P");
            Sprite body = Resources.Load<Sprite>($"{MonsterBodyImgPath + id}_B");

            scriptable.SetMonsterScriptable(idx, splitMonsterData, dropInfos[id], prefab, profile, body);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
            if (prefab != null) { EditorUtility.SetDirty(prefab); }
        }

        GameObject gameManager = AssetDatabase.LoadMainAssetAtPath(GameManagerPath) as GameObject;
        MonsterManager monManager = gameManager.GetComponentInChildren<MonsterManager>();
        monManager.SetMonsterData(datas);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(gameManager);

        Debug.Log("몬스터 정보 불러오기 완료");
    }

    [MenuItem("Utilities/GenerateItems")]
    private static void GenerateItemData()
    {
        // 아이템 정보
        string[] allItemLines = File.ReadAllLines(CSVPath + ItemCSVName);

        uint[] itemCnt = new uint[(int)EItemType.LAST];

        List<ItemScriptable>[] datas = new List<ItemScriptable>[(int)EItemType.LAST] { new(), new(), new(), new() };

        for (uint i = 1; i<allItemLines.Length; i++)
        {
            string si = allItemLines[i];
            string[] splitItemData = ModifyLines(si);

            if (splitItemData.Length != (int)EItemAttribute.LAST)
            {
                Debug.Log(si + $"does not have {(int)EItemAttribute.LAST} attributes");
                return;
            }

            string id = splitItemData[(int)EItemAttribute.ID];

            ItemScriptable scriptable;
            uint type;
            switch (id[0])
            {
                case ValueDefine.WEAPON_CODE:
                    type = (uint)EItemType.WEAPON;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as WeaponScriptable;
                    if (scriptable == null)
                    {
                        scriptable = ScriptableObject.CreateInstance<WeaponScriptable>();
                        AssetDatabase.CreateAsset(scriptable, $"{ItemScriptablePaths[type] + id}.asset");
                    }
                    break;
                case ValueDefine.PATTERN_CODE:
                    type = (uint)EItemType.PATTERN;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as PatternScriptable;
                    if (scriptable == null)
                    {
                        scriptable = ScriptableObject.CreateInstance<PatternScriptable>();
                        AssetDatabase.CreateAsset(scriptable, $"{ItemScriptablePaths[type] + id}.asset");
                    }
                    break;
                case ValueDefine.THROW_ITEM_CODE:
                    type = (uint)EItemType.THROW;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as ThrowItemScriptable;
                    if (scriptable == null)
                    {
                        scriptable = ScriptableObject.CreateInstance<ThrowItemScriptable>();
                        AssetDatabase.CreateAsset(scriptable, $"{ItemScriptablePaths[type] + id}.asset");
                    }
                    break;
                case ValueDefine.OTHER_ITEM_CODE:
                    type = (uint)EItemType.OTHERS;
                    scriptable = AssetDatabase.LoadMainAssetAtPath($"{ItemScriptablePaths[type] + id}.asset")as OtherItemScriptable;
                    if (scriptable == null)
                    {
                        scriptable = ScriptableObject.CreateInstance<OtherItemScriptable>();
                        AssetDatabase.CreateAsset(scriptable, $"{ItemScriptablePaths[type] + id}.asset");
                    }
                    break;
                default: Debug.LogError("맞는 ID 없음"); return;
            }
            uint idx = itemCnt[type]++;
            datas[type].Add(scriptable);

            string prefabPath = ItemPrefabPaths[type];

            GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{prefabPath + id}.prefab") as GameObject;
            if (prefab != null)
            {
                switch (id[0])
                {
                    case ValueDefine.WEAPON_CODE:
                        WeaponScript weapon = prefab.GetComponent<WeaponScript>();
                        if (weapon != null) { weapon.SetScriptable((WeaponScriptable)scriptable); }
                        break;
                    case ValueDefine.PATTERN_CODE:

                        break;
                    case ValueDefine.THROW_ITEM_CODE:
                        ThrowItemScript throwItem = prefab.GetComponent<ThrowItemScript>();
                        if (throwItem != null) { throwItem.SetScriptable((ThrowItemScriptable)scriptable); }
                        break;
                    case ValueDefine.OTHER_ITEM_CODE:

                        break;
                    default: Debug.LogError("맞는 ID 없음"); return;
                }
            }


            Sprite image = Resources.Load<Sprite>($"{ItemImgPath[type] + id}");

            scriptable.SetItemScriptable(idx, splitItemData, prefab, image);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
            if (prefab != null) { EditorUtility.SetDirty(prefab); }
        }

        GameObject gameManager = AssetDatabase.LoadMainAssetAtPath(GameManagerPath) as GameObject;
        ItemManager itemManager = gameManager.GetComponentInChildren<ItemManager>();
        itemManager.SetItemData(datas);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(gameManager);

        Debug.Log("아이템 정보 불러오기 완료");
    }

    [MenuItem("Utilities/GeneratePowers")]
    private static void GeneratePowerData()
    {
        // 스킬 정보
        string[] allPowerLines = File.ReadAllLines(CSVPath + PowerCSVName);

        List<PowerScriptable> datas = new();

        for (uint i = 1; i < allPowerLines.Length; i++)
        {
            uint idx = i - 1;
            string si = allPowerLines[i];
            string[] splitPowerData = ModifyLines(si);

            if (splitPowerData.Length != (int)EPowerAttribute.LAST)
            {
                Debug.Log(si + $"does not have {(int)EPowerAttribute.LAST} values.");
                return;
            }

            string id = splitPowerData[(int)EPowerAttribute.ID];

            PowerScriptable scriptable = AssetDatabase.LoadMainAssetAtPath($"{PowerScriptablePath + id}.asset")as PowerScriptable;

            if (scriptable == null)
            {
                scriptable = ScriptableObject.CreateInstance<PowerScriptable>();
                AssetDatabase.CreateAsset(scriptable, $"{PowerScriptablePath + id}.asset");
            }
            datas.Add(scriptable);

            GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{PowerPrefabPath + id}.prefab") as GameObject;
            if (prefab != null)
            {
                PlayerPowerScript script = prefab.GetComponentInChildren<PlayerPowerScript>();
                if (script == null) { Debug.LogError("스킬에 스크립트 없음"); continue; }
                script.SetScriptable(scriptable);
            }

            Sprite icon = Resources.Load<Sprite>($"{PowerIconPath + id}");

            scriptable.SetPowerScriptable(idx, splitPowerData, prefab, icon);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
            if (prefab != null) { EditorUtility.SetDirty(prefab); }
        }

        GameObject gameManager = AssetDatabase.LoadMainAssetAtPath(GameManagerPath) as GameObject;
        PowerManager powerManager = gameManager.GetComponentInChildren<PowerManager>();
        powerManager.SetPowerData(datas);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(gameManager);

        Debug.Log("권능 정보 불러오기 완료");
    }

    [MenuItem("Utilities/GenerateStory")]
    private static void GenerateStory()
    {
        // 대화 정보
        string[] allDialogueLines = File.ReadAllLines(CSVPath + DialogueCSVName);
        List<DialogueScriptable> dialogueData = new();

        string dialNPC = "";

        uint dialIdx = 0;
        for (uint i = 1; i < allDialogueLines.Length; i++)
        {
            string si = allDialogueLines[i];
            string[] splitDialogueData = ModifyLines(si);

            string newNPC = splitDialogueData[(int)EDialogueAttributes.NPC];
            dialNPC = newNPC != "" ? newNPC : dialNPC;
            int.TryParse(splitDialogueData[(int)EDialogueAttributes.DIALOGUE_IDX], out int dialogueIdx);
            List<string[]> datas = new();
            while (i < allDialogueLines.Length)
            {
                datas.Add(splitDialogueData);
                if (i == allDialogueLines.Length - 1) { break; }
                si = allDialogueLines[++i];
                splitDialogueData = ModifyLines(si);
                if (splitDialogueData[(int)EDialogueAttributes.DIALOGUE_IDX] != "") { i--; break; }
            }

            string folderPath = $"{DialogueScriptablePath}/{dialNPC}";
            // 폴더가 이미 존재하는지 확인
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(DialogueScriptablePath[..(DialogueScriptablePath.Length-1)], dialNPC);
                AssetDatabase.SaveAssets();
            }

            string code = $"{dialNPC}_{dialogueIdx+1}";

            DialogueScriptable scriptable = AssetDatabase.LoadMainAssetAtPath($"{folderPath}/{code}.asset") as DialogueScriptable;

            if (scriptable == null)
            {
                scriptable = ScriptableObject.CreateInstance<DialogueScriptable>();
                AssetDatabase.CreateAsset(scriptable, $"{folderPath}/{code}.asset");
            }
            dialogueData.Add(scriptable);

            scriptable.SetScriptable(dialIdx++, dialNPC, datas);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
        }


        // 퀘스트 정보
        string[] allQuestLines = File.ReadAllLines(CSVPath + QuestCSVName);

        List<QuestScriptable> questData = new();

        for (uint i = 1; i < allQuestLines.Length; i++)
        {
            uint idx = i - 1;
            string si = allQuestLines[i];
            string[] splitQuestData = ModifyLines(si);

            if (splitQuestData.Length != (int)EQuestAttributes.LAST)
            {
                Debug.Log(si + $"does not have {(int)EQuestAttributes.LAST} values.");
                return;
            }

            string id = splitQuestData[(int)EQuestAttributes.ID];

            QuestScriptable scriptable = AssetDatabase.LoadMainAssetAtPath($"{QuestScriptablePath + id}.asset") as QuestScriptable;

            if (scriptable == null)
            {
                scriptable = ScriptableObject.CreateInstance<QuestScriptable>();
                AssetDatabase.CreateAsset(scriptable, $"{QuestScriptablePath + id}.asset");
            }
            questData.Add(scriptable);

            scriptable.SetQuestScriptable(idx, splitQuestData);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
        }

        foreach (QuestScriptable quest in questData)                    // 이어지는 퀘스트 추가
        {
            List<NPCDialogue> questDials = quest.ResultDialogues;
            foreach (NPCDialogue questDial in questDials)
            {
                DialogueScriptable curDial = null;
                foreach (DialogueScriptable compDial in dialogueData)
                {
                    if (questDial == compDial.NPCDial) { curDial = compDial; break; }
                }
                if (curDial == null) { break; }
                foreach (DialLine line in curDial.Lines)
                {
                    DialQuest dialQuest = line.ResultQuest;
                    EQuestName dialResultQuest = dialQuest.Quest;
                    if (line.HasQuest && dialQuest.Function == EDialQuestFunction.START && !quest.ResultQuests.Contains(dialResultQuest))
                    {
                        quest.AddResultQuest(dialResultQuest);
                    }
                }
            }
        }
        foreach (DialogueScriptable dial in dialogueData)       // 최초 시작 대화 설정1
        {
            foreach (DialLine line in dial.Lines)
            {
                foreach (NPCDialogue result in line.ResultDialogues)
                {
                    foreach (DialogueScriptable data in dialogueData)
                    {
                        if (dial == data || !data.IsOpenAtFirst) { continue; }
                        if (data.NPCDial == result) { data.DisableFirstOpen(); break; }
                    }
                }
            }
        }
        foreach (QuestScriptable quest in questData)            // 최초 시작 대화 설정2
        {
            foreach (NPCDialogue result in quest.ResultDialogues)
            {
                foreach (DialogueScriptable data in dialogueData)
                {
                    if (!data.IsOpenAtFirst) { continue; }
                    if (data.NPCDial == result) { data.DisableFirstOpen(); break; }
                }
            }
        }

        GameObject hellMap = AssetDatabase.LoadMainAssetAtPath(HellMapPath) as GameObject;
        OasisNPC[] oasisList = hellMap.GetComponentsInChildren<OasisNPC>();
        if (oasisList.Length != (int)EOasisName.LAST) { Debug.LogError("맵에 오아시스 개수 다름"); return; }
        AltarScript[] altarList = hellMap.GetComponentsInChildren<AltarScript>();
        if (altarList.Length != (int)EAltarName.LAST) { Debug.LogError("맵에 제단 개수 다름"); return; }
        SlateScript[] slateList = hellMap.GetComponentsInChildren<SlateScript>();
        if (slateList.Length != (int)ESlateName.LAST) { Debug.LogError("맵에 석판 개수 다름"); return; }

        // NPC 정보
        string[] allNPCLines = File.ReadAllLines(CSVPath + NPCCSVName);

        List<NPCScriptable> npcData = new();

        for (uint i = 1; i < allNPCLines.Length; i++)
        {
            uint idx = i - 1;
            string si = allNPCLines[i];
            string[] splitNPCData = ModifyLines(si);

            if (splitNPCData.Length != (int)ENPCAttribute.LAST)
            {
                Debug.Log(si + $"NPC 데이터 줄 개수 모자람");
                return;
            }

            string npc = splitNPCData[(int)ENPCAttribute.SNPC];
            ENPCType npcType = StoryManager.String2NPC(npc).Type;

            NPCScriptable scriptable;
            if (npcType == ENPCType.OASIS) { scriptable = AssetDatabase.LoadMainAssetAtPath($"{NPCScriptablePath + npc}.asset") as OasisScriptable; }
            else { scriptable = AssetDatabase.LoadMainAssetAtPath($"{NPCScriptablePath + npc}.asset") as NPCScriptable; }

            bool IsExist = scriptable != null;
            if (!IsExist) 
            {
                if (npcType == ENPCType.OASIS) {scriptable =ScriptableObject.CreateInstance<OasisScriptable>(); }
                else { scriptable = ScriptableObject.CreateInstance<NPCScriptable>(); }
            }

            scriptable.SetNPCScriptable(idx, splitNPCData);

            foreach (DialogueScriptable dial in dialogueData)               // 대화, 퀘스트 중 NPC에 해당하는 거 추가
            {
                if (dial.NPC == scriptable.NPC)
                {
                    scriptable.AddDialogue(dial);

                    foreach (DialLine line in dial.Lines)
                    {
                        DialQuest resultQuests = line.ResultQuest;
                        if (resultQuests.Function == EDialQuestFunction.START)
                        {
                            scriptable.AddQuest(questData[(int)resultQuests.Quest]);
                        }
                    }
                }
            }


            if (!IsExist)
            {
                AssetDatabase.CreateAsset(scriptable, $"{NPCScriptablePath + npc}.asset");
            }

            if (npcType == ENPCType.OASIS)
            {
                uint oasisIdx = idx;
                oasisList[oasisIdx].SetOasis(oasisIdx, scriptable);
            }
            else if (npcType == ENPCType.ALTAR)
            {
                uint altarIdx = idx - (int)EOasisName.LAST;
                altarList[altarIdx].SetAltar(altarIdx, scriptable);
            }
            else if (npcType == ENPCType.SLATE)
            {
                uint slateIdx = idx - (int)EOasisName.LAST - (int)EAltarName.LAST;
                slateList[slateIdx].SetSlate(slateIdx, scriptable);
            }
            else
            {
                //GameObject prefab = AssetDatabase.LoadMainAssetAtPath($"{NPCPrefabPath + npc}.prefab") as GameObject;
                //if (prefab == null) { continue; }
                //if (!prefab.TryGetComponent<NPCScript>(out var script)) { Debug.Log(npc + " 스크립트 없음"); continue; }
                //script.SetScriptable(scriptable);
                //AssetDatabase.SaveAssets();
                //EditorUtility.SetDirty(prefab);
            }

            npcData.Add(scriptable);

            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
        }

        EditorUtility.SetDirty(hellMap);

        GameObject gameManager = AssetDatabase.LoadMainAssetAtPath(GameManagerPath) as GameObject;
        StoryManager storyManager = gameManager.GetComponentInChildren<StoryManager>();
        storyManager.SetNPCData(npcData);
        storyManager.SetDialogueData(dialogueData);
        storyManager.SetQuestData(questData);

        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(gameManager);

        Debug.Log("스토리 정보 불러오기 완료");
    }
}
#endif