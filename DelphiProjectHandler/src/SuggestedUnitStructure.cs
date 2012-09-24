/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/12/2009
 * Time: 10:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DelphiProjectHandler
{
	/// <summary>
	/// Description of IcarusSuggestedUnitUses.
	/// </summary>
	public class SuggestedUnitStructure: IDisposable
	{
		public SuggestedUnitStructure()
		{
			fUses = new UnitStructure();
			fToDelete = new UnitList();
			fMoveToInterface = new UnitList();
		}
		
		public void Dispose()
		{
			fUses = null;
			fToDelete = null;
			fMoveToInterface = null;
		}
		
		public UnitStructure Uses
		{
			get { return fUses; }
		}
		protected UnitStructure fUses;
		
		public UnitList ToDelete
		{
			get { return fToDelete; }
		}
		protected UnitList fToDelete;
		
		public UnitList MoveToInterface
		{
			get { return fMoveToInterface; }
		}
		protected UnitList fMoveToInterface;

        public SuggestedUnitStructure Clone()
        {
            SuggestedUnitStructure vResult = new SuggestedUnitStructure();
            vResult.MoveToInterface.AddRange(MoveToInterface);
            vResult.ToDelete.AddRange(ToDelete);
            vResult.Uses.Interface.AddRange(Uses.Interface);
            vResult.Uses.Implementation.AddRange(Uses.Implementation);
            return vResult;
        }
    }
}
