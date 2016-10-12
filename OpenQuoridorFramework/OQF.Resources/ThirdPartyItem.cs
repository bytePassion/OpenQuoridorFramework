using System;
using System.ComponentModel;

#pragma warning disable 0067

namespace OQF.Resources
{
	public class ThirdPartyItem : INotifyPropertyChanged
	{
		public ThirdPartyItem(string itemtext, string itemUriText, Uri itemUri)
		{
			ItemText = itemtext;
			ItemUriText = itemUriText;
			ItemUri = itemUri;
		}

		public string ItemText    { get; }		
		public string ItemUriText { get; }
		public Uri    ItemUri     { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}