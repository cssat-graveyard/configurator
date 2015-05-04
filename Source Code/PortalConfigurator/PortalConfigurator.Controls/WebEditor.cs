using mshtml;
using System;
using System.Windows.Forms;

namespace CustomControls
{
	public delegate void WebEditorHtmlChanged(object source, EventArgs e);

	public class WebEditor : WebBrowser
	{
		public event WebEditorHtmlChanged OnHtmlChanged;

		public WebEditor()
			: base()
		{
			this.ScriptErrorsSuppressed = true;
			this.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(_onDocComplete);
		}

		private void _onDocComplete(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			this.Document.Body.SetAttribute("contentEditable", "true");
			this.Document.Body.Style = "font-family:Arial;font-size:10pt";
			this.Document.AttachEventHandler("onkeydown", _onKeyDown);
		}

		private void _onKeyDown(object sender, EventArgs e)
		{
			IHTMLDocument2 _doc = this.Document.DomDocument as IHTMLDocument2;
			IHTMLEventObj _eo = _doc.parentWindow.@event;
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
				IHTMLDocument2 _doc = this.Document.DomDocument as IHTMLDocument2;
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
