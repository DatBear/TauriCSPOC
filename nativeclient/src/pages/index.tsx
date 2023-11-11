import { useEffect } from "react";
import socket, { invoke } from "~/utils/NativeSocket";

export default function Home() {
  useEffect(() => {
    let interval = setInterval(async () => {
      const response = await invoke<string>("Echo", { str: "test " + new Date() }, true);
      console.log('response', response);
    }, 1000);
    return () => clearInterval(interval);
  }, []);

  return <>
    <h1 className="text-2xl">
      Testing!
    </h1>
  </>
}