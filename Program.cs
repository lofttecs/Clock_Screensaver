using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;

namespace screensaver2
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //二重起動チェック
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, Application.ProductName);
            if (mutex.WaitOne(0, false) == false)
            {
                return;
            }
            GC.KeepAlive(mutex);
            mutex.Close();

            if (args.Length > 0)
            {
                // 2 文字のコマンド ライン引数を取得します。
                string arg = args[0].ToLower(CultureInfo.InvariantCulture).Trim().Substring(0, 2);
                switch (arg)
                {
                    case "/c":
                        // オプション ダイアログを表示します。
                        //MessageBox.Show("オプション設定はありません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form2());
                        break;
                    case "/p":
                        // プレビュー
                        ShowPreview(args);
                        break;
                    case "/s":
                        // スクリーン セーバーのフォームを表示します。
                        ShowScreenSaver();
                        break;
                    default:
                        MessageBox.Show("コマンド ライン引数が無効です :" + arg, "コマンド ライン引数が無効です。", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            else
            {
                // 渡される引数がない場合、スクリーン セーバーを表示します。
                ShowScreenSaver();
            }

        }
        static void ShowScreenSaver()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            foreach (Screen screen in Screen.AllScreens)
            {
                Form1 screensaver = new Form1(screen.Bounds);
                screensaver.Show();
            }
            Application.Run();
        }
        static void ShowPreview(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //args[1] はプレビューウィンドウのハンドル
            Application.Run(new Form1(new IntPtr(long.Parse(args[1]))));
        }
    }
}
