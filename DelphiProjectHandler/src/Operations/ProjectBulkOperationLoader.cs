using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DelphiProjectHandler.Model;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace DelphiProjectHandler.Operations
{
    public class ProjectBulkOperationLoader
    {
        public static ProjectBulkOperations FromStream(Stream aStream)
        {
            XmlSerializer vSerializer = new XmlSerializer(typeof(ProjectBulkOperations));
            //if (!vSerializer.CanDeserialize(XmlReader.Create(aStream)))
            //    throw new InvalidDataException("Unsupported stream format");

            return vSerializer.Deserialize(aStream) as ProjectBulkOperations;
        }
    }
}
