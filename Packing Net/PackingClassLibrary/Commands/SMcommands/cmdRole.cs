using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PackingClassLibrary.CustomEntity.SMEntitys;

namespace PackingClassLibrary.Commands.SMcommands
{
   public class cmdRole
    {
       local_x3v6Entities ent = new local_x3v6Entities();

       /// <summary>
       /// Check That user IsAdmin User Or Not.
       /// </summary>
       /// <param name="UserID">
       /// Guid UserID
       /// </param>
       /// <returns>
       /// Boolean True Or Flase;
       /// </returns>
       public Boolean IsSuperUser(Guid UserID)
       {
           Boolean _return = true;

           try
           {
               Guid RoleID = ent.Users.FirstOrDefault(i => i.UserID == UserID).RoleId;

               String Action = ent.Roles.FirstOrDefault(i => i.RoleId == RoleID).Action.ToString();
               string[] isAdmin = Action.Split('&')[0].Split('-');
               foreach (String Act in isAdmin)
               {
                   if(Act.ToUpper() == "FALSE")
                   {
                       _return = false;
                       break;
                   }
               }
           }
           catch (Exception)
           {}

           return _return;
       }


       /// <summary>
       /// Check that user has manager Override permission.
       /// </summary>
       /// <param name="UserID"></param>
       /// <returns></returns>
       public Boolean CanOverride(Guid UserID)
       {
           Boolean _return = true;
           try
           {
               Guid RoleID = ent.Users.FirstOrDefault(i => i.UserID == UserID).RoleId;

               String Action = ent.Roles.FirstOrDefault(i => i.RoleId == RoleID).Action.ToString();
               string[] permission = Action.Split('&')[1].Split('-');
               
                   if (permission[3].ToUpper() == "FALSE")
                   {
                       _return = false;
                   }
           }
           catch (Exception)
           { }

           return _return;
       }

       /// <summary>
       /// Update Role Permissons
       /// </summary>
       /// <param name="RoleID">
       /// Guid RoleID to be update
       /// </param>
       /// <param name="RoleName">
       /// </param>
       /// <param name="IsSuperUser"></param>
       /// <param name="View"></param>
       /// <param name="Scan"></param>
       /// <param name="Rescan"></param>
       /// <param name="Override"></param>
       /// <returns></returns>
       public Boolean SetRole(Guid RoleID,String RoleName ,Boolean IsSuperUser, Boolean View, Boolean Scan, Boolean Rescan, Boolean Override)
       {
           Boolean _return = false;

           try
           {
               Role role = ent.Roles.FirstOrDefault(i => i.RoleId == RoleID);
               role.Name = RoleName;

               String permission= "";
               if (IsSuperUser)
                   permission = "True-True-True-True&";
               else
                   permission = "False-False-False-False&";
               if (View)
                   permission = permission + "True";
               else
                   permission = permission + "False";
               if (Scan)
                   permission = permission + "-True";
               else
                   permission = permission + "-False";
               if (Rescan)
                   permission = permission + "-True";
               else
                   permission = permission + "-False";
               if (Override)
                   permission = permission + "-True";
               else
                   permission = permission + "-False";
               role.Action = permission;
               ent.SaveChanges();


               _return = true;
           }
           catch (Exception)
           {}
           return _return;
       }

    }
}
