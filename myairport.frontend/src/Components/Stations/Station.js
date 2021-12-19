import { useEffect, useRef } from 'react';
import './Station.css';

const Station = ({station}) => {
    const stationsList = useRef(null);
    useEffect(() => {
        for (let i = 1; i <= 8; i++) {
            let node = document.createElement('div');
            node.className = 'station station' + i
            stationsList.current.appendChild(node);
        }
        for (let i = 1; i <= 5; i++) {
            let node = document.createElement('div');
            node.className = 'green green' + i
            stationsList.current.appendChild(node);
        }
        for (let i = 1; i <= 8; i++) {
            let node = document.createElement('div');
            node.className = 'blue blue' + i
            stationsList.current.appendChild(node);
        }
    }, [])
    
    useEffect(() => {
        if(!station) return;
        debugger;
        for (let i = 1; i <= 8; i++) {
            station[i-1].plane
            ? stationsList.current.children[i-1].innerHTML = station[i-1].plane.planeName
            : stationsList.current.children[i-1].innerHTML = '';
        }
    }, [station])
    return <div ref={stationsList} className='airport center'/>
}


export default Station;