using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
namespace PackingClassLibrary.Commands.SMcommands
{

     

    public class cmdDeleteBoxSave
    {

        local_x3v6Entities entx3v6 = new local_x3v6Entities();

        #region DeletedBoxSave
        public string saveDeletedBox(List<cstDeletedBoxSave> lsPackingOb)
        {
            string Retuen = "Fail";
            try
            {
                foreach (var _Delete in lsPackingOb)
                {
                    DeletedBoxSave _DeleteBox = new DeletedBoxSave();
                    _DeleteBox.DeletedBoxID = _Delete.DeletedBoxID;
                    _DeleteBox.BoxNumber = _Delete.BoxNumber;
                    _DeleteBox.UserID = _Delete.UserID;
                    _DeleteBox.SKUName = _Delete.SKUName;
                    _DeleteBox.SKUQuantity = _Delete.SKUQuantity;
                    _DeleteBox.DeleteDateTime = Convert.ToDateTime(_Delete.DeleteDateTime);
                    _DeleteBox.Location = _Delete.Location;
                    _DeleteBox.StationName = _Delete.StationName;
                    _DeleteBox.ActionStatus = _Delete.ActionStatus;

                    //view added extra

                    entx3v6.AddToDeletedBoxSaves(_DeleteBox);
                }
                entx3v6.SaveChanges();
                Retuen = "Success";
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("SetPakingDetailsCommand.Execute()", Ex.Message.ToString());
            }
            return Retuen;
        }
        #endregion
    }
}
