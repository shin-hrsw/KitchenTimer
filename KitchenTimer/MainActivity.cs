using System.Timers;
using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace KitchenTimer
{
    [Activity(Label = "KitchenTimer", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Timer _timer;
        private DateTime _lastTime;
        private enum UnitKindEnum { Minute, Second }
        private int _remainingMilliSec = 0; // 残り時間(ミリ秒)
        private bool _isCountDown = false;  // タイマー作動中か

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            var btn10min = FindViewById<Button>(Resource.Id.Add10MinButton);
            var btn1min = FindViewById<Button>(Resource.Id.Add1MinButton);
            var btn10sec = FindViewById<Button>(Resource.Id.Add10SecButton);
            var btn1sec = FindViewById<Button>(Resource.Id.Add1SecButton);
            var btnclear = FindViewById<Button>(Resource.Id.ClearButton);
            var btnstart = FindViewById<Button>(Resource.Id.StartButton);

            // 残り時間追加処理を登録
            btn10min.Click += (sender, e) =>
            {
                AddRemainingTime(UnitKindEnum.Minute, 10);
                ShowRemainingTime();
            };
            btn1min.Click += (sender, e) =>
            {
                AddRemainingTime(UnitKindEnum.Minute, 1);
                ShowRemainingTime();
            };
            btn10sec.Click += (sender, e) =>
            {
                AddRemainingTime(UnitKindEnum.Second, 10);
                ShowRemainingTime();
            };
            btn1sec.Click += (sender, e) =>
            {
                AddRemainingTime(UnitKindEnum.Second, 1);
                ShowRemainingTime();
            };
            // 残り時間クリア処理
            btnclear.Click += (sender, e) =>
            {
                this._remainingMilliSec = 0;
                ShowRemainingTime();
            };

            btnstart.Click += (sender, e) =>
            {
                this._isCountDown = !this._isCountDown;
                if (this._isCountDown)
                {
                    // 開始時刻を設定してタイマースタート
                    this._lastTime = DateTime.Now;
                    this._timer.Start();
                    btnstart.Text = "ストップ";
                }
                else
                {
                    // タイマーを停止してボタンのテキストを戻す
                    this._timer.Stop();
                    btnstart.Text = "スタート";
                }
            };

            // タイマー設定
            // 100ms間隔で残り時間をチェック
            this._timer = new Timer();
            this._timer.Interval = 100;
            this._timer.Elapsed += OnElapsed;
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                this._remainingMilliSec -= (int)(DateTime.Now - this._lastTime).TotalMilliseconds;
                if (this._remainingMilliSec <= 0)
                {
                    this._timer.Stop();
                    // アラームの代わりにメッセージ表示
                    var alert = new AlertDialog.Builder(this);
                    alert.SetTitle("タイマー");
                    alert.SetMessage("終わったよ");
                    alert.Show();

                    FindViewById<Button>(Resource.Id.StartButton).Text = "スタート";
                    this._remainingMilliSec = 0;
                }
                this._lastTime = DateTime.Now;
                ShowRemainingTime();
            });
        }

        private void AddRemainingTime(UnitKindEnum unit, int value)
        {
            switch (unit)
            {
                case UnitKindEnum.Minute: { value *= 60; break; }
                case UnitKindEnum.Second: { break; }
            }
            value = value * 1000;
            this._remainingMilliSec += value;
        }

        private void ShowRemainingTime()
        {
            var txt = FindViewById<TextView>(Resource.Id.RemainingTimeTextView);
            var sec = this._remainingMilliSec / 1000;
            txt.Text = $"{sec / 60:f0}:{sec % 60:d2}";
        }
    }
}

