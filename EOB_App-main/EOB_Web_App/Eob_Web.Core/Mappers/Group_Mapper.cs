using Eob_Web.Core.Models;
using Eob_Web.Core.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eob_Web.Core.Mappers
{
    public class Group_Mapper
    {
        public Group View_To_Model(Group_View group_View)
        {
            var group = new Group
            {
                Id = group_View.Id,
                Name = group_View.Name,
                Department = group_View.Department,
                Company_Id = group_View.Company_Id 
            };

            return group;
        }

        public void Model_To_View(Group group, Group_View group_View)
        {
            group_View.Id = group.Id;
            group_View.Name = group.Name;
            group_View.Department = group.Department;
            group_View.Company_Id = group.Company_Id;
        }
    }
}
