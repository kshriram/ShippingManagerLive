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
    public class cmdDeletedPalletSave
    {
        local_x3v6Entities entx3v6 = new local_x3v6Entities();

        #region DeletedPalletSave
        public string saveDeletedPallet(List<cstDeletedPalletSave> lsPackingOb)
        {
            string Retuen = "Fail";
            try
            {
                foreach (var _Delete in lsPackingOb)
                {
                    DeletedPalletSave _DeleteBox = new DeletedPalletSave();
                    _DeleteBox.DeletedPalletID = _Delete.DeletedPalletID;
                    _DeleteBox.BoxNumber = _Delete.BoxNumber;
                    _DeleteBox.UserID = _Delete.UserID;
                    _DeleteBox.PalletNumber = _Delete.PalletNumber;
                    _DeleteBox.DeletedDateTime = Convert.ToDateTime(_Delete.DeleteDateTime);
                    _DeleteBox.Location = _Delete.Location;
                    _DeleteBox.StationName = _Delete.StationName;
                    _DeleteBox.ActionStatus = _Delete.ActionStatus;

                    //view added extra

                    entx3v6.AddToDeletedPalletSaves(_DeleteBox);
                }
                entx3v6.SaveChanges();
                Retuen = "Success";
            }
            catch (Exception Ex)
            {
                Error_Loger.elAction.save("saveDeletedPallet()", Ex.Message.ToString());
            }
            return Retuen;
        }
        #endregion
    }
}
