using System.ComponentModel;
using OQF.Tools.FrameworkExtensions;

namespace OQF.Tools.WpfTools.ViewModelBase
{
	public abstract class ViewModel : DisposingObject, 
                                      IViewModel                                      
    {
        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
}