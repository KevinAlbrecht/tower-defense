using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private Sprite wallSprite;

    public static MapTile[,] mapTiles;
    public static List<Vector3> EnemiesPath;
    public static MapTile startTile { get; private set; }
    public static MapTile endTile { get; private set; }

    public GameObject sceneryTile;
    public GameObject sceneryCoordinatesText;


    private void Start()
    {
        mapTiles = new MapTile[mapWidth, mapHeight];
        this.generateMap();
        var exit = this.calculateDijkstraPath();

        EnemiesPath = new List<Vector3>();

        while (exit != null)
        {
            EnemiesPath.Add(exit.position);
            var tile = mapTiles[(int)exit.position.x, (int)exit.position.y];
            //tile.Content[0].GetComponent<SpriteRenderer>().color = Color.green;
            exit = exit.precedent;
        }
        EnemiesPath.Reverse();
    }

    private bool isEdge(int x, int y)
    {
        if (x == 0 || y == 0 || x == mapWidth - 1 || y == mapHeight - 1) return true;
        return false;
    }

    private void generateMap()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //GameObject newsceneryCoordinatesText = Instantiate(sceneryCoordinatesText);
                GameObject newSceneryTile = Instantiate(sceneryTile);

                MapTile tile = new MapTile();

                newSceneryTile.transform.position = new Vector2(x, y);
                //newsceneryCoordinatesText.transform.position = new Vector3(x, y, -10);

                //var coordinateTextMesh = newsceneryCoordinatesText.GetComponent<TextMesh>();
                //coordinateTextMesh.text = $"{x},{y}";
                //coordinateTextMesh.color = Color.green;


                if (isEdge(x, y))
                {
                    newSceneryTile.GetComponent<SpriteRenderer>().color = new Color(18 / 255, 49 / 255, 132 / 255);
                    //newSceneryTile.GetComponent<SpriteRenderer>().sprite = wallSprite;
                    tile.isBlocked = true;
                }

                tile.Content.Add(newSceneryTile);
                tile.position = new Vector2(x, y);

                mapTiles[x, y] = tile;
            }
        }
        int[,] coordinates = new int[,] {
            { Random.Range(1, mapWidth-1), 0 },
            { Random.Range(1, mapWidth-1), mapHeight - 1 } };

        startTile = mapTiles[coordinates[0, 0], coordinates[0, 1]];
        endTile = mapTiles[coordinates[1, 0], coordinates[1, 1]];
        startTile.Content[0].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        endTile.Content[0].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    private float getTotalDistance(Vector3 position)
    {
        return Vector3.Distance(startTile.position, position) + Vector3.Distance(endTile.position, position);
    }

    private bool isVectorOutOfRange(Vector3 p)
    {
        return (p.x <= 0 ||
          p.x >= mapWidth - 1 ||
          p.y <= 0 ||
          p.x >= mapHeight - 1) ? true : false;
    }

    private Mem calculateDijkstraPath()
    {
        //debug safe loop
        int i = 0;


        var visited = new Dictionary<Vector3, Mem>();

        var startingPoint = new Mem() { distance = getTotalDistance(startTile.position), precedent = null, position = startTile.position };
        Mem prevPosition = null;

        List<Mem> nextToVisits = new List<Mem>();
        List<Mem> toVisits = new List<Mem>() { startingPoint };

        while (toVisits.Count > 0)
        {
            //debug safe loop
            i++;
            if (i >= 1000)
            {
                Debug.Log("SAFE LOOP EXIT");
                break;
            }
            //

            var currentToVisit = toVisits[0];
            toVisits.RemoveAt(0);


            if (currentToVisit.position == endTile.position)
            {
                return currentToVisit;
            }

            if (visited.ContainsKey(currentToVisit.position))
                continue;

            visited.Add(currentToVisit.position, currentToVisit);
            //if (visited.ContainsKey(currentToVisit.position))
            //{
            //    var oldVisited = visited[currentToVisit.position];
            //    if (oldVisited.distance > currentToVisit.distance)
            //    {
            //        visited[currentToVisit.position] = currentToVisit;
            //    }
            //}
            //else
            //{
            //    visited.Add(currentToVisit.position, currentToVisit);
            //}

            float x = currentToVisit.position.x,
                y = currentToVisit.position.y;

            Vector3 left = new Vector3(x - 1, y);
            Vector3 right = new Vector3(x + 1, y);
            Vector3 top = new Vector3(x, y + 1);
            Vector3 bottom = new Vector3(x, y - 1);

            if (!isVectorOutOfRange(left) && !visited.ContainsKey(left))
                nextToVisits.Add(new Mem() { position = left, precedent = currentToVisit });
            if (!isVectorOutOfRange(right) && !visited.ContainsKey(right))
                nextToVisits.Add(new Mem() { position = right, precedent = currentToVisit });
            if (!isVectorOutOfRange(top) && !visited.ContainsKey(top))
                nextToVisits.Add(new Mem() { position = top, precedent = currentToVisit });
            if (!isVectorOutOfRange(bottom) && !visited.ContainsKey(bottom))
                nextToVisits.Add(new Mem() { position = bottom, precedent = currentToVisit });

            if (toVisits.Count == 0)
            {
                toVisits.AddRange(nextToVisits);
                toVisits.OrderByDescending(t => t.distance);
                nextToVisits = new List<Mem>();
            }
        }

        Debug.Log($"Task FINISHED badly");
        return null;
    }
    class Mem
    {
        public Vector3 position { get; set; }
        public float distance { get; set; }
        public Mem precedent { get; set; }
        public bool isOver { get; set; }
    }
    public class MapTile
    {
        public List<GameObject> Content { get; set; }
        public Vector3 position { get; set; }
        public bool isBlocked { get; set; }

        public MapTile()
        {
            Content = new List<GameObject>();
        }
    }
}
