using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using DelphiProjectHandler.Controller;
using DelphiProjectHandler.LibraryPaths;
using DelphiProjectHandler.Model;
using DelphiProjectHandler.Operations;
using DelphiProjectHandler.Utils;

namespace DelphiProjectHandler.View
{
    public partial class FormDelphiProjectManager : Form, IFixProjectSettingsView, IFixUnitSettingsView, ILibraryPathSettingsView, IUnitCleanupSettingsView
    {
        private IFixProjectSettings FixProjectSettings = new FixProjectSettings();
        private IFixProjectSettingsController FixProjectSettingsController = new FixProjectSettingsController();
        private IFixProjectActionsController ProjectManipulatorController = new FixProjectActionsController();
        private IFixUnitSettings FixUnitSettings = new FixUnitSettings();
        private IFixUnitSettingsController FixUnitSettingsController = new FixUnitSettingsController();
        private IFixUnitActionsController UnitManipulatorController = new FixUnitActionsController();
        private ILibraryPathSettings LibraryPathSettings = new LibraryPathSettings();
        private ILibraryPathSettingsController LibraryPathSettingsController = new LibraryPathSettingsController();
        private IUnitCleanupSettings UnitCleanupSettings = new UnitCleanupSettings();
        private IUnitCleanupSettingsController UnitCleanupSettingsController = new UnitCleanupSettingsController();
        private IUnitCleanupActionsController UnitCleanupActionsController = new UnitCleanupActionsController();
        private IProjectBulkOperationController ProjectBulkOperationController = new ProjectBulkOperationController();

        #region (IFixProjectSettingsView Implementation)

        void IFixProjectSettingsView.UpdateView(IFixProjectSettings aModel)
        {
            textBoxFixProjectsFiles.AssignLines(aModel.Projects);
            textBoxFixProjectsInterfaceUnits.AssignLines(aModel.UnitsToManipulate);
            ToolStripMenuItemFixProjectsActionAddUnitPathsToDPRs.Checked = aModel.AddUnitPathsToDPRs;
        }

        #endregion
        #region IFixUnitSettingsView Members

        public void UpdateView(IFixUnitSettings aModel)
        {
            textBoxFixUnitsFiles.AssignLines(aModel.UnitsToModify);
            textBoxFixUnitsInterfaceUnits.AssignLines(aModel.InterfaceUnits);
            textBoxFixUnitsImplementationUnits.AssignLines(aModel.ImplementationUnits);
            textBoxFixUnitsUnitConstraints.AssignLines(aModel.UnitConstraints);
        }

        #endregion
        #region (ILibraryPathSettingsView Implementation)

        void ILibraryPathSettingsView.UpdateView(ILibraryPathSettings aModel)
        {
            FillStripDownWithSupportedProviders(aModel.SupportedProviders);
            textBoxSettingsLibraryPaths.AssignLines(aModel.LibraryPaths);
        }

        #endregion
        #region (IUnitCleanupSettingsView Implementation)

        void IUnitCleanupSettingsView.UpdateView(IUnitCleanupSettings aModel)
        {
            textBoxIcarusUnits.AssignLines(aModel.UnitsToClean);
            textBoxIcarusIgnoredUnits.AssignLines(aModel.IgnoredUnits);
            if (!textBoxIcarusReport.Text.Equals(aModel.IcarusReport))
                textBoxIcarusReport.Text = aModel.IcarusReport;
        }
        
        #endregion

        public FormDelphiProjectManager()
        {
            InitializeComponent();

            WireUp();
        }

        protected void WireUp()
        {
            FixProjectSettings.AddObserver(this);
            FixProjectSettingsController.Model = FixProjectSettings;
            ProjectManipulatorController.Model = FixProjectSettings;
            FixUnitSettings.AddObserver(this);
            FixUnitSettingsController.Model = FixUnitSettings;
            UnitManipulatorController.Model = FixUnitSettings;
            LibraryPathSettingsController.Model = LibraryPathSettings;
            UnitCleanupSettingsController.Model = UnitCleanupSettings;
            UnitCleanupActionsController.Model = UnitCleanupSettings;
            LibraryPathSettings.AddObserver(this);
            UnitCleanupSettings.AddObserver(this);
            (this as IFixProjectSettingsView).UpdateView(FixProjectSettings);
            (this as ILibraryPathSettingsView).UpdateView(LibraryPathSettings);
            (this as IUnitCleanupSettingsView).UpdateView(UnitCleanupSettings);
        }

