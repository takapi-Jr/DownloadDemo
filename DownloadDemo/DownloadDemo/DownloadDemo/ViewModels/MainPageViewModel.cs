using DownloadDemo.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using Xamarin.Forms;

namespace DownloadDemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ReactiveProperty<ImageSource> DownloadImageSource { get; } = new ReactiveProperty<ImageSource>();
        public ReactiveProperty<string> DownloadUrl { get; } = new ReactiveProperty<string>("http://www.dada-data.net/uploads/image/hausmann_abcd.jpg");
        public AsyncReactiveCommand DownloadCommand { get; } = new AsyncReactiveCommand();
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            DownloadCommand.Subscribe(async () =>
            {
                DownloadImageSource.Value = null;

                // パーミッションチェック
                var grantedFlag = await Common.CheckPermissions(Common.DownloadPermissions);
                if (!grantedFlag)
                {
                    return;
                }
                
                // ファイルダウンロード
                var filePath = await Common.DownloadFileAsync(DownloadUrl.Value);
                DownloadImageSource.Value = ImageSource.FromFile(filePath);
            }).AddTo(this.Disposable);
        }
    }
}
