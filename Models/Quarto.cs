namespace AppHotel.Models;

public class Quarto
{
    public string NomeQuarto { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public double ValorDiariaAdulto { get; set; }
    public double ValorDiariaCrianca { get; set; }
    public string ImagemUrl { get; set; } = string.Empty;
}