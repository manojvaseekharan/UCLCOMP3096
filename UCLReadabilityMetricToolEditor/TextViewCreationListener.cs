using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualStudio.Text;

namespace UCLReadabilityMetricToolEditor
{
    [ContentType("code")]
    [Export(typeof(IWpfTextViewCreationListener))]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class TextViewCreationListener : IWpfTextViewCreationListener
    {

        /// <summary>
        /// Contains objects that manage tracking activities.
        /// </summary>
        private List<ClassManager> classManagers = new List<ClassManager>();
                
        public void TextViewCreated(IWpfTextView textView)
        {
            String className = GetClassName(textView);
            ClassManager cm = new ClassManager(className,textView);
            classManagers.Add(cm);
            textView.Closed += textView_Closed;
        }

        /// <summary>
        /// Upon closing a document, it's classManager object is removed from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textView_Closed(object sender, EventArgs e)
        {
            for(int i = 0; i < classManagers.Count; i ++)
            {
                if(classManagers[i].GetTextView().Equals(sender as IWpfTextView))
                {
                    classManagers[i].Close();
                    classManagers.Remove(classManagers[i]);
                }
            }
        }

        private String GetClassName(IWpfTextView textView)
        {
            ITextBuffer tb = textView.TextBuffer;
            ITextDocument td;
            var rc = tb.Properties.TryGetProperty<ITextDocument>(typeof(ITextDocument), out td);
            String filePath = td.FilePath;
            return filePath.Split('\\').Last();
        }

  
    }
}
