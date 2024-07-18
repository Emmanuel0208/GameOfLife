using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public int rows = 20; // N�mero de filas en la cuadr�cula
    public int columns = 20; // N�mero de columnas en la cuadr�cula
    public float cellSize = 20f; // Tama�o de cada c�lula en unidades de Unity
    public GameObject cellPrefab; // Prefab de la c�lula
    private CellScript[,] grid; // Matriz para almacenar las c�lulas
    public static GridManager Instance { get; private set; } // Instancia singleton
    public bool IsRunning { get; private set; } = false; // Indica si la simulaci�n est� en curso

    private Coroutine simulationCoroutine; // Referencia a la corrutina de la simulaci�n

    private void Awake()
    {
        Instance = this; // Configuraci�n de la instancia singleton
    }

    private void Start()
    {
        GenerateGrid(); // Generar la cuadr�cula al iniciar
    }

    private void GenerateGrid()
    {
        grid = new CellScript[rows, columns]; // Inicializar la matriz de c�lulas

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                // Instanciar y posicionar cada c�lula
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
            IsRunning = true; // Iniciar la simulaci�n
            simulationCoroutine = StartCoroutine(SimulationRoutine());
        }
    }

    public void PauseSimulation()
    {
        if (simulationCoroutine != null)
        {
            StopCoroutine(simulationCoroutine); // Pausar la simulaci�n
            simulationCoroutine = null;
            IsRunning = false;
        }
    }

    private IEnumerator SimulationRoutine()
    {
        while (true)
        {
            UpdateGrid(); // Actualizar la cuadr�cula
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
                    // Aplicar reglas para c�lulas vivas
                    newStates[i, j] = aliveNeighbors == 2 || aliveNeighbors == 3;
                }
                else
                {
                    // Aplicar reglas para c�lulas muertas
                    newStates[i, j] = aliveNeighbors == 3;
                }
            }
        }

        // Actualizar los estados de las c�lulas
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
                if (i == 0 && j == 0) continue; // Saltar la c�lula actual

                int nx = x + i;
                int ny = y + j;

                // Verificar l�mites de la cuadr�cula y contar vecinos vivos
                if (nx >= 0 && ny >= 0 && nx < rows && ny < columns && grid[nx, ny].isAlive)
                {
                    count++;
                }
            }
        }

        return count;
    }
}
