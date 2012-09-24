/*
 * Created by SharpDevelop.
 * User: SG0894652
 * Date: 8/11/2009
 * Time: 6:02 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DelphiProjectHandler
{
	/// <summary>
	/// Container that stored uses list of a unit. Both, interface and implementation.
	/// </summary>
	public class UnitStructure: IDisposable
	{
		public UnitStructure()
		{
			fImplementation = new UnitList();
			fInterface = new UnitList();
		}
		
		public UnitList Implementation
		{
			get { return fImplementation; }
		}
		private UnitList fImplementation;
		
		public UnitList Interface
		{
			get { return fInterface; }
		}
		private UnitList fInterface;
		
		public void Dispose()
		{
			fImplementation = null;
			fInterface = null;
		}
	}
}
