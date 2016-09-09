using System.ComponentModel;
using FrameworkExtensionLib;

namespace WpfLib.ViewModelBase
{
	public abstract class ViewModel : DisposingObject, 
                                      IViewModel                                      
    {
        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
}