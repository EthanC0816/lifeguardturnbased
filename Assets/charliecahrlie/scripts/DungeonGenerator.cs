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

    List<Cell> board;
    public List<int> mazePathOrder = new List<int>();
    public List<Transform> boardSpaces = new List<Transform>();

    void Start()
    {
        MazeGenerator();
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

            Transform space = FindSpaceInRoom(newRoom.transform);
            if (space != null)
                boardSpaces.Add(space);
        }
    }

    Transform FindSpaceInRoom(Transform room)
    {
        foreach (Transform child in room)
        {
            string n = child.name.ToLower();

            if (n.Contains("space"))
            {
                foreach (Transform sub in child)
                {
                    string sn = sub.name.ToLower();
                    if (sn.Contains("standpoint"))
                        return sub;
                }

                return child;
            }
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
        StartCoroutine(TeleportPlayers());
    }

    IEnumerator TeleportPlayers()
    {
        yield return new WaitForSeconds(2f);

        PlayerController[] players = FindObjectsOfType<PlayerController>();

        foreach (var p in players)
        {
            p.transform.position = boardSpaces[0].position;
            p.transform.rotation = boardSpaces[0].rotation;
        }
    }
    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        if (cell - size.x >= 0 && !board[(cell - size.x)].visited)
            neighbors.Add((cell - size.x));

        if (cell + size.x < board.Count && !board[(cell + size.x)].visited)
            neighbors.Add((cell + size.x));

        if ((cell + 1) % size.x != 0 && !board[(cell + 1)].visited)
            neighbors.Add((cell + 1));

        if (cell % size.x != 0 && !board[(cell - 1)].visited)
            neighbors.Add((cell - 1));

        return neighbors;
    }
}
