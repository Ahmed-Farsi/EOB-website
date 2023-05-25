using Eob_Web.Core.Models;
using Eob_Web.Core.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eob_Web.Core.Mappers
{
    public class Eob_Mapper
    {
        public Eob View_To_Model(Eob_View eob_View)
        {
            var eob = new Eob
            {
                Id = eob_View.Id,
                Serial_Number = eob_View.Serial_Number,
                Network_Id = eob_View.Network_Id,
                Node_Id = eob_View.Node_Id,
                Node_Name = eob_View.Node_Name,
                Company_Id = eob_View.Company_Id,
                Group_Id = eob_View.Group_Id,
                Subscription_Id = eob_View.Subscription_Id
            };

            if (eob.Group_Id == 0)
            { 
                eob.Group_Id = null;
            }

            return eob;
        }

        public void Model_To_View(Eob eob, Eob_View eob_View)
        {
            eob_View.Id = eob.Id;
            eob_View.Serial_Number = eob.Serial_Number;
            eob_View.Network_Id = eob.Network_Id;
            eob_View.Node_Id = eob.Node_Id;
            eob_View.Node_Name = eob.Node_Name;
            eob_View.Company_Id = eob.Company_Id;
            eob_View.Subscription_Active = eob.Subscription.Active;

            if (eob.Subscription_Id.HasValue)
                eob_View.Subscription_Id = eob.Subscription_Id.Value;
            else
                eob_View.Subscription_Id = 0;

            if (eob.Group_Id.HasValue)
                eob_View.Group_Id = eob.Group_Id.Value;
            else
                eob_View.Group_Id = 0;
        }
    }
}
