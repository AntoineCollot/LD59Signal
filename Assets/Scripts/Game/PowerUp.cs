using System;

[Flags]
public enum PowerUp
{
    None = 0,
    Split = 1<<0,
    OppositeWave = 1<<1,
    AmplitudeMax = 1<<2,
    InTheBack = 1<<3,
    HeavyArtillery = 1<<4,
}