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

        Microsoft.Maui.Handlers.StepperHandler.Mapper.AppendToMapping(
            "FixStepperColors",
            (handler, stepper) =>
            {
                var platform = handler.PlatformView;
                if (platform == null) return;

                var darkBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                    Microsoft.UI.ColorHelper.FromArgb(255, 27, 67, 50));

                platform.Resources["RepeatButtonForeground"] = darkBrush;
                platform.Resources["RepeatButtonForegroundPressed"] = darkBrush;
                platform.Resources["RepeatButtonForegroundPointerOver"] = darkBrush;
                platform.Resources["RepeatButtonForegroundDisabled"] =
                    new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Microsoft.UI.ColorHelper.FromArgb(255, 160, 160, 160));

                if (platform is Microsoft.UI.Xaml.FrameworkElement fe)
                {
                    fe.Loaded += (s, e2) =>
                    {
                        ApplyStepperButtonColors(fe, darkBrush);
                    };
                }
            });

        static void ApplyStepperButtonColors(Microsoft.UI.Xaml.FrameworkElement root,
            Microsoft.UI.Xaml.Media.SolidColorBrush brush)
        {
            int count = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(root, i);
                if (child is Microsoft.UI.Xaml.Controls.Primitives.RepeatButton btn)
                    btn.Foreground = brush;
                if (child is Microsoft.UI.Xaml.FrameworkElement feChild)
                    ApplyStepperButtonColors(feChild, brush);
            }
        }
        #endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}