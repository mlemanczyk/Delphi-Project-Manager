using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Operations;
using DelphiProjectHandler.Model;
using System.IO;
using DelphiProjectHandler.Dialogs.FileSelectors.Specialized;
using DelphiProjectHandler.Dialogs.FileSelectors;

namespace DelphiProjectHandler.Controller
{
    public class ProjectBulkOperationController: IProjectBulkOperationController
    {
        protected XmlFileSelector Selector { get; private set; }
        protected string FileName { get; private set; }

        private static FixProjectActionsController CreateController(string aProjectName, IList<string> aUnits)
        {
            FixProjectActionsController vController = new FixProjectActionsController();
            vController.Model = new FixProjectSettings();
            vController.Model.Projects.Add(aProjectName);
            vController.Model.UnitsToManipulate = aUnits;
            return vController;
        }

        public bool ProcessFile()
        {
            ProjectBulkOperations vProjects = ProjectBulkOperationLoader.FromStream(new FileStream(FileName, FileMode.Open));
            foreach (ProjectBulkOperation vProject in vProjects)
            {
                if (vProject.Remove.Count > 0)
                {
                    FixProjectActionsController vController = CreateController(vProject.ProjectName, vProject.Remove);
                    vController.RemoveUnits();
                }

                if (vProject.Add.Count > 0)
                {
                    FixProjectActionsController vController = CreateController(vProject.ProjectName, vProject.Add);
                    vController.AddUnits();
                }
            }
            return true;
        }

        public ProjectBulkOperationController()
        {
            Selector = new XmlFileSelector(new FilesSelector());
            FileName = "";
        }

        public bool SelectFile()
        {
            ICollection<string> vFiles = Selector.SelectFiles();
            if (vFiles.Count == 0)
                return false;

            FileName = vFiles.ElementAt(0);
            return true;
        }
    }
}
