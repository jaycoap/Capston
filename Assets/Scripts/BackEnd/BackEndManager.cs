using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;

public class BackEndManager : MonoBehaviour // 싱글톤으로 만들고 파괴되지 않는 게임오브젝트로 만든후 초기화 코드, 에러관리 함수
{
    private static BackEndManager instance = null;
    public static BackEndManager MyInstance { get => instance; set => instance = value; }
    [SerializeField] private GameObject joinfail;
    [SerializeField] private Text join1;
    void Awake()
    {   
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        InitBackEnd();
    }

    // 뒤끝 초기화
    
    private void InitBackEnd()
    {

        var bro = Backend.Initialize(true);
        Debug.Log("뒤끝 초기화 진행" + bro);
        if (bro.IsSuccess())
        {
            Debug.Log(Backend.Utils.GetServerTime());
        }

            // 실패
        else
        {
           ShowErrorUI(bro);
        }
        

    }

    public void ShowErrorUI(BackendReturnObject backendReturn)
    {
        int statusCode = int.Parse(backendReturn.GetStatusCode());

        switch (statusCode)
        {
            case 401:
                Debug.Log("ID or Password Error");
                join1.text = "ID or Password Error";
                break;
            case 403:
                Debug.Log(backendReturn.GetErrorCode());
                join1.text = backendReturn.GetErrorCode();
                break;
            case 404:
                Debug.Log("game not found");
                join1.text = "game not found";
                break;
            case 408:
                // 타임아웃 오류(서버에서 응답이 늦거나, 네트워크 등이 끊겨 있는 경우)
                // 요청 오류
                Debug.Log(backendReturn.GetMessage());
                join1.text = backendReturn.GetMessage();
                break;

            case 409:
                Debug.Log("Duplicated customId, 중복된 customId 입니다");
                join1.text = "Duplicated customId, 중복된 customId 입니다";
                break;

            case 410:
                Debug.Log("bad refreshToken, 잘못된 refreshToken 입니다");
                join1.text = "bad refreshToken, 잘못된 refreshToken 입니다";
                break;

            case 429:
                // 데이터베이스 할당량을 초과한 경우
                // 데이터베이스 할당량 업데이트 중인 경우
                Debug.Log(backendReturn.GetMessage());
                join1.text = backendReturn.GetMessage();
                break;

            case 503:
                // 서버가 정상적으로 작동하지 않는 경우
                Debug.Log(backendReturn.GetMessage());
                join1.text = backendReturn.GetMessage();
                break;

            case 504:
                // 타임아웃 오류(서버에서 응답이 늦거나, 네트워크 등이 끊겨 있는 경우)
                Debug.Log(backendReturn.GetMessage());
                join1.text = backendReturn.GetMessage();
                break;

        }

        joinfail.SetActive(true);
    }
    

    
}
