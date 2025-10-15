using Android.App;
using Android.Content.Res;
using Android.Runtime;
using PLinkageApp;

namespace PLinkageApp.Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(nint handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
            if (view is Entry)
            {
                // Remove underline - use global:: for absolute clarity
                handler.PlatformView.BackgroundTintList =
                    ColorStateList.ValueOf(global::Android.Graphics.Color.Transparent);

                //// Change placeholder text color
                //handler.PlatformView.SetHintTextColor(
                //    ColorStateList.ValueOf(global::Android.Graphics.Color.Black));
            }
        });

        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
        {
            if (handler.PlatformView is global::Android.Widget.EditText editText)
            {
                // Remove underline
                editText.BackgroundTintList = ColorStateList.ValueOf(global::Android.Graphics.Color.Transparent);
            }
        });
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}