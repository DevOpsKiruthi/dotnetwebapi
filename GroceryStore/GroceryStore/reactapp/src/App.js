import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './components/NavBar';
import Home from './components/Home';
import AddProduct from './components/AddProduct';
import DisplayProduct from './components/DisplayProduct';
import Footer from './components/Footer';
import './App.css';

function App() {
  return (
    <Router>
      <div className='App'>
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/addproduct" element={<AddProduct />} />
          <Route path="/displayproduct" element={<DisplayProduct />} />
        </Routes>
        <Footer />
      </div>
    </Router>
  );
}

export default App;
