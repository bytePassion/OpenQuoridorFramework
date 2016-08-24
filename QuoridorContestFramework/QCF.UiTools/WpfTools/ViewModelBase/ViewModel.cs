using System.ComponentModel;
using QCF.UiTools.FrameworkExtensions;

namespace QCF.UiTools.WpfTools.ViewModelBase
{
	public abstract class ViewModel : DisposingObject, 
                                      IViewModel                                      
    {
        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
}