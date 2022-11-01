using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using BackEnd;
using Random = UnityEngine.Random;
using LitJson;
using UnityEngine.SceneManagement;


public enum GameState
{
    menu,
    inGame,
    start,
    gameover
}

[Serializable]
public class HPsetting
{
    //적 레벨당 올라가는 체력 비율
    public int EnemyHP_X;
    //각 몬스터의 기본체력
    public int slimeHP = 100;
    public int slime2HP = 150;
    public int slime3HP = 200;
    public int slime4HP = 300;
    public int slimeBossHP = 1000;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    public bool isPause = false;
    public GameState currentGameState = GameState.start; // 게임상태 설정
    private static GameManager _instance;
    private GameObject player;
    public Rigidbody2D playerRigid;
    
    public BackEndNickname backendnickname;
    public HPsetting hpSetting;
    spawnManager spawnmanager;
    
    public Text idText;
    

    //게임 세팅 변수
    private int activelevel = 0; // 레벨설정
    private string myname = "test1"; // 닉네임설정
    private int maxHp = 0; // 최대체력 설정
    private int maxMp = 0; // 최대마나 설정
    private int maxExp = 0; // 레벨업에 필요한 경험치 설정
    private double maxCheck = 0; // 레벨업시 필요경험치 = 경험치 x 1.2
    private int Expcheck = 0; //레벨업에 필요한 경험치를 제외하고 나머지를 다음 레벨 경험치로 이관
    private int HP = 0; // 현재체력
    private int MP = 0; // 현재마나
    private int STR = 0;  // 공격력(평타 공격)
    private int INT = 0; //  주문력(스킬 공격력)
    private int FIT = 0; // 캐릭터 체력 마나 스텟
    private int EXP = 0;  //현재경험치
    private int APPoint = 0; // 스텟 포인트
    private int Gold = 0;
    private int AD1 = 0; // 공격 1타
    private int AD2 = 0; // 공격 2타
    private int AD3 = 0; // 공격 3타
    private int MaxAD = 0; //최대 공격 데미지
    private int MinAD = 0; // 최소 공격 데미지
    private int MaxAP = 0; // 최대 스킬 데미지
    private int MinAP = 0; // 최소 스킬 데미지
    private int RushAP = 0; //Q스킬 데미지
    private int SlashAP = 0; //W스킬 데미지
    private int SwordAP = 0; //E스킬 데미지
    int attackCount = 0; // 공격 타수 카운트
    private int Time_HP = 5; //자동 HP회복량
    private int Time_MP = 10; // 자동 MP회복량
    private float Time_delay = 10f; // 자동회복 쿨타임
    private bool isDelay;



    //게임 상태 변수
    public bool menu = false;
    public bool firstcheck = true;
    private bool backmenu = false;
    
    public InputField Idfield;
    public InputField Pwfield;
    public Button EnterButton;

    //스테이지 관리
    public int StageClearNum;

    StageManager stageManager;


    void Start()
    {
        backendnickname = GetComponent<BackEndNickname>();
        spawnmanager = GetComponent<spawnManager>();
        stageManager = GetComponent<StageManager>();
        Idfield.Select();
        

        


    }