        private void FillStripDownWithSupportedProviders(IList<LibraryPathType> aSupportedProviders)
        {
            ToolStripItem[] vItems = new ToolStripMenuItem[aSupportedProviders.Count];
            for (int vEnumIdx = 0; vEnumIdx < aSupportedProviders.Count; vEnumIdx++)
                vItems[vEnumIdx] = CreateToolStripItem(aSupportedProviders[vEnumIdx]);
            toolStripDropDownButtonLoadPreset.DropDownItems.Clear();
            toolStripDropDownButtonLoadPreset.DropDownItems.AddRange(vItems);
        }

        private static ToolStripItem CreateToolStripItem(LibraryPathType iEnumValue)
        {
            ToolStripMenuItem vItem = new ToolStripMenuItem(iEnumValue.ToString());
            vItem.Tag = iEnumValue;
            return vItem;
        }

        private void buttonFixProjectsSelectProjects_Click(object sender, EventArgs e)
        {
            FixProjectSettingsController.SelectProjects();
        }

        private void buttonFixProjectsSelectInterfaceUnits_Click(object sender, EventArgs e)
        {
            FixProjectSettingsController.SelectUnits();
        }

        private void buttonFixProjectsAddUnitsStart_Click(object sender, EventArgs e)
        {
            ProjectManipulatorController.AddUnits();
        }

        private void buttonFixProjectsImportInterfaceUnits_Click(object sender, EventArgs e)
        {
            FixProjectSettingsController.ImportUnits();
        }

        private void buttonFixProjectsImportProjects_Click(object sender, EventArgs e)
        {
            FixProjectSettingsController.ImportProjects();
        }

        private void buttonFixProjectsRemoveUnits_Click(object sender, EventArgs e)
        {
            ProjectManipulatorController.RemoveUnits();
        }

        private void buttonFixProjectsFixPaths_Click(object sender, EventArgs e)
        {
            ProjectManipulatorController.FixPaths();
        }

        private void buttonFixProjectsFixForms_Click(object sender, EventArgs e)
        {
            ProjectManipulatorController.FixForms();
        }

