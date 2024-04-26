using LIN.Types.Calendar.Models;
using LIN.Types.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LIN.Calendar.Shared.Drawers;


public partial class DrawerEvent
{


    public EventModel EventModel { get; set; } = new();


    /// <summary>
    /// ID del elemento Html.
    /// </summary>
    public string _id = $"element-{Guid.NewGuid()}";


    /// <summary>
    /// Patron de búsqueda.
    /// </summary>
    private string Pattern { get; set; } = string.Empty;




    /// <summary>
    /// Evento al ocultar.
    /// </summary>
    [Parameter]
    public Action OnHide { get; set; } = () => { };


    /// <summary>
    /// Evento al mostrar.
    /// </summary>
    [Parameter]
    public Action OnShow { get; set; } = () => { };


    [Parameter]
    public Action<EventModel> OnSuccess { get; set; } = (e) => { };



    [Parameter]
    public Action<int> OnDelete { get; set; } = (e) => { };




    /// <summary>
    /// Abrir drawer.
    /// </summary>
    public async void Show(EventModel model)
    {
        EventModel = model;
        await JS.InvokeVoidAsync("ShowDrawer", _id, "btn-close-panel-ide", "btn-close-panel-ide-cll", "btn-close-panel-ide-cll2");
        StateHasChanged();
    }



    /// <summary>
    /// Evento al ocultar.
    /// </summary>
    [JSInvokable("OnHide")]
    public void HideJS() => OnHide.Invoke();



    /// <summary>
    /// Evento al abrir.
    /// </summary>
    [JSInvokable("OnShow")]
    public void ShowJS() => OnShow.Invoke();




    (string, string) GetClass()
    {
        switch (EventModel.Type)
        {
            case Types.Calendar.Enumerations.EventTypes.Event:
                return ("bg-amber-400", "text-amber-400");

            case Types.Calendar.Enumerations.EventTypes.Anniversary:
                return ("bg-blue-500", "text-blue-500");

            case Types.Calendar.Enumerations.EventTypes.Meeting:
                return ("bg-green-500", "text-green-500");

            default:
                return ("bg-rose-500", "text-rose-500");
        }
    }


    (bool, string) GetFaltante(DateTime date, DateTime endTime)
    {

        var diferencia = date - DateTime.Now;



        if (diferencia.Ticks < 0)
        {

            if (DateTime.Now < endTime)
            {

                diferencia = DateTime.Now - date;

                if (diferencia.TotalDays < -1)
                    return (true, $"Inicio hace {Math.Abs(diferencia.TotalDays):#} dias");

                if (diferencia.TotalMinutes < -60)
                    return (true, $"Inicio hace {Math.Abs(diferencia.TotalHours):#} horas");

                return (true, $"Inicio hace {Math.Abs(diferencia.TotalMinutes):#} minutos");
            }


            diferencia = DateTime.Now - endTime;

            if (diferencia.TotalDays > 1)
                return (false, $"Finalizo hace {Math.Abs(diferencia.TotalDays):#} dias");

            if (diferencia.TotalMinutes > 60)
                return (false, $"Finalizo hace {Math.Abs(diferencia.TotalHours):#} horas");

            return (false, $"Finalizo hace {Math.Abs(diferencia.TotalMinutes):#} minutos");


        }



        if (diferencia.TotalDays > 1)
            return (false, $"Faltan {diferencia.TotalDays:#} dias");

        if (diferencia.TotalMinutes > 60)
            return (false, $"Faltan {diferencia.TotalHours:#} horas");

        return (false, $"Faltan {diferencia.TotalMinutes:#} minutos");

    }



    async void Delete()
    {
        var delete = await LIN.Access.Calendar.Controllers.Events.Delete(EventModel.Id, LIN.Access.Calendar.Session.Instance.Token);

        if (delete.Response == Responses.Success)
            OnDelete(EventModel.Id);

    }


}


