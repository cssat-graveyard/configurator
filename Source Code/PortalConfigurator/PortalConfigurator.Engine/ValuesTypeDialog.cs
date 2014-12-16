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

		public ValuesTypeDialog()
		{
			InitializeComponent();
			this.Subject = new FilterParameter();
			this.Original = new FilterParameter();
			this.ChangedValueColor = Color.LemonChiffon;
		}

		public ValuesTypeDialog(ref FilterParameter subject, ref FilterParameter original, Color changedValueColor)
		{
			InitializeComponent();
			this.Subject = subject;
			this.Original = original;
			this.ChangedValueColor = changedValueColor;
		}

		private void ValuesTypeInterface_Load(object sender, EventArgs e)
		{
			List<ValuesType> possibleValueTypes = Subject.GetPossibleValuesTypes();
			List<ValuesType> valueTypes = Enum.GetValues(typeof(ValuesType)).Cast<ValuesType>().ToList();
			valueTypes.RemoveAt(0);
			typeListView.Columns[0].Width = typeListView.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;

			for (int i = 0; i < valueTypes.Count; i++)
			{
				typeListView.Items.Add(Enums.GetString(valueTypes.ElementAt(i)));
				typeListView.Items[i].ForeColor = possibleValueTypes.Contains(valueTypes.ElementAt(i)) ? default(Color) : Color.Gray;

				if (Subject.ValuesType == valueTypes.ElementAt(i))
					typeListView.SelectedIndices.Add(i);
			}
		}

		private void typeListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (typeListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = typeListView.SelectedIndices[0];

				if (typeListView.Items[selectedIndex].ForeColor != Color.Gray)
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
					typeListView.SelectedIndices.Add(((int)Subject.ValuesType) - 1);
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
