using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public void SetRedHumanType(bool on)
    {
        if (on)
        {
            SaveSettings.players[0] = "HUMAN";
        }
    }
	public void SetRedCpuType(bool on)
	{
		if (on)
		{
			SaveSettings.players[0] = "CPU";
		}
	}
	public void SetGreenHumanType(bool on)
	{
		if (on)
		{
			SaveSettings.players[1] = "HUMAN";
		}
	}
	public void SetGreenCpuType(bool on)
	{
		if (on)
		{
			SaveSettings.players[1] = "CPU";
		}
	}
	public void SetBlueHumanType(bool on)
	{
		if (on)
		{
			SaveSettings.players[2] = "HUMAN";
		}
	}
	public void SetBlueCpuType(bool on)
	{
		if (on)
		{
			SaveSettings.players[2] = "CPU";
		}
	}
	public void SetYellowHumanType(bool on)
	{
		if (on)
		{
			SaveSettings.players[3] = "HUMAN";
		}
	}
	public void SetYellowCpuType(bool on)
	{
		if (on)
		{
			SaveSettings.players[3] = "CPU";
		}
	}
}

public static class SaveSettings 
{
    public static string[] players = new string[4];
}
