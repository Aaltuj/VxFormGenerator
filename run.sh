#!/bin/bash

PS3="Select framework: "
select i in Bootstrap Plain exit
do
  case $i in
    Bootstrap) 
    FRAMEWORK="--configuration Bootstrap" 
    break
    ;;
    Plain) 
    FRAMEWORK=""
    break
    ;;
    exit) exit;;
  esac
done

PS3="Select kind of application: "
select i in Server Wasm exit
do
  case $i in
    Server) dotnet watch --project ./VxFormGeneratorDemo.Server/VxFormGeneratorDemo.Server.csproj run $FRAMEWORK 
    break
    ;;
    Wasm) dotnet watch --project ./VxFormGeneratorDemo.Wasm/VxFormGeneratorDemo.Wasm.csproj run $FRAMEWORK
    break
    ;;
    exit) exit;;
  esac
done

#dotnet watch --project ./Demo/FormGeneratorDemo.csproj run --configuration Bootstrap
