using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using ReplaysUnpackCS.Data;

namespace ReplaysUnpackCS.Processing
{
    public static class ReplayReader
    {
        private static IEnumerable<(int, byte[])> ChunkData(byte[] data, int len = 8)
        {
            int idx = 0;
            for (var s = 0; s <= data.Length; s += len)
            {
                byte[] g;
                try
                {
                    g = data[s..(s + len)];
                }
                catch (ArgumentOutOfRangeException)
                {
                    g = data[s..];
                }

                idx += 1;
                yield return (idx, g);
            }
        }

        public static void ReadReplay(FileStream replayFile)
        {
            byte[] bReplaySignature = new byte[4];
            byte[] bReplayBlockCount = new byte[4];
            byte[] bReplayBlockSize = new byte[4];
            byte[] bReplayJSONData;

            replayFile.Read(bReplaySignature, 0, 4);
            replayFile.Read(bReplayBlockCount, 0, 4);
            replayFile.Read(bReplayBlockSize, 0, 4);

            int jsonDataSize = BitConverter.ToInt32(bReplayBlockSize, 0);

            bReplayJSONData = new byte[jsonDataSize];
            replayFile.Read(bReplayJSONData, 0, jsonDataSize);

            var memStream = new MemoryStream();

            replayFile.CopyTo(memStream);
            var sBfishKey = "\x29\xB7\xC9\x09\x38\x3F\x84\x88\xFA\x98\xEC\x4E\x13\x19\x79\xFB";
            var bBfishKey = sBfishKey.Select(Convert.ToByte).ToArray();
            var bfish = new BlowFish(bBfishKey);
            long prev = 0;
            using var compressedData = new MemoryStream();
            foreach (var chunk in ChunkData(memStream.ToArray()[8..]))
            {
                try
                {
                    var decryptedBlock = BitConverter.ToInt64(bfish.Decrypt_ECB(chunk.Item2));
                    if (prev != 0)
                    {
                        decryptedBlock ^= prev;
                    }

                    prev = decryptedBlock;
                    compressedData.Write(BitConverter.GetBytes(decryptedBlock));
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            compressedData.Seek(2, SeekOrigin.Begin); //DeflateStream doesn't strip the header so we strip it manually.
            var decompressedData = new MemoryStream();
            using (DeflateStream df = new(compressedData, CompressionMode.Decompress))
            {
                df.CopyTo(decompressedData);
            }

            //Console.WriteLine(decompressedData.Length);
            decompressedData.Seek(0, SeekOrigin.Begin);

            while (decompressedData.Position != decompressedData.Length)
            {
                var np = new NetworkPacket(decompressedData);

                //Console.WriteLine("{0}: {1}", np.time, np.type);
                if (np.Type == "08")
                {
                    var em = new EntityMethod(np.RawData);

                    // Console.WriteLine("{0}: {1}: {2}\n", em.entityId, em.messageId, em.data.Value.Length);
                    if (em.MessageId == 124) // 10.10=124
                    {
                        // Console.WriteLine("{0}: {1}\n", em.entityId, em.messageId);

                        //var unk1 = new byte[8]; //?
                        //em.data.Value.Read(unk1);

                        var arenaID = new byte[8];
                        em.Data.Value.Read(arenaID);

                        var teamBuildTypeID = new byte[1];
                        em.Data.Value.Read(teamBuildTypeID);

                        var blobPreBattlesInfoSize = new byte[1];
                        em.Data.Value.Read(blobPreBattlesInfoSize);
                        var blobPreBattlesInfo = new byte[blobPreBattlesInfoSize[0]];
                        em.Data.Value.Read(blobPreBattlesInfo);

                        var blobPlayersStatesSize = new byte[1];
                        em.Data.Value.Read(blobPlayersStatesSize);

                        if (blobPlayersStatesSize[0] == 255)
                        {
                            var blobPlayerStatesRealSize = new byte[2];
                            em.Data.Value.Read(blobPlayerStatesRealSize);
                            var playerStatesRealSize = BitConverter.ToUInt16(blobPlayerStatesRealSize);
                            em.Data.Value.Read(new byte[1]); //?

                            // blobPlayerStates will contain players' information like account id, server realm, etc...
                            // but it is serialized via Python's pickle.
                            // We use Razorvine's Pickle Unpickler for that.

                            var blobPlayerStates = new byte[playerStatesRealSize];
                            em.Data.Value.Read(blobPlayerStates);

                            Razorvine.Pickle.Unpickler.registerConstructor("CamouflageInfo", "CamouflageInfo",
                                new CamouflageInfo());
                            var k = new Razorvine.Pickle.Unpickler();

                            ArrayList players = (ArrayList)k.load(new MemoryStream(blobPlayerStates));

                            foreach (ArrayList player in players)
                            {
                                foreach (object[] properties in player)
                                {
                                    //Console.WriteLine("{0}: {1}", Constants.PropertyMapping[(int)properties[0]].PadRight(21, ' '), properties[1]);
                                }

                                Console.WriteLine("");
                            }
                            /*
                                ...
                                accountDBID          : 2016494874
                                avatarId             : 919187
                                camouflageInfo       : 4205768624, 0
                                clanColor            : 13427940
                                clanID               : 2000008825
                                clanTag              : TF44
                                crewParams           : System.Collections.ArrayList
                                dogTag               : System.Collections.ArrayList
                                fragsCount           : 0
                                friendlyFireEnabled  : False
                                id                   : 537149649
                                invitationsEnabled   : True
                                isAbuser             : False
                                isAlive              : True
                                isBot                : False
                                isClientLoaded       : False
                                isConnected          : True
                                isHidden             : False
                                isLeaver             : False
                                isPreBattleOwner     : False
                                killedBuildingsCount : 0
                                maxHealth            : 27500
                                name                 : notyourfather
                                playerMode           : Razorvine.Pickle.Objects.ClassDict
                                preBattleIdOnStart   : 537256655
                                preBattleSign        : 0
                                prebattleId          : 537256655
                                realm                : ASIA
                                shipComponents       : System.Collections.Hashtable
                                shipId               : 919188
                                shipParamsId         : 4288591856
                                skinId               : 4288591856
                                teamId               : 0
                                ttkStatus            : False
                                ...
                             */
                        }
                    }
                    else if (em.MessageId == 122) // 10.10=122,
                    {
                        var bEntityId = new byte[4];
                        em.Data.Value.Read(bEntityId);
                        var entityId = BitConverter.ToUInt32(bEntityId);

                        var bMessageGroupSize = new byte[1];
                        em.Data.Value.Read(bMessageGroupSize);
                        var bMessageGroup = new byte[bMessageGroupSize[0]];
                        em.Data.Value.Read(bMessageGroup);
                        var messageGroup = Encoding.UTF8.GetString(bMessageGroup);

                        var bMessageContentSize = new byte[1];
                        em.Data.Value.Read(bMessageContentSize);
                        var bMessageContent = new byte[bMessageContentSize[0]];
                        em.Data.Value.Read(bMessageContent);
                        var messageContent = Encoding.UTF8.GetString(bMessageContent);

                        Console.WriteLine("{0} : {1} : {2}", entityId, messageGroup, messageContent);
                        /*
                            615476 : battle_team : cv run
                            615474 : battle_common : nb
                            615488 : battle_team : lol
                            615452 : battle_team : lol
                            615480 : battle_team : ???????bug?
                            615480 : battle_team : ?????????
                            615480 : battle_team : ??
                            615474 : battle_common : ??????
                            615452 : battle_team : ???????
                            615480 : battle_team : ???
                            615480 : battle_team : ??
                            615480 : battle_team : ????`
                            615480 : battle_team : ?TM???
                            615452 : battle_team : ??
                            615480 : battle_team : ????????
                            615480 : battle_team : ????????
                            615480 : battle_team : ??
                            615452 : battle_team : ? ???????
                         */
                    }
                }
            }
        }
    }
}