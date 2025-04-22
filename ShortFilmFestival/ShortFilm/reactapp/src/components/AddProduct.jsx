import React, { useState } from 'react';
import './AddProduct.css';

const AddProduct = () => {
  const initialFormData = {
    productName: '',
    category: '',
    stockQuantity: '',
    price: '',
    expiryDate: ''
  };

  const [formData, setFormData] = useState(initialFormData);
  const [errors, setErrors] = useState({});
  const [isSuccessModalOpen, setIsSuccessModalOpen] = useState(false);

  const validateForm = () => {
    let errors = {};
    let isValid = true;

    if (!formData.productName) {
      errors.productName = 'Product Name is required';
      isValid = false;
    }

    if (!formData.category) {
      errors.category = 'Category is required';
      isValid = false;
    }

    if (!formData.stockQuantity) {
      errors.stockQuantity = 'Stock Quantity is required';
      isValid = false;
    }

    if (!formData.price) {
      errors.price = 'Price is required';
      isValid = false;
    }

    if (!formData.expiryDate) {
      errors.expiryDate = 'Expiry Date is required';
      isValid = false;
    }
    setErrors(errors);
    return isValid;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (validateForm()) {
      try {
        const dataToSend = {
          ...formData,
          stockQuantity: parseInt(formData.stockQuantity),
          price: parseInt(formData.price)
        };

        console.log(dataToSend);

        const response = await fetch('https://8080-bbefdebbdfedacd317244353ebabbcadeeefceactwo.premiumproject.examly.io/addStoreitem', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(dataToSend)
        });
        console.log(response);

        if (response.ok) {
          setIsSuccessModalOpen(true);
          setFormData(initialFormData);  // Reset form to initial state
        } else {
          console.error('Failed to add product to the store');
        }
      } catch (error) {
        console.error('Error while making the POST request:', error);
      }
    }
  };

  const handleInputChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const closeSuccessModal = () => {
    setIsSuccessModalOpen(false);
  };

  return (
    <div className="add-product">
      <h2>Add Product to Store</h2>
      <form onSubmit={handleSubmit} noValidate>
        <div>
          <label htmlFor="productName">Product Name:</label>
          <input
            type="text"
            id="productName"
            name="productName"
            value={formData.productName}
            onChange={handleInputChange}
            required
          />
          {errors.productName && <div className="error">{errors.productName}</div>}
        </div>
        <div>
          <label htmlFor="category">category:</label>
          <select 
            id="category"
            name="category"
            value={formData.category}
            onChange={handleInputChange}
            required
          >
            <option value="">Select a category</option>
            <option value="diary">Dairy</option>
            <option value="snacks">Snacks</option>
            <option value="packagedFoods">Packaged Foods</option>
            <option value="beverages">Beverages</option>
            <option value="bakery">Bakery</option>
            <option value="others">Others</option>
          </select>
          {errors.category && <div className="error">{errors.category}</div>}
        </div>
        <div>
          <label htmlFor="stockQuantity">Stock Quantity:</label>
          <input
            type="number"
            id="stockQuantity"
            name="stockQuantity"
            value={formData.stockQuantity}
            onChange={handleInputChange}
            required
          />
          {errors.stockQuantity && <div className="error">{errors.stockQuantity}</div>}
        </div>
        <div>
          <label htmlFor="price">Price:</label>
          <input
            type="number"
            id="price"
            name="price"
            value={formData.price}
            onChange={handleInputChange}
            required
          />
          {errors.price && <div className="error">{errors.price}</div>}
        </div>
        <div>
          <label htmlFor="expiryDate">Expiry Date:</label>
          <input
            type="date"
            id="expiryDate"
            name="expiryDate"
            value={formData.expiryDate}
            onChange={handleInputChange}
            required
          />
          {errors.expiryDate && <div className="error">{errors.expiryDate}</div>}
        </div>
        <div>
          <button type="submit">Submit</button>
        </div>
      </form>

      {isSuccessModalOpen && (
        <div className="success-modal">
          <p>Product added successfully!</p>
          <button onClick={closeSuccessModal}>Close</button>
        </div>
      )}
    </div>
  );
};

export default AddProduct;
