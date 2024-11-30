using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Warhead;
using Exiled.Permissions.Extensions;
using MEC;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OmegaWarhead
{
    public class Plugin : Plugin<Class1>
    {
        public override string Author { get; } = "ClaudioPanConQueso|114514";
        public override string Name { get; } = "BetterOmegaWarhead|Omega核弹";
        public override Version Version { get; } = new Version(2,0);
        public static Plugin Singleton;
        public bool OmegaActivated = false;
        public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();
        public List<Player> HelikopterSurvivors = new List<Player>();
        public override void OnEnabled()
        {
            Singleton = this;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;
            Exiled.Events.Handlers.Warhead.Starting += OnWarheadStart;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Singleton = null;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;
            Exiled.Events.Handlers.Warhead.Starting -= OnWarheadStart;
            base.OnDisabled();
        }
        public void OnRestartingRound()
        {
            foreach (var coroutine in Coroutines)
                Timing.KillCoroutines(coroutine);
            HelikopterSurvivors.Clear();
            OmegaActivated = false;
            Coroutines.Clear();
        }
        public void OnWarheadStart(StartingEventArgs ev)
        {
            if (Plugin.Singleton.Config.ReplaceAlpha)
            {
                ev.IsAllowed = false;
                OmegaWarhead();
            }
            if (OmegaActivated)
            {
                ev.IsAllowed = false;
            }
        }
        public void StopOmega()
        {
            OmegaActivated = false;
            Cassie.Clear();
            HelikopterSurvivors.Clear();
            Cassie.MessageTranslated(Config.StopCassie, "Omega Warhead detonation stopped|Omega 弹头引爆停止");
            foreach (var coroutine in Plugin.Singleton.Coroutines)
                Timing.KillCoroutines(coroutine);
            foreach (Room room in Room.List)
                room.ResetColor();
        }
        public void OmegaWarhead()
        {
            OmegaActivated = true;
            foreach (Room room in Room.List)
                room.Color = Color.blue;

            Cassie.MessageTranslated(Config.Cassie, "<b><color=red>OMEGA核弹已启动</color></b>\n请撤离至避难所");
            Map.Broadcast(10, Plugin.Singleton.Config.ActivatedMessage);

            Coroutines.Add(Timing.CallDelayed(150, () =>
            {
                foreach (Door checkpoint in Door.List)
                {
                    if (checkpoint.Type == DoorType.CheckpointEzHczA || checkpoint.Type == DoorType.CheckpointEzHczB || checkpoint.Type == DoorType.CheckpointLczA || checkpoint.Type == DoorType.CheckpointLczB)
                    {
                        checkpoint.IsOpen = true;
                        checkpoint.Lock(69420, DoorLockType.Warhead);
                    }
                }
            }));

            Coroutines.Add(Timing.CallDelayed(179, () =>
            {
                Timing.CallDelayed(4, () =>
                {
                    foreach (Player Helikopter in Player.List)
                        if (HelikopterSurvivors.Contains(Helikopter))
                        {
                            Helikopter.DisableAllEffects();
                            Helikopter.Scale = new Vector3(1, 1, 1);
                            Helikopter.Position = new Vector3(178, 1000, -59);
                            Timing.CallDelayed(2, HelikopterSurvivors.Clear);
                        }
                });
                foreach (Player People in Player.List)
                {
                    if (People.CurrentRoom.Type == RoomType.EzShelter)
                    {
                        People.IsGodModeEnabled = true;
                        Timing.CallDelayed(0.2f, () =>
                        {
                            People.IsGodModeEnabled = false;
                            People.EnableEffect(EffectType.Flashed, 2);
                            People.Position = new Vector3(-53, 988, -50);
                            People.EnableEffect(EffectType.Blinded, 5);
                            Warhead.Detonate();
                            Warhead.Shake();
                        });
                    }
                    else if (!HelikopterSurvivors.Contains(People))
                    {
                        People.Kill("Omega Warhead.");
                    }
                }

                foreach (Room room in Room.List)
                    room.Color = Color.blue;
            }));

            //Don't bully me pls
            Coroutines.Add(Timing.CallDelayed(158, () =>
            {
                for (int i = 10; i > 0; i--)
                    Map.Broadcast(1, Plugin.Singleton.Config.HelicopterMessage + i);
                Timing.CallDelayed(12, () =>
                {
                    Vector3 HelicopterZone = new Vector3(178, 993, -59);
                    foreach (Player player in Player.List)
                        if (Vector3.Distance(player.Position, HelicopterZone) <= 10)
                        {
                            player.Broadcast(4, Plugin.Singleton.Config.HelicopterEscape);
                            player.Position = new Vector3(293, 978, -52);
                            player.Scale = new Vector3(0, 0, 0);
                            player.EnableEffect(EffectType.Flashed, 12);
                            HelikopterSurvivors.Add(player);
                            Timing.CallDelayed(0.5f, () =>
                            {
                                player.EnableEffect(EffectType.Ensnared);
                            });
                        }
                });
                RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
            }));
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Stop : ICommand
    {
        public string Command { get; } = "stopomegawarhead";

        public string[] Aliases { get; } = { "stopomega", "stopow", "sow" };

        public string Description { get; } = "Stops the Omega Warhead.";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender.CheckPermission("omegawarhead"))
            {
                if (Plugin.Singleton.OmegaActivated)
                {
                    Plugin.Singleton.StopOmega();
                    response = "Omega Warhead stopped.";
                    return false;
                }
                else
                {
                    response = "Omega Warhead is already stopped.";
                    return false;
                }
            }
            else
            {
                response = $"You need {Plugin.Singleton.Config.Permissions} permissions to use this command!";
                return true;
            }
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Activate : ICommand
    {
        public string Command { get; } = "activateomegawarhead";

        public string[] Aliases { get; } = { "activateomega", "activateow", "aow" };

        public string Description { get; } = "Activates the Omega Warhead.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender.CheckPermission("omegawarhead"))
            {
                if (!Plugin.Singleton.OmegaActivated)
                {
                    Plugin.Singleton.OmegaWarhead();
                    response = "Omega Warhead activated.";
                    return false;
                }
                else
                {
                    response = "Omega Warhead is already activated.";
                    return false;
                }
            }
            else
            {
                response = $"You need {Plugin.Singleton.Config.Permissions} permissions to use this command!";
                return true;
            }
        }
    }
}

