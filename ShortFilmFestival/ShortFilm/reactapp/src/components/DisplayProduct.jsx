import React, { useState, useEffect } from 'react';
import myImage from '../assets/displayimage.jpg'; 
import './DisplayProduct.css';

const DisplayProduct = () => {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch('https://8080-bbefdebbdfedacd317244353ebabbcadeeefceactwo.premiumproject.examly.io/getAllStoreitem', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
          }
        });
        if (response.ok) {
          const data = await response.json();
          setProducts(data);
        } else {
          console.error('Failed to fetch products from the store');
        }
      } catch (error) {
        console.error('Error while fetching products:', error);
      }
    }
    fetchData();
  }, []);

  return (
    <div>
      <h2>Products in Store</h2>
      <table className="product-table">
        <thead>
          <tr>
            <th>Image</th>
            <th>Name</th>
            <th>Category</th>
            <th>Remaining Stock</th>
            <th>Price</th>
            <th>Exipry Date</th>
          </tr>
        </thead>
        <tbody>
          {products.map((product) => (
            <tr key={product.id}>
              <td><img src={myImage} alt="Product" className="product-image" /></td>
              <td>{product.productName}</td>
              <td>{product.category}</td>
              <td>{product.stockQuantity}</td>
              <td>${product.price}</td>
              <td>{product.expiryDate}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default DisplayProduct;
