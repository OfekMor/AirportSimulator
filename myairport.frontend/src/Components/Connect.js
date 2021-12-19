import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const Connect = (setConnection,setStation) => {
    let connection = new HubConnectionBuilder()
    .withUrl('https://localhost:44379/airportServer')
    .configureLogging(LogLevel.Information)
    .build();

    connection.on("StationsStatus", (list) =>{
        setStation(list);
    });
    
    connection.start();
    setConnection(connection);
    console.log('connecting complte!');
}

export default Connect;