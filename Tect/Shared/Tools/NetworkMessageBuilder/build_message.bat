SET executable=google.protobuf.tools\3.21.10\tools\windows_x64\protoc.exe


SET SERVER_SRC_DIR=..\..\..\Server\Managed\AsunaServer\Network\Message\Protobuf
SET CLIENT_SRC_DIR=..\..\..\Client\UnityProj\Assets\Asuna\Network\Message\Protobuf


%executable%  --csharp_out=%SERVER_SRC_DIR% ServerSystemMessage.proto
%executable%  --csharp_out=%SERVER_SRC_DIR% SharedSystemMessage.proto


%executable%  --csharp_out=%CLIENT_SRC_DIR% SharedSystemMessage.proto