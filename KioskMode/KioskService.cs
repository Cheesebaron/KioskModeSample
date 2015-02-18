using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;

namespace KioskMode
{
    /// <summary>
    /// This class helps detecting when Home button is pressed and new apps are launched
    /// </summary>
    [Service]
    public class KioskService : Service
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Context _context;

        public override void OnDestroy()
        {
            if(_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();

            base.OnDestroy();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags,
            int startId)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _context = this;

            try
            {
                RunAsync(_cancellationTokenSource.Token);
            }
            catch { }

            return base.OnStartCommand(intent, flags, startId);
        }

        private async Task RunAsync(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                HandleKioskMode();
                await Task.Delay(1800, token);
            }
        }

        private void HandleKioskMode()
        {
            if (IsKioskModeActive(this) && IsInBackground())
                RestoreApp();
        }

        private bool IsInBackground()
        {
            var am = _context.GetSystemService(ActivityService).JavaCast<ActivityManager>();
            var taskInfo = am.GetRunningTasks(1);
            var componentInfo = taskInfo[0].TopActivity;
            return _context.ApplicationContext.PackageName != componentInfo.PackageName;
        }

        private void RestoreApp()
        {
            var intent = new Intent(_context, typeof (KioskModeActivity));
            intent.AddFlags(ActivityFlags.NewTask);
            _context.StartActivity(intent);
        }

        private const string PrefKioskMode = "pref_kiosk_mode";
        private static bool IsKioskModeActive(Context context)
        {
            var sp = PreferenceManager.GetDefaultSharedPreferences(context);
            return sp.GetBoolean(PrefKioskMode, false);
        }

        public override IBinder OnBind(Intent intent) { return null; }
    }
}