using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Views;
using Android.Widget;

namespace KioskMode
{
    [Activity(Label = "Kiosk", MainLauncher = true, Icon = "@drawable/icon")]
    public class KioskModeActivity : Activity
    {
        private const string PrefKioskMode = "pref_kiosk_mode";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Do this before SetContentView
            Window.AddFlags(WindowManagerFlags.DismissKeyguard);

            SetContentView(Resource.Layout.Main);

            var on = FindViewById<Button>(Resource.Id.turnOn);
            var off = FindViewById<Button>(Resource.Id.turnOff);

            var sp = PreferenceManager.GetDefaultSharedPreferences(this);

            on.Click += (_, __) => {
                var edit = sp.Edit();
                edit.PutBoolean(PrefKioskMode, true);
                edit.Commit();
            };

            off.Click += (_, __) => {
                var edit = sp.Edit();
                edit.PutBoolean(PrefKioskMode, false);
                edit.Commit();
            };
        }

        public override void OnBackPressed()
        {
            // Disable pressing back, yo!
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            if (!hasFocus)
            {
                var closeDialog = new Intent(Intent.ActionCloseSystemDialogs);
                SendBroadcast(closeDialog);
            }
        }

        //Optional: Disable buttons (i.e. volume buttons)
        private readonly IList<Keycode> _blockedKeys = new[] {Keycode.VolumeDown, Keycode.VolumeUp};
        public override bool DispatchKeyEvent(KeyEvent e)
        {
            if (_blockedKeys.Contains(e.KeyCode))
                return true;

            return base.DispatchKeyEvent(e);
        }
    }
}

