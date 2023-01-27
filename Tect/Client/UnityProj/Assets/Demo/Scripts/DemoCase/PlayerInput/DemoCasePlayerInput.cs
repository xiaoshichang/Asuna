using System.Collections;
using Asuna.Application;
using Asuna.Entity;
using Asuna.Input;
using Asuna.Timer;
using Asuna.Utils;
using Demo.UIBasic;
using UnityEngine;

namespace Demo.LoadScene
{
    public class DemoCasePlayerInput : DemoCaseLoadScene
    {
        public override void InitDemo()
        {
            base.InitDemo();
            _InitMainPlayerInput();
        }

        public override void ReleaseDemo()
        {
            _ReleaseMainPlayerInput();
            base.ReleaseDemo();
        }

        public override string GetDemoName()
        {
            return "Player Input";
        }
        
        
        public override void Tick(float dt)
        {
        }
    }
}