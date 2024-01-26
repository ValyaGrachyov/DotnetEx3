import axios from "axios"

function throwError(err) {
    if (err.response)
        throw new Error(err.response.data);
    throw new Error("Error during request.");
}

class AxiosWrapper {
    constructor(url = "https://localhost:7240") {
        const options = {
            baseURL: url,
            timeout: 10000,
            ssl: false,
            headers: {
                'Accept': 'application/json',
                'Content-type': 'application/json; charset=UTF-8',
                "Access-Control-Allow-Origin": url,
                "X-Requested-With": "XMLHttpRequest"
            },
            withCredentials: true,
        }

        const token = sessionStorage.getItem("token");
        this.axiosInstance = axios.create(options);
        this.axiosInstance.defaults.headers["Authorization"] = `Bearer ${token}`;
    }

    async login(username, password) {
        let userInfo = {token: null, username: null};
        await this.axiosInstance.post("/account/login", {username: username, password: password})
        .then(response =>
             userInfo = response.data )
        .catch(err => 
            throwError(err));

        sessionStorage.setItem("token", userInfo.token);
        sessionStorage.setItem("username", userInfo.username);
        this.axiosInstance.defaults.headers["Authorization"] = `Bearer ${userInfo.token}`;
    }

    async register(username, password) {
        await this.axiosInstance.post("/account/register", {username: username, password: password})
        .then(() => {})
        .catch(err => throwError(err))
    }

    async logout() {
        sessionStorage.removeItem("token");
        sessionStorage.removeItem("username");
        this.axiosInstance.defaults.headers["Authorization"] = '';
    }

    async testSession() {
        let isValidSession = false;
        await this.axiosInstance.get("/account/token/check")
            .then(() => {isValidSession = true})
            .catch(() => {});
        return isValidSession;
    }

    async joinRoom(roomId) {
        let joinResult = false;
        
        await this.axiosInstance.post(`/rooms/${roomId}/join`)
            .then(() => {joinResult = true})
            .catch((err) => {
                if (err.data)
                    alert(err.data);
            });

        return joinResult;
    }

    async getrooms() {
       const rooms = await this.axiosInstance.get("/games"); 
       return rooms;       
    }

    async getroom(id) {
        const room = await this.axiosInstance.get(`/games/${id}`);
        if (room.data)
            return room.data.value;
        return {roomId: id};
    }


    async getUsersRaiting() {
        const rates = await this.axiosInstance.get("/rate");
        return rates;
    }
}

const API = new AxiosWrapper();
export default API;