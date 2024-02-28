import React from 'react';
import logo from '../logo.svg';
import '../App.css';

const HomePage = () => {
  return (
    <div>
       <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <p>
            Welcome to Stargate
          </p>
          <a
            className="App-link"
            href="https://reactjs.org"
            target="_blank"
            rel="noopener noreferrer"
          >          </a>
        </header>
    </div>
  );
}

export default HomePage;