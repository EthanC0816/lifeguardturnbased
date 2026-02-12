using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;
        public bool obligatory;

        public int ProbabilityOfSpawning(int x, int y)
        {
            if (x >= minPosition.x && x <= maxPosition.x &&
                y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1;
            }

            return 0;
        }
    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;

    public Transform[] ghosts;
    public bool[] ghostOccupied;


    List<Cell> board;
    public List<int> mazePathOrder = new List<int>();
    public List<Transform> boardSpaces = new List<Transform>();
    public List<Transform> graveSpawnPoints = new List<Transform>();


    void Start()
    {
        MazeGenerator();
    }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if(Application.targetFrameRate != 60)
            Application.targetFrameRate = 60;
    }

    void GenerateDungeon()
    {
        foreach (int index in mazePathOrder)
        {
            int i = index % size.x;
            int j = index / size.x;

            Cell currentCell = board[index];

            int randomRoom = -1;
            List<int> availableRooms = new List<int>();

            for (int k = 0; k < rooms.Length; k++)
            {
                int p = rooms[k].ProbabilityOfSpawning(i, j);

                if (p == 2)
                {
                    randomRoom = k;
                    break;
                }
                else if (p == 1)
                {
                    availableRooms.Add(k);
                }
            }

            if (randomRoom == -1)
            {
                if (availableRooms.Count > 0)
                    randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                else
                    randomRoom = 0;
            }

            var newRoom = Instantiate(
                rooms[randomRoom].room,
                new Vector3(i * offset.x, 0, -j * offset.y),
                Quaternion.identity,
                transform
            ).GetComponent<RoomBehaviour>();

            newRoom.UpdateRoom(currentCell.status);
            newRoom.name += " " + i + "-" + j;

            // Grave spawn points
            List<Transform> spawns = FindSpawnPoints(newRoom.transform);
            spawns.Sort((a, b) => a.name.CompareTo(b.name));
            graveSpawnPoints.AddRange(spawns);

            // Board tile
            Transform tile = FindBoardSpace(newRoom.transform);
            if (tile != null)
            {
                boardSpaces.Add(tile);
            }
            else
            {
                Debug.LogError($"Room {newRoom.name} has NO Standpoint tile!");
            }
        }
    }


    Transform FindBoardSpace(Transform room)
    {
        foreach (Transform t in room.GetComponentsInChildren<Transform>(true))
        {
            if (t.name.ToLower().EndsWith("standpoint"))
                return t;
        }

        return null;
    }



    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        mazePathOrder.Clear();

        for (int j = 0; j < size.y; j++)
        {
            if (j % 2 == 0)
            {
                for (int i = 0; i < size.x; i++)
                {
                    int index = j * size.x + i;
                    mazePathOrder.Add(index);
                }
            }
            else
            {
                for (int i = size.x - 1; i >= 0; i--)
                {
                    int index = j * size.x + i;
                    mazePathOrder.Add(index);
                }
            }
        }

        for (int k = 0; k < mazePathOrder.Count - 1; k++)
        {
            int a = mazePathOrder[k];
            int b = mazePathOrder[k + 1];

            if (b == a + 1)
            {
                board[a].status[2] = true;
                board[b].status[3] = true;
            }
            else if (b == a - 1)
            {
                board[a].status[3] = true;
                board[b].status[2] = true;
            }
            else if (b == a + size.x)
            {
                board[a].status[1] = true;
                board[b].status[0] = true;
            }
            else if (b == a - size.x)
            {
                board[a].status[0] = true;
                board[b].status[1] = true;
            }
        }

        GenerateDungeon();
        ghostOccupied = new bool[boardSpaces.Count];
        StartCoroutine(TeleportPlayers());
        StartCoroutine(GhostRiseSequence());
    }
    List<Transform> FindSpawnPoints(Transform room)
    {
        List<Transform> points = new List<Transform>();

        Transform spawnParent = room.Find("SpawnPoints"); // or whatever the parent is called

        if (spawnParent != null)
        {
            foreach (Transform child in spawnParent)
            {
                points.Add(child);
            }
        }

        return points;
    }

    IEnumerator TeleportPlayers()
    {
        yield return new WaitForSeconds(0.5f);

        PlayerController[] players = FindObjectsOfType<PlayerController>();

        System.Array.Sort(players, (a, b) => a.name.CompareTo(b.name));

        for (int i = 0; i < players.Length; i++)
        {
            if (i < graveSpawnPoints.Count)
            {
                players[i].transform.position = graveSpawnPoints[i].position;
                players[i].transform.rotation = graveSpawnPoints[i].rotation * Quaternion.Euler(0, 180, 0);

                players[i].currentSpaceIndex = 0;
            }
            else
            {
                Debug.LogWarning("Not enough spawn points for all players!");
            }
        }
    }
    IEnumerator GhostRiseSequence()
    {
        yield return new WaitForSeconds(1f);

        float riseTime = 2;
        float targetY = 0.2f;
        float elapsed = 0f;

        Vector3[] startPos = new Vector3[ghosts.Length];
        Vector3[] endPos = new Vector3[ghosts.Length];

        for (int i = 0; i < ghosts.Length; i++)
        {
            startPos[i] = ghosts[i].transform.position;

            
            endPos[i] = new Vector3(
                startPos[i].x,
                targetY,
                startPos[i].z
            );
        }

        while (elapsed < riseTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / riseTime;

            for (int i = 0; i < ghosts.Length; i++)
            {
                ghosts[i].transform.position = Vector3.Lerp(startPos[i], endPos[i], t);
            }

            yield return null;
        }

        
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].transform.position = endPos[i];
        }
    }


}
