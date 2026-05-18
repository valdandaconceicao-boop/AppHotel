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
        // --- Fix Picker placeholder ---
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

        // --- Fix DatePicker: prevent text/icon from disappearing on hover ---
        Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping(
            "FixDatePickerHover",
            (handler, datePicker) =>
            {
                if (handler.PlatformView is not Microsoft.UI.Xaml.FrameworkElement fe) return;

                fe.Loaded += (s, e2) =>
                {
                    FixControlOnHover(fe);
                };
            });

        // --- Fix Stepper: prevent +/- icons from disappearing on hover ---
        Microsoft.Maui.Handlers.StepperHandler.Mapper.AppendToMapping(
            "FixStepperHover",
            (handler, stepper) =>
            {
                if (handler.PlatformView is not Microsoft.UI.Xaml.FrameworkElement fe) return;

                var darkBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                    Microsoft.UI.ColorHelper.FromArgb(255, 27, 67, 50));
                var hoverBg = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                    Microsoft.UI.ColorHelper.FromArgb(255, 220, 218, 212));
                var pressedBg = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                    Microsoft.UI.ColorHelper.FromArgb(255, 27, 67, 50));
                var pressedFg = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                    Microsoft.UI.Colors.White);

                fe.Resources["RepeatButtonForeground"] = darkBrush;
                fe.Resources["RepeatButtonForegroundPointerOver"] = darkBrush;
                fe.Resources["RepeatButtonForegroundPressed"] = pressedFg;
                fe.Resources["RepeatButtonBackgroundPointerOver"] = hoverBg;
                fe.Resources["RepeatButtonBackgroundPressed"] = pressedBg;

                fe.Loaded += (s, e2) =>
                {
                    FixStepperButtons(fe, darkBrush);
                };
            });

        static void FixControlOnHover(Microsoft.UI.Xaml.FrameworkElement root)
        {
            int count = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(root, i);

                // Fix Button: keep foreground visible on hover
                if (child is Microsoft.UI.Xaml.Controls.Button btn)
                {
                    var fg = btn.Foreground;
                    btn.PointerEntered += (s, e) => { btn.Foreground = fg; };
                    btn.PointerExited += (s, e) => { btn.Foreground = fg; };
                }

                if (child is Microsoft.UI.Xaml.FrameworkElement feChild)
                    FixControlOnHover(feChild);
            }
        }

        static void FixStepperButtons(Microsoft.UI.Xaml.FrameworkElement root,
            Microsoft.UI.Xaml.Media.SolidColorBrush brush)
        {
            int count = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(root, i);
                if (child is Microsoft.UI.Xaml.Controls.Primitives.RepeatButton btn)
                {
                    btn.Foreground = brush;
                    btn.PointerEntered += (s, e) => { btn.Foreground = brush; };
                    btn.PointerExited += (s, e) => { btn.Foreground = brush; };
                    btn.PointerPressed += (s, e) => { btn.Foreground = brush; };
                    btn.PointerReleased += (s, e) => { btn.Foreground = brush; };
                }
                if (child is Microsoft.UI.Xaml.FrameworkElement feChild)
                    FixStepperButtons(feChild, brush);
            }
        }
        #endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}