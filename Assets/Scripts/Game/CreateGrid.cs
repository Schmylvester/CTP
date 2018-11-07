using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [SerializeField] AddUnit add_unit;
    [SerializeField] Transform grid_parent;
    [SerializeField] Grid grid;
    [SerializeField] GameObject cell_prefab;
    List<Cell> cell_objects;

    private void Start()
    {
        cell_objects = new List<Cell>();
        createGrid(20, 10);
        setNeighbours();
        add_unit.init();
        add_unit.addUnit(Class.Rogue, new Vector2Int(0, 1));
        add_unit.addUnit(Class.Warrior, new Vector2Int(0, 2));
        add_unit.addUnit(Class.Warrior, new Vector2Int(0, 6));
    }

    void createGrid(int w, int h)
    {
        transform.position -= new Vector3(((float)w / 2) * grid.cellSize.x, ((float)h / 2) * grid.cellSize.y);
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector3 position = grid.CellToWorld(new Vector3Int(x, y, 0));
                Cell cell = Instantiate(cell_prefab, position,
                    Quaternion.identity, grid_parent).GetComponent<Cell>();
                cell.setPos(x, y);
                cell_objects.Add(cell);
            }
        }
    }

    private void setNeighbours()
    {
        for (int i = 0; i < cell_objects.Count; i++)
        {
            for (int j = 0; j < cell_objects.Count; j++)
            {
                Vector2Int pos_a = cell_objects[i].getPos();
                Vector2Int pos_b = cell_objects[j].getPos();

                if (pos_a.x == pos_b.x)
                    if (pos_a.y == pos_b.y + 1)
                        cell_objects[i].addNeighbour(cell_objects[j], Dir.S);
                    else if (pos_a.y == pos_b.y - 1)
                        cell_objects[i].addNeighbour(cell_objects[j], Dir.N);
                if (pos_a.y == pos_b.y)
                    if (pos_a.x == pos_b.x + 1)
                        cell_objects[i].addNeighbour(cell_objects[j], Dir.W);
                    else if (pos_a.x == pos_b.x - 1)
                        cell_objects[i].addNeighbour(cell_objects[j], Dir.E);
            }
        }
    }
}
