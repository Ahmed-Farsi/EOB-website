using Eob_Web.Core.Models;
using Eob_Web.Core.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eob_Web.Core.Mappers
{
    public class ZeroTier_Mapper
    {
        public void Model_To_View(ZT_Node node, ZT_Node_View node_View)
        {
            node_View.Node_Name = node.name;
            node_View.Node_Description = node.description;
            node_View.Node_Ip = node.config.ipAssignments.FirstOrDefault();
            node_View.Authorized = node.config.authorized;
            node_View.Network_Id = node.networkId;
            node_View.Node_Id = node.nodeId;
        }
    }
}
