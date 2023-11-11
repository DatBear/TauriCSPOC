# Tauri-CSharp-POC

## About
* This is a proof-of-concept [tauri](https://tauri.app/)-like implementation using C#. Tauri provides a mechanism to build a native js/typescript application for Mac/Windows/iOS by hosting a WebView and using IPC to call Rust functions from the front-end.
* This project does something similar, using .NET MAUI as the UI framework, pointing a webview to a locally hosted app, and hosting a websocket server to allow native method calls from js/typescript with C#/.NET core.

## Components
* [nativeclient/](./nativeclient/) is the Next.js app created with create-t3-app.
    * [index.tsx](./nativeclient/src/pages/index.tsx) is a sample using the `invoke()` to call the native C# code.
    * [NativeSocket.ts](./nativeclient/src/utils/NativeSocket.ts) is the api that would be provided similar to Tauri's front-end library, used to invoke native methods from js/typescript.
* [nativemaui/nativemaui](./nativemaui/nativemaui/) is the native client app that also hosts the websocket server
* [nativemaui/nativecore](./nativemaui/nativecore/) is the core library that a user would have to import and decorate their RPC/native classes with `[Processor]` and methods with the `[Request]` attribute.
* [nativemaui/nativeexternal/EchoProcessor.cs](./nativemaui/nativeexternal/EchoProcessor.cs) is an example of a native class/method implementation that can be called from the front-end. This is similar to the main.rs in a tauri app, and the assembly produced from this is loaded dynamically at runtime. `invoke()` on the client side works by using reflection to find the name of the method being called and invoke it with the named parameters passed from the client.