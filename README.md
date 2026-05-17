# 🏨 App Hotel - MAUI

Projeto desenvolvido como parte da **Agenda 13 - Desenvolvimento Mobile**.
Este é um aplicativo multiplataforma construído com **.NET MAUI (C# e XAML)**.

## 📱 Sobre o Projeto

O aplicativo permite a simulação de uma reserva de hotel com as seguintes funcionalidades:
- 🧑‍💼 Coleta de dados do hóspede principal (Nome, E-mail, Telefone).
- 📅 Seleção do período da estadia (Check-in e Check-out com validação).
- 👥 Definição da quantidade de hóspedes (Adultos e Crianças) usando controles `Stepper`.
- 🛏️ Escolha do tipo de quarto através de um `Picker`, com cálculo automático baseado na quantidade de dias e hóspedes.
- 🧑‍💻 Página autoral "Sobre o Desenvolvedor" com foto real, implementando navegação estruturada.

## 📸 Telas do Aplicativo

*(As imagens abaixo mostram a interface do aplicativo rodando. Para adicionar seus próprios prints, basta substituir ou adicionar arquivos na pasta `prints` e fazer o commit no GitHub)*

### Tela Principal e Tela Sobre:
*(Você pode colocar os prints aqui)*

![Tela Principal](prints/tela_principal.png)
![Tela Sobre](prints/tela_sobre.png)

## 🛠️ Tecnologias Utilizadas

- **C# / .NET 8**
- **.NET MAUI** (Multi-platform App UI)
- **XAML** para componentização e estilização
- **Padrão MVVM** (Model-View-ViewModel) para separar a interface (MainPage) da lógica de negócios (HotelLogica).

## 🚀 Como executar este projeto

1. Tenha o **Visual Studio 2022** instalado com o workload de **Desenvolvimento de interface do usuário de aplicativo multiplataforma do .NET** (.NET MAUI).
2. Clone o repositório:
   ```bash
   git clone https://github.com/valdandaconceicao-boop/AppHotel.git
   ```
3. Abra a solução no Visual Studio e clique no botão verde de **Windows Machine** (ou Emulador Android) para executar.
