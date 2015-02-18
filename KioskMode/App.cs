using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace KioskMode
{
    [Application(Label = "Kiosk Mode", Icon = "@drawable/icon")]
    public class App : Application
    {
        private PowerManager.WakeLock _wakeLock;

        public App(IntPtr nativeRef, JniHandleOwnership transfer) 
            : base(nativeRef, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();

            // Wake device
            RegisterKioskModeScreenOffReceiver();

            // Handle Home button and backgrounding
            StartKioskService();
        }

        private void StartKioskService() { StartService(new Intent(this, typeof (KioskService))); }

        private void RegisterKioskModeScreenOffReceiver()
        {
            var filter = new IntentFilter(Intent.ActionScreenOff);
            var onScreenOffReceiver = new OnScreenOffReceiver();
            RegisterReceiver(onScreenOffReceiver, filter);
        }

        public PowerManager.WakeLock GetWakeLock()
        {
            if (_wakeLock == null)
            {
                var pm = GetSystemService(PowerService).JavaCast<PowerManager>();
                _wakeLock = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup,
                    "wakeup");
            }

            return _wakeLock;
        }
    }
}