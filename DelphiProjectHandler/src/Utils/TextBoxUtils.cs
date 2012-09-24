using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DelphiProjectHandler.Utils
{
    public static class TextBoxUtils
    {
        private static string BuildNewText(string aOldText, bool aAddEmptyLineBefore, ICollection<string> aLines)
        {
            StringBuilder vNewText = new StringBuilder(aOldText);
            if (aAddEmptyLineBefore)
                vNewText.AppendLine();
            foreach (string iLine in aLines)
                vNewText.AppendLine(iLine);
            if (vNewText.Length >= Environment.NewLine.Length)
                vNewText.Remove(vNewText.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            return vNewText.ToString();
        }

        public static void AssignLines(this TextBox aTextBox, ICollection<string> aLines)
        {
            if (aTextBox.Lines.SequenceEqual<string>(aLines))
                return;

            aTextBox.Text = BuildNewText("", false, aLines);
        }

        public static void AddLines(this TextBox aTextBox, ICollection<string> aLines)
        {
            aTextBox.SuspendLayout();
            try
            {
                String vOldText = aTextBox.Text;
                var vAddEmptyLineBefore = false;
                if (!vOldText.Equals(String.Empty) && !vOldText.EndsWith(Environment.NewLine))
                    vAddEmptyLineBefore = true;
                aTextBox.Text = BuildNewText(vOldText, vAddEmptyLineBefore, aLines);
            }
            finally
            {
                aTextBox.ResumeLayout();
            }
        }
    }
}
