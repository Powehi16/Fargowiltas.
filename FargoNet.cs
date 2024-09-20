using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas;

public static class FargoNet
{
	public const byte SummonNPCFromClient = 0;

	private const bool Debug = false;

	public static void SendData(int dataType, int dataA, int dataB, string text, int playerID, float dataC, float dataD, float dataE, int clientType)
	{
		NetMessage.SendData(dataType, dataA, dataB, NetworkText.FromLiteral(text), playerID, dataC, dataD, dataE, clientType);
	}

	public static ModPacket WriteToPacket(ModPacket packet, byte msg, params object[] param)
	{
		packet.Write(msg);
		foreach (object obj in param)
		{
			object obj2 = obj;
			object obj3 = obj2;
			if (!(obj3 is byte[]))
			{
				if (!(obj3 is bool))
				{
					if (!(obj3 is byte))
					{
						if (!(obj3 is short))
						{
							if (!(obj3 is int))
							{
								if (!(obj3 is float))
								{
									if (obj3 is string)
									{
										packet.Write((string)obj);
									}
								}
								else
								{
									packet.Write((float)obj);
								}
							}
							else
							{
								packet.Write((int)obj);
							}
						}
						else
						{
							packet.Write((short)obj);
						}
					}
					else
					{
						packet.Write((byte)obj);
					}
				}
				else
				{
					packet.Write((bool)obj);
				}
			}
			else
			{
				byte[] array = (byte[])obj;
				byte[] array2 = array;
				foreach (byte b in array2)
				{
					packet.Write(b);
				}
			}
		}
		return packet;
	}

	public static void SyncAI(Entity codable, float[] ai, int aitype)
	{
		int entType = ((!(codable is NPC)) ? ((codable is Projectile) ? 1 : (-1)) : 0);
		if (entType != -1)
		{
			int id = ((codable is NPC) ? ((NPC)codable).whoAmI : ((Projectile)codable).identity);
			SyncAI(entType, id, ai, aitype);
		}
	}

	public static void SyncAI(int entType, int id, float[] ai, int aitype)
	{
		object[] ai2 = new object[ai.Length + 4];
		ai2[0] = (byte)entType;
		ai2[1] = (short)id;
		ai2[2] = (byte)aitype;
		ai2[3] = (byte)ai.Length;
		for (int m = 4; m < ai2.Length; m++)
		{
			ai2[m] = ai[m - 4];
		}
		SendFargoNetMessage(1, ai2);
	}

	public static object[] WriteVector2Array(Vector2[] array)
	{
		List<object> list = new List<object> { array.Length };
		for (int i = 0; i < array.Length; i++)
		{
			Vector2 vec = array[i];
			list.Add(vec.X);
			list.Add(vec.Y);
		}
		return list.ToArray();
	}

	public static void WriteVector2Array(Vector2[] array, BinaryWriter writer)
	{
		writer.Write(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			Vector2 vec = array[i];
			writer.Write(vec.X);
			writer.Write(vec.Y);
		}
	}

	public static Vector2[] ReadVector2Array(BinaryReader reader)
	{
		int arrayLength = reader.ReadInt32();
		Vector2[] array = new Vector2[arrayLength];
		for (int m = 0; m < arrayLength; m++)
		{
			array[m] = new Vector2(reader.ReadSingle(), reader.ReadSingle());
		}
		return array;
	}

	public static void SendFargoNetMessage(int msg, params object[] param)
	{
		if (Main.netMode != 0)
		{
			WriteToPacket(ModContent.GetInstance<Fargowiltas>().GetPacket(), (byte)msg, param).Send();
		}
	}

	public static void HandlePacket(BinaryReader bb, byte msg)
	{
		bool flag = false;
		try
		{
			if (msg == 0)
			{
				int playerID = bb.ReadByte();
				int bossType = bb.ReadInt16();
				bool spawnMessage = bb.ReadBoolean();
				int npcCenterX = bb.ReadInt32();
				int npcCenterY = bb.ReadInt32();
				string overrideDisplayName = bb.ReadString();
				bool namePlural = bb.ReadBoolean();
				if (Main.netMode == 2)
				{
					Fargowiltas.SpawnBoss(Main.player[playerID], bossType, spawnMessage, new Vector2(npcCenterX, npcCenterY), overrideDisplayName, namePlural);
				}
			}
		}
		catch (Exception e)
		{
			ModContent.GetInstance<Fargowiltas>().Logger.Error(((Main.netMode == 2) ? "--SERVER-- " : "--CLIENT-- ") + "ERROR HANDLING MSG: " + msg + ": " + e.Message);
			ModContent.GetInstance<Fargowiltas>().Logger.Error(e.StackTrace);
			ModContent.GetInstance<Fargowiltas>().Logger.Error("-------");
		}
	}

	public static void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		bool flag = false;
		if (Main.netMode == 2 && (toWho > -1 || fromWho > -1))
		{
			PlayerConnected();
		}
	}

	public static void PlayerConnected()
	{
		bool flag = false;
	}

	public static void SendNetMessage(int msg, params object[] param)
	{
		SendNetMessageClient(msg, -1, param);
	}

	public static void SendNetMessageClient(int msg, int client, params object[] param)
	{
		try
		{
			if (Main.netMode != 0)
			{
				WriteToPacket(ModContent.GetInstance<Fargowiltas>().GetPacket(), (byte)msg, param).Send(client);
			}
		}
		catch (Exception e)
		{
			ModContent.GetInstance<Fargowiltas>().Logger.Error(((Main.netMode == 2) ? "--SERVER-- " : "--CLIENT-- ") + "ERROR SENDING MSG: " + msg + ": " + e.Message);
			ModContent.GetInstance<Fargowiltas>().Logger.Error(e.StackTrace);
			ModContent.GetInstance<Fargowiltas>().Logger.Error("-------");
			string param2 = string.Empty;
			for (int m = 0; m < param.Length; m++)
			{
				param2 += param[m];
			}
			ModContent.GetInstance<Fargowiltas>().Logger.Error("PARAMS: " + param2);
			ModContent.GetInstance<Fargowiltas>().Logger.Error("-------");
		}
	}
}
