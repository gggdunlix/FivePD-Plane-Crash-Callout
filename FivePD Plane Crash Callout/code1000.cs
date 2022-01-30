using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.API;
using FivePD.API.Utils;


namespace Code1000
{
    [CalloutProperties("Code 1000 (Plane Crash)", "GGGDunlix", "0.1.18")]
    public class Code1000 : Callout
    {
        Ped pilot;
        Vehicle plane;

        public Code1000()
        {
            Random random = new Random();
            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(Vector3Extension.Around(Game.PlayerPed.Position, 200f))));
            ShortName = "Code 1000 (Plane Crash)";
            CalloutDescription = "A plane has crashed. Go to it's location and search for injuries, criminals, etc. Respond in Code 3 High.";
            ResponseCode = 3;
            StartDistance = 150f;
        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
            var carlist = new[]
            {
                VehicleHash.CargoPlane,
                VehicleHash.Cuban800,
                VehicleHash.Luxor,
                VehicleHash.Luxor2,
                VehicleHash.Dodo,
                VehicleHash.Duster,
                VehicleHash.Titan

            };
            plane = await SpawnVehicle(carlist[RandomUtils.Random.Next(carlist.Length)], Location);
            pilot = await SpawnPed(RandomUtils.GetRandomPed(), Location);
            pilot.AlwaysKeepTask = true;
            pilot.BlockPermanentEvents = true;
            plane.AttachBlip();
            CitizenFX.Core.Native.API.ForceAmbientSiren(true);



        }

        public override void OnStart(Ped player)
        {
            base.OnStart(player);
            pilot.AttachBlip();
            Utilities.ExcludeVehicleFromTrafficStop(plane.NetworkId, true);
            World.AddExplosion(plane.Position, ExplosionType.PlaneRocket, 50f, 15, pilot, true, false);
            CitizenFX.Core.Native.API.ExplodeVehicle(plane.NetworkId, true, false);
            CitizenFX.Core.Native.API.ForceAmbientSiren(false);
            GameplayCamera.Shake(CameraShake.LargeExplosion, 0);



        }
    }


}
