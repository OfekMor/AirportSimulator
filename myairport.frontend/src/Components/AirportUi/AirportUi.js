import { useState, useEffect } from 'react';
import './AirportUi.css';
import Simulation from '../Simulation/Simulation';

const AirportUi = ({connection,station}) => {

    const [flag, setFlag] = useState(false);
    useEffect(() => {
        connection.connectionState == "Connected" && connection.invoke("StationsStatus");
    }, [connection.connectionState])
    return <div>
        {
            !flag
            ? <div className='planes center'> 
                <div className='welcomBtn center' onClick={() => setFlag(true)}>Airport Simulation!</div> 
            </div>
            : <Simulation station={station}/>
        }
</div>
}

export default AirportUi;