using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DelphiProjectHandler.Operations
{
    public class DelphiFileAgentError : Exception
    {
        public DelphiFileAgentError(string aMessage): base(aMessage)
        {
        }
    }

    public class DelphiFileAgent: IDelphiFileAgent
    {
        public void Execute(String aUnitFileName, IDelphiFileOperation aOperation)
        {
            Execute(aUnitFileName, new IDelphiFileOperation[] { aOperation });
        }

        public void Execute(String aUnitFileName, params IDelphiFileOperation[] aOperations)
        {
            FileModifierAgent vFileAgent = new FileModifierAgent(aUnitFileName);
            foreach (IDelphiFileOperation vOperation in aOperations)
            {
                try
                {
                    vOperation.Initialize(aUnitFileName, vFileAgent.Content);
                    if (!vOperation.CanProcess())
                        return;

                    vFileAgent.Content = vOperation.Execute();
                }
                catch (Exception e)
                {
                    throw new DelphiFileAgentError(String.Format("Error processing file: {0}. {1}", aUnitFileName, e.Message));
                    
                }
            }
            if (vFileAgent.Modified)
                vFileAgent.Save(aUnitFileName);
        }
    }
}
