using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.View;
using DelphiProjectHandler.LibraryPaths;

namespace DelphiProjectHandler.Model
{
    public class LibraryPathSettings: ILibraryPathSettings
    {
        #region (Private)

        private IList<string> fLibraryPaths = new List<string>();
        private IList<ILibraryPathSettingsView> fObservers = new List<ILibraryPathSettingsView>();
        private IList<LibraryPathType> fSupportedProviders = LibraryPathsProviderFactory.ListSupportedProviders();

        #endregion
        #region (Protected)

        protected IList<ILibraryPathSettingsView> Observers
        {
            get { return fObservers; }
        }

        #endregion
        #region (ILibraryPathSettings Implementation)

        public IList<LibraryPathType> SupportedProviders
        {
            get { return fSupportedProviders; }
        }

        public IList<string> LibraryPaths
        {
            get
            {
                return fLibraryPaths;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("LibraryPaths");
                fLibraryPaths = value;
            }
        }

        public void AddObserver(ILibraryPathSettingsView aView)
        {
            Observers.Add(aView);
        }

        public void RemoveObserver(ILibraryPathSettingsView aView)
        {
            Observers.Remove(aView);
        }

        public void NotifyObservers()
        {
            foreach (ILibraryPathSettingsView vObserver in Observers)
                vObserver.UpdateView(this);
        }

        #endregion
    }
}
