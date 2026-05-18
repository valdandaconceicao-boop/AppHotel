namespace AppHotel.Models;

public class Reserva
{
    public Hospede? Hospede { get; set; }
    public Quarto? QuartoSelecionado { get; set; }
    public int QuantidadeDias => (DataCheckOut - DataCheckIn).Days;
    public DateTime DataCheckIn => Hospede?.DataCheckIn ?? DateTime.Today;
    public DateTime DataCheckOut => Hospede?.DataCheckOut ?? DateTime.Today.AddDays(1);

    public double ValorTotal
    {
        get
        {
            if (Hospede == null || QuartoSelecionado == null)
                return 0;

            int dias = QuantidadeDias;
            if (dias <= 0) return 0;

            double totalAdultos = Hospede.QuantidadeAdultos * QuartoSelecionado.ValorDiariaAdulto * dias;
            double totalCriancas = Hospede.QuantidadeCriancas * QuartoSelecionado.ValorDiariaCrianca * dias;

            return totalAdultos + totalCriancas;
        }
    }
}