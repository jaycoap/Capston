using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Random = UnityEngine.Random;

//타일
[Serializable]
public class TB
{
    public TileBase FloorTile;
}
//플렛폼 개수를 조절하는 변수
[Serializable]
public class Platform
{
    //플렛폼 Y좌표의 최대(2칸 단위로 ex : 3 이면 0 2 4 6 까지)
    public int Platform_Y_MAX;
    public int Platform5;
    public int Platform4;
}
public class TileMapManager : MonoBehaviour
{

    [SerializeField] private Tilemap Tilemap;
    //바닥 가로길이 (가운데 기준 좌우, 홀수일 경우 오른쪽에 하나더)
    [SerializeField] private float FloorWidth;
    //바닥 깊이
    [SerializeField] private int Floordepth;
    public TB tb;
    public Platform platform;

    private void Start()
    {
        Tilemap.ClearAllTiles();
        MakeFloor();
        MakePlatform();
    }
    //바닥을 만드는 함수
    private void MakeFloor()
    {
        float halfWidth = FloorWidth / 2;
        for(int e = 0 ; e < Floordepth ; e++)
        {
            for (int i = -Mathf.FloorToInt(halfWidth); i < Mathf.RoundToInt(halfWidth); i++)
            {
                Tilemap.SetTile(new Vector3Int(i, -2 - e, 0), tb.FloorTile);
            }
        }
    }  

