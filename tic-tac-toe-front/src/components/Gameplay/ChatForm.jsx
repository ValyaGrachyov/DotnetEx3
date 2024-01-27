import { AuthData } from "../Auth/AuthWrapper";
import { useReducer } from "react";


export default function ChatForm({sendMessage, messages}) {
    const [ formData, setFormData ] = useReducer((formData, newItem) => { return ( {...formData, ...newItem} )}, {message: ""});
    const { user } = AuthData();

    return <div className="chat">
        <div className="messages">
            {messages.map((x, i) => (
                                <div 
                                    key={ i } 
                                    className='message'
                                    style={{ color: user.username === x.username ? 'blue' : '' }}>
                                    { `${x.username}: ${x.message}` }
                                </div>
                            ))}
        </div>
        <div className="chat-input">
                <input type="text"  placeholder="message"
                       value={formData?.message} 
                       onChange={(e) => setFormData({message: e.target.value?.trim()}) }/>
                <button onClick={() => {sendMessage(formData.message)}} >Send</button>
        </div>
    </div>
}