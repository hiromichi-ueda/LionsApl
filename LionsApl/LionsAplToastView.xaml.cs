using AiForms.Dialogs.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LionsApl
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LionsAplToastView : ToastView
    {
        public LionsAplToastView()
        {
            InitializeComponent();
        }

        // 開始アニメーションの定義
        public override void RunPresentationAnimation() { }

        // 終了アニメーションの定義
        public override void RunDismissalAnimation() { }

        // クリーンアップ処理の定義
        public override void Destroy() { }
    }
}