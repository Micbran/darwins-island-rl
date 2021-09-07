using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int columns;
    [SerializeField] private int rows;

    [SerializeField] private Image gridDisplay;

    private Canvas parentCanvas;
    private Transform baseTransform;

    private void Awake()
    {
        this.baseTransform = this.transform;
        this.parentCanvas = this.GetComponent<Canvas>();
        this.CreateGrid();
    }

    private void CreateGrid()
    {
        for (int row = 0; row < this.rows; row++)
        {
            for (int col = 0; col < this.columns; col++)
            {
                Instantiate(gridDisplay, this.baseTransform, false);
            }
        }
    }
}
