using System;
using System.Globalization;
using System.ServiceProcess;
using System.Timers;


namespace WebExStatsSvc
{
    public partial class DataSvc : ServiceBase
    {
        private Timer _svcTimer;

        public DataSvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (_svcTimer == null)
            {
                ResetTimer();
            }

            if (_svcTimer != null) _svcTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            _svcTimer.Enabled = false;
            _svcTimer.Dispose();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _svcTimer.Enabled = false;

            if (!CanRun())
            {
                _svcTimer.Enabled = true;
                return;
            }
            if (!await DoProcess.PullData())
            {
                Stop();
            }
            else
            {
                _svcTimer.Enabled = true;
            }



        }

        private void ResetTimer()
        {
            _svcTimer = new Timer
            {

                Interval = Properties.Settings.Default.TimerInterval,
                AutoReset = true

            };

            _svcTimer.Elapsed += TimerElapsed;

        }

        private static bool CanRun()
        {
            var nowDate = DateTime.Now;
            var startTime = Convert.ToDateTime(nowDate.ToShortDateString() + " " + Properties.Settings.Default.RunTime, CultureInfo.CurrentCulture);

            return nowDate >= startTime && nowDate <= startTime.AddMinutes(1);

            //return true;

        }

    }
}

