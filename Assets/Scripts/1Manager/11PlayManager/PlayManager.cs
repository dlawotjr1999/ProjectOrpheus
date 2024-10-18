using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Inst;


    // 메인 함수
    public static bool IsPlaying { get; private set; }                // 플레이 중인지
    public static SaveData CurSaveData { get; private set; }
    public static bool IsNewData { get { return CurSaveData == null; } }
    private static bool IsFromTitle { get; set; }
    public static void SetCurData(SaveData _data) { CurSaveData = _data; IsFromTitle = true; }
    private static void StartPlay()
    {
        if (IsNewData && IsFromTitle) 
        {
            CurSaveData = new();
            GameManager.AddGameData(CurSaveData);
        }
        IsPlaying = true;
        GameManager.SetControlMode(EControlMode.THIRD_PERSON);
        GameManager.SetMouseSensitive(1);
    }
    public static void EndPlay()
    {
        IsPlaying = false;
        IsFromTitle = false;
    }
    public static void RestAtPoint(OasisNPC _oasis)
    {
        StartBlackoutUI();
        Player.RestAnimation();
        GameManager.PlaySE(EEnvironmentSE.REST);
        Inst.StartCoroutine(Inst.RestDoneCoroutine(_oasis));
    }
    private IEnumerator RestDoneCoroutine(OasisNPC _oasis)
    {
        yield return new WaitForSeconds(2);
        TeleportPlayer(_oasis.RespawnPoint);
        Player.RestorePlayer();
        yield return new WaitForSeconds(1);
        EndBlackoutUI();
        GameManager.SaveGameData(_oasis.PointName);
    }
    public static void TransportToOasis(EOasisName _target)
    {
        StartBlackoutUI();
        GameManager.PlaySE(EEnvironmentSE.TRANSPORT);
        Inst.StartCoroutine(Inst.TransportCoroutine(_target));
    }
    private IEnumerator TransportCoroutine(EOasisName _oasis)
    {
        yield return new WaitForSeconds(2);
        TeleportPlayer(OasisList[(int)_oasis].RespawnPoint);
        yield return new WaitForSeconds(1);
        EndBlackoutUI();
    }
    public static void PlayerDead()
    {
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
        ShowDeathUI();
    }
    public static void RestartGame()
    {
        GameManager.LoadGame(0);
    }


    // 플레이어
    private static PlayerController Player { get; set; }
    public static void SetCurPlayer(PlayerController _player) { Player = _player; }                                                         // 플레이어 등록
    public static bool CheckIsPlayer(ObjectScript _object) { return _object == Player; }                                                    // 플레이어인지 확인
    public static bool IsPlayerSet { get { return Player != null; } }                                                                       // 플레이어 등록 여부
    public static bool IsPlayerGuarding { get { return Player.IsGuarding; } }                                                               // 플레이어 가드 중
    public static bool IsPlayerDead { get { return Player.IsDead; } }                                                                       // 플레이어 사망 여부
    public static bool IsPlayerLightOn { get { return Player.IsLightOn; } }                                                                 // 플레이어 능력 사용중
    public static Vector3 PlayerPos { get { if (IsPlayerSet) return Player.transform.position; return ValueDefine.NULL_VECTOR; } }            // 플레이어 위치
    public static Vector2 PlayerPos2 { get { if (IsPlayerSet) return Player.Position2; return ValueDefine.NULL_VECTOR; } }                   // 플레이어 평면 위치
    public static float PlayerDirection { get { return Player.Direction; } }
    public static Vector2 PlayerAimDirection { get { return Player.PlayerAimDirection; } }                                                  // 카메라 조준 벡터
    public static Transform PlayerTransform { get { if (IsPlayerSet) return Player.transform; return null; } }
    public static Transform CameraFocusTransform { get { if (IsPlayerSet) { return Player.CameraFocus; } return null; } }
    public static PlayerStatInfo PlayerStatInfo { get { return Player.GetStatInfo(); } }
    public static void ApplyStatReset() { Player.ResetStatInfo(); }
    public static void ApplyPlayerStat() { Player.ApplyStat(); }
    public static SPlayerWeaponInfo PlayerWeaponInfo { get { return Player.CurWeaponInfo; } }
    public static float GetDistToPlayer(Vector2 _pos) { if (!IsPlayerSet) return -1; return (PlayerPos2-_pos).magnitude; }                   // 플레이어와의 거리
    public static void SetPlayerWeapon(EWeaponName _weapon) { Player.SetCurWeapon(_weapon); }                                               // 무기 설정
    public static void StopPlayerInteract() { Player.StopInteract(); }                                                                      // 상호작용 종료
    public static void StopPlayerInteract(InteractScript _interact) { Player.StopInteract(_interact); }
    public static void ResetPlayer() { Player.ResetPlayerAction(); }
    public static void TeleportPlayer(Vector3 _pos) { Player.TeleportPlayer(_pos); }
    public static void GetPlayerAdjust(AdjustInfo _adjust) { Player.GetAdjust(_adjust); }
    public static void GetPlayerAdjust(AdjustInfo _adjust, ObjectAttackScript _attack) { Player.GetAdjust(_adjust, _attack); }
    public static void ModifyPlayerAdjust(ObjectAttackScript _attack, float _time) { Player.ModifyAdjust(_attack, _time); }
    public static void CreateImpulse(float _impulse) { Player.GetImpulse(_impulse); }
    public static void TempGetBuff(float _amount, float _time) { Player.GetAdjust(new(EAdjType.MAX_HP, _amount, _time)); }


    // 카메라
    [SerializeField]
    private CameraManager m_cameraManager;
    private static CameraManager CameraManager { get { return Inst.m_cameraManager; } }
    public static CinemachineFreeLook PlayerFreeLook { get { return CameraManager.PlayerFreeLook; } }
    public static float CameraRotation { get { return CameraManager.CameraRotation; } }                                                     // 카메라 좌우 각도
    public static float CameraAngle { get { return CameraManager.CameraAngle; } }                                                           // 카메라 위아래 각도
    public static void SetCameraMode(EControlMode _mode) { CameraManager.SetCameraMode(_mode); }                                            // 조작 모드 전달
    public static void SetNPCView() { CameraManager.SetNPCView(); }
    public static void SetCameraSensitive(float _sensitive) { CameraManager.SetCameraSensitive(_sensitive); }                               // 마우스 민감도 전달
    public static void LooseCameraFocus() { CameraManager.LooseFocus(); }


    // 인벤토리
    private InventoryManager m_invenManager;
    private static InventoryManager InvenManager { get { return Inst.m_invenManager; } }
    public static InventoryElm[] PlayerInventory { get { return InvenManager.Inventory; } }                                                     // 인벤토리 아이템 목록
    private static void InventoryEditted() { UpdateInfoUI(); }
    public static void AddInventoryItem(SItem _item, int _num) { InvenManager.AddInventoryItem(_item, _num); }                                  // 빈 인벤토리에 아이템 추가
    public static void AddInventoryItem(SItem _item, int _num, bool _isNew) { InvenManager.AddInventoryItem(_item, _num, _isNew); }             // 빈 인벤토리에 아이템 추가 (신규)
    public static void SetInventoryItem(int _idx, SItem _item, int _num) { InvenManager.SetInventoryItem(_idx, _item, _num); }                  // 인벤토리 해당 Idx에 아이템 설정
    public static void RemoveInventoryItem(int _idx) { InvenManager.RemoveInventoryItem(_idx); }                                                // 인벤토리 해당 Idx 아이템 제거
    public static void SwapItemInven(int _idx1, int _idx2) { InvenManager.SwapItemInven(_idx1, _idx2); InventoryEditted(); }
    public static bool[] WeaponObtained { get { return InvenManager.WeaponObatined; } }
    public static EWeaponName CurWeapon { get { return InvenManager.CurWeapon; } }                                                              // 장착 중인 무기
    public static void ObtainWeapon(EWeaponName _weapon) { InvenManager.ObtainWeapon(_weapon); }
    public static void SetCurWeapon(EWeaponName _weapon) { InvenManager.SetCurWeapon(_weapon); }                                                // 무기 설정
    public static void EquipWeapon(EWeaponName _weapon) { InvenManager.EquipWeapon(_weapon); }                                                  // 무기 장착
    public static EPatternName CurHealPattern { get { return InvenManager.CurHealPattern; } }                                                   // 현재 회복 아이템
    public static EPatternName[] HealPatternList { get { return InvenManager.HealPatternList; } }                                               // 등록된 회복 아이템
    private static void HealPatternEditted() { UpdateInfoUI(); UpdateHealItemSlot(); }
    public static void UseHealPattern() { InvenManager.UseHealItem(); HealPatternEditted(); }                                                   // 회복 아이템 사용
    public static void RegisterHealPattern(EPatternName _pattern) { InvenManager.RegisterHealItem(_pattern); HealPatternEditted(); }            // 등록
    public static EThrowItemName CurThrowItem { get { return InvenManager.CurThrowItem; } }                                                     // 현재 던지기 아이템 (LAST == null)
    public static List<EThrowItemName> ThrowItemList { get { return InvenManager.ThrowItemList; } }                                             // 등록된 던지기 아이템
    private static void ThrowItemEditted() { UpdateInfoUI(); UpdateThrowItemSlot(); }

    public static void UseThrowItem() { InvenManager.UseThrowItem(); UpdateInfoUI(); UpdateThrowItemSlot(); }                                   // 던지기 아이템 사용
    public static void AddThrowItem(EThrowItemName _item) { InvenManager.AddThrowItem(_item); ThrowItemEditted(); }                             // 던지기 아이템 추가
    public static void SetThrowItem(int _idx, EThrowItemName _item) { InvenManager.SetThrowItem(_idx, _item); ThrowItemEditted(); }             // 위치 지정
    public static void SwapThrowItem(int _idx1, int _idx2) { InvenManager.SwapThrowItem(_idx1, _idx2); ThrowItemEditted(); }                    // 바꾸기
    public static void RemoveThrowItem(int _idx) { InvenManager.RemoveThrowItem(_idx); ThrowItemEditted(); }                                    // 제거
    public static int SoulNum { get { return InvenManager.SoulNum; } }                                                                          // 영혼 개수
    public static int PurifiedNum { get { return InvenManager.PurifiedNum; } }                                                                  // 성불 영혼 개수
    public static int[] PatternNum { get { return InvenManager.PatternNum; } }                                                                  // 문양별 개수
    public static void AddSoul(int _num) { InvenManager.AddSoul(_num); }                                                                        // 영혼 추가
    public static void AddPurified(int _num) { InvenManager.AddPurified(_num); }                                                                // 성불 영혼 추가
    public static void UseSoul(int _num) { InvenManager.LooseSoul(_num); }                                                                      // 영혼 사용
    public static void LooseSoul(int _num, bool _absorbed) { InvenManager.LooseSoul(_num, _absorbed); }                                         // 영혼 흡수 당함
    public static void UsePurified(int _num) { InvenManager.UsePurified(_num); }                                                                // 성불 영혼 사용

    // 스토리
    private QuestManager m_questManager;
    private static QuestManager QuestManager { get { return Inst.m_questManager; } }
    public static List<QuestInfo> QuestInfoList { get { return QuestManager.QuestInfoList; } }
    public static void SetQuestStatus(EQuestName _quest, EQuestState _status) { QuestManager.SetQuestStatus(_quest, _status); }
    public static void SetQuestProgress(EQuestName _quest, float _prog) { QuestManager.SetQuestProgress(_quest, _prog); }
    public static void GiveUpQuest(EQuestName _quest) { QuestManager.GiveUpQuest(_quest); }


    // 환경
    private EnvironmentManager m_environmentManager;
    private static EnvironmentManager EnvironmentManager { get { return Inst.m_environmentManager; } }
    public static Vector3 MapLB { get { return EnvironmentManager.MapLB; } }
    public static Vector3 MapRT { get { return EnvironmentManager.MapRT; } }
    public static float MapWidth { get { return EnvironmentManager.MapWidth; } }
    public static float MapHeight { get { return EnvironmentManager.MapHeight; } }
    public static OasisNPC[] OasisList { get { return EnvironmentManager.OasisList; } }
    public static bool[] OasisVisited { get { return EnvironmentManager.OasisVisited; } }
    public static AltarScript[] AltarList { get { return EnvironmentManager.AltarList; } }
    public static SlateScript[] SlateList { get { return EnvironmentManager.SlateList; } }
    public static MonsterSpawnPoint[] SpawnPointList { get { return EnvironmentManager.SpawnPointList; } }
    public static void VisitOasis(EOasisName _oasis) { EnvironmentManager.VisitOasis(_oasis); }
    public static void MonsterKilled(EMonsterName _monster, EMonsterDeathType _type) { EnvironmentManager.MonsterKilled(_monster, _type); }
    public static void UnlockDialogue(NPCDialogue _dial) { EnvironmentManager.UnlockDialogue(_dial); }

    public static void TempSetNPCs(NPCScript[] _list) { EnvironmentManager.TempSetNPCs(_list); }                // 임시 NPC 설정


    // 플레이어 능력치, 권능
    private PlayerForceManager m_forceManager;
    private static PlayerForceManager ForceManager { get { return Inst.m_forceManager; } }
    public static int LeftStatPoint { get { return ForceManager.LeftStatPoint; } }
    public static int UsedStatPoint { get { return ForceManager.UsedStatPoint; } }
    public static void AddStatPoint(int _add) { ForceManager.AddStatPoint(_add); }
    public static void UpgradeStat(int[] _point) { ForceManager.UpgradeStat(_point); }                                                                  // 포인트 투자로 인한 업그레이드
    public static void UpgradeStat(EStatName _stat, int _amount) { ForceManager.UpgradeStat(_stat, _amount, true); }                                    // 특정 스탯으로 업그레이드
    public static void ResetStat() { ForceManager.ResetStat(); }
    public static bool[] PowerObtained { get { return ForceManager.PowerObtained; } }
    public static EPowerName[] PowerSlot { get { return ForceManager.PowerSlot; } }                                                                     // 권능 슬롯
    public static void RegisterPowerSlot(EPowerName _popwer, int _idx) { ForceManager.RegisterPowerSlot(_popwer, _idx); UpdatePowerSlot(); }            // 권능 슬롯 등록
    public static void ObtainPower(EPowerName _power) { ForceManager.ObtainPower(_power); }


    // GUI
    private PlayUIManager m_playUIManager;
    private static PlayUIManager PlayUIManager { get { return Inst.m_playUIManager; } }

    // 온오프 UI
    public static bool IsOptionOpen { get { return PlayUIManager.IsOptionOpen; } }
    public static void ToggleOptionUI(bool _on) { PlayUIManager.ToggleOptionUI(_on); }                                                                  // 옵션 UI 여닫기
    public static bool IsPlayerUIOpen { get { return PlayUIManager.IsPlayerUIOpen; } }
    public static void TogglePlayerUI(bool _on) { PlayUIManager.TogglePlayerUI(_on); }                                                      // 플레이어 인포 UI 업데이트
    public static void UpdateInfoUI() { PlayUIManager.UpdateInfoUI(); }                                                                     // 재화 업데이트
    public static void UpdateMaterials() { PlayUIManager.UpdateMaterials(); }                                                               // 맵 UI 여닫기
    public static bool IsMapUIOpen { get { return PlayUIManager.IsMapUIOpen; } }
    public static void ToggleMapUI(bool _on) { PlayUIManager.ToggleMapUI(_on); }                                                            // 퀘스트 창 여닫기
    public static bool IsQuestUIOpen { get { return PlayUIManager.IsQuestUIOpen; } }
    public static void ToggleQuestUI(bool _on) { PlayUIManager.ToggleQuestUI(_on); }                                                        // 퀘스트 창 여닫기
    public static void OpenOasisUI(OasisNPC _npc) { PlayUIManager.OpenOasisUI(_npc); }                                                      // 오아시스 UI 열기
    public static void CloseOasisUI() { PlayUIManager.CloseOasisUI(); }                                                                     // 오아시스 UI 닫기

    // 메인 캔버스 상시 UI
    public static void SetPlayerMaxHP(float _hp) { PlayUIManager.SetMaxHP(_hp); }                                                                       // 체력바 최대 체력
    public static void SetPlayerCurHP(float _hp) { PlayUIManager.SetCurHP(_hp); }                                                                       // 체력바 현재 체력
    public static void UpdatePowerSlot() { PlayUIManager.UpdatePowerSlot(); }                                                                           // 스킬 슬롯 UI
    public static void UsePowerSlot(int _idx, float _cooltime) { PlayUIManager.UsePowerSlot(_idx, _cooltime); }                                         // 스킬 쿨타임 진행
    public static void UpdateThrowItemSlot() { PlayUIManager.UpdateThrowItemSlot(); }                                                                   // 던지기 아이템 UI
    public static void UpdateHealItemSlot() { PlayUIManager.UpdateHealItemSlot(); }                                                                     // 회복 아이템 UI
    public static void UpdateQuestSidebar() { PlayUIManager.UpdateQuestSideBar(); }                                                                     // 퀘스트 바 업데이트
    public static void SetMinimapScale(float _scale) { PlayUIManager.SetMinimapScale(_scale); }                                                         // 미니맵 축척 설정
    public static void SetStaminaRate(float _rate) { PlayUIManager.SetStaminaRate(_rate); }                                                             // 스태미나 비율
    public static void SetLightRate(float _rate) { PlayUIManager.SetLightRate(_rate); }                                                                 // 능력 비율
    public static void SetLightState(bool _on) { PlayUIManager.SetLightState(_on); }                                                                    // 고갈 설정
    public static void ShowRaycastAim() { PlayUIManager.ShowRaycastAim(); }                                                                             // 레이캐스트 에임 on
    public static void SetRaycastAimState(bool _on) { PlayUIManager.SetRaycastAimState(_on); }                                                          // 레이캐스트 에임 상태
    public static void HideRaycastAim() { PlayUIManager.HideRaycastAim(); }                                                                             // 레이캐스트 에임 off

    // 인게임 UI
    public static void AddIngameAlarm(string _alarm) { PlayUIManager.AddAlarm(_alarm); }                                                                // 인게임 알람
    public static void ShowEnterRegion(ERegion _region) { PlayUIManager.ShowEnterRegion(_region); }
    public static void ShowBossHPBar(BossMonster _boss) { PlayUIManager.ShowBossHPBar(_boss); }
    public static void SetBossHP(float _hp) { PlayUIManager.SetBossHP(_hp); }
    public static void HideBossHPBar() { PlayUIManager.HideBossHPBar(); }
    public static void ShowInteractInfo(string _info) { PlayUIManager.ShowInteractInfo(_info); }                                                        // 상호작용 키 on
    public static void HideInteractInfo() { PlayUIManager.HideInteractInfo(); }                                                                         // 상호작용 키 off
    public static void ShowDeathUI() { PlayUIManager.ShowDeathUI(); }
    public static void ShowPowerAim(Vector3 _pos, float _radius, float _range) { PlayUIManager.ShowPowerAim(_pos, _radius, _range); }                   // 권능 에임 on
    public static Vector3 TracePowerAim(Vector3 _pos, float _range) { return PlayUIManager.TracePowerAim(_pos, _range); }                               // 권능 에임 위치 설정
    public static void HidePowerAim() { PlayUIManager.HidePowerAim(); }                                                                                 // 권능 에임 off
    public static void DrawThrowLine(Vector3 _force, float _mass, Vector3 _start) { PlayUIManager.DrawThrowLine(_force, _mass, _start); }               // 던지기 궤적 그리기
    public static void HideThrowLine() { PlayUIManager.HideThrowLine(); }                                                                               // 던지기 궤적 off
    private static void StartBlackoutUI() { PlayUIManager.StartBlackout(); }                                                                            // // fade 시작
    private static void EndBlackoutUI() { PlayUIManager.EndBlackout(); }                                                                                // fade 종료
    public static void ShowBlindMark() { PlayUIManager.ShowBlindMark(); }                                                                               // 실명 on
    public static void HideBlindMark() { PlayUIManager.HideBlindMark(); }                                                                               // 실명 off

    // NPC UI
    public static void ShowNPCQuestUI(EQuestName _quest, bool _isStart, FPointer _confirm) { PlayUIManager.ShowNPCQuestUI(_quest, _isStart, _confirm); }    // 퀘스트 수락/거절 창 표시
    public static void OpenDialogueUI(NPCScript _npc, int _idx) { PlayUIManager.OpenDialogueUI(_npc, _idx); }                                           // NPC 대화창 열기
    public static void OpenSlateUI(SlateScript _slate) { PlayUIManager.OpenSlateUI(_slate); }
    public static bool IsDialogueOpend { get { return PlayUIManager.IsDialogueUIOpend; } }                                                              // NPC 대화창 열렸는지 확인

    // 기타
    public static Vector2 NormalizeLocation(Transform _obj) { return PlayUIManager.NormalizeLocation(_obj); }                                          // 위치 정규화(3D -> 2D)



    private void SetSubManagers()
    {
        m_invenManager = GetComponent<InventoryManager>();
        m_invenManager.SetManager();
        m_questManager = GetComponent<QuestManager>();                  // Quest가 Environment보다 위에 있어야함
        m_questManager.SetManager();
        m_environmentManager = GetComponent<EnvironmentManager>();
        m_environmentManager.SetManager();
        m_forceManager = GetComponent<PlayerForceManager>();
        m_forceManager.SetManager();
        m_playUIManager = GetComponent<PlayUIManager>();
        m_playUIManager.SetManager();
    }

    private void Awake()
    {
        if (Inst != null) { Destroy(Inst.gameObject); }
        Inst = this;
        SetSubManagers();
    }
    private void Start()
    {
        StartPlay();
        GameManager.PlayBGM(EBGM.FIELD_BGM);
    }
}
