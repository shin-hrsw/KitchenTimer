using Android.App;
using Android.Widget;
using Android.OS;

namespace KitchenTimer
{
    [Activity(Label = "KitchenTimer", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private int _remainingMilliSec = 0; // 残り時間(ミリ秒)

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var btn10min = FindViewById<Button>(Resource.Id.Add10MinButton);
            var btn1min = FindViewById<Button>(Resource.Id.Add1MinButton);
            var btn10sec = FindViewById<Button>(Resource.Id.Add10SecButton);
            var btn1sec = FindViewById<Button>(Resource.Id.Add1SecButton);
            var btnclear = FindViewById<Button>(Resource.Id.ClearButton);

            btn10min.Click += Btn10min_Click;
            btn1min.Click += Btn1min_Click;
            btn10sec.Click += Btn10sec_Click;
            btn1sec.Click += Btn1sec_Click;
            btnclear.Click += (sender, e) =>
            {
                this._remainingMilliSec = 0;
                ShowRemainingTime();
            };
        }

        private void Btn1sec_Click(object sender, System.EventArgs e)
        {
            this._remainingMilliSec += 1000;
            ShowRemainingTime();
        }

        private void Btn10sec_Click(object sender, System.EventArgs e)
        {
            this._remainingMilliSec += 10 * 1000;
            ShowRemainingTime();
        }

        private void Btn1min_Click(object sender, System.EventArgs e)
        {
            this._remainingMilliSec += 60 * 1000;
            ShowRemainingTime();
        }

        private void Btn10min_Click(object sender, System.EventArgs e)
        {
            this._remainingMilliSec += 10 * 60 * 1000;
            ShowRemainingTime();
        }

        private void ShowRemainingTime()
        {
            var txt = FindViewById<TextView>(Resource.Id.RemainingTimeTextView);
            var sec = this._remainingMilliSec / 1000;
            txt.Text = $"{sec / 60:f0}:{sec % 60:d2}";
        }
    }
}

