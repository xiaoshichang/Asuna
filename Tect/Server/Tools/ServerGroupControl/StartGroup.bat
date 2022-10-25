SET executable=..\..\AsunaServer\Asuna.Application\bin\Debug\net6.0\Asuna.Application.exe
SET config=ServerGroupConfig.json
python GroupControl.py start %executable% %config%