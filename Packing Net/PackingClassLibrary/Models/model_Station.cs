using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.Commands;
using PackingClassLibrary.CustomEntity;
using PackingClassLibrary.CustomEntity.SMEntitys;
using PackingClassLibrary.Commands.SMcommands;

namespace PackingClassLibrary.Models
{
    /// <summary>
    /// Author: Avinash.
    /// Versiom: Alfa.
    /// Station Information model.
    /// </summary>
    public class model_Station
    {

        public static Guid StationID { get; set; }
        public cstStationMasterTbl StationInfo { get; set; }

        /// <summary>
        /// Get Station Information 
        /// set station Information n
        /// Delete station Information methods object.
        /// </summary>
        public cmdStation StationFucntions = new cmdStation();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public model_Station() { }

        /// <summary>
        /// Parameterised constructor.
        /// </summary>
        /// <param name="StationIDc">Guid Station ID</param>
        public model_Station(Guid StationIDc)
        {
            StationID = StationIDc;
            //Set Model Infotion;
            _modelSetStationInfotmation();
        }

        /// <summary>
        /// Parameterised Constructor 
        /// </summary>
        /// <param name="DeviceID">String Device number.(MAC Address)</param>
        public model_Station(String DeviceID) 
        {
            StationID = StationFucntions.GetStationList(DeviceID).FirstOrDefault().StationID;
            //Set Model Infotion;
            _modelSetStationInfotmation();
        }

        /// <summary>
        /// Model Station information Object Fill.
        /// </summary>
        private void _modelSetStationInfotmation()
        {
            StationInfo = StationFucntions.GetStationList(StationID);
        }

    }
}
