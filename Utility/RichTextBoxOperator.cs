using System.Windows.Controls;
using System.Windows.Documents;

namespace Epide.Utility
{
    public static class RichTextBoxOperator
    {
        internal static void RTBoxSet(RichTextBox rtBox, string content)
        {
            rtBox.Document.Blocks.Clear();
            var tmpParagraph = new Paragraph();
            tmpParagraph.Inlines.Add(content);
            rtBox.Document.Blocks.Add(tmpParagraph);
        }

        internal static string RTBoxGet(RichTextBox rtBox)
        {
            return new TextRange(rtBox.Document.ContentStart, rtBox.Document.ContentEnd).Text;
        }
    }
}