import React from 'react';
import { Routes, Route, Link } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';

function App() {
  return (
    <div style={{ textAlign: 'center', margin: '20px' }}>
      <Routes>
        <Route
          path="/"
          element={
            <div>
              <h1>Welcome to My App</h1>
              <nav>
                <Link to="/login" style={{ margin: '10px' }}>Login</Link>
                <Link to="/register" style={{ margin: '10px' }}>Register</Link>
              </nav>
            </div>
          }
        />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </div>
  );
}

export default App;
