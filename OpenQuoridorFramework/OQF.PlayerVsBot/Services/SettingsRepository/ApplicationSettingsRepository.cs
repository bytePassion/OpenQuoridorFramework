using System.IO;
using System.Linq;
using System.Text;
using OQF.Visualization.Resources;

namespace OQF.PlayerVsBot.Services.SettingsRepository
{
	internal class ApplicationSettingsRepository : IApplicationSettingsRepository
	{
		private const string FilePath = "ApplicationSettings.qor";

		private bool isDebugSectionExpanded;
		private bool isProgressSecionExpanded;
		private string selectedLanguageCode;
		private string lastUsedBotPath;

		public ApplicationSettingsRepository()
		{
			LoadValues();
		}

		private void LoadValues()
		{
			if (!File.Exists(FilePath))
			{
				LoadDefaultValues();
				return;
			}
				

			var valueString = File.ReadAllText(FilePath);

			var valueParts = valueString.Split('|');

			if (valueParts.Length != 4)
			{
				LoadDefaultValues();
				return;
			}

			LastUsedBotPath          =            valueParts[0];
			SelectedLanguageCode     =            valueParts[1];
			IsProgressSecionExpanded = bool.Parse(valueParts[2]);
			IsDebugSectionExpanded   = bool.Parse(valueParts[3]);
		}

		private void LoadDefaultValues()
		{
			LastUsedBotPath = null;
			SelectedLanguageCode = Languages.AvailableCountryCodes().FirstOrDefault();
			IsDebugSectionExpanded = false;
			IsProgressSecionExpanded = true;
		}

		private void SaveValues()
		{
			var sb = new StringBuilder();

			sb.Append(LastUsedBotPath);
			sb.Append('|');
			sb.Append(SelectedLanguageCode);
			sb.Append('|');
			sb.Append(IsProgressSecionExpanded);
			sb.Append('|');
			sb.Append(IsDebugSectionExpanded);

			File.WriteAllText(FilePath, sb.ToString());
		}
		
		public string LastUsedBotPath
		{
			get { return lastUsedBotPath; }
			set
			{
				lastUsedBotPath = value;
				SaveValues();
			}
		}		

		public string SelectedLanguageCode
		{
			get { return selectedLanguageCode; }
			set
			{
				selectedLanguageCode = value;
				SaveValues();
			}
		}

		public bool IsProgressSecionExpanded
		{
			get { return isProgressSecionExpanded; }
			set
			{
				isProgressSecionExpanded = value;
				SaveValues();
			}
		}

		public bool IsDebugSectionExpanded
		{
			get { return isDebugSectionExpanded; }
			set
			{
				isDebugSectionExpanded = value;
				SaveValues();
			}
		}
	}
}