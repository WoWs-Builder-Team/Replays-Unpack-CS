using System.Collections.Generic;

namespace ReplaysUnpackCS
{
    public static class Constants
    {
        public enum PropertyMapper
        {
            AccountId = 0,
            AntiAbuseEnabled = 1,
            AvatarId = 2,
            CamouflageInfo = 3,
            ClanColor = 4,
            ClanId = 5,
            ClanTag = 6,
            CrewParams = 7,
            DogTag = 8,
            FragsCount = 9,
            FriendlyFireEnabled = 10,
            Id = 11,
            InvitationsEnabled = 12,
            IsAbuser = 13,
            IsAlive = 14,
            IsBot = 15,
            IsClientLoaded = 16,
            IsConnected = 17,
            IsHidden = 18,
            IsLeaver = 19,
            IsPreBattleOwner = 20,
            IsTShooter = 21,
            KilledBuildingsCount = 22,
            MaxHealth = 23,
            Name = 24,
            PlayerMode = 25,
            PreBattleIdOnStart = 26,
            PreBattleSign = 27,
            PreBattleId = 28,
            Realm = 29,
            ShipComponents = 30,
            ShipConfigDump = 31,
            ShipId = 32,
            ShipParamsId = 33,
            SkinId = 34,
            TeamId = 35,
            TtkStatus = 36,
        }
        
        #region Static Fields and Constants

        public static Dictionary<int, string> PropertyMapping = new()
        {
            { 0, "accountDBID" },
            { 1, "antiAbuseEnabled" },
            { 2, "avatarId" },
            { 3, "camouflageInfo" },
            { 4, "clanColor" },
            { 5, "clanID" },
            { 6, "clanTag" },
            { 7, "crewParams" },
            { 8, "dogTag" },
            { 9, "fragsCount" },
            { 10, "friendlyFireEnabled" },
            { 11, "id" },
            { 12, "invitationsEnabled" },
            { 13, "isAbuser" },
            { 14, "isAlive" },
            { 15, "isBot" },
            { 16, "isClientLoaded" },
            { 17, "isConnected" },
            { 18, "isHidden" },
            { 19, "isLeaver" },
            { 20, "isPreBattleOwner" },
            { 21, "isTShooter" },
            { 22, "killedBuildingsCount" },
            { 23, "maxHealth" },
            { 24, "name" },
            { 25, "playerMode" },
            { 26, "preBattleIdOnStart" },
            { 27, "preBattleSign" },
            { 28, "prebattleId" },
            { 29, "realm" },
            { 30, "shipComponents" },
            { 31, "shipConfigDump" },
            { 32, "shipId" },
            { 33, "shipParamsId" },
            { 34, "skinId" },
            { 35, "teamId" },
            { 36, "ttkStatus" },
        };

        #endregion
    }
}