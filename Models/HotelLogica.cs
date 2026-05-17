using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AppHotel.Models;

/// <summary>
/// HotelLogica é o "cérebro" da aplicação — aqui ficam todas as regras de negócio.
///
/// Implementamos INotifyPropertyChanged para que a interface (XAML) seja atualizada
/// automaticamente sempre que uma propriedade mudar. Isso é o padrão MVVM:
///   - Model     → os dados (Quarto, Hospede, Reserva)
///   - View      → a tela (MainPage.xaml)
///   - ViewModel → esta classe (HotelLogica), que conecta tudo
///
/// Na prática: quando o usuário altera a data no DatePicker, o Binding
/// avisa essa classe, que recalcula o preço e notifica a tela de volta.
/// </summary>
public class HotelLogica : INotifyPropertyChanged
{
    // ─── SISTEMA DE NOTIFICAÇÃO (INotifyPropertyChanged) ──────────────────
    // Esse evento é o mecanismo que avisa a tela quando algo mudou.
    // O XAML "assina" o evento automaticamente quando usa {Binding}.
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Dispara o evento de notificação para um campo específico.
    /// Usamos nameof(Propriedade) para evitar strings "mágicas" que quebram
    /// em refactorings — é mais seguro e legível.
    /// </summary>
    protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    // ─── LISTA DE QUARTOS ─────────────────────────────────────────────────
    // ObservableCollection: diferente de List<T>, ela dispara eventos
    // quando itens são adicionados/removidos, mantendo o Picker atualizado.
    public ObservableCollection<Quarto> ListaQuartos { get; set; } = new();

    // ─── PROPRIEDADES LIGADAS À TELA (backing field + notificação) ─────────
    // O padrão é sempre:
    //   1. Campo privado (_campo) guarda o valor real
    //   2. Propriedade pública que o XAML usa via {Binding}
    //   3. O setter chama OnPropertyChanged para atualizar a tela

    private Quarto? _quartoSelecionado;
    /// <summary>Quarto escolhido pelo usuário no Picker.</summary>
    public Quarto? QuartoSelecionado
    {
        get => _quartoSelecionado;
        set
        {
            _quartoSelecionado = value;
            // Notificamos tudo que depende do quarto escolhido
            OnPropertyChanged(nameof(QuartoSelecionado));
            OnPropertyChanged(nameof(ValorTotal));
            OnPropertyChanged(nameof(TemQuartoSelecionado));
            OnPropertyChanged(nameof(DescricaoQuartoSelecionado));
            OnPropertyChanged(nameof(PrecoQuartoSelecionado));
        }
    }

    private string _nome = string.Empty;
    /// <summary>Nome completo do hóspede principal.</summary>
    public string Nome
    {
        get => _nome;
        set { _nome = value; OnPropertyChanged(nameof(Nome)); }
    }

