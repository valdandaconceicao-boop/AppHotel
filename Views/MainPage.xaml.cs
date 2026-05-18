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
        var resultado = _logica.ObterResumo();

        ResultadoFrame.IsVisible = true;
        ResultadoLabel.Text = resultado;

        bool isErro = resultado.Contains("preencha", StringComparison.OrdinalIgnoreCase)
                    || resultado.Contains("selecione", StringComparison.OrdinalIgnoreCase)
                    || resultado.Contains("deve ser", StringComparison.OrdinalIgnoreCase)
                    || resultado.Contains("nao acomoda", StringComparison.OrdinalIgnoreCase);

        ResultadoLabel.TextColor = isErro
            ? Color.FromArgb("#92400E")
            : Color.FromArgb("#166534");

        ResultadoFrame.BackgroundColor = isErro
            ? Color.FromArgb("#FFFBEB")
            : Color.FromArgb("#F0FDF4");

        ResultadoFrame.Stroke = isErro
            ? Color.FromArgb("#FCD34D")
            : Color.FromArgb("#86EFAC");

        BtnNovaReserva.IsVisible = !isErro;
    }

    private void OnNovaReservaClicked(object sender, EventArgs e)
    {
        _logica.Limpar();
        ResultadoFrame.IsVisible = false;
        BtnNovaReserva.IsVisible = false;
    }

    private async void OnBotaoSobreClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Sobre());
    }
}