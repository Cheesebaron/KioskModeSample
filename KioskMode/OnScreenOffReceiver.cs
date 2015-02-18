using Android.Content;
using Android.Preferences;

namespace KioskMode
{
    public class OnScreenOffReceiver : BroadcastReceiver
    {
        private const string PrefKioskMode = "pref_kiosk_mode";

        public override void OnReceive(Context context, Intent intent)
        {
            if (Intent.ActionScreenOff != intent.Action) return;

            var ctx = (App) context.ApplicationContext;
            if (IsKioskModeActive(ctx))
                WakeUpDevice(ctx);
        }

        private static void WakeUpDevice(App context)
        {
            var wakeLock = context.GetWakeLock();
            if (wakeLock.IsHeld)
                wakeLock.Release();

            wakeLock.Acquire();

            wakeLock.Release();
        }

        private static bool IsKioskModeActive(Context context)
        {
            var sp = PreferenceManager.GetDefaultSharedPreferences(context);
            return sp.GetBoolean(PrefKioskMode, false);
        }
    }
}