    private string _email = string.Empty;
    /// <summary>E-mail de contato. Usado apenas para exibição — sem envio real.</summary>
    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(nameof(Email)); }
    }

    private string _telefone = string.Empty;
    /// <summary>Telefone de contato do hóspede.</summary>
    public string Telefone
    {
        get => _telefone;
        set { _telefone = value; OnPropertyChanged(nameof(Telefone)); }
    }

    private int _quantidadeAdultos = 1;
    /// <summary>
    /// Quantidade de adultos. Mínimo: 1 (uma reserva precisa de pelo menos um adulto).
    /// O Stepper já garante esse mínimo visualmente, mas a lógica também valida.
    /// </summary>
    public int QuantidadeAdultos
    {
        get => _quantidadeAdultos;
        set
        {
            _quantidadeAdultos = value;
            OnPropertyChanged(nameof(QuantidadeAdultos));
            OnPropertyChanged(nameof(ValorTotal)); // Preço muda com a quantidade
        }
    }

    private int _quantidadeCriancas;
    /// <summary>Quantidade de crianças. Pode ser zero. Algumas diárias de criança são R$ 0.</summary>
    public int QuantidadeCriancas
    {
        get => _quantidadeCriancas;
        set
        {
            _quantidadeCriancas = value;
            OnPropertyChanged(nameof(QuantidadeCriancas));
            OnPropertyChanged(nameof(ValorTotal));
        }
    }

    private DateTime _dataCheckIn = DateTime.Today;
    /// <summary>
    /// Data de entrada. Inicializa com hoje para facilitar o uso.
    /// Quando o usuário muda o check-in, também avisamos a DataMinimaSaida
    /// para que o DatePicker de check-out ajuste seu mínimo automaticamente.
    /// </summary>
    public DateTime DataCheckIn
    {
        get => _dataCheckIn;
        set
        {
            _dataCheckIn = value;
            OnPropertyChanged(nameof(DataCheckIn));
            OnPropertyChanged(nameof(DataMinimaSaida)); // Check-out mínimo muda junto
            OnPropertyChanged(nameof(ValorTotal));

            // BUG CORRIGIDO: se o check-out for antes do novo check-in,
            // ajustamos automaticamente para check-in + 1 dia.
            if (DataCheckOut <= value)
                DataCheckOut = value.AddDays(1);
        }
    }

    private DateTime _dataCheckOut = DateTime.Today.AddDays(1);
    /// <summary>Data de saída. Deve ser sempre posterior ao check-in.</summary>
    public DateTime DataCheckOut
    {
        get => _dataCheckOut;
        set
        {
            _dataCheckOut = value;
            OnPropertyChanged(nameof(DataCheckOut));
            OnPropertyChanged(nameof(ValorTotal));
        }
    }

    // ─── PROPRIEDADES CALCULADAS (somente leitura, não têm setter) ─────────
    // Essas propriedades não armazenam valor — elas calculam na hora que são lidas.
    // Por isso não têm backing field, apenas um getter.

    /// <summary>Data mínima para check-in: hoje. Impede reservas no passado.</summary>
    public DateTime DataMinima => DateTime.Today;

    /// <summary>
    /// Data mínima para check-out: sempre um dia após o check-in escolhido.
    /// O DatePicker de check-out usa isso para bloquear datas inválidas.
    /// </summary>
    public DateTime DataMinimaSaida => DataCheckIn.AddDays(1);

    /// <summary>Verdadeiro quando o usuário já escolheu um quarto. Controla visibilidade de campos.</summary>
    public bool TemQuartoSelecionado => QuartoSelecionado != null;

    /// <summary>Descrição do quarto selecionado para exibir abaixo do Picker.</summary>
    public string DescricaoQuartoSelecionado => QuartoSelecionado?.Descricao ?? string.Empty;

    /// <summary>Texto formatado com os preços da diária para exibição rápida.</summary>
    public string PrecoQuartoSelecionado
    {
        get
        {
            if (QuartoSelecionado == null) return string.Empty;
            var adulto = QuartoSelecionado.ValorDiariaAdulto.ToString("C2");
            var crianca = QuartoSelecionado.ValorDiariaCrianca;

            // Se a diária de criança for 0, informamos que não acomoda crianças
            return crianca > 0
                ? $"Adulto: {adulto}/dia • Criança: {crianca:C2}/dia"
                : $"Adulto: {adulto}/dia • Não acomoda crianças";
        }
    }

    /// <summary>
    /// Valor total calculado da reserva.
    /// BUG CORRIGIDO: a versão anterior retornava o preço de 1 dia mesmo quando
    /// o check-out era antes do check-in. Agora retornamos 0 nesse caso.
    /// </summary>
    public double ValorTotal
    {
        get
        {
            if (QuartoSelecionado == null) return 0;

            var dias = (DataCheckOut - DataCheckIn).Days;

            // Datas inválidas → sem preço (antes retornava dias = 1, o que era errado)
            if (dias <= 0) return 0;

            return (QuantidadeAdultos * QuartoSelecionado.ValorDiariaAdulto
                  + QuantidadeCriancas * QuartoSelecionado.ValorDiariaCrianca) * dias;
        }
    }

    // ─── CONSTRUTOR ────────────────────────────────────────────────────────
    /// <summary>
    /// Inicializa a lógica com os quartos disponíveis no hotel.
    /// Em um app real, esses dados viriam de uma API ou banco de dados.
    /// Por enquanto, usamos dados fixos (hardcoded) para fins didáticos.
    /// </summary>
    public HotelLogica()
    {
        ListaQuartos.Add(new Quarto
        {
            NomeQuarto = "Suíte Master",
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
            NomeQuarto = "Quarto Família",
            Descricao = "Espaçoso para toda a família",
            ValorDiariaAdulto = 300.0,
            ValorDiariaCrianca = 100.0
        });
        ListaQuartos.Add(new Quarto
        {
            NomeQuarto = "Quarto Individual",
            Descricao = "Conforto e praticidade para um hóspede",
            ValorDiariaAdulto = 180.0,
            ValorDiariaCrianca = 0.0  // Não acomoda crianças
        });
    }

    // ─── MÉTODOS DE NEGÓCIO ─────────────────────────────────────────────────

    /// <summary>
    /// Valida os dados e retorna um resumo da reserva ou uma mensagem de erro.
    /// Sempre validar antes de exibir resultado — nunca confiar que o usuário
    /// preencheu tudo certo. Isso é "defensive programming".
    /// </summary>
    public string ObterResumo()
    {
        // Validação 1: campo obrigatório
        if (string.IsNullOrWhiteSpace(Nome))
            return "⚠️ Preencha o nome do hóspede para continuar.";

        // Validação 2: seleção obrigatória
        if (QuartoSelecionado == null)
            return "⚠️ Selecione um tipo de quarto para continuar.";

        // Validação 3: datas coerentes
        var dias = (DataCheckOut - DataCheckIn).Days;
        if (dias <= 0)
            return "⚠️ A data de check-out deve ser posterior ao check-in.";

        // Validação 4 (regra de negócio): Quarto Individual não acomoda crianças
        if (QuantidadeCriancas > 0 && QuartoSelecionado.ValorDiariaCrianca == 0)
            return $"⚠️ O \"{QuartoSelecionado.NomeQuarto}\" não acomoda crianças. Escolha outro quarto ou remova as crianças.";

        // Tudo certo — montamos o resumo final
        var hospedes = $"{QuantidadeAdultos} adulto(s)";
        if (QuantidadeCriancas > 0)
            hospedes += $" e {QuantidadeCriancas} criança(s)";

        return $"✅ Reserva confirmada!\n" +
               $"Hóspede: {Nome}\n" +
               $"Quarto: {QuartoSelecionado.NomeQuarto}\n" +
               $"Período: {DataCheckIn:dd/MM} → {DataCheckOut:dd/MM} ({dias} dia(s))\n" +
               $"Hóspedes: {hospedes}\n" +
               $"💰 Total: {ValorTotal:C2}";
    }

    /// <summary>
    /// Limpa todos os campos, voltando ao estado inicial.
    /// Útil para o botão "Nova Reserva" — evita que dados antigos
    /// permaneçam na tela após uma reserva ser feita.
    /// </summary>
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
        OnPropertyChanged(nameof(ValorTotal));
    }
}