import { useEffect, useState } from 'react';
import './App.css';
import AirportUi from './Components/AirportUi/AirportUi';
import Connect from './Components/Connect';
import { HubConnectionBuilder } from "@microsoft/signalr";

function App() {

  const [connection, setConnection] = useState();
  const [station, setStation] = useState();
  
  const isLandscape = () => 
  window.matchMedia('(orientation:landscape)').matches,
  [orientation, setOrientation] = useState(isLandscape() ? 'landscape' : 'portrait'),
  onWindowResize = () => {              
    clearTimeout(window.resizeLag)
    window.resizeLag = setTimeout(() => {
      delete window.resizeLag                       
      setOrientation(isLandscape() ? 'landscape' : 'portrait')
    }, 200)
  }

useEffect(() => (
  Connect(setConnection,setStation),
  onWindowResize(),
  window.addEventListener('resize', onWindowResize),
  () => window.removeEventListener('resize', onWindowResize)
),[])

  return (
    <div className="App">
      {
        orientation !== 'landscape'
        ? <h1>Please rotate your phone.</h1>
        : !connection
        ? <h1>Connecting...</h1>
        : <AirportUi connection={connection} station={station}/>
      }
    </div>
  );
}

export default App;