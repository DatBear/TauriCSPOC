//todo make this into an npm package
import { v4 as uuid } from 'uuid';

let _socket: WebSocket;

export const socket = () => {
  if (!_socket) {
    _socket = new WebSocket(`ws://${location.host.indexOf(':') > 0 ? location.host.substring(0, location.host.indexOf(':')) : location.host}:4051`);
    _socket.onmessage = onMessage;
  }

  return new Promise<WebSocket>((resolve, reject) => {
    const onOpen = () => {
      resolve(_socket);
    }
    _socket.onopen = onOpen;
    if (_socket.readyState == _socket.OPEN) {
      resolve(_socket);
    }
  });
};

const onMessage = (msg: MessageEvent<any>) => {
  try {
    let data = JSON.parse(msg.data);
    let eventName = `ws-ev-${data.id}`;
    const event = new CustomEvent(eventName, { detail: data.data });
    document.dispatchEvent(event);
  } catch (e) {
    console.error('error sending event for data: ', msg.data);
  }
}


export const invoke = async <T>(command: string, data: any, log: boolean = false) => {
  const id = uuid();
  let s = await socket();

  let str = JSON.stringify({ command, data, id });
  s.send(str);
  if (log) {
    console.log('sent ', str);
  }
  return new Promise((resolve, reject) => {
    const eventListener = (msg: CustomEvent<any>) => {
      //@ts-ignore -- typescript doesn't like custom events
      document.removeEventListener(eventName, eventListener);
      if (log) {
        console.log('received', str);
      }
      resolve(msg.detail as T);
    }
    const eventName = `ws-ev-${id}`;
    //@ts-ignore -- typescript doesn't like custom events
    document.addEventListener(eventName, eventListener);
  });
}

export default socket;