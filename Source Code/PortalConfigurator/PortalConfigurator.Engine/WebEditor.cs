using mshtml;
using System;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public class WebEditor : WebBrowser
	{
		private IHTMLEventObj _eo;
		private IHTMLDocument2 _doc;
		private string _elementTag;

		public WebEditor()
		{
			this.ScriptErrorsSuppressed = true;
			this.DocumentText = String.Empty;
			this.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(_onDocComplete);
			_doc = Document.DomDocument as IHTMLDocument2;
		}

		private void _onDocComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			_doc.designMode = "on";
			this.Document.AttachEventHandler("onkeydown", _onKeyDown);
		}

		private void _onKeyDown(object sender, EventArgs e)
		{
			_eo = _doc.parentWindow.@event;
			IHTMLTxtRange range = _doc.selection.createRange() as IHTMLTxtRange;

			if (!_eo.shiftKey && _eo.keyCode == (int)Keys.Enter)
			{
				IHTMLElement br = _doc.createElement("BR");
				range.pasteHTML(br.outerHTML);
			}
			else if (_eo.shiftKey && _eo.keyCode == (int)Keys.Enter)
			{
				range.execCommand("InsertParagraph", false, null);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Enter)
			{
				IHTMLTxtRange range = _doc.selection.createRange() as IHTMLTxtRange;
				range.select();
				return true;
			}
			else if (keyData == (Keys.Shift | Keys.Enter))
			{
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
