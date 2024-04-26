using Microsoft.AspNetCore.Components;

namespace LIN.Calendar.Shared;


public partial class Calendar
{

    /// <summary>
    /// Lista de eventos.
    /// </summary>
    [Parameter]
    public List<Types.Calendar.Models.EventModel> Events { get; set; } = [];


    /// <summary>
    /// Al presionar sobre una fecha.
    /// </summary>
    [Parameter]
    public Action<DateTime?> OnClick { get; set; } = (date) => { };



    /// <summary>
    /// Hoy.
    /// </summary>
    private readonly DateTime Today = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


    [Parameter]
    public DateTime Selected { get; set; }



    /// <summary>
    /// Año actual.
    /// </summary>
    private int CurrentYear = DateTime.Now.Year;



    /// <summary>
    /// Mes actual.
    /// </summary>
    private int CurrentMonth = DateTime.Now.Month;



    /// <summary>
    /// Al establecer parámetros.
    /// </summary>
    protected override void OnParametersSet()
    {
        StateHasChanged();
        base.OnParametersSet();
    }



    /// <summary>
    /// Agregar 1 Mes.
    /// </summary>
    void After()
    {
        var newDate = new DateTime(CurrentYear, CurrentMonth, 1).AddMonths(1);
        CurrentMonth = newDate.Month;
        CurrentYear = newDate.Year;
        StateHasChanged();
    }



    /// <summary>
    /// Eliminar un mes.
    /// </summary>
    void Before()
    {
        var newDate = new DateTime(CurrentYear, CurrentMonth, 1).AddMonths(-1);
        CurrentMonth = newDate.Month;
        CurrentYear = newDate.Year;
        StateHasChanged();
    }


}