﻿using System;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace VariableItemListView
{
    public sealed partial class VariableItemListView : UserControl
    {
        
        public VariableListViewModel VM
        {
            get { return (VariableListViewModel)GetValue(VMProperty); }
            set { SetValue(VMProperty, value); }
        }

        
        public static readonly DependencyProperty VMProperty =
            DependencyProperty.Register(nameof(VM), typeof(VariableListViewModel), typeof(VariableItemListView), null);

        //public VariableListViewModel VM { get; set; }

        public VariableItemListView()
        {
            VM = new VariableListViewModel();
            this.InitializeComponent();
        }
    }
}
