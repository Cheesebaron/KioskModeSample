using Android.App;
using Android.Content;

namespace KioskMode
{
    /// <summary>
    /// BroadcastReceiver which starts the KiosMode app when the device has booted.
    /// </summary>
    [BroadcastReceiver]
    [IntentFilter(new []{ Intent.ActionBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent i)
        {
            var intent = new Intent(context, typeof (KioskModeActivity));
            intent.AddFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }
    }
}