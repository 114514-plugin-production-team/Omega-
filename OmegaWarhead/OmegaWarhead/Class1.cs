using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaWarhead
{
    public class Class1 : Exiled.API.Interfaces.IConfig
    {
        public bool IsEnabled {  get; set; } = true;
        public bool Debug { get; set; } = true;
        [Description("如果为 true，则将 Alpha Warhead 替换为 Omega Warhead")]
        public bool ReplaceAlpha { get; set; } = false;
        [Description("Broadcast that will appear when the rescue helicopter is coming|救援直升机来时出现的广播.")]
        public string HelicopterMessage { get; set; } = "<color=blue>救援直升机在:</color> ";
        [Description("Broadcast that will appear when the player escapes in the helicopter.|当玩家乘坐直升机逃脱时出现的广播")]
        public string HelicopterEscape { get; set; } = "You escaped in the helicopter.";
        [Description("Broadcast that will appear when the Omega Warhead is activated.|激活 Omega 弹头时出现的广播")]
        public string ActivatedMessage { get; set; } = "<b><color=red>OMEGA核弹已启动</color></b>\n请撤离至避难所.";
        [Description("Cassie message when Omega Warhead is stopped|Omega 核弹 停止时的 Cassie 消息")]
        public string StopCassie { get; set; } = "pitch_0.9 Omega Warhead detonation stopped";
        [Description("Cassie message of Omega Warhead (Not recommended to modify this)|Omega 核弹 的 Cassie 消息（不建议修改此内容）")]
        public string Cassie { get; set; } = "pitch_0.2 .g3 .g3 .g3 pitch_0.9 attention . attention . activating omega warhead . detonation in 3 minutes . please evacuate in the breach shelter or in the helicopter . please evacuate now . 170 seconds until destruction .g3 . .  .g3 . .  . .g3 . . .g3 . . .g3 . . . . .g3 . . . . .g3 . . . .g3 . . . .g3 . . . .g3 . . . .g3 . . . .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 120 seconds  .g3 . .  .g3 . .  .g3 . .  .g3 . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .  .g3 . . .   .g3 . . .   .g3 . .   .g3 . . .   .g3 . . .   .g3 . . .   .g3 . . .   .g3 . . .   .g3 . . .   .g3 . . .   .g3 . . .   .g3 . . .   .g3 . . .   .g3 . . .   1 minute .g3 .  . .g3 .  . .g3 .  . .g3 .  . .g3 .  . .g3 .  . .g3 .  . .g3 .  . .g3 .  . .g3 .  . .g3 .  . .g3 .  . .g3 .  . .g3 .  . 30 seconds . all checkpoint doors are open . please evacuate . 20 . 19 . 18 . 17 . 16 . 15 . 14 . 13 . 12 . 11 . 10 seconds 9 . 8 . 7 . 6 . 5 . 4 . 3 . 2 . 1 . pitch_0.7 0";

        [Description("Permissions of the plugin.|插件的权限")]
        public string Permissions { get; set; } = "omegawarhead";
    }
}
