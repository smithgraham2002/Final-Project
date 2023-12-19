using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using Random = UnityEngine.Random;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength; // set Total Dungeon Length and Width
    public int roomWidthMin, roomLengthMin; // set Room  Length and Width
    public int maxIterations; // max amount of Iterations for BSP
    public int corridorWidth; // set corridor Width
    public GameObject enemy; // Enemy mesh
    public GameObject goal; // Goal mesh
    public GameObject Player;
    public NavMeshSurface set;
    public Material material; // Floor + Enemy Material
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier; // Set Bottom Corner Modifier in Editor
    [Range(0.7f, 1.0f)]
    public float roomTopCornerMidifier; // Set Top Corner Modifier in Editor
    [Range(0, 2)]
    public int roomOffset; // Set Room Offset in Editor
   
    public GameObject wallVertical, wallHorizontal; // Wall Objects
    
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    List<NavMeshSurface> surface = new List<NavMeshSurface>();
    List<UnityEngine.AI.NavMeshAgent> enemies = new List<UnityEngine.AI.NavMeshAgent>();

    void Start()
    {
        CreateDungeon();
    }

    public void CreateDungeon()
    {
        DestroyAllChildren();
        DugeonGenerator generator = new DugeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeon(maxIterations,
            roomWidthMin,
            roomLengthMin,
            roomBottomCornerModifier,
            roomTopCornerMidifier,
            roomOffset,
            corridorWidth);
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        // wallParent.layer = LayerMask.NameToLayer("whatisGround");
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
        for (int x =0; x < surface.Count; x++){
            surface[x].BuildNavMesh();
        }
        int[] spawnPoints = new int[surface.Count / 3]; 
        for (int x = 0; x < surface.Count/3; x++)
        {
            GameObject guy = Instantiate(enemy);
            int room_spawn = Random.Range(0, listOfRooms.Count);
            for (int y = 0; y < surface.Count / 3; y++)
            {
                if (spawnPoints[y] == room_spawn)
                {
                    room_spawn = Random.Range(0, listOfRooms.Count);
                }
            }
            guy.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(new Vector3((listOfRooms[room_spawn].BottomLeftAreaCorner.x + listOfRooms[room_spawn].TopRightAreaCorner.x) / 2, 1.113f, (listOfRooms[room_spawn].BottomLeftAreaCorner.y + listOfRooms[room_spawn].TopRightAreaCorner.y) / 2));
            spawnPoints[x] = room_spawn;
            
        }
        // GameObject enemy = new GameObject("Enemy",typeof(MeshFilter),typeof(MeshRenderer),typeof(CapsuleCollider));
        // enemy.GetComponent<MeshFilter>().mesh = enem;
        // enemy.GetComponent<MeshRenderer>().material = material;
        //int room_spawn = Random.Range(0,listOfRooms.Count);
        //enemy.GetComponent<NavMeshAgent>().Warp(new Vector3((listOfRooms[room_spawn].BottomLeftAreaCorner.x+listOfRooms[room_spawn].TopRightAreaCorner.x)/2,1.113f,(listOfRooms[room_spawn].BottomLeftAreaCorner.y+listOfRooms[room_spawn].TopRightAreaCorner.y)/2));
        int player_spawn = Random.Range(0,listOfRooms.Count);
        for (int y = 0; y < surface.Count / 3; y++)
        {
            if (spawnPoints[y] == player_spawn)
            {
                player_spawn = Random.Range(0, listOfRooms.Count);
            }
        }
        /*while (player_spawn == room_spawn){
            player_spawn = Random.Range(0,listOfRooms.Count);
        }*/
        Player.transform.position =  new Vector3((listOfRooms[player_spawn].BottomLeftAreaCorner.x+listOfRooms[player_spawn].TopRightAreaCorner.x)/2,1.113f,(listOfRooms[player_spawn].BottomLeftAreaCorner.y+listOfRooms[player_spawn].TopRightAreaCorner.y)/2);
        int goal_spawn = Random.Range(0,listOfRooms.Count);
        while (goal_spawn == player_spawn){
            goal_spawn = Random.Range(0,listOfRooms.Count);
        }
        goal.transform.position =  new Vector3((listOfRooms[goal_spawn].BottomLeftAreaCorner.x+listOfRooms[goal_spawn].TopRightAreaCorner.x)/2,1.113f,(listOfRooms[goal_spawn].BottomLeftAreaCorner.y+listOfRooms[goal_spawn].TopRightAreaCorner.y)/2);
        CreateWalls(wallParent);
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        // mesh.layer = "Ground";

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer),typeof(MeshCollider), typeof(NavMeshSurface));
        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshCollider>().sharedMesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;
        dungeonFloor.layer = LayerMask.NameToLayer("whatisGround");

        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        surface.Add(dungeonFloor.GetComponent<NavMeshSurface>());
        // dungeonFloor.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point)){
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyAllChildren()
    {
        while(transform.childCount != 0)
        {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}