using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AppHotel.Models;

public class HotelLogica : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public ObservableCollection<Quarto> ListaQuartos { get; set; } = new();

    private Quarto? _quartoSelecionado;
    public Quarto? QuartoSelecionado
    {
        get => _quartoSelecionado;
        set
        {
            if (_quartoSelecionado == value) return;
            _quartoSelecionado = value;
            OnPropertyChanged(nameof(QuartoSelecionado));
            OnPropertyChanged(nameof(TemQuartoSelecionado));
            OnPropertyChanged(nameof(DescricaoQuartoSelecionado));
            OnPropertyChanged(nameof(PrecoQuartoSelecionado));
            OnPropertyChanged(nameof(ValorTotal));
        }
    }

    private string _nome = string.Empty;
    public string Nome
    {
        get => _nome;
        set
        {
            if (_nome == value) return;
            _nome = value;
            OnPropertyChanged(nameof(Nome));
        }
    }

    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set
        {
            if (_email == value) return;
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    private string _telefone = string.Empty;
    public string Telefone
    {
        get => _telefone;
        set
        {
            if (_telefone == value) return;
            _telefone = value;
            OnPropertyChanged(nameof(Telefone));
        }
    }

    private double _quantidadeAdultos = 1;
    public double QuantidadeAdultos
    {
        get => _quantidadeAdultos;
        set
        {
            var clamped = Math.Clamp(Math.Round(value), 1, 10);
            if (Math.Abs(_quantidadeAdultos - clamped) < 0.001) return;
            _quantidadeAdultos = clamped;
            OnPropertyChanged(nameof(QuantidadeAdultos));
            OnPropertyChanged(nameof(ValorTotal));
        }
    }

    private double _quantidadeCriancas;
    public double QuantidadeCriancas
    {
        get => _quantidadeCriancas;
        set
        {
            var clamped = Math.Clamp(Math.Round(value), 0, 5);
            if (Math.Abs(_quantidadeCriancas - clamped) < 0.001) return;
            _quantidadeCriancas = clamped;
            OnPropertyChanged(nameof(QuantidadeCriancas));
            OnPropertyChanged(nameof(ValorTotal));
        }
    }

    private DateTime _dataCheckIn = DateTime.Today;
    public DateTime DataCheckIn
    {
        get => _dataCheckIn;
        set
        {
            if (_dataCheckIn == value) return;
            _dataCheckIn = value;
            OnPropertyChanged(nameof(DataCheckIn));
            OnPropertyChanged(nameof(DataMinimaSaida));
            OnPropertyChanged(nameof(ValorTotal));

            if (DataCheckOut <= value)
                DataCheckOut = value.AddDays(1);
        }
    }

    private DateTime _dataCheckOut = DateTime.Today.AddDays(1);
    public DateTime DataCheckOut
    {
        get => _dataCheckOut;
        set
        {
            if (_dataCheckOut == value) return;
            _dataCheckOut = value;
            OnPropertyChanged(nameof(DataCheckOut));
            OnPropertyChanged(nameof(ValorTotal));
        }
    }

    public DateTime DataMinima => DateTime.Today;
    public DateTime DataMinimaSaida => DataCheckIn.AddDays(1);
    public bool TemQuartoSelecionado => QuartoSelecionado != null;
    public string DescricaoQuartoSelecionado => QuartoSelecionado?.Descricao ?? string.Empty;

    public string PrecoQuartoSelecionado
    {
        get
        {
            if (QuartoSelecionado == null) return string.Empty;
            var adulto = QuartoSelecionado.ValorDiariaAdulto.ToString("C2");
            var crianca = QuartoSelecionado.ValorDiariaCrianca;
            return crianca > 0
                ? $"Adulto: {adulto}/dia | Crianca: {crianca:C2}/dia"
                : $"Adulto: {adulto}/dia | Nao acomoda criancas";
        }
    }

    public double ValorTotal
    {
        get
        {
            if (QuartoSelecionado == null) return 0;
            var dias = (DataCheckOut - DataCheckIn).Days;
            if (dias <= 0) return 0;
        return ((int)QuantidadeAdultos * QuartoSelecionado.ValorDiariaAdulto
            + (int)QuantidadeCriancas * QuartoSelecionado.ValorDiariaCrianca) * dias;
        }
    }

    public HotelLogica()
    {
        ListaQuartos.Add(new Quarto
        {
            NomeQuarto = "Suite Master",
            Descricao = "Quarto luxuoso com vista para o mar",
            ValorDiariaAdulto = 350.0,
            ValorDiariaCrianca = 150.0
        });
        ListaQuartos.Add(new Quarto
        {
            NomeQuarto = "Quarto Duplo",
            Descricao = "Ideal para casais ou amigos",
            ValorDiariaAdulto = 250.0,
            ValorDiariaCrianca = 120.0
        });
        ListaQuartos.Add(new Quarto
        {
            NomeQuarto = "Quarto Familia",
            Descricao = "Espacoso para toda a familia",
            ValorDiariaAdulto = 300.0,
            ValorDiariaCrianca = 100.0
        });
        ListaQuartos.Add(new Quarto
        {
            NomeQuarto = "Quarto Individual",
            Descricao = "Conforto e praticidade para um hospede",
            ValorDiariaAdulto = 180.0,
            ValorDiariaCrianca = 0.0
        });
    }

    public string ObterResumo()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            return "Preencha o nome do hospede para continuar.";

        if (QuartoSelecionado == null)
            return "Selecione um tipo de quarto para continuar.";

        var dias = (DataCheckOut - DataCheckIn).Days;
        if (dias <= 0)
            return "A data de check-out deve ser posterior ao check-in.";

        if ((int)QuantidadeCriancas > 0 && QuartoSelecionado.ValorDiariaCrianca == 0)
            return $"O \"{QuartoSelecionado.NomeQuarto}\" nao acomoda criancas. Escolha outro quarto ou remova as criancas.";

        var hospedes = $"{(int)QuantidadeAdultos} adulto(s)";
        if ((int)QuantidadeCriancas > 0)
            hospedes += $" e {(int)QuantidadeCriancas} crianca(s)";

        return $"Reserva confirmada!\n" +
            $"Hospede: {Nome}\n" +
            $"Quarto: {QuartoSelecionado.NomeQuarto}\n" +
            $"Periodo: {DataCheckIn:dd/MM} a {DataCheckOut:dd/MM} ({dias} dia(s))\n" +
            $"Hospedes: {hospedes}\n" +
            $"Total: {ValorTotal:C2}";
    }

    public void Limpar()
    {
        Nome = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        QuantidadeAdultos = 1;
        QuantidadeCriancas = 0;
        DataCheckIn = DateTime.Today;
        DataCheckOut = DateTime.Today.AddDays(1);
        QuartoSelecionado = null;
    }
}