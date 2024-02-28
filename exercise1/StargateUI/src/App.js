import React from 'react';
import logo from './logo.svg';
import './App.css';
import Dummy_Component from "dummy-component";
import {
  BrowserRouter as Router,
  Routes,
  Route,
} from "react-router-dom";
import Navbar from './Components/Navbar';
import HomePage from './Pages/Home';
import About from './Pages/About';
import CreateDuty from './Pages/CreateDuty';
import PersonSearch from './Pages/PersonSearch';
import People from './Pages/People';
import Duties from './Pages/Duties';
function App() {
  return (
    <Router>
      <div className="App">
        <Navbar />
        <Routes>
            <Route exact path="/" element={<HomePage />} />
            <Route path="/About" element={<About />} />
            <Route path="/CreateDuty" element={<CreateDuty />} />
            <Route path="/People" element={<People />} />
            <Route path="/Search" element={<PersonSearch />} />
            <Route path="/DutySearch" element={<Duties />} />
        </Routes>
      </div>
    </Router>
    
  );
}

export default App;
