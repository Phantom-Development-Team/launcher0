using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UiDesktopApp1.Views.Pages;
using UiDesktopApp1.Models;
using System.Collections.ObjectModel;

namespace UiDesktopApp1.Services
{
    public static class Store
    {
        public static List<ExtRemote> extRemote;
        //public static List<ExtItem> extLocal;
        public static ObservableCollection<ExtItem> extLocal;

    }
}
