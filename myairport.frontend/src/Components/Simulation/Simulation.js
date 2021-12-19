import './Simulation.css';
import Station from '../Stations/Station';

const Simulation = ({station}) => {

    return <div>
        <div className='airportTitle'>Welcome to your Airport!</div>
        <div id='simulatorBg' className='simulatorBg'/>
        <Station station={station}/>
    </div>
}

export default Simulation;