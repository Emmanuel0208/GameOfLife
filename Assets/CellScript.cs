using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour, IPointerClickHandler
{
    public bool isAlive; // Estado de vida de la c�lula
    public Image image; // Componente de imagen para representar el color de la c�lula

    private void Start()
    {
        image = GetComponent<Image>(); // Obtener el componente Image
        UpdateColor(); // Actualizar el color de la c�lula
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive; // Establecer el estado de vida
        UpdateColor(); // Actualizar el color de la c�lula
    }

    private void UpdateColor()
    {
        if (isAlive)
        {
            image.color = Color.yellow; // Color amarillo para c�lulas vivas
        }
        else
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 0.78f); // Color gris con alfa 200 para c�lulas muertas
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
