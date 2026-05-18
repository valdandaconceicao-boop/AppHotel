using Microsoft.Extensions.Logging;

namespace AppHotel;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if WINDOWS
        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(
            "FixPickerTitle",
            (handler, picker) =>
            {
                var comboBox = handler.PlatformView as Microsoft.UI.Xaml.Controls.ComboBox;
                if (comboBox != null)
                {
                    comboBox.Header = null;
                    comboBox.HeaderTemplate = null;
                    comboBox.PlaceholderText = picker.Title ?? "Selecione";
                    comboBox.PlaceholderForeground = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Microsoft.UI.Colors.Gray);
                }
            });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}