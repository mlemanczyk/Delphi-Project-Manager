using System;

namespace DelphiProjectHandler.Operations
{
    public abstract class DelphiOperation
    {
        protected virtual string ReplaceContent(string iContent, string iOldValue, string iNewValue)
        {
            return iContent.Replace(iOldValue, iNewValue);
        }
    }
}
