@echo off

echo Building CSGO GC base...
call ..\..\Protogen\protogen.cmd -s:..\ -i:"steammessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\CSGO\SteamMsgBase.cs" -t:csharp -ns:"SteamKit2.GC.CSGO.Internal"
call ..\..\Protogen\protogen.cmd -s:..\ -i:"gcsystemmsgs.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\CSGO\SteamMsgGCSystem.cs" -t:csharp -ns:"SteamKit2.GC.CSGO.Internal"
call ..\..\Protogen\protogen.cmd -s:..\ -i:"base_gcmessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\CSGO\SteamMsgGC.cs" -t:csharp -ns:"SteamKit2.GC.CSGO.Internal"
call ..\..\Protogen\protogen.cmd -s:..\ -i:"gcsdk_gcmessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\CSGO\SteamMsgGCSDK.cs" -t:csharp -ns:"SteamKit2.GC.CSGO.Internal"
call ..\..\Protogen\protogen.cmd -s:..\ -i:"econ_gcmessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\CSGO\SteamMsgGCEcon.cs" -t:csharp -ns:"SteamKit2.GC.CSGO.Internal"
call ..\..\Protogen\protogen.cmd -s:..\ -i:"engine_gcmessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\CSGO\SteamMsgGCEngine.cs" -t:csharp -ns:"SteamKit2.GC.CSGO.Internal"

echo Building CSGO messages...
call ..\..\Protogen\protogen.cmd -s:..\ -i:"cstrike15_gcmessages.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\CSGO\MsgGC.cs" -t:csharp -ns:"SteamKit2.GC.CSGO.Internal"
