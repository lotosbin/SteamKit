@echo off

echo Building TF2 GC base...
call ..\..\Protogen\protogen.cmd -s:..\ -i:"steammessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\TF2\SteamMsgBase.cs" -t:csharp -ns:"SteamKit2.GC.TF2.Internal"
call ..\..\Protogen\protogen.cmd -s:..\ -i:"gcsystemmsgs.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\TF2\SteamMsgGCSystem.cs" -t:csharp -ns:"SteamKit2.GC.TF2.Internal"
call ..\..\Protogen\protogen.cmd -s:..\ -i:"base_gcmessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\TF2\SteamMsgGC.cs" -t:csharp -ns:"SteamKit2.GC.TF2.Internal"
call ..\..\Protogen\protogen.cmd -s:..\ -i:"gcsdk_gcmessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\TF2\SteamMsgGCSDK.cs" -t:csharp -ns:"SteamKit2.GC.TF2.Internal"
call ..\..\Protogen\protogen.cmd -s:..\ -i:"econ_gcmessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\TF2\SteamMsgGCEcon.cs" -t:csharp -ns:"SteamKit2.GC.TF2.Internal"

echo Building TF2 GC messages
call ..\..\Protogen\protogen.cmd -s:..\ -i:"tf_gcmessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\TF2\MsgGC.cs" -t:csharp -ns:"SteamKit2.GC.TF2.Internal"
