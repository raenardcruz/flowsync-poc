import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

const signalRConnection = new HubConnectionBuilder()
    .withUrl('https://localhost:7217/logichub', {
        withCredentials: true
    })
    .configureLogging(LogLevel.Information)
    .build();

signalRConnection.onclose(async () => {
    await start();
});

async function start() {
    try {
        await signalRConnection.start();
        console.log('Logic Hub Connected.');
    } catch (err) {
        console.log('Logic Hub Connection Error: ', err);
        setTimeout(start, 5000);
    }
}

start();

export default signalRConnection;
