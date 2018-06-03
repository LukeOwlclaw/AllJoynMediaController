using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAlljoynExplorer.Support
{
    public static class Dispatcher
    {
        internal async static Task Dispatch(Action p)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
                () =>
                {
                    p.Invoke();
                }
            );
        }
    }
}
