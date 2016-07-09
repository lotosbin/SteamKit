@echo off

call ..\..\Protogen\protogen.cmd -s:..\ -i:"gc.proto" -o:"..\..\..\SteamKit2\SteamKit2\Base\Generated\GC\MsgBaseGC.cs" -t:csharp -ns:"SteamKit2.GC.Internal"
