using System;

using System.Collections.Generic;
using DelphiProjectHandler.Operations.Projects;

namespace DelphiProjectHandler.Operations
{
    /// <summary>
    /// Modifies Delphi projects, e.g. by adding units, changing unit paths etc.
    /// </summary>
    public class DelphiProjectAgent
    {
    	protected FileModifierAgent FileAgent
    	{
    		get { return fFileAgent; }
    	}
    	private FileModifierAgent fFileAgent;
    	
        public DelphiProjectAgent(string iProjectName)
        {
        	fFileAgent = new FileModifierAgent(iProjectName);
        }

        public bool Modified
        {
        	get { return FileAgent.Modified; }
        }
        
        public void Save(string iProjectName)
        {
        	FileAgent.Save(iProjectName);
        }
    }
}
