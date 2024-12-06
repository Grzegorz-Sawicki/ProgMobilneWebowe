using L5Shared.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L5MAUI.MessageBox
{
    internal class MauiMessageDialogService : IMessageDialogService
    {
        public void ShowMessage(string message)
        {
            Shell.Current.DisplayAlert("Message", message, "OK");
        }
    }
}