    private void MakePlatform()
    {
        float halfWidth = FloorWidth / 2;
        //0 ~ FloorWidth-1 까지 있는 배열 생성
        //-halfWidth ~ 반올림(halfWidth)-1 까지 좌표를 나타냄 
        //원하는 좌표 +내림(halfWidth)를 하면 원하는 좌표의 인덱스가 나옴
        int[] platformX = new int[(int)FloorWidth];
        //인덱스 좌표의 Y좌표 변수
        int[] platformY = new int[(int)FloorWidth];
        //배열 초기화
        //0은 할당 안된 좌표 1은 할당 된 좌표
        for (int i = 0; i < FloorWidth; i++)
        {
            
            // -2 ~ 0 좌표는 플레이어 스폰 지역이기 때문에 제외
            if(i >= -2 + halfWidth && i <= 1 + halfWidth)
            {
                platformX[i] = 1;
            }
            else
            {
                platformX[i] = 0;
            }
            //기본 바닥 좌표로 초기화
            platformY[i] = -2;
        }
        //시행회수가 너무 많을때 탈출하기 위한 변수
        int escape = 0;
        //크기가 5인 플렛폼 생성
        for (int i = 0 ; i < platform.Platform5 ; i++)
        {
            //랜덤으로 X,Y 좌표를 고름 Y좌표는 2칸 간격으로
            int R_X = Random.Range(8, (int)FloorWidth -8);
            int R_Y = Random.Range(0, platform.Platform_Y_MAX) * 2;
            //체크를 한 구간에 할당된 좌표가 있는지 나타내는 변수
            bool X_Assignment = false;
            //발판 설치가 가능한지 체크하는 변수
            bool Y_able = true;

            //5칸 이내에 할당된 좌표가 있는지 체크
            for (int e = 0; e < 5; e++)
            {
                if (platformX[R_X + e - 2] == 1)
                {
                    //체크 for문 탈출 
                    e = 5;
                    //발판 설치를 안했으니 i값 유지를 위해 -1
                    i--;
                    escape++;

                    X_Assignment = true;
                }
            }
            //발판 설치가 가능한지 체크
            if (!X_Assignment)
            {
                for(int e = 0 ; e < R_Y / 2 ; e++)
                {
                    //왼쪽에 할당된 발판 체크
                    if (platformX[R_X - e - 3] == 1)
                    {
                        //왼쪽에서 e + 1 번째 발판의 Y 좌표 차이가 2 + e*2 초과 이면 설치 불가능
                        if(Mathf.Abs(platformY[R_X - e - 3] - R_Y) > 2 + e*2 )
                        {
                            Y_able = false;
                            //발판 설치가 불가능해 플렛폼 설치를 못해서 i값 유지를 위해 -1
                            i--;
                            escape++;
                        }
                        
                    }
                    //오른쪽에 할당된 발판 체크
                    if (platformX[R_X + e + 3] == 1)
                    {
                        //오른쪽에서 e + 1 번째 발판의 Y 좌표 차이가 2 + e*2 초과 이면 설치 불가능
                        if (Mathf.Abs(platformY[R_X + e + 3] - R_Y) > 2 + e*2)
                        {
                            Y_able = false;
                            //발판 설치가 불가능해 플렛폼 설치를 못해서 i값 유지를 위해 -1
                            i--;
                            escape++;
                        }

                    }
                }    
            }
            
            //5칸 안에 할당된 좌표가 없고 발판을 설치 해야 할 경우 설치 가능할때 플렛폼 설치 및 할당
            if (!X_Assignment && Y_able)
            {
                //양쪽에 발판이 있는지 체크 하는 변수
                bool RplatformExist = false;
                bool LplatformExist = false;
                int RplatformX = 0;
                int RplatformY = 0;
                int LplatformX = 0;
                int LplatformY = 0;
                for (int e = 0; e < R_Y / 2; e++)
                {
                    //플렛폼 왼쪽부터 왼쪽에 할당된 발판 체크
                    if (platformX[R_X - e - 3] == 1 && LplatformExist == false)
                    {
                        LplatformExist = true;
                        LplatformX = e;
                        LplatformY = platformY[R_X - e - 3];
                    }
                    //플렛폼 오른쪽부터 오른쪽에 할당된 발판 체크
                    if (platformX[R_X + e + 3] == 1 && RplatformExist == false)
                    {
                        RplatformExist = true;
                        RplatformX = e;
                        RplatformY = platformY[R_X + e + 3];
                    }
                }
                for (int e = 0; e < R_Y / 2; e++)
                {
                    if (!LplatformExist)
                    {
                        //가장 왼쪽부터 발판이 없고 발판이 필요할시 설치
                        Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth) - 2 - (R_Y / 2 - e), 2 * e, 0), tb.FloorTile);
                        platformX[R_X - 2 - (R_Y / 2 - e)] = 1;
                        platformY[R_X - 2 - (R_Y / 2 - e)] = 2 * e;
                    }
                    else
                    {
                        //왼쪽에 거리가 1칸 이상 벌어진 발판이 있을시 발판설치  
                        if(LplatformX >= 1)
                        {
                            for (int o = 0 ; o < LplatformX ; o++)
                            {
                                //2칸 차이 날때
                                if ((LplatformY - R_Y) / (LplatformX + 1) < 2)
                                {
                                    Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth) - 3 - e, R_Y + (LplatformY - R_Y) / Mathf.Abs((LplatformY - R_Y)) * 2, 0), tb.FloorTile);
                                    platformX[R_X - 3 - e] = 1;
                                    platformY[R_X - 3 - e] = R_Y + (LplatformY - R_Y) / Mathf.Abs((LplatformY - R_Y)) * 2;
                                }
                                //2칸 초과 차이 날때
                                else
                                {
                                    Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth) - 3 - e, R_Y + (LplatformY - R_Y) / (LplatformX + 1), 0), tb.FloorTile);
                                    platformX[R_X - 3 - e] = 1;
                                    platformY[R_X - 3 - e] = R_Y + (LplatformY - R_Y) / (LplatformX + 1);
                                }
                                
                            }
                        }  
                    }
                    if (!RplatformExist)
                    {
                        //가장 오른쪽부터 발판이 없고 발판이 필요할시 설치
                        Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth) + 2 + (R_Y / 2 - e), 2 * e, 0), tb.FloorTile);
                        platformX[R_X + 2 + (R_Y / 2 - e)] = 1;
                        platformY[R_X + 2 + (R_Y / 2 - e)] = 2 * e;
                    }
                    else
                    {
                        //오른쪽에 거리가 1칸 이상 벌어진 발판이 있을시 발판설치  
                        if (RplatformX >= 1)
                        {
                            for (int o = 0; o < RplatformX; o++)
                            {
                                //2칸 차이 날때
                                if ((LplatformY - R_Y) / (LplatformX + 1) < 2)
                                {
                                    Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth) - 3 - e, R_Y + (RplatformY - R_Y) / Mathf.Abs((RplatformY - R_Y)) * 2, 0), tb.FloorTile);
                                    platformX[R_X - 3 - e] = 1;
                                    platformY[R_X - 3 - e] = R_Y + (RplatformY - R_Y) / Mathf.Abs((RplatformY - R_Y)) * 2;
                                }
                                //2칸 초과 차이 날때
                                else
                                {
                                    Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth) - 3 - e, R_Y + (RplatformY - R_Y) / (RplatformX + 1), 0), tb.FloorTile);
                                    platformX[R_X - 3 - e] = 1;
                                    platformY[R_X - 3 - e] = R_Y + (RplatformY - R_Y) / (RplatformX + 1);
                                }
                            }
                        }
                    }
                }

                Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth), R_Y, 0), tb.FloorTile);
                Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth) - 1, R_Y, 0), tb.FloorTile);
                Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth) - 2, R_Y, 0), tb.FloorTile);
                Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth) + 1, R_Y, 0), tb.FloorTile);
                Tilemap.SetTile(new Vector3Int(R_X - Mathf.FloorToInt(halfWidth) + 2, R_Y, 0), tb.FloorTile);
                for(int e = 0 ; e < 5 ; e++)
                {
                    platformX[R_X + e - 2] = 1;
                    platformY[R_X + e - 2] = R_Y;
                }
            }  
            //시행회수가 너무 많을경우 탈출
            if(escape > 100)
            {
                break;
            }
        }
    }
}
