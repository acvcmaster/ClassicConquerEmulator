using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCO
{
    public enum ChatType : ushort
    {
        Talk = 0x7D0,
        Whisper = 0x7D1,
        Action = 0x7D2,
        Team = 0x7D3,
        Guild = 0x7D4,
        Top = 0x7D5,
        Spouse = 0x7D6,
        Yell = 0x7D8,
        Friend = 0x7D9,
        Broadcast = 0x7DA,
        Center = 0x7DB,
        Ghost = 0x7DD,
        Service = 0x7DE,
        Dialog = 0x834,
        LoginInformation = 0x835,
        VendorHawk = 0x838,
        Webpage = 0x839,
        MessageBox = 0x840,
        MiniMap = 0x83C,
        MiniMap2 = 0x83D,
        FriendsOfflineMessage = 0x83E,
        GuildBulletin = 0x83F,
        TradeBoard = 0x899,
        FriendBoard = 0x89A,
        TeamBoard = 0x89B,
        GuildBoard = 0x89C,
        OthersBoard = 0x89D,
        SystemBBS = 0x89E
    }
    public enum ChatColor
    {
        Blue = 0x0000FF,
        Red = 0xFF0000,
        Green = 0x00FF00,
        Yellow = 0x00FFFF,
        Pink = 0xFF00FF,
        Black = 0x000000,
        White = 0xFFFFFF,
        Default = 16777215
    }
}
