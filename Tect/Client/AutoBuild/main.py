import subprocess
import os

unity_exe_path = r"C:\Program Files\Unity\Hub\Editor\2021.3.12f1\Editor\Unity.exe"
project_path = r"..\unityProj"
config_path = os.path.abspath("Config/Windows64_Debug.json")


def refresh_project_settings():
    print("refresh_project_settings start")
    # https://docs.unity3d.com/Manual/EditorCommandLineArguments.html
    arg = []
    arg.append(unity_exe_path)
    arg.append("-batchmode")
    arg.extend(["-stackTraceLogType", "None"])
    arg.extend(["-projectPath", project_path])
    arg.extend(["-executeMethod", "Asuna.Build.BuildPlayerPipeline.RefreshProjectSettingForBuild"])
    arg.extend(["-logFile", "Logs/RefreshProjectSettingForBuild.log"])
    arg.extend(["-BuildConfig", config_path])
    process = subprocess.Popen(arg)
    process.wait()

    if process.returncode != 0:
        print("refresh_project_settings error")
        exit(1)
    else:
        print("refresh_project_settings ok")


def build_player():
    print("build_player start")
    arg = []
    arg.append(unity_exe_path)
    arg.append("-batchmode")
    arg.extend(["-stackTraceLogType", "None"])
    arg.extend(["-projectPath", project_path])
    arg.extend(["-executeMethod", "Asuna.Build.BuildPlayerPipeline.BuildPlayer"])
    arg.extend(["-logFile", "Logs/BuildPlayer.log"])
    arg.extend(["-BuildConfig", config_path])
    process = subprocess.Popen(arg)
    process.wait()
    if process.returncode != 0:
        print("build_player error")
        exit(1)
    else:
        print("build_player ok")


def build_pipeline():
    """
    打包流水线
    """
    refresh_project_settings()
    build_player()


if __name__ == '__main__':
    build_pipeline()


