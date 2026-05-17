namespace AppHotel.Views;

/// <summary>
/// Code-behind da tela "Sobre o Desenvolvedor".
///
/// Essa tela é simples e estática — não tem ViewModel porque não há
/// dados dinâmicos. Tudo foi definido direto no XAML.
/// O único comportamento necessário é a navegação de volta.
/// </summary>
public partial class Sobre : ContentPage
{
    public Sobre()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Remove essa tela da pilha de navegação, voltando para a MainPage.
    /// PopAsync é o "desfazer" do PushAsync que trouxe a gente até aqui.
    /// </summary>
    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}