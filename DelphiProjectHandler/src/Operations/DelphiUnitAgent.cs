/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/12/2009
 * Time: 10:58 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using DelphiProjectHandler.Operations.Units;

namespace DelphiProjectHandler.Operations
{
	/// <summary>
	/// Description of DelphiUnitAgent.
	/// </summary>
	public class DelphiUnitAgent
    {
        #region (Protected Properties)

        protected FileModifierAgent FileAgent
    	{
    		get { return fFileAgent; }
    	}
    	private FileModifierAgent fFileAgent;
    	
        #endregion

        public DelphiUnitAgent(string aUnitFilename)
		{
        	fFileAgent = new FileModifierAgent(aUnitFilename);
		}

        public void CleanupUnits(SuggestedUnitStructure aSuggestedStructure, IList<string> aIgnoredUnits)
		{
			DelphiUnitCleanerOperation vOperation = new DelphiUnitCleanerOperation();
			FileAgent.Content = vOperation.Execute(FileAgent.Content, aSuggestedStructure, aIgnoredUnits);
		}
		
        public bool Modified
        {
        	get { return FileAgent.Modified; }
        }
        
        public void Save(string aUnitFilename)
        {
        	FileAgent.Save(aUnitFilename);
        }
    }
}
