using System.ComponentModel;
using Lib.FrameworkExtension;

namespace Lib.Wpf.ViewModelBase
{
	public abstract class ViewModel : DisposingObject, 
                                      IViewModel                                      
    {
        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
}