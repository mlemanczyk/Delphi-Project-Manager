using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DelphiProjectHandler.Model
{
    [Serializable]
    [XmlType(TypeName="project")]
    public class ProjectBulkOperation
    {
        [XmlAttribute("name")]
        public string ProjectName { get; set; }
        [XmlArray("add")]
        [XmlArrayItem("file")]
        public List<string> Add { get; set; }
        [XmlArray("remove")]
        [XmlArrayItem("file")]
        public List<string> Remove { get; set; }

        public ProjectBulkOperation()
        {
            Add = new List<string>();
            Remove = new List<string>();
        }
    }
    
    [Serializable]
    [XmlType(TypeName="projects")]
    public class ProjectBulkOperations: List<ProjectBulkOperation>
    {
    }
}
