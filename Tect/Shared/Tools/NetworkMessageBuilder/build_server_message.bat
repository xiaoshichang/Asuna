SET executable=google.protobuf.tools\3.21.10\tools\windows_x64\protoc.exe
SET SRC_DIR=..\..\..\Server\Managed\AsunaServer.NetworkMessage\Message\Protobuf

%executable%  --csharp_out=%SRC_DIR% ServerSystemMessage.proto