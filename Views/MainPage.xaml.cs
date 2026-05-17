using AppHotel.Models;

namespace AppHotel.Views;

/// <summary>
/// Code-behind da tela principal.
///
/// Em MAUI com MVVM, o XAML cuida da aparência e os Bindings cuidam dos dados.
/// O code-behind fica enxuto: só trata eventos de UI que precisam de lógica
/// que não faz sentido colocar no ViewModel (ex: navegação, animação, alertas).
/// </summary>
public partial class MainPage : ContentPage
{
    // Guardamos a referência à lógica como campo readonly.
    // "readonly" garante que não vamos substituir o objeto por acidente depois.
    private readonly HotelLogica _logica;

    public MainPage()
    {
        InitializeComponent(); // Carrega e constrói todos os elementos do XAML

        // Criamos a instância da lógica e a definimos como BindingContext.
        // A partir daqui, qualquer {Binding Nome} no XAML vai buscar
        // a propriedade Nome dentro de _logica.
        _logica = new HotelLogica();
        BindingContext = _logica;
    }

    /// <summary>
    /// Disparado quando o usuário clica em "Calcular Reserva".
    /// Pedimos o resumo à lógica e exibimos na tela com cor adequada.
    /// </summary>
    private void OnCalcularClicked(object sender, EventArgs e)
    {
        var resultado = _logica.ObterResumo();

        // Exibimos o card de resultado (estava oculto)
        ResultadoFrame.IsVisible = true;
        ResultadoLabel.Text = resultado;

        // Feedback visual: aviso em laranja, sucesso em verde
        bool isErro = resultado.StartsWith("⚠️");
        ResultadoLabel.TextColor = isErro
            ? Color.FromArgb("#B45309")   // Âmbar escuro — legível e amigável
            : Color.FromArgb("#15803D");  // Verde escuro — transmite confirmação

        ResultadoFrame.BackgroundColor = isErro
            ? Color.FromArgb("#FFFBEB")   // Fundo amarelo suave
            : Color.FromArgb("#F0FDF4");  // Fundo verde suave
    }

    /// <summary>
    /// Navega para a tela "Sobre o Desenvolvedor".
    /// PushAsync empilha a nova tela por cima — o usuário volta com o botão voltar
    /// (nativo do NavigationPage ou o botão que colocamos na tela Sobre).
    /// </summary>
    private async void OnBotaoSobreClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Sobre());
    }
}