using Altseed2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altseed2Extension.Extension
{
    public class Sound
    {
        private static float s_bgmVolume = 1;
        private static float s_seVolume = 1;

        public static Sound Bgm { get; private set; }
        public static HashSet<int> SoundIds { get; } = new HashSet<int>();

        /// <summary>
        /// BGMとして再生
        /// </summary>
        /// <param name="sound">音源</param>
        /// <param name="fade">フェード時間</param>
        /// <param name="startPoint">ループ開始</param>
        /// <param name="endPoint">ループ終了</param>
        public static void StartBgm(Sound sound, float fade = 0, float? startPoint = null, float? endPoint = null)
        {
            if (Bgm != null && Engine.Sound.GetIsPlaying(Bgm.id))
            {
                Engine.Sound.FadeOut(Bgm.id, fade);
            }
            if (sound.sound == null) return;
            sound.sound.IsLoopingMode = true;
            sound.sound.LoopStartingPoint = startPoint != null ? (float)startPoint : 0;
            sound.sound.LoopEndPoint = endPoint != null ? (float)endPoint : sound.sound.Length;
            Bgm = sound;
            Bgm.id = Engine.Sound.Play(Bgm.sound);
            Engine.Sound.SetVolume(Bgm.id, BgmVolume);
            Engine.Sound.FadeIn(Bgm.id, fade);
        }

        /// <summary>
        /// BGMを止める
        /// </summary>
        /// <param name="fade">フェード時間</param>
        public static void StopBgm(float fade = 0)
        {
            if (Bgm != null && Engine.Sound.GetIsPlaying(Bgm.id))
            {
                Engine.Sound.FadeOut(Bgm.id, fade);
            }
        }

        /// <summary>
        /// 全てのSEをフェードさせる
        /// </summary>
        /// <param name="duration"></param>
        public static void FadeAllSE(float duration)
        {
            foreach (var id in SoundIds)
            {
                Engine.Sound.FadeOut(id, duration);
            }
        }

        /// <summary>
        /// BGMボリューム
        /// </summary>
        public static float BgmVolume
        {
            get => s_bgmVolume;
            set
            {
                if (s_bgmVolume >= 0f && s_bgmVolume <= 1f) s_bgmVolume = value;
            }
        }

        /// <summary>
        /// SEボリューム
        /// </summary>
        public static float SeVolume
        {
            get => s_seVolume;
            set
            {
                if (s_seVolume >= 0f && s_seVolume <= 1f) s_seVolume = value;
            }
        }

        Altseed2.Sound sound;
        int id;

        public Altseed2.Sound InternalSound => sound;

        /// <summary>
        /// 多重再生認めるか
        /// </summary>
        public bool IsMultiplePlay { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public int ID => id;

        public Sound(string path, bool isMultiplePlay = true, bool isDecompressed = false)
        {
            if (path == null) return;
            sound = Altseed2.Sound.Load(path, isDecompressed);
            if (sound == null)
                Engine.Log.Error(LogCategory.User, path + "not found.");
        }

        /// <summary>
        /// 再生
        /// </summary>
        /// <returns>再生ID</returns>
        public int Play(float volume = 1f)
        {
            if (IsMultiplePlay && GetIsPlaying() && sound == null) return -1;
            id = Engine.Sound.Play(sound);
            Engine.Sound.SetVolume(id, SeVolume * volume);
            SoundIds.Add(id);
            return id;
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="id">再生ID</param>
        /// <returns></returns>
        public int Stop(int? id = null)
        {
            Engine.Sound.FadeOut(id ?? this.id, 1 / 60f);
            var res = id ?? this.id;
            this.id = -1;
            SoundIds.Remove(id ?? this.id);
            return res;
        }

        /// <summary>
        /// 音源が再生されているかを取得
        /// </summary>
        /// <returns></returns>
        public bool GetIsPlaying()
        {
            return Engine.Sound.GetIsPlaying(id);
        }
    }
}
