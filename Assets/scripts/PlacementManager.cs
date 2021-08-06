using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    private GameObject dummy;
    private GameObject hoverTile;

    public Camera cam;
    public GameObject SoldierEl;
    public LayerMask mask;

    public SpriteRenderer debug;
    public GameObject go;

    public Vector2 GetMousePosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
    public GameObject GetCurrentHoverTile()
    {
        Vector2 mousePosition = GetMousePosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0, 0), 0.1f, mask, -100, 100);
        if (hit.collider != null)
        {
            Vector3 v = hit.collider.gameObject.transform.position;

            var txt = go.GetComponent<UnityEngine.UI.Text>();
            txt.color = Color.red;
            txt.text = $"{v.x};{v.y} / {mousePosition.x.ToString("N2")} ; {mousePosition.y.ToString("N2")}";

            //if (debug != null)
            //    debug.color = Color.green;
            //debug = hit.collider.GetComponent<SpriteRenderer>();
            //debug.color = Color.red;


            int x = Mathf.RoundToInt(v.x), y = Mathf.RoundToInt(v.y);

            MapGenerator.MapTile tile = MapGenerator.mapTiles[x, y];
            if (debug != null)
                debug.color = Color.green;
            debug = tile.Content[0].GetComponent<SpriteRenderer>();
            debug.color = Color.red;

            return hit.collider.gameObject;
        }
        else
            if (debug != null)
            debug.color = Color.green;

        return null;
    }

    private void MoveDummy()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var tile = GetCurrentHoverTile();
    }
}
