SET executable=..\..\Bin\net6.0\AsunaServer.exe
SET config=ServerGroupConfig.json
python GroupControl.py start %executable% %config%