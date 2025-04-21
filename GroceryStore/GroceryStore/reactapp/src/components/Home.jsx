import React from 'react';
import './Home.css';

const Home = () => {
  return (
    <div className="home_container">
      <h2>Welcome to Fresh Groceries!</h2>
      <div className="shop_info">
          <h3>Our store is located at:</h3>
          <p>1234 Grocery Store Lane</p>
          <p>Grocery, ST 12345</p>
      </div>
    </div>
  );
};

export default Home;
