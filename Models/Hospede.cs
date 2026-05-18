namespace AppHotel.Models;

public class Hospede
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public int QuantidadeAdultos { get; set; } = 1;
    public int QuantidadeCriancas { get; set; } = 0;
    public DateTime DataCheckIn { get; set; } = DateTime.Today;
    public DateTime DataCheckOut { get; set; } = DateTime.Today.AddDays(1);
}