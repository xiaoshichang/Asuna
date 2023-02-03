﻿using Asuna.Asset;
using Asuna.Camera;
using Asuna.Coroutine;
using Asuna.Entity;
using Asuna.Gameplay;
using Asuna.Input;
using Asuna.Network;
using Asuna.Timer;
using Asuna.UI;


namespace Asuna.Application
{
    public static class G
    {

        public static void SetupConfig(ApplicationRoot root)
        {
            Application = root;
            ApplicationSetting = root.ApplicationSetting;
        }
        
        /// <summary>
        /// core 代表可以在其他 manager 的初始化流程中引用
        /// </summary>
        public static void SetupCoreManagers(ApplicationRoot root)
        {
            PlayerInputManager = root.PlayerInputManager;
            CoroutineManager = root.CoroutineManager;
            TimerManager = root.TimerManager;
            AssetManager = root.AssetManager;
        }
        
        /// <summary>
        /// other 代表不能在其他 manager 的初始化流程中引用
        /// </summary>
        public static void SetupOtherManagers(ApplicationRoot root)
        {
            NetworkManager = root.NetworkManager;
            UIManager = root.UIManager;
            EntityManager = root.EntityManager;
        }

        /// <summary>
        /// system 是 gameplay 层面的东西，更偏向于逻辑系统
        /// </summary>
        public static void SetupGameplay(GameplayInstance gameplayInstance)
        {
            GameplayInstance = gameplayInstance;
        }

        public static void ResetCoreManagers()
        {
            AssetManager = null;
            TimerManager = null;
            CoroutineManager = null;
            PlayerInputManager = null;
            ApplicationSetting = null;
        }

        public static void ResetOtherManagers()
        {
            EntityManager = null;
            UIManager = null;
            NetworkManager = null;
        }

        public static void ResetGameplay()
        {
            GameplayInstance = null;
        }

        public static ApplicationSetting ApplicationSetting;
        public static ApplicationRoot Application;
        public static PlayerInputManager PlayerInputManager;
        public static CoroutineManager CoroutineManager;
        public static TimerMgr TimerManager;
        public static AssetManager AssetManager;
        public static NetworkManager NetworkManager;
        public static UIManager UIManager;
        public static EntityManager EntityManager;
        public static GameplayInstance GameplayInstance;
        
        public static GMSystem GMSystem;

    }
}