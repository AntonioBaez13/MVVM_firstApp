﻿using Stylet;
using System.Collections.ObjectModel;

namespace MVVM_firstApp.Pages
{
    public class ShellViewModel : Screen
    {
        public ObservableCollection<Combination> Combinations { get; set; }

        public ShellViewModel()
        {
            Combinations = new ObservableCollection<Combination>();
        }

    }
}
