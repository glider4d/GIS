using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Kts.Gis.Models;
using System.Reflection;

namespace Kts.Gis.Data
{
    public class WcfDocumentAccessService: BaseWcfDataAccessService, IDocumentAccessService
    {
        public WcfDocumentAccessService(ChannelFactory<IWcfDispatchServer> chanelFactory) : base(chanelFactory)
        {

        }

        public Guid AddDocument(DocumentModel doc, byte[] data, SchemaModel schema)
        {
            return (Guid)callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, doc, data, schema);
        }

        public void DeleteDocument(DocumentModel doc, SchemaModel schema)
        {
            callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, doc, schema);
        }

        public byte[] GetData(DocumentModel doc, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, doc, schema) as byte[];
        }

        public List<DocumentModel> GetDocuments(IObjectModel obj, SchemaModel schema)
        {
            return callDispathServerMethod(this.GetType(), MethodBase.GetCurrentMethod().Name, obj, schema) as List<DocumentModel>;
        }
    }
}
