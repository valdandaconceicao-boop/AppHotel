# App Hotel - MAUI

Projeto desenvolvido para a Agenda 13 da disciplina de Desenvolvimento Mobile.
Trata-se de um aplicativo multiplataforma para simulação de reservas de hotel construído com .NET MAUI.

## Sobre o Projeto

O aplicativo permite preencher os dados de um hóspede e calcular o valor total de uma estadia. Funcionalidades implementadas:
- Cadastro de dados básicos do hóspede (Nome, E-mail, Telefone).
- Seleção de período de estadia (Check-in e Check-out) com validação de datas.
- Controle de quantidade de hóspedes (Adultos e Crianças) utilizando Steppers limitadores.
- Seleção de tipo de quarto através de Picker, com exibição de valor e acomodação.
- Cálculo automático do valor total baseado na quantidade de dias e regras de negócio.
- Página sobre o desenvolvedor com navegação.

## Tecnologias

- C#
- .NET 8
- .NET MAUI
- Padrão MVVM (Model-View-ViewModel) para separação de lógica e interface.

## Como Executar

1. Necessário ter o Visual Studio 2022 instalado com suporte para .NET MAUI.
2. Clone este repositório:
   ```bash
   git clone https://github.com/valdandaconceicao-boop/AppHotel.git
   ```
3. Abra a solução no Visual Studio.
4. Selecione "Windows Machine" ou um Emulador Android como destino e execute o projeto (F5).

## Telas

*(Para adicionar os prints do aplicativo, salve as imagens na pasta `prints/` e referencie aqui)*

- [Tela Principal](prints/tela_principal.png)
- [Tela Sobre](prints/tela_sobre.png)
