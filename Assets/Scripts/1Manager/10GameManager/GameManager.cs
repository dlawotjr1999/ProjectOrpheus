using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Inst;

    public static bool IsInTitle { get { return SceneManager.GetActiveScene().buildIndex == ValueDefine.TITLE_SCENE_IDX; } }
    public static bool IsInGame { get { return SceneManager.GetActiveScene().buildIndex > ValueDefine.LOADING_SCENE_IDX
                && SceneManager.GetActiveScene().buildIndex <= ValueDefine.HELL_SCENE_IDX; /*임시 조건*/ } }
    public static void StartGame()
    {
        MoveToLoading(null);
    }
    public static void LoadGame(int _idx)
    {
        SaveData data = GameData[_idx];
        MoveToLoading(data);
    }
    private static void MoveToLoading(SaveData _data)
    {
        PlayManager.SetCurData(_data);
        SceneManager.LoadScene(ValueDefine.LOADING_SCENE_IDX);
    }
    public static void ReturnToTitle()
    {
        SceneManager.LoadScene(ValueDefine.TITLE_SCENE_IDX);
    }


    // 데이터
    private DataManager m_dataManager;
    public static DataManager DataManager { get { return Inst.m_dataManager; } }
    public static List<SaveData> GameData { get { return DataManager.GameData; } }                                                          // 저장된 것들
    public static void SaveGameData(EOasisName _oasis) { DataManager.SaveCurData(_oasis); }                                                 // 오아시스에서 저장
    public static void AddGameData(SaveData _data) { DataManager.AddGameData(_data); }                                                      // 저장 데이터 추가
    public static void RegisterData(IHaveData _data) { DataManager.RegisterData(_data); }                                                   // 데이터에 등록(기록 시작)


    // 화면
    private DisplayManager m_displayManager;
    public static DisplayManager DisplayManager { get { return Inst.m_displayManager; } }
    public static float WidthRatio { get { return DisplayManager.WidthRatio; } }                                                            // 화면 너비 비율
    public static float HeightRatio { get { return DisplayManager.HeightRatio; } }                                                          // 화면 높이 비율


    // 소리
    private SoundManager m_soundManager;
    public static SoundManager SoundManager { get { return Inst.m_soundManager; } }
    public static float BGMVolume { get { return SoundManager.BGMVolume; } }
    public static float SEVolume { get { return SoundManager.SEVolume; } }
    private static GameObject SEPrefab { get { return SoundManager.SEPrefab; } }
    public static void SetBGMVolume(int _volume) { SoundManager.SetBGMVolume(_volume); }
    public static void SetSEVolume(int _volume) { SoundManager.SetSEVolume(_volume); }
    public static void PlayBGM(EBGM _bgm) { SoundManager.PlayBGM(_bgm); }
    public static void ChangeBGM(EBGM _bgm) { SoundManager.ChangeBGM(_bgm); }
    public static void StopBGM() { SoundManager.StopBGM(); }
    public static void PlaySE(EPlayerSE _se) { SoundManager.PlaySE(_se); }
    public static void PlaySE(EPlayerSE _se, Vector3 _point) { SoundManager.PlaySE(_se, _point); }
    public static void PlaySE(EMonsterSE _se) { SoundManager.PlaySE(_se); }
    public static void PlaySE(EMonsterSE _se, Vector3 _point) { SoundManager.PlaySE(_se, _point); }
    public static void PlaySE(EPowerSE _se) { SoundManager.PlaySE(_se); }
    public static void PlaySE(EPowerSE _se, Vector3 _point) { SoundManager.PlaySE(_se, _point); }
    public static void PlaySE(EEnvironmentSE _se) { SoundManager.PlaySE(_se); }
    public static void PlaySE(EEnvironmentSE _se, Vector3 _point) { SoundManager.PlaySE(_se, _point); }
    public static void PlaySE(ESystemSE _se) { SoundManager.PlaySE(_se); }
    public static void PlaySE(AudioClip _clip, Vector3 _point) { SoundManager.PlaySE(_clip, _point); }


    // 입력
    private InputManager m_inputManager;
    public static InputManager InputManager { get { return Inst.m_inputManager; } }
    public static InputSystem.PlayerActions PlayerInputs { get { return InputManager.PlayerInputs; } }                                      // 플레이어 Input
    public static InputSystem.UIControlActions UIControlInputs { get { return InputManager.UIControlInputs; } }                             // UI조작 Input
    public static EControlMode ControlMode { get { return InputManager.CurControlMode; } }                                                  // 조작 모드
    public static float MouseSensitive { get { return InputManager.MouseSensitive; } }                                                      // 마우스 민감도
    public static void SetControlMode(EControlMode _mode) { InputManager.SetControlMode(_mode); }                                           // 조작 모드 변경
    public static void SetMouseSensitive(float _sensitive) { InputManager.SetMouseSensitive(_sensitive); }                                  // 마우스 민감도 설정


    // 아이템
    private ItemManager m_itemManager;
    private static ItemManager ItemManager { get { return Inst.m_itemManager; } }
    private static GameObject[] ItemArray { get { return ItemManager.ItemArray; } }
    public static ItemInfo GetItemInfo(SItem _item) { return ItemManager.GetItemInfo(_item); }                                              // 아이템 정보
    public static ItemScriptable GetItemData(SItem _item) { return ItemManager.GetItemData(_item); }                                        // 아이템 스크립터블
    public static ItemInfo GetWeaponInfo(EWeaponName _weapon) { return GetItemInfo(new SItem(EItemType.WEAPON, (int)_weapon)); }            // 무기 정보
    public static GameObject GetThorwItemPrefab(EThrowItemName _item) { return ItemManager.GetThrowItemPrefab(_item); }                     // 투척 아이템 프리펍
    public static GameObject GetDropItemPrefab(EItemType _item) { return ItemManager.GetDropItemPrefab(_item); }                            // 드랍 아이템 프리펍

    // 권능
    private PowerManager m_powerManager;
    private static PowerManager PowerManager { get { return Inst.m_powerManager; } }
    private static GameObject[] PowerArray { get { return PowerManager.PowerArrays; } }
    public static PowerInfo GetPowerInfo(EPowerName _power) { return PowerManager.GetPowerInfo(_power); }                                   // 스킬 정보
    public static PowerScriptable GetPowerData(EPowerName _power) { return PowerManager.GetPowerData(_power); }                             // 스킬 스크립터블
    public static GameObject GetPowerObj(EPowerName _power) { return PowerManager.GetPowerObj(_power); }                                    // 스킬 프리펍


    // 몬스터
    private MonsterManager m_monsterManager;
    private static MonsterManager MonsterManager { get { return Inst.m_monsterManager; } }
    public static GameObject[] MonsterArray { get { return MonsterManager.MonsterArray; } }
    public static MonsterInfo GetMonsterInfo(EMonsterName _monster) { return MonsterManager.GetMonsterInfo(_monster); }                     // 몬스터 정보
    public static MonsterScriptable[] MonsterData { get { return MonsterManager.MonsterData; } }
    public static MonsterScriptable GetMonsterData(EMonsterName _monster) { return MonsterManager.GetMonsterData(_monster); }               // 몬스터 스크립터블
    public static GameObject GetMonsterObj(EMonsterName _monster) { return MonsterManager.GetMonsterObj(_monster); }                        // 몬스터 프리펍
    public static GameObject GetWolfPeckPrefab(Vector3 _position) { return MonsterManager.GetWolfPeckPrefab(_position); }
    public static bool CheckNClearMonster(EMonsterName _monster) { return MonsterManager.CheckNClearMonster(_monster); }                    // 최초 처치 확인



    // 이펙트
    private EffectManager m_effectManager;
    private static EffectManager EffectManager { get { return Inst.m_effectManager; } }
    private static GameObject[] EffectArray { get { return EffectManager.EffectArray; } }
    public static GameObject GetEffectObj(EEffectName _effect) { return EffectManager.GetEffectObj(_effect); }                              // 이펙트 오브젝트 받기


    // 스토리
    private StoryManager m_storyManager;
    private static StoryManager StoryManager { get { return Inst.m_storyManager; } }
    public static NPCScriptable GetNPCData(SNPC _npc) { return StoryManager.GetNPCData(_npc); }                                             // NPC 정보
    public static DialogueScriptable GetDialogueData(EDialogueName _name) { return StoryManager.GetDialogueData(_name); }                   // 대화 정보
    public static DialogueScriptable GetDialogueData(SNPC _npc, int _idx) { return StoryManager.GetDialogueData(_npc, _idx); }              // 대화 정보
    public static QuestScriptable GetQeustData(EQuestName _quest) { return StoryManager.GetQuestData(_quest); }                             // 퀘스트 정보

    // UI
    private UIManager m_uiManager;
    public static UIManager UIManager { get { return Inst.m_uiManager; } }
    public static Sprite GetMonsterProfile(EMonsterName _monster) { return UIManager.GetMonsterProfile(_monster); }                         // 몬스터 프로필
    public static Sprite GetMonsterBodyImg(EMonsterName _monster) { return UIManager.GetMonsterBodyImg(_monster); }                         // 몬스터 전신
    public static Sprite GetItemSprite(SItem _item) { return UIManager.GetItemSprite(_item); }                                              // 아이템 이미지
    public static Sprite GetPowerSprite(EPowerName _power) { return UIManager.GetPowerSprite(_power); }                                     // 스킬 이미지


    private PoolManager m_poolManager;
    private static PoolManager PoolManager { get { return Inst.m_poolManager; } }



    private void SetSubManagers()
    {
        m_dataManager = GetComponent<DataManager>();
        m_dataManager.SetManager();
        m_displayManager = GetComponent<DisplayManager>();
        m_soundManager = GetComponent<SoundManager>();
        m_inputManager = GetComponent<InputManager>();
        m_inputManager.SetManager();
        m_itemManager = GetComponent<ItemManager>();
        m_itemManager.SetManager();
        m_powerManager = GetComponent<PowerManager>();
        m_powerManager.SetManager();
        m_monsterManager = GetComponent<MonsterManager>();
        m_monsterManager.SetManager();
        m_effectManager = GetComponent<EffectManager>();
        m_effectManager.SetManager();
        m_storyManager = GetComponent<StoryManager>();
        m_uiManager = GetComponent<UIManager>();
        m_poolManager = GetComponent<PoolManager>();
        m_poolManager.SetManager(ItemArray, PowerArray, MonsterArray, EffectArray, SEPrefab);
    }

    private void Awake()
    {
        if (Inst != null) { Destroy(gameObject); return; }

        Inst = this;
        DontDestroyOnLoad(gameObject);

        SetSubManagers();
    }
}
