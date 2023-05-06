using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace UiDesktopApp1.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            ApplicationTitle = "SD WebUI 启动器 v4";

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "基本参数:",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.DashboardPage)
                },
                new NavigationItem()
                {
                    Content = "版本管理:",
                    PageTag = "version",
                    Icon = SymbolRegular.DataHistogram24,
                    PageType = typeof(Views.Pages.DataPage)
                },
                new NavigationItem()
                {
                    Content = "扩展管理:",
                    PageTag = "exts",
                    Icon = SymbolRegular.PlugConnected20,
                    PageType = typeof(Views.Pages.Page1)
                },
                new NavigationItem()
                {
                    Content = "模型管理:",
                    PageTag = "models",
                    Icon = SymbolRegular.Mail12,
                    PageType = typeof(Views.Pages.Models)
                },
                 new NavigationItem()
                {
                    Content = "相关推荐:",
                    PageTag = "links",
                    Icon = SymbolRegular.Link12,
                    PageType = typeof(Views.Pages.Links)
                }
            };

            NavigationItem to = new NavigationItem()
            {
                Content = "亮灯",
                PageTag = "Toggle theme",
                Icon = SymbolRegular.Lightbulb16,

            };
            to.Click += OnToggleThemeClicked;

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                to,
                new NavigationItem()
                {
                    Content = "设置",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            

            //var toggleThemeNavigationViewItem = new NavigationViewItem
            //{
            //    Content = "Toggle theme",
            //    Icon = new SymbolIcon { Symbol = SymbolRegular.PaintBrush24 }
            //};
            //toggleThemeNavigationViewItem.Click += OnToggleThemeClicked;

            //_footerMenuItems.Add(toggleThemeNavigationViewItem);

            _isInitialized = true;
        }

        private void OnToggleThemeClicked(object sender, RoutedEventArgs e)
        {
            NavigationItem item = (NavigationItem)sender;
            if (item.Content.ToString() == "亮灯")
            {
                item.Content = "关灯";
            } else
            {
                item.Content = "亮灯";
            }

            var currentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();

            Wpf.Ui.Appearance.Theme.Apply(currentTheme == Wpf.Ui.Appearance.ThemeType.Light ? Wpf.Ui.Appearance.ThemeType.Dark : Wpf.Ui.Appearance.ThemeType.Light);
        }
    }
}