    void Update()
    {
       
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            StartCoroutine(IdSelect());
        }
        if (currentGameState == GameState.inGame) // 게임이 시작되었을때 사용됨.
        {
            
            if ((isDelay == false && HP <maxHp) || (isDelay == false && MP < maxMp))
            {
                isDelay = true;
                setPlayerHP(HP + Time_HP);
                setPlayerMP(MP + Time_MP);
                StartCoroutine(HealTime());
            }   
            setPlayerHP(HP);
            if (Input.GetKeyDown(KeyCode.A))
            {
                attackCount = attackCount + 1;


                if (attackCount == 1)
                {
                    ADAttackFirst();
                }

                else if (attackCount == 2)
                {
                    ADAttackSecond();
                }

                else if (attackCount == 3)
                {
                    ADAttackThird();
                    attackCount = 1;
                }
                Energy_sword();

            }
            Energy_rush();
            Energy_slash();
            
        }
        else
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.O)) // 게임 일시정지 
        {   
            
            BackToMenu();

        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            EXP += 200;
            
            
        }
        
        
        

    }
    
    IEnumerator IdSelect()
    {
        
            if (Idfield.isFocused == true) //로그인시 tab키 입력하면 Pw필드로 이동
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    Pwfield.Select();
                }
            }

            if (Idfield.text != null && Pwfield.text != null) //아이디, 비밀번호 입력후 Enter키를 입력하면 자동으로 로그인
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    EnterButton.onClick.Invoke();
                }
            }
        
        yield break;
    }

   
    public int GetClearStage()
    {
        return StageClearNum;
    }
    public int SetClearStage(int CurrentStage)
    {
        StageClearNum = CurrentStage;
        return StageClearNum;
    }
    public int GetDBStage()
    {
        StageClearNum = stageManager.GetDBStage();
        
        return StageClearNum;
    }
    public void GetDBButton()
    {
        GetDBStage();
    }
   
    public void Ingame() // 게임 시작시 설정값 입력 
    {
        currentGameState = GameState.inGame;

        playerManager.isStart = true;

        spawnmanager.SponEnemy(0, new Vector2(50, 0));

        spawnmanager.SponEnemy(1, new Vector2(80, 0));
        LoadGameState();



        return;
    }
    
    public void GameOver()//게임 끝날시
    {
        currentGameState = GameState.gameover;
       
        return;
    }
    public void BackToMenu() // 메뉴로 돌아갈시 
    {
        if (backmenu == false)
        {
            Time.timeScale = 0;
            backmenu = true;
            return;
        }
        if (backmenu == true)
        { 
            Time.timeScale = 1;
            backmenu = false;
            return;
        }
        
       
        
    }

    public void SetGameState()// 게임상태 설정
    {
        
        
           activelevel = 1;
           myname = idText.text; // 닉네임 설정
           maxHp += 70; // 최대 체력 설정.
           maxMp += 200; // 최대마나
           maxExp += 150; // 1랩때 최대 경험치 
           HP += maxHp; // 초기 체력 설정
           MP += maxMp; // 초기 마나 설정
           STR += 7; // 초기 공격력 설정
           INT += 12; // 초기 주문력 설정
           FIT += 3; // 초기 체력 마나 스텟 설정
           EXP += 0; // 초기 경험치 세팅
           APPoint = 0;
           Gold = 0;
                
                
        
    }
    public void LoadGameState()// 게임상태 설정
    {
        firstcheck = true;
        if (firstcheck == true) // 게임이 시작되면 밑 같이 설정
        {
            
            activelevel = BackEndGameInfo.instance.GetLevel(); // 레벨 설정
            myname = idText.text; // 닉네임 설정
            maxHp = BackEndGameInfo.instance.GetMaxHP(); // 최대 체력 설정
            maxMp = BackEndGameInfo.instance.GetMaxMP(); // 최대마나
            maxExp = BackEndGameInfo.instance.GetMaxEXP(); // 최대 경험치 
            HP = BackEndGameInfo.instance.GetMaxHP(); //  체력 설정
            MP = BackEndGameInfo.instance.GetMaxMP(); //  마나 설정
            STR = BackEndGameInfo.instance.GetSTR(); // 공격력 설정
            INT = BackEndGameInfo.instance.GetINT(); // 주문력 설정
            FIT = BackEndGameInfo.instance.GetFIT(); //  체력 마나 스텟 설정
            EXP = BackEndGameInfo.instance.GetEXP(); //  경험치 세팅
            APPoint = BackEndGameInfo.instance.GetAPPoint(); // APPoint 설정
            StageClearNum = StageManager.instance.GetDBStage(); // Stage 정보
            Gold = BackEndGameInfo.instance.GetGold(); // Gold설정

            firstcheck = false;
        }
        else if (firstcheck == false) // 아니면 return
        {
            return;
        }

    }

    

    
    //플레이어 공격 분리
    public void ADAttackFirst() // 1타
    {
        MinAD = (STR  + (FIT / 2)) * Random.Range(1, 2);
        MaxAD = (STR + FIT) * Random.Range(1, 3);
        AD1 = Random.Range(MinAD, MaxAD);
    }
    public int Return1AD()
    {
        return AD1;
    }
    public void ADAttackSecond() // 2타
    {
        MinAD = (STR + (FIT / 2)) * Random.Range(1, 2);
        MaxAD = (STR + FIT) * Random.Range(1, 3);
        AD2 = Random.Range(MinAD, MaxAD);
    }
    public int Return2AD()
    {
        return AD2;
    }
    public void ADAttackThird() // 3타
    {
        MinAD = (STR + (FIT / 2)) * Random.Range(1, 2);
        MaxAD = (STR + FIT) * Random.Range(1, 3);
        AD3 = Random.Range(MinAD, MaxAD);
    }
    public int Return3AD()
    {
        return AD3;
    }

   

    public void Energy_slash() //Q스킬
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {

            MinAP = (INT + (FIT / 2)) * 1;
            MaxAP = (INT + FIT ) * Random.Range(1, 3);
            SlashAP = Random.Range(MinAP, MaxAP);
            
        }
        else
        {
            return;
        }

    }
    public int ReturnSlash()
    {
        return SlashAP;
    }
    public void Energy_rush()//W스킬
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            MinAP = (INT + (FIT / 2)) * 1;
            MaxAP = (INT + FIT) * Random.Range(1, 5);
            RushAP = Random.Range(MinAP, MaxAP);
        }
        else
        {
            return;
        }
    }
    public int ReturnRush()
    {
        return RushAP;
    }
    public void Energy_sword()//E스킬
    {

        MinAP = (INT + (FIT / 2)) * 1;
        MaxAP = (INT + FIT) * Random.Range(1, 3);
        SwordAP = Random.Range(MinAP, MaxAP);


        
    }
    
    public int ReturnSword()
    {
        return SwordAP;
    }

    public void DecreaseMP(int mp)
    {
        MP = MP - mp;
    }

    

    

    // 플레이어 피격시
    public void PlayerDamage(int PlayerHit) //PlayerHIt에는 몬스터 몬스터 공격력을 추가.
    {
        if (HP - PlayerHit >= 0)
        {
            HP = HP-PlayerHit;
            setPlayerHP(HP);
        }
        else if(HP - PlayerHit <= 0)
        {
            setPlayerHP(HP);
            Dead();
            
        }
    }
    public int setPlayerHP(int setHP)
    {
        HP = setHP;
        return HP;
    }
    public int setPlayerMP(int setMP)
    {
        MP = setMP;
        return MP;
    }

    IEnumerator HealTime()
    {
        yield return new WaitForSeconds(Time_delay);
        isDelay = false;

    }

    //포션사용시 HP,MP 따로 증가하게 설정
    public int usePotionHealHP(int PotionHeal)//사용시 HP 증가
    {
        if (HP + PotionHeal >= maxHp) // HP+힐량이 최대체력보다 높을시 현재 체력을 최대 체력으로 설정.
        {
            HP = maxHp;
        }
        else // 아니면 HP에 힐량 추가
        {
            HP += PotionHeal;
        }
        return HP;
    }
    public int usePotionHealMP(int PotionHeal)//사용시 MP 증가
    {

        if (MP + PotionHeal >= maxMp) // MP+힐량이 최대마나보다 높을시 현재 마나을 최대 마나으로 설정.
        {
            MP = maxMp;
        }
        else // 아니면 MP에 힐량 추가
        {
            MP += PotionHeal;
        }
        return MP;
    }

    //스텟증가 관련
    public void UpSTR() //STR증가시
    {
        if (APPoint <= 0)
        {
            return;
        }
        else
        {
            STR += 1;
            APPoint -= 1;
        }
    }
    public void UpINT() //INT증가시
    {
        if (APPoint <= 0)
        {
            return;
        }
        else
        {
            INT += 1;
            APPoint -= 1;
        }
    }
    public void UpFIT() //FIT증가시
    {
        if (APPoint <= 0)
        {
            return;
        }
        else
        {
            FIT += 1;
            maxHp += 10;
            maxMp += 10;
            APPoint -= 1;
            HP = maxHp;
            MP = maxMp;
        }
    }

    public void setLevel() // 레벨 세팅
    {
        if (firstcheck == false) // 게임이 시작되야 레벨 및 경험치 설정
        {
            if (EXP >= maxExp && EXP - maxExp >= 1) //현재 경험치가 최대경험치 이상일때 레벨 +1, 현재 경험치와 최대경험치 차이가 1 이상이면
            {

                activelevel += 1;
                APPoint += 3; //스텟 포인트 3추가
                HP = maxHp;
                MP = maxMp;
                Expcheck = EXP - maxExp; // 현재 경험치와 최대경험치를 뺀 나머지를 현재 경험치로 설정.
                EXP = Expcheck;

                if (EXP - maxExp == 0) // 아니면 현재경험치는 0
                {
                    EXP = 0;
                }
                if (activelevel % 10 <= 0) // 10렙당 경험치 1.2배
                {
                    maxCheck = maxExp * 1.25;
                }
                else// 아니면 1.2배
                {
                    maxCheck = maxExp * 1.2;
                }

                maxExp = (int)maxCheck;
            }
        }
        else if (firstcheck == true) // 아니면 실행 x
        {
            return;
        }

    }

    public int getLevel() // 레벨 불러오기
    {
        return activelevel;
    }

    //초기 세팅
    public string getName() // 닉네임 불러오기
    {
        return myname;
    }

    public int getExp() // 현재 경험치 불러오기
    {
        return EXP;
    }

    public int getmaxExp() // 최대 경험치 불러오기
    {
        return maxExp;
    }

    public int getexp(int newExp) // 경험치 추가
    {
        EXP += newExp;
        return EXP;
    }
    public int getHp() //HP불러오기 및 세팅
    {
        if (HP <= 0)
        {
            HP = 0;
        }
        else if (HP >= maxHp)
        {
            HP = maxHp;
        }
        return HP;
    }
    public int getmaxHp() //MAX HP 불러오기
    {
        return maxHp;
    }
    public int getMp() //MP 불러오기 및 세팅
    {
        if( MP <= 0)
        {
            MP = 0;
        }
        else if (MP >= maxMp)
        {
            MP = maxMp;
        }
        return MP;
    }
    public int getmaxMp() //MAX MP 불러오기
    {
        return maxMp;
    }
    public int getSTR() // STR불러오기
    {
        return STR;
    }
    public int getINT() // INT불러오기
    {
        return INT;
    }
    public int getFIT() // FIT불러오기
    {
        return FIT;
    }
    public int getAPPoint() // AP불러오기
    {
        return APPoint;
    }
    public int setSTR(int newSTR) // STR 스텟 증가
    {
        STR += newSTR;
        return STR;
    }
    public int setINT(int newINT) // INT 스텟증가
    {
        INT += newINT;
        return INT;
    }
    public int setFIT(int newFIT) // FIT 스텟증가
    {
        FIT += newFIT;
        return FIT;
    }

    public int getGold() //골드 불러오기
    {
        return Gold;
    }

    public int setGold(int newGold) // 골드 증가
    {
        Gold += newGold;
        return Gold;
    }

    public void payGold(int pay) // 골드 사용
    {
        if (Gold-pay <= 0)
        {
            return;
        }
        else
        {
            Gold -= pay;
        }
    }

    public int SlimeHPSet()
    {
        int enemyHP = hpSetting.slimeHP + (hpSetting.EnemyHP_X * hpSetting.slimeHP * getLevel());
        return enemyHP;
    }
    public int Slime2HPSet()
    {
        int enemyHP = hpSetting.slime2HP + (hpSetting.EnemyHP_X * hpSetting.slime2HP * getLevel());
        return enemyHP;
    }

    public int Slime3HPSet()
    {
        int enemyHP = hpSetting.slime3HP + (hpSetting.EnemyHP_X * hpSetting.slime3HP * getLevel());
        return enemyHP;
    }

    public int Slime4HPSet()
    {
        int enemyHP = hpSetting.slime4HP + (hpSetting.EnemyHP_X * hpSetting.slime4HP * getLevel());
        return enemyHP;
    }

    public int SlimeBossHPSet()
    {
        int enemyHP = hpSetting.slimeBossHP + (hpSetting.EnemyHP_X * hpSetting.slimeBossHP * getLevel());
        return enemyHP;
    }



    public void Dead() // 캐릭터 사망시 사용될 함수.
    {
        setPlayerHP(HP);
        player = GameObject.FindWithTag("Player");
        player.GetComponent<Animator>().SetBool("isDie", true);
        
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(this.gameObject);

        player = GameObject.FindWithTag("Player");
        

        Screen.fullScreen = false;
    }

    
}

public class GameContorl
{
    public void IsPause()
    {
        GameManager.Instance.isPause = true;
    }
}
