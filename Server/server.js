import WebSocket, { WebSocketServer } from 'ws';

const wss = new WebSocketServer({ port: 8080 });

console.log("Server is starting...")

wss.on('connection', function connection(ws) {
  ws.on('message', function message(data) {
    console.log('received: %s', data);
    wss.clients.forEach(function each(client) {
      if (client.readyState === WebSocket.OPEN) {
        console.log("send: %s", data)
        client.send(data);
      }
    });
  });

  ws.send('something');

  ws.on('close', function close() {
    console.log(ws.id," close")
  })
});