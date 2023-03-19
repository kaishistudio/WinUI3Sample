using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Documents;

namespace WinUI3Sample.Models;
public class KSAppData : INotifyPropertyChanged
{

    private string _myname = "";
    public string MyName
    {
        get => _myname;
        set
        {
            _myname = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MyName)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
