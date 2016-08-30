using System.ComponentModel;
using QCF.Tools.FrameworkExtensions;

namespace QCF.Tools.WpfTools.ViewModelBase
{
	public abstract class ViewModel : DisposingObject, 
                                      IViewModel                                      
    {
        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
}