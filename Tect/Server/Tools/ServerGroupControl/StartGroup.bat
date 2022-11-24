SET executable=..\..\Bin\net6.0\AsunaServer.Application.exe
SET config=ServerGroupConfig.json
python GroupControl.py start %executable% %config%