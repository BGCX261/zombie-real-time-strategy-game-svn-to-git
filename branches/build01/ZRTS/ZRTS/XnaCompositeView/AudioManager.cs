using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZRTSModel.Factories;
using System.Collections;
using ZRTS.InputEngines;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace ZRTS.XnaCompositeView
{
    /// <summary>
    /// Handle and manage all audio effect associated with the gameplay
    /// </summary>
    public static class AudioManager
    {

        public static bool audioReady = true;       // Flag for checking if audio hardware is supported by the machine


        /// <summary>
        /// Initializes all the audio content.
        /// </summary>
        /// <param name="Content">The ContentManager to use</param>
        public static void Initialize(ContentManager Content)
        {
            try
            {
                // Songs
                GameplayMusic = Content.Load<Song>("audio/GameplayMusic");
                TitleMusic = Content.Load<Song>("audio/TitleMusic");
                VictoryMusic = Content.Load<Song>("audio/VictoryMusic");

                // Sound Effects
                Soldier_Attack = Content.Load<SoundEffect>("audio/Soldier_Attack");
                Soldier_Dying = Content.Load<SoundEffect>("audio/Soldier_Dying");
                Worker_Dying = Content.Load<SoundEffect>("audio/Worker_Dying");
                Worker_Attack = Content.Load<SoundEffect>("audio/Worker_Attack");
                Zombie_Dying = Content.Load<SoundEffect>("audio/Zombie_Dying");
                Zombie_Attack = Content.Load<SoundEffect>("audio/Zombie_Attack");
            }
            catch (Exception e)
            {
                audioReady = false;
            }

            MediaPlayer.Volume = 0.50f;
        }

        // Songs
        public static Song GameplayMusic;
        public static Song TitleMusic;
        public static Song VictoryMusic;

        // Sound Effects
        public static SoundEffect Soldier_Attack;
        public static SoundEffect Soldier_Dying;
        public static SoundEffect Worker_Dying;
        public static SoundEffect Worker_Attack;
        public static SoundEffect Zombie_Dying;
        public static SoundEffect Zombie_Attack;

        /// <summary>
        /// Playing sound effect based on action and unit types
        /// </summary>
        /// <param name="type">Action Type</param>
        /// <param name="subtype">Unit type</param>
        public static void play(string type, string subtype)
        {
            // Sanity Check
            if (!audioReady)
            {
                return;
            }

            // Play a sound based on parameters
            if(type.Equals("music"))
            {
                if (subtype.Equals("gameplay"))
                {
                    MediaPlayer.Play(GameplayMusic);
                }
                else if (subtype.Equals("title"))
                {
                    MediaPlayer.Play(TitleMusic);
                }
                else if (subtype.Equals("victory"))
                {
                    MediaPlayer.Play(VictoryMusic);
                }
            }
            else if (type.Equals("dead"))
            {
                if (subtype.Equals("zombie"))
                {
                    Zombie_Dying.Play(0.60f, 0, 0);
                }
                else if (subtype.Equals("worker"))
                {
                    Worker_Dying.Play(0.30f, 0, 0);
                }
                else if (subtype.Equals("soldier"))
                {
                    Soldier_Dying.Play(0.30f, 0, 0);
                }
             }
             else if (type.Equals("attack"))
             {
                if (subtype.Equals("zombie"))
                {
                    Zombie_Attack.Play(0.40f, 0, 0);
                }
                else if (subtype.Equals("worker"))
                {
                    Worker_Attack.Play(0.40f, 0, 0);
                }
                else if (subtype.Equals("soldier"))
                {
                    Soldier_Attack.Play(0.30f, 0, 0);
                }
             }
        }
        
    }
}
