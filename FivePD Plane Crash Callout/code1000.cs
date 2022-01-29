using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;


namespace Code1000
{
    [CalloutProperties("Code 1000 (Plane Crash)", "GGGDunlix", "0.0.1")]
    public class Code1000 : Callout
    {
        Ped suspect;
        Vehicle plane;

        public Code1000()
        {
            Random random = new Random();
            InitInfo(Vector3Extension.ApplyOffset(Game.PlayerPed.Position, CitizenFX.Core.Vector3.));
            ShortName = "Code 1000 (Plane Crash)";
            CalloutDescription = "A plane is falling and about to land. Go to it's location and search for injuries, criminals, etc. Respond in Code 3 High.";
            ResponseCode = 3;
            StartDistance = 1100f;
        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
            var carlist = new[]
            {
                VehicleHash.CargoPlane,
                VehicleHash.Cuban800
            };
            plane = await SpawnVehicle(carlist[RandomUtils.Random.Next(carlist.Length)], Location);
            suspect = await SpawnPed(RandomUtils.GetRandomPed(), Location);
            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            plane.AttachBlip();
            
        }

        public override void OnStart(Ped player)
        {
            base.OnStart(player);
            suspect.AttachBlip();
            Utilities.ExcludeVehicleFromTrafficStop(plane.NetworkId, true);
            suspect.SetIntoVehicle(plane, VehicleSeat.Driver);
            Utilities.RequestBackup(Utilities.Backups.Code99);
        }
    }


}
