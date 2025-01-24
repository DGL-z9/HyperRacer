using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글턴
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    
    // 프리팹
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject roadPrefab;

    // UI 관련
    [SerializeField] private MoveButton leftMoveButton;
    [SerializeField] private MoveButton rightMoveButton;
    [SerializeField] private TMP_Text   gasText;
    
    // 자동차
    private CarController carController;
    
    // 도로 오브젝트
    private Queue<GameObject> roadPool = new Queue<GameObject>();
    private const int roadCount = 3;

    private int roadIndex = 0;
    
    // 도로 이동
    private List<GameObject> activeRoad = new List<GameObject>();
    
    // 상태
    public enum State{Start, Play, End}
    public State gameState { get; private set; }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        // Road 오브젝트 풀 초기화
        InitializeRoadPool();
        
        // 게임 상태 Start로 변경
        gameState = State.Start;
        
        // 게임 시작
        StartGame();
    }

    private void Update()
    {

        switch (gameState)
        {
            case State.Start:
                break;
            
            case State.Play:
                // 활성화된 도로 이동
                foreach (GameObject road in activeRoad)
                {
                    road.transform.Translate(Vector3.back * Time.deltaTime);
                }
        
                // Gas 정보 출력
                if (carController != null) gasText.text = carController.Gas.ToString();
                break;
            
            case State.End:
                break;
        }
    }

    private void StartGame()
    {
        // 도로 생성
        SpawnRoad(Vector3.zero);
        
        // 자동차 생성
        carController = Instantiate(carPrefab, new Vector3(0, 0, -3f), Quaternion.identity).GetComponent<CarController>();

        // 버튼에 자동차 컨트롤 기능 적용
        leftMoveButton.OnMoveButtonDown  += () => { carController.Move(-1f); };
        rightMoveButton.OnMoveButtonDown += () => { carController.Move(+1f); };
        
        // 게임 상태를 Play로 변경
        gameState = State.Play;
    }

    public void EndGame()
    {
        gameState = State.End;
        
        // 자동차 제거
        Destroy(carController.gameObject);
        
        // 도로 제거
        foreach (var road in activeRoad)
        {
            road.SetActive(false);
        }
        
        // 게임 오버 패널 표시
    }

    #region 도로 생성 및 관리
    /// <summary>
    /// 도로 오브젝트 풀 초기화
    /// </summary>
    private void InitializeRoadPool()
    {
        for (int i = 0; i < roadCount; i++)
        {
            GameObject road = Instantiate(roadPrefab);
            road.SetActive(false);
            roadPool.Enqueue(road);
        }
    }
    
    /// <summary>
    /// 도로 오브젝트 풀에서 불러와 배치하는 함수
    /// </summary>
    public void SpawnRoad(Vector3 position)
    {
        GameObject road;
        if (roadPool.Count > 0)
        {
            road = roadPool.Dequeue();
            road.SetActive(true);
            road.transform.position = position;
        }
        else
        {
            road = Instantiate(carPrefab, position, Quaternion.identity);
        }
        // 가스 아이템 생성
        if (roadIndex > 0 && roadIndex % 2 == 0)
        {
            road.GetComponent<RoadController>().SpawnGas();
        }
        
        activeRoad.Add(road);
        roadIndex++;
    }

    public void DespawnRoad(GameObject road)
    {
        road.SetActive(false);
        activeRoad.Remove(road);
        roadPool.Enqueue(road);
    }
    #endregion
}