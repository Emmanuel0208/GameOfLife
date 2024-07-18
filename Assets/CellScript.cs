using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour, IPointerClickHandler
{
    public bool isAlive; // Estado de vida de la célula
    public Image image; // Componente de imagen para representar el color de la célula

    private void Start()
    {
        image = GetComponent<Image>(); // Obtener el componente Image
        UpdateColor(); // Actualizar el color de la célula
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive; // Establecer el estado de vida
        UpdateColor(); // Actualizar el color de la célula
    }

    private void UpdateColor()
    {
        if (isAlive)
        {
            image.color = Color.yellow; // Color amarillo para células vivas
        }
        else
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 0.78f); // Color gris con alfa 200 para células muertas
        }
    }

    public void ToggleAlive()
    {
        SetAlive(!isAlive); // Cambiar el estado de vida
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleAlive(); // Cambiar el estado de vida al hacer clic
    }
}
