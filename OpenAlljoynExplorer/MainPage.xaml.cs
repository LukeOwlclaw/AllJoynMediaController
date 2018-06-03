using OpenAlljoynExplorer.Controllers;
using OpenAlljoynExplorer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VariableItemListView.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OpenAlljoynExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageModel VM { get; set; }
        public MainPageController Controller { get; set; }


        //public List<VariableType> Files { get; set; }
        public MainPage()
        {
            VM = new MainPageModel();
            //Files = new List<VariableType>();
            //Files.Add(new VariableType(1));
            //Files.Add(new VariableType(2));
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Controller = new MainPageController(VM);
            Controller.Start();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            //access datacontext.Items and add  some!
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VM.VariableListViewModel.Items.Add(new VariableType(2));
            VM.VariableListViewModel.Items.Add(new VariableType(1));
            //VM.Items.Add(new VariableType(2));
        }
    }
}
