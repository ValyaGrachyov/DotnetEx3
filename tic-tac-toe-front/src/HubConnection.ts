import * as signalR from "@microsoft/signalr";
const URL = process.env.GAMEHUB ?? "https://localhost:7240/game-hub";

class Connector {
    private connection: signalR.HubConnection;
    public events: (onMessageReceived: (event: any) => void) => void;
    static instance: Connector;
    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(URL)
            .withAutomaticReconnect()
            .build();
        this.connection.start().catch(err => document.write(err));
        this.events = (onGameEvent) => {
            this.connection.on("GameEvent", event => {
                onGameEvent(event);
            });
        };
    }
    public makeTurn = (roomId: string, row: number, column: number) => {
        this.connection.send("makeTurn", roomId, row, column).then(x => console.log("sent"))
    }
    public static getInstance(): Connector {
        if (!Connector.instance)
            Connector.instance = new Connector();
        return Connector.instance;
    }
}

export default Connector.getInstance;