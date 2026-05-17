using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AppHotel.Models;

public class HotelLogica : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ObservableCollection<Quarto> ListaQuartos { get; set; } = new();

    private Quarto? _quartoSelecionado;
    public Quarto? QuartoSelecionado
    {
        get => _quartoSelecionado;
        set { _quartoSelecionado = value; OnPropertyChanged(nameof(QuartoSelecionado)); OnPropertyChanged(nameof(ValorTotal)); }
    }

    private string _nome = string.Empty;
    public string Nome
    {
        get => _nome;
        set { _nome = value; OnPropertyChanged(nameof(Nome)); }
    }

    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(nameof(Email)); }
    }

    private string _telefone = string.Empty;
    public string Telefone
    {
        get => _telefone;
        set { _telefone = value; OnPropertyChanged(nameof(Telefone)); }
    }

    private int _quantidadeAdultos = 1;
    public int QuantidadeAdultos
    {
        get => _quantidadeAdultos;
        set { _quantidadeAdultos = value; OnPropertyChanged(nameof(QuantidadeAdultos)); OnPropertyChanged(nameof(ValorTotal)); }
    }

    private int _quantidadeCriancas;
    public int QuantidadeCriancas
    {
        get => _quantidadeCriancas;
        set { _quantidadeCriancas = value; OnPropertyChanged(nameof(QuantidadeCriancas)); OnPropertyChanged(nameof(ValorTotal)); }
    }

    private DateTime _dataCheckIn = DateTime.Now;
    public DateTime DataCheckIn
    {
        get => _dataCheckIn;
        set { _dataCheckIn = value; OnPropertyChanged(nameof(DataCheckIn)); OnPropertyChanged(nameof(ValorTotal)); }
    }

    private DateTime _dataCheckOut = DateTime.Now.AddDays(1);
    public DateTime DataCheckOut
    {
        get => _dataCheckOut;
        set { _dataCheckOut = value; OnPropertyChanged(nameof(DataCheckOut)); OnPropertyChanged(nameof(ValorTotal)); }
    }

    public double ValorTotal
    {
        get
        {
            if (QuartoSelecionado == null) return 0;
            var dias = (DataCheckOut - DataCheckIn).Days;
            if (dias <= 0) dias = 1;
            return (QuantidadeAdultos * QuartoSelecionado.ValorDiariaAdulto + QuantidadeCriancas * QuartoSelecionado.ValorDiariaCrianca) * dias;
        }
    }

    public HotelLogica()
    {
        ListaQuartos.Add(new Quarto { NomeQuarto = "Suíte Master", Descricao = "Quarto luxuoso com vista para o mar", ValorDiariaAdulto = 350.0, ValorDiariaCrianca = 150.0 });
        ListaQuartos.Add(new Quarto { NomeQuarto = "Quarto Duplo", Descricao = "Ideal para casais ou amigos", ValorDiariaAdulto = 250.0, ValorDiariaCrianca = 120.0 });
        ListaQuartos.Add(new Quarto { NomeQuarto = "Quarto Família", Descricao = "Espaçoso para toda a família", ValorDiariaAdulto = 300.0, ValorDiariaCrianca = 100.0 });
        ListaQuartos.Add(new Quarto { NomeQuarto = "Quarto Individual", Descricao = "Conforto e praticidade para um hóspede", ValorDiariaAdulto = 180.0, ValorDiariaCrianca = 0.0 });
    }

    public string ObterResumo()
    {
        if (string.IsNullOrWhiteSpace(Nome)) return "Preencha os dados do hóspede.";
        if (QuartoSelecionado == null) return "Selecione um quarto.";
        var dias = (DataCheckOut - DataCheckIn).Days;
        if (dias <= 0) return "Data de check-out deve ser maior que check-in.";
        return $"Reserva para {Nome} no {QuartoSelecionado.NomeQuarto} por {dias} dia(s). Total: {ValorTotal:C2}";
    }

    public void Limpar()
    {
        Nome = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        QuantidadeAdultos = 1;
        QuantidadeCriancas = 0;
        DataCheckIn = DateTime.Now;
        DataCheckOut = DateTime.Now.AddDays(1);
        QuartoSelecionado = null;
        OnPropertyChanged(nameof(ValorTotal));
    }
}