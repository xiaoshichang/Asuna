using System;
using System.Collections.Generic;
using System.Diagnostics;
using Asuna.Utils;
using UnityEditor;

namespace Asuna.Check
{
    public delegate bool CheckTaskAction();
    
    public enum CheckTaskResult
    {
        Ready,
        Success,
        Fail
    }
    
    public class CheckTask
    {
        public string Name;
        public CheckTaskAction Action;
        public CheckTaskResult Result;
        public float Duration;

        public void Run()
        {
            ADebug.Assert(Result == CheckTaskResult.Ready);
            ADebug.Assert(Action != null);
            
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Result = Action.Invoke() ? CheckTaskResult.Success : CheckTaskResult.Fail;
            stopWatch.Stop();
            Duration = stopWatch.Elapsed.Seconds;
        }
    }
    
    public abstract class CheckPipeline
    {
        public void Run()
        {
            var pipelineName = GetType().Name;
            
            for (var i = 0; i < _Tasks.Count; i++)
            {
                var task = _Tasks[i];
                EditorUtility.DisplayProgressBar($"{pipelineName}", $"{task.Name}", (float)(i + 1) / _Tasks.Count);
                task.Run();
                ADebug.Info($"{pipelineName} - {task.Name}[{i + 1}/{_Tasks.Count}] : result: {task.Result} | duration: {task.Duration} seconds");
                if (task.Result == CheckTaskResult.Fail)
                {
                    break;
                }
            }
            EditorUtility.ClearProgressBar();
        }
        
        protected void AddTask(string actionName, CheckTaskAction action)
        {
            var task = new CheckTask()
            {
                Name = actionName,
                Action = action,
                Result = CheckTaskResult.Ready,
                Duration = 0
            };
            _Tasks.Add(task);
        }

        private readonly List<CheckTask> _Tasks = new List<CheckTask>();
    }
}