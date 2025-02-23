using LIN.Types.Calendar.Models;
using LIN.Types.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LIN.Calendar.Shared.Drawers;


public partial class DrawerPeople
{


    int Type = 0;


    public EventModel EventModel { get; set; } = new()
    {
        DateStart = DateTime.Now,
        EndStart = DateTime.Now,
    };


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




    /// <summary>
    /// Abrir drawer.
    /// </summary>
    public async void Show(DateTime time)
    {
        EventModel.DateStart = (time == default ? DateTime.Now : time);
        EventModel.EndStart = (time == default ? DateTime.Now : time);
        await JS.InvokeVoidAsync("ShowDrawer", _id, "btn-close-panel-ide");
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




    async void Create()
    {

        var actual = EventModel;
        EventModel = new()
        {
            DateStart = DateTime.Now,
            EndStart = DateTime.Now,
        };

        actual.Type = (Types.Calendar.Enumerations.EventTypes)Type;

        // Evaluar.
        if (actual.EndStart < actual.DateStart)
            actual.EndStart = actual.DateStart;

        var response = await LIN.Access.Calendar.Controllers.Events.Create(LIN.Access.Calendar.Session.Instance.Token, actual);

        if (response.Response == Responses.Success)
        {
            actual.Id = response.LastId;
            OnSuccess(actual);
        }

    }


}