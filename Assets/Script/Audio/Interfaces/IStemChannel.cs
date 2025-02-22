﻿using System;
using Cysharp.Threading.Tasks;
using YARG.Core.Audio;

namespace YARG.Audio
{
    public interface IStemChannel : IDisposable
    {
        public SongStem Stem { get; }
        public double LengthD { get; }
        public float LengthF => (float) LengthD;

        public double Volume { get; }

        public event Action ChannelEnd;

        public int Load(float speed);

        public void FadeIn(float maxVolume);
        public UniTask FadeOut();

        public void SetVolume(double newVolume);

        public void SetReverb(bool reverb);

        public void SetSpeed(float speed);
        public void SetWhammyPitch(float percent);

        public double GetPosition(bool bufferCompensation = true);
        public void SetPosition(double position, bool bufferCompensation = true);

        public double GetLengthInSeconds();
    }
}