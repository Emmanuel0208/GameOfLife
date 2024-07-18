using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public int rows = 20; // Número de filas en la cuadrícula
    public int columns = 20; // Número de columnas en la cuadrícula
    public float cellSize = 20f; // Tamaño de cada célula en unidades de Unity
    public GameObject cellPrefab; // Prefab de la célula
    private CellScript[,] grid; // Matriz para almacenar las células
    public static GridManager Instance { get; private set; } // Instancia singleton
    public bool IsRunning { get; private set; } = false; // Indica si la simulación está en curso

    private Coroutine simulationCoroutine; // Referencia a la corrutina de la simulación

    private void Awake()
    {
        Instance = this; // Configuración de la instancia singleton
    }

    private void Start()
    {
        GenerateGrid(); // Generar la cuadrícula al iniciar
    }

    private void GenerateGrid()
    {
        grid = new CellScript[rows, columns]; // Inicializar la matriz de células

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Instanciar y posicionar cada célula
                GameObject cellObject = Instantiate(cellPrefab, transform);
                RectTransform cellRectTransform = cellObject.GetComponent<RectTransform>();
                cellRectTransform.sizeDelta = new Vector2(cellSize, cellSize);
                cellObject.transform.localPosition = new Vector3(j * cellSize, -i * cellSize, 0);
                grid[i, j] = cellObject.GetComponent<CellScript>();
            }
        }
    }

    public void StartSimulation()
    {
        if (simulationCoroutine == null)
        {
            IsRunning = true; // Iniciar la simulación
            simulationCoroutine = StartCoroutine(SimulationRoutine());
        }
    }

    public void PauseSimulation()
    {
        if (simulationCoroutine != null)
        {
            StopCoroutine(simulationCoroutine); // Pausar la simulación
            simulationCoroutine = null;
            IsRunning = false;
        }
    }

    private IEnumerator SimulationRoutine()
    {
        while (true)
        {
            UpdateGrid(); // Actualizar la cuadrícula
            yield return new WaitForSeconds(0.5f); // Esperar medio segundo entre actualizaciones
        }
    }

    private void UpdateGrid()
    {
        bool[,] newStates = new bool[rows, columns]; // Matriz temporal para nuevos estados

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int aliveNeighbors = CountAliveNeighbors(i, j); // Contar vecinos vivos
                if (grid[i, j].isAlive)
                {
                    // Aplicar reglas para células vivas
                    newStates[i, j] = aliveNeighbors == 2 || aliveNeighbors == 3;
                }
                else
                {
                    // Aplicar reglas para células muertas
                    newStates[i, j] = aliveNeighbors == 3;
                }
            }
        }

        // Actualizar los estados de las células
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                grid[i, j].SetAlive(newStates[i, j]);
            }
        }
    }

    private int CountAliveNeighbors(int x, int y)
    {
        int count = 0; // Contador de vecinos vivos

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // Saltar la célula actual

                int nx = x + i;
                int ny = y + j;

                // Verificar límites de la cuadrícula y contar vecinos vivos
                if (nx >= 0 && ny >= 0 && nx < rows && ny < columns && grid[nx, ny].isAlive)
                {
                    count++;
                }
            }
        }

        return count;
    }
}
