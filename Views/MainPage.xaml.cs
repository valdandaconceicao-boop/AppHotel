using AppHotel.Models;

namespace AppHotel.Views;

public partial class MainPage : ContentPage
{
    private readonly HotelLogica _logica;

    public MainPage()
    {
        InitializeComponent();
        _logica = new HotelLogica();
        BindingContext = _logica;
    }

    private void OnCalcularClicked(object sender, EventArgs e)
    {
        ResultadoLabel.Text = _logica.ObterResumo();
    }

    private async void OnBotaoSobreClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Sobre());
    }
}