        private void toolStripDropDownButtonLoadPreset_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                LibraryPathSettingsController.LoadPreset((LibraryPathType)e.ClickedItem.Tag);
            }
            catch (NotSupportedException)
            {
                MessageBox.Show("Selected preset is not supported, yet.", "Load Library Path Preset", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        void ToolStripButtonNormalizePathsClick(object sender, EventArgs e)
        {
            LibraryPathSettingsController.NormalizePaths();
        }
        
        void ButtonRemoveUnusedUnitsClick(object sender, EventArgs e)
        {
            UnitCleanupActionsController.RemoveUnusedUnits();
        }
        
        void ToolStripButtonIcarusReportPasteFromClipboardClick(object sender, EventArgs e)
        {
            UnitCleanupSettingsController.PasteIcarusReport();
        }
        
        void ToolStripButtonIcarusLoadUnitsClick(object sender, EventArgs e)
        {
        	try
        	{
                UnitCleanupSettingsController.LoadFromLibraryPaths(LibraryPathSettings.LibraryPaths);
        	}
        	catch (Exception iError)
        	{
        		MessageBox.Show(iError.Message, "Load units from library paths", MessageBoxButtons.OK, MessageBoxIcon.Error);
        	}
        }

        private void ToolStripMenuItemFixProjectsActionAddUnitPathsToDPRs_Click(object sender, EventArgs e)
        {
            FixProjectSettingsController.SwitchAddUnitPathsToDPRs();
        }

        private void textBoxFixProjectsFiles_TextChanged(object sender, EventArgs e)
        {
            FixProjectSettingsController.SetProjects(textBoxFixProjectsFiles.Lines);
        }

        private void textBoxFixProjectsInterfaceUnits_TextChanged(object sender, EventArgs e)
        {
            FixProjectSettingsController.SetUnitsToManipulate(textBoxFixProjectsInterfaceUnits.Lines);
        }

        private void textBoxFixProjectsUnitConstraints_TextChanged(object sender, EventArgs e)
        {
            FixProjectSettingsController.SetUnitConstraints(textBoxFixProjectsUnitConstraints.Lines);
        }

        private void textBoxSettingsLibraryPaths_TextChanged(object sender, EventArgs e)
        {
            LibraryPathSettingsController.SetLibraryPaths(textBoxSettingsLibraryPaths.Lines);
        }

        private void textBoxIcarusUnits_TextChanged(object sender, EventArgs e)
        {
            UnitCleanupSettingsController.SetUnitsToClean(textBoxIcarusUnits.Lines);
        }

        private void textBoxIcarusIgnoredUnits_TextChanged(object sender, EventArgs e)
        {
            UnitCleanupSettingsController.SetIgnoredUnits(textBoxIcarusIgnoredUnits.Lines);
        }

        private void textBoxIcarusReport_TextChanged(object sender, EventArgs e)
        {
            UnitCleanupSettingsController.SetIcarusReport(textBoxIcarusReport.Text);
        }

        private void toolStripButtonFixUnitsSelectFiles_Click(object sender, EventArgs e)
        {
            FixUnitSettingsController.SelectUnitsToModify();
        }

        private void toolStripButtonFixUnitsImportFiles_Click(object sender, EventArgs e)
        {
            FixUnitSettingsController.ImportUnitsToModify();
        }

        private void textBoxFixUnitsFiles_TextChanged(object sender, EventArgs e)
        {
            FixUnitSettingsController.SetUnitsToModify(textBoxFixUnitsFiles.Lines);
        }

        private void toolStripButtonFixUnitsSelectInterfaceUnits_Click(object sender, EventArgs e)
        {
            FixUnitSettingsController.SelectInterfaceUnits();
        }

        private void toolStripButtonFixUnitsImportInterfaceUnits_Click(object sender, EventArgs e)
        {
            FixUnitSettingsController.ImportInterfaceUnits();
        }

        private void textBoxFixUnitsInterfaceUnits_TextChanged(object sender, EventArgs e)
        {
            FixUnitSettingsController.SetInterfaceUnits(textBoxFixUnitsInterfaceUnits.Lines);
        }

        private void toolStripButtonFixUnitsSelectImplementationUnits_Click(object sender, EventArgs e)
        {
            FixUnitSettingsController.SelectImplementationUnits();
        }

        private void toolStripButtonFixUnitsImportImplementationUnits_Click(object sender, EventArgs e)
        {
            FixUnitSettingsController.ImportImplementationUnits();
        }

        private void textBoxFixUnitsImplementationUnits_TextChanged(object sender, EventArgs e)
        {
            FixUnitSettingsController.SetImplementationUnits(textBoxFixUnitsImplementationUnits.Lines);
        }

        private void textBoxFixUnitsUnitConstraints_TextChanged(object sender, EventArgs e)
        {
            FixUnitSettingsController.SetUnitConstraints(textBoxFixUnitsUnitConstraints.Lines);
        }

        private void toolStripButtonFixUnitsActionAddUnits_Click(object sender, EventArgs e)
        {
            UnitManipulatorController.AddUnits();
        }

        private void toolStripButtonFixUnitsActionRemoveUnits_Click(object sender, EventArgs e)
        {
            UnitManipulatorController.RemoveUnits();
        }

        private void toolStripButtonFixProjectsActionBulkOperation_Click(object sender, EventArgs e)
        {
            if (!ProjectBulkOperationController.SelectFile())
                return;

            ProjectBulkOperationController.ProcessFile();
        }

    }
}