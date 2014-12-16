using Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class ValuesTypeDialog : Form
	{
		private FilterParameter Subject { get; set; }
		private FilterParameter Original { get; set; }
		private Color ChangedValueColor { get; set; }
		private List<ValuesType> PossibleValueTypes { get; set; }
		private List<ValuesType> ValuesTypes { get; set; }
		private string[] ValuesTypeNames { get; set; }

		public ValuesTypeDialog()
		{
			InitializeComponent();
			this.Subject = new FilterParameter();
			this.Original = new FilterParameter();
			this.ChangedValueColor = Color.LemonChiffon;
			this.PossibleValueTypes = new List<ValuesType>();
			this.ValuesTypes = new List<ValuesType>();
			this.ValuesTypeNames = new string[0];
		}

		public ValuesTypeDialog(ref FilterParameter subject, ref FilterParameter original, Color changedValueColor)
		{
			InitializeComponent();
			this.Subject = subject;
			this.Original = original;
			this.ChangedValueColor = changedValueColor;
			this.PossibleValueTypes = subject.GetPossibleValuesTypes();
			this.ValuesTypes = Enum.GetValues(typeof(ValuesType)).Cast<ValuesType>().Where(p => p != ValuesType.NoValues).ToList();
			this.ValuesTypeNames = Enums.GetFormattedValuesTypeEnumNames().Where(p => Enums.GetValuesTypeEnum(p) != ValuesType.NoValues).ToArray<string>();
		}

		private void ValuesTypeInterface_Load(object sender, EventArgs e)
		{
			typeListView.Columns[0].Width = typeListView.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;

			for (int i = 0; i < ValuesTypeNames.Length; i++)
			{
				typeListView.Items.Add(ValuesTypeNames.ElementAt(i));
				typeListView.Items[i].ForeColor = PossibleValueTypes.Contains(ValuesTypes.ElementAt(i)) ? default(Color) : Color.Gray;
			}

			if (PossibleValueTypes.Contains(Subject.ValuesType))
				typeListView.SelectedIndices.Add(ValuesTypes.FindIndex(p => p == Subject.ValuesType));
			else
				typeListView.SelectedIndices.Add(ValuesTypes.FindIndex(p => p == ValuesType.DictionaryStrings));
		}

		private void typeListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (typeListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = typeListView.SelectedIndices[0];

				if (PossibleValueTypes.Contains(ValuesTypes.ElementAt(selectedIndex)))
				{
					ValuesType valuesType = (ValuesType)(selectedIndex + 1);
					JObject exampleJson = new JObject();

					if (Subject.ValuesType != valuesType)
						Subject.ValuesType = valuesType;

					Subject.AddValuesJson(ref exampleJson);
					string selectedText = Enums.GetString(Subject.Display.SelectedType);

					if (Subject.Display.SelectedList.Count > 1)
						selectedText = String.Concat(selectedText, " List");

					selectedTypeTextBox.Text = selectedText;
					disabledTypeTextBox.Text = Enums.GetString(Subject.Display.DisabledType);

					if (Subject.Display.SelectedList.Count != 0 || Subject.Display.DisabledList.Count != 0)
					{
						JObject displayJson = new JObject();
						Subject.Display.AddSelectedJson(ref displayJson);
						Subject.Display.AddDisalbedJson(ref displayJson);
						exampleJson.Add("display", displayJson);
					}

					exampleJsonTextBox.Text = exampleJson.ToString();
					UpdateFieldBackgroundColors();
				}
				else
				{
					typeListView.SelectedIndices.Clear();
					typeListView.SelectedIndices.Add(ValuesTypes.FindIndex(p => p == Subject.ValuesType));
				}
			}
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void UpdateFieldBackgroundColors()
		{
			for (int i = 0; i < typeListView.Items.Count; i++)
			{
				if (((int)Subject.ValuesType) - 1 == i)
					typeListView.Items[i].BackColor = Subject.ValuesType == Original.ValuesType ? default(Color) : ChangedValueColor;
				else
					typeListView.Items[i].BackColor = default(Color);
			}

			bool selectedTypesEqual = Subject.Display.SelectedType == Original.Display.SelectedType;
			bool selectedListsEqual = Subject.Display.SelectedList.SequenceEqual(Original.Display.SelectedList);
			selectedTypeTextBox.BackColor = selectedTypesEqual && selectedListsEqual ? SystemColors.Window : ChangedValueColor;
			disabledTypeTextBox.BackColor = Subject.Display.DisabledType == Original.Display.DisabledType ? SystemColors.Window : ChangedValueColor;
		}
	}
